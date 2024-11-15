using DG.Tweening;
using InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Player Controller References")]
    private PlayerControllerInputs inputs;
    private Animator animator;

    [SerializeField] private float attackCooldown = 0.8f;
    private float lastAttackTime = 0;
    public float AttackCooldown => attackCooldown;

    private void Awake()
    {
        // Get the required components
        inputs = GetComponent<PlayerControllerInputs>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAttackInput();
    }

    private void HandleAttackInput()
    {
        // If the time passed since the last attack is greater than the attack cooldown and the attack button is pressed, do attack
        if (Time.time > lastAttackTime + attackCooldown && inputs.attack)
        {   
                PerformAttack();
        }
    }

    private void PerformAttack()
    {
        // Update the last attack time to the current time
        lastAttackTime = Time.time;
        

        // Play the attack animation
        animator.SetTrigger("Attack");
        SoundManager.PlaySound(SoundType.SWING);
    }
}
