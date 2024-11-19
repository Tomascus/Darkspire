using DG.Tweening;
using InputSystem;
using System.Collections;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Player Controller References")]
    private PlayerControllerInputs inputs;
    private Animator animator;
    private PlayerMovementController movementController;
    private WeaponCollider weaponCollider;

    [Header("Combat Settings")]

    [SerializeField] private float attackCooldown = 0.8f;
    private float lastAttackTime = 0;
    public float AttackCooldown => attackCooldown;
    private bool isAttacking = false;

    // List of animations for player combo system - better to use a list for flexibility and scalability
    private readonly string[] attackAnimations = { "Attack1", "Attack2", "Attack3" };

    private void Awake()
    {
        inputs = GetComponent<PlayerControllerInputs>();
        animator = GetComponent<Animator>();
        movementController = GetComponent<PlayerMovementController>();
        weaponCollider = GetComponentInChildren<WeaponCollider>();
    }

    private void Update()
    {
        HandleAttackInput();
    }

    private void HandleAttackInput()
    {
        if (Time.time > lastAttackTime + attackCooldown && inputs.attack && !isAttacking)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        lastAttackTime = Time.time;

        // Disable movement but keep rotation enabled
        isAttacking = true;
        weaponCollider.SetAttacking(true); // Make the weapon collider register attacks
        movementController.SetMovementEnabled(false);

        // Choose a random attack animation trigger
        string randAttack = attackAnimations[Random.Range(0, attackAnimations.Length)];
        animator.SetTrigger(randAttack);

        // Reset hits on the weapon before next attack
        weaponCollider.ResetHits();

        // Re-enable movement after the attack animation
        StartCoroutine(AllowMovement());
    }

    private IEnumerator AllowMovement()
    {
        // Time the player has to wait before they can move again
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        weaponCollider.SetAttacking(false); // Stop the weapon collider from registering attacks
        movementController.SetMovementEnabled(true);
    }

}