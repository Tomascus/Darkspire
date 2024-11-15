using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Attributes")]
    public EnemyAttributes attributes; // ScriptableObject for enemy stats
    private float currentHealth;

    [Header("Detection")]
    public Transform player;
    private bool isPlayerDetected = false;
    private bool inAttackRange = false;

    [Header("Combat Settings")]
    [SerializeField] private float attackCooldown = 2f; 
    private float lastAttackTime = 0f;
    [SerializeField] private float attackDamage; 

    [Header("AI Navigation & Animator")]
    private NavMeshAgent agent;
    private Animator animator;

    private bool isDead = false; 

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        getEnemyStats();
    }

    private void Update()
    {
        if (isDead) return;

        DetectPlayer();

        if (isPlayerDetected)
        {
            RotateTowardsPlayer();
            ChasePlayer();

            if (inAttackRange)
            {
                PerformAttack();
            }
            else
            {
                ChasePlayer();
            }
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
    }

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


        if (isPlayerDetected)
        {
            agent.destination = player.position; // Set destination to the player's position if detected
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true; // Enemy stops moving if player is not detected
        }
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
        // Move towards the player if detected and not in attack range
        if (isPlayerDetected && !inAttackRange)
        {
            agent.destination = player.position;
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

}
