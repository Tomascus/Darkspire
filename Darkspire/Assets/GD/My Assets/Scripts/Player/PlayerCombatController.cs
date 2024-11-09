using InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Player Controller References")]
    private PlayerControllerInputs inputs;
    private CharacterController characterController;
    private Animator animator;

    [SerializeField] private float attackCooldown = 0.8f;
    private int numberOfClicks = 0;
    private float lastClickedTime = 0;
    private float nextAttackTime = 0;

    private void Awake()
    {
        // Get the required components
        inputs = GetComponent<PlayerControllerInputs>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }


    // this combo script will need some upgrades as it does not work 100% correctly but the functionality is there
    // inspired by DanCS :How To Make a Combo System in Unity in Less than 7 Minutes [available at: https://www.youtube.com/watch?v=gHaJUNiItmQ&t=309s]
    private void Update()
    {
        ResetAttackState();
        HandleAttackInput();
    }

    private void ResetAttackState()
    {
        // check if current animation's passed time is greaten than 0.7f and then check if the current animation is attack1
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            //if so stop this animation 
            animator.SetBool("isAttacking1", false);
        }
        // check if current animation's passed time is greaten than 0.7f and then check if the current animation is attack2
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            //if so stop this animation
            animator.SetBool("isAttacking2", false);
            //this is the last combo attack so reset clicks to zero 
            numberOfClicks = 0;
        }

        //if the time passed since the last click is greater than the attack cooldown reset the number of clicks
        if (Time.time - lastClickedTime > attackCooldown)
        {
            numberOfClicks = 0;
        }
    }

    private void HandleAttackInput()
    {
        //if the time passed since the last attack is greater than the attack cooldown and the attack button is pressed do attack 
        if (Time.time > nextAttackTime && inputs.attack)
        {
            HandleAttacks();
        }
    }

    private void HandleAttacks()
    {
        // get time of the last click
        lastClickedTime = Time.time;
        //make sure that the new value of clicks stays between 0 and 2
        numberOfClicks = Mathf.Clamp(numberOfClicks + 1, 0, 2);

        // if the first click then play first animation 
        if (numberOfClicks == 1)
        {
            animator.SetBool("isAttacking1", true);
        }
        // if the second click then play second animation and stop the first one
        else if (numberOfClicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            animator.SetBool("isAttacking1", false);
            animator.SetBool("isAttacking2", true);
        }      
    }
}

