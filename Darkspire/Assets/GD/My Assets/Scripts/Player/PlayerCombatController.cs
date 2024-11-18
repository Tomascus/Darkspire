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

    [Header("Combat Settings")]
    [SerializeField] private Collider attackCollider;
    [SerializeField] private float attackCooldown = 0.8f;
    private float lastAttackTime = 0;
    public float AttackCooldown => attackCooldown;

    private bool isAttacking = false;

    private void Awake()
    {
        inputs = GetComponent<PlayerControllerInputs>();
        animator = GetComponent<Animator>();
        movementController = GetComponent<PlayerMovementController>();
    }

    private void Update()
    {
        HandleAttackInput();
    }

    private void HandleAttackInput()
    {
        if (Time.time > lastAttackTime + attackCooldown && inputs.attack)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        lastAttackTime = Time.time;

        // Disable movement but keep rotation enabled
        isAttacking = true;
        movementController.SetMovementEnabled(false);

        animator.SetTrigger("Attack");

        StartCoroutine(ActivateAttackCollider());

        // Re-enable movement after the attack animation
        StartCoroutine(AllowMovement());
    }

    private IEnumerator ActivateAttackCollider()
    {
        attackCollider.enabled = true;

        // Time the collider stays active
        yield return new WaitForSeconds(0.1f);

        attackCollider.enabled = false;
    }

    private IEnumerator AllowMovement()
    {
        // Time the player has to wait before they can move again
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        movementController.SetMovementEnabled(true);
    }

}
