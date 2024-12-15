using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class BossController : MonoBehaviour
{
    [Header("Attributes")]
    public EnemyAttributes attributes; // ScriptableObject for enemy stats
    private float currentHealth;

    [Header("Detection")]
    public Transform player;
    [SerializeField] private PlayerUI playerUI;
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

    [Header("Boss Attributes")]
    [SerializeField] private float specialCooldown = 30f; 
    [SerializeField] private float phase2Stats = 1.5f; // The boss gets stronger in phase 2
    [SerializeField] private GameObject effectPrefab;
    private float specialRadius = 5f;
    private bool isPhase2 = false;
    private float lastSpecialTime; // Tracks the last time since the special attack was used
    private bool isCasting = false; 

    [Header("AI Navigation & Animator")]
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Health Bar Slider")]
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Slider healthBar;

    [Header("Damage Numbers & Particles")]
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private Transform hitPoint;

    [Header("Spawned Object")]
    [SerializeField] private GameObject deathSpawn;

    private bool isDead = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GetEnemyStats();
        NewPatrolPoint();
        InitializeHealthBar(); // Initialize the health bar
    }

    private void Update()
    {
        if (isDead) return;

        DetectPlayer();

        if (isPlayerDetected)
        {
            PlayerDetectedAction();

            if (Time.time >= lastSpecialTime + specialCooldown)
            {
                SpecialAttack();
                lastSpecialTime = Time.time;
            }

            if (!isPhase2 && currentHealth <= attributes.health/2)
            {
                EnterPhase2();
            }
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
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
    }

    private void GetEnemyStats()
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
        BossChase();

        if (inAttackRange)
        {
            PerformAttack();
        }
    }

    // Upgraded chase method that predicts where the player goes by calculating the player velocity
    private void BossChase()
    {
        if (!inAttackRange && !isCasting)
        {
            Vector3 playerVelocity = (player.position - agent.destination) / Time.deltaTime;
            Vector3 predictPosition = player.position + playerVelocity.normalized * 2f; // The boss predicts where the player will be in 2 seconds

            agent.SetDestination(predictPosition);
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true; // Stops moving when in attack range
        }
    }

    private void Patrol()
    {
        // Check if the enemy has reached the patrol destination (a point within the patrol area) and also checks if it is still calculating path
        if (agent.remainingDistance < 0.5f && !agent.pathPending) 
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

    // Special attack method using coroutine to wait for casting time - gives the player a chance to react
    private void SpecialAttack()
    {
        StartCoroutine(PerformSpecialAttack());
    }

    private IEnumerator PerformSpecialAttack()
    {
        isCasting = true;
        animator.SetTrigger("CastSpell");

        Vector3 targetPosition = player.position; // This determines where the spell will spawn - offset of current position for player reaction

        // Wait for the animation to finish
        yield return new WaitForSeconds(1.5f);

        // Create a spell effect at the target position and destroy it after 2 seconds
        if (player != null)
        {
            GameObject spell = Instantiate(effectPrefab, targetPosition, Quaternion.identity);
            
            Destroy(spell, 2f);

            // Apply damage to the player if they are in special attack radius
            Collider[] hitColliders = Physics.OverlapSphere(player.position, specialRadius);
            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("Player"))
                {
                    // This checks checks the distance between the player and the hit object to see if the player is hit
                    float distanceToPlayer = Vector3.Distance(hit.transform.position, targetPosition);
                    if (distanceToPlayer <= specialRadius)
                    {
                        PlayerUI.OnTakeDamage(attackDamage * 1.5f);
                    }
                }
            }
        }
        isCasting = false;
    }

    // Phase 2 - Makes the boss stronger and faster
    private void EnterPhase2()
    {
        isPhase2 = true;
        agent.speed *= phase2Stats;
        attackDamage *= phase2Stats;
        Debug.Log("Boss entered PHASE 2");
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

        hitParticle();

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (damageText != null)
        {
            StartCoroutine(ShowDamageNumber(damage));
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        animator.SetTrigger("TakeDamage");
    }

    private IEnumerator ShowDamageNumber(int damage)
    {
        damageText.text = "-" + damage.ToString();
        damageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        damageText.gameObject.SetActive(false);
        damageText.text = string.Empty;
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("You killed the boss");
        animator.SetTrigger("Die");
        agent.isStopped = true; // Stop the enemy movement
        playerUI.AddXP(attributes.xpReward); // Add XP to the player when the enemy dies
        StartCoroutine(SpawnNPC());
    }

    private IEnumerator SpawnNPC()
    {
    
        yield return new WaitForSeconds(5f);

        // Spawns the NPC after 5 seconds at the boss location
        if (deathSpawn != null)
        {
            Instantiate(deathSpawn, transform.position, Quaternion.identity);
        }

        // Destroy the boss object after 5 seconds
        Destroy(gameObject);
    }

    private void InitializeHealthBar()
    {
        GameObject healthBarInstance = Instantiate(healthBarPrefab, transform); // Instantiates the health bar prefab
        healthBar = healthBarInstance.GetComponentInChildren<Slider>(); // Gets the slider component from the health bar prefab

        //Set initial values 
        healthBar.maxValue = attributes.health;
        healthBar.value = currentHealth;

    }

    private void hitParticle()
    {
        if (hitEffect != null && hitPoint != null) // Here I check if the values are defined for the enemy
        {
            GameObject effect = Instantiate(hitEffect, hitPoint.position, Quaternion.identity); // I create new object at the hit point, indenity quaterion means no rotation
            Destroy(effect, 2.0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(patrolCenter, patrolSize);
    }
}
