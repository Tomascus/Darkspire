using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Attributes")]
    public EnemyAttributes attributes; // ScriptableObject for enemy stats
    private float currentHealth;

    [Header("Detection")]
    public Transform player;
    private bool isPlayerDetected = false;
    private bool inAttackRange = false;

    [Header("Patrolling Settings")]
    [SerializeField] private Vector3 patrolCenter; // The starting point of the patrol area - enemy position
    [SerializeField] private Vector3 patrolSize; // Adjustments for the size of the patrol area
    private Vector3 patrolDestination; 
    private bool isPatrolling = true;
    private bool isWaiting = false; // This a check to see if the enemy is waiting before moving to the next patrol point

    [Header("Combat Settings")]
    [SerializeField] private float attackCooldown = 2f; 
    private float lastAttackTime = 0f;
    [SerializeField] private float attackDamage; 

    [Header("AI Navigation & Animator")]
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Health Bar Slider")]
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Slider healthBar;

    private bool isDead = false; 

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        getEnemyStats();
        NewPatrolPoint();
        InitializeHealthBar(); //initialize the health bar 
    }

    private void Update()
    {
        if (isDead) return;

        DetectPlayer();

        if (isPlayerDetected)
        {
            PlayerDetectedAction();
        }
        else if (isPatrolling && !isWaiting)
        {
            Patrol();
        }

        // This is for the check when enemy loses the player is outside the patrol area - it will return the enemy to a point within the patrol area and start patrolling again
        if (!isPlayerDetected && !isWaiting && !IsInPatrolArea())
        {
            NewPatrolPoint(); // Return to patrol if outside patrol area
            isPatrolling = true; // Ensure the enemy resumes patrolling
        }

        UpdateAnimations();

    }

    private void UpdateAnimations()
    {
        // Set animation parameters based on enemy state
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
    }

    private void getEnemyStats()
    {
        currentHealth = attributes.health;
        agent.speed = attributes.movementSpeed;
        attackDamage = attributes.attackDamage;
        agent.stoppingDistance = attributes.attackRange;
        patrolCenter = transform.position; // Sets the start of patrolling to the enemy locations
    }

    // Constant check for player detection - when detected it changes the flags to start AI actions
    private void DetectPlayer()
    {
        // Use a small detection range to start following the player
        float playerDistance = Vector3.Distance(transform.position, player.position); // Calculates the distance to player

        if (playerDistance <= attributes.detectionRange) // Checks if the player is within detection range based on the enemy scriptable object value
        {
            isPlayerDetected = true;
        }
        else
        {
            isPlayerDetected = false;
        }



        if (playerDistance <= attributes.attackRange) // Checks if the player is within attack range based on the enemy scriptable object value
        {
            inAttackRange = true;
        }
        else
        {
            inAttackRange = false;
        }
    }

    // This will make the enemy move towards the player if the player is detected and if in range it attacks
    private void PlayerDetectedAction()
    {
        isPatrolling = false; // Stop patrolling when the player is detected

        RotateTowardsPlayer();
        ChasePlayer();

        if (inAttackRange)
        {
            PerformAttack();
        }
    }

    private void Patrol()
    {
        // Check if the enemy has reached the patrol destination (a point within the patrol area) and also checks if it is still calculating path
        if (agent.remainingDistance < 0.5f && !agent.pathPending) // 
        {
            if (!isWaiting)
            {
                StartCoroutine(NextPatrolDelay()); // Sets the enemy to idle state to wait for next patrol point
            }
        }
    }

    private IEnumerator NextPatrolDelay()
    {
        isWaiting = true;
        agent.isStopped = true; 
        yield return new WaitForSeconds(5f); 
        agent.isStopped = false;

        NewPatrolPoint(); // Sets a new patrol point after waiting
        isWaiting = false;
    }

    private void NewPatrolPoint()
    {
        // Create a random point within the the patrol area - calculates the point by picking random x and z values within the patrol area (divided by 2 to have substantial moves each time)
        Vector3 randomPoint = new Vector3(
            Random.Range(patrolCenter.x - patrolSize.x / 2, patrolCenter.x + patrolSize.x / 2),
            patrolCenter.y,
            Random.Range(patrolCenter.z - patrolSize.z / 2, patrolCenter.z + patrolSize.z / 2)
        );

        // Uses navmesh to find the closest point on the NavMesh to the random point 
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolSize.magnitude / 2, NavMesh.AllAreas))
        {
            // Sets new destination to new navmesh point that was found and makes the enemy move to it
            patrolDestination = hit.position;
            agent.SetDestination(patrolDestination);
        }
    }

    private bool IsInPatrolArea()
    {
        // Checks if the enemy is within the patrol area, if not it will return false
        return patrolCenter.x - patrolSize.x / 2 <= transform.position.x && transform.position.x <= patrolCenter.x + patrolSize.x / 2 &&
               patrolCenter.z - patrolSize.z / 2 <= transform.position.z && transform.position.z <= patrolCenter.z + patrolSize.z / 2;
    }


    private void RotateTowardsPlayer()
    {
        // Rotate smoothly toward the player only if detected
        Vector3 direction = (player.position - transform.position).normalized; // Calculates the direction to the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Calculates the rotation to look at the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f); // Smoothly rotates towards the player
        //IMPLEMENT CUSTOM ROTATION SPEED ??
    }

     private void ChasePlayer()
    {
        // Move the enemy towards the player if it is not in attack range
        if (!inAttackRange)
        {
            agent.destination = player.position;
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true; // Stops moving when in attack range
        }
    }

    private void PerformAttack()
    {
        // Checks if the time that has passed since the last attack is greater than the attack cooldown -> executes attack if so and damages the player
        if (Time.time >= lastAttackTime + attackCooldown) 
        {
            lastAttackTime = Time.time;

            // Randomly choose between two attack animations
            int randAttack = Random.Range(0, 2); 

            if (randAttack == 0)
            {
                // Trigger first attack animation
                animator.SetTrigger("Attack1");
            }
            else
            {
                // Trigger second attack animation
                animator.SetTrigger("Attack2");
            }

            Debug.Log("Enemy attack");
            
            DamagePlayer();
        }
    }

    private void InitializeHealthBar()
    {
        GameObject healthBarInstance = Instantiate(healthBarPrefab, transform); // Instantiates the health bar prefab
        healthBar = healthBarInstance.GetComponentInChildren<Slider>(); // Gets the slider component from the health bar prefab

        //Set initial values 
        healthBar.maxValue = attributes.health;
        healthBar.value = currentHealth;

    }

    private void DamagePlayer()
    {
        // Checks if the player is within attack range and damages the player if so
        if (player.gameObject.CompareTag("Player"))
        {
            
            PlayerUI.OnTakeDamage(attackDamage); 
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        animator.SetTrigger("TakeDamage");
    }

    private void Die()
    { 
        isDead = true;
        Debug.Log("You killed the enemy");
        animator.SetTrigger("Die");
        agent.isStopped = true; // Stop the enemy movement
        Destroy(gameObject, 5f); 
    }

    // Visualize the patrol area in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(patrolCenter, patrolSize);
    }
}
