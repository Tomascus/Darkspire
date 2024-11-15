using InputSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementController : MonoBehaviour
{
    #region Feilds

    [Header("Player Controller References")]
    private PlayerControllerInputs inputs;
    private CharacterController characterController;
    private Animator animator;


    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;

    // Useful for different weight limits - light, medium, heavy builds ingame
    [Tooltip("Rotation Speed")]
    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.12f;

    // Dodge Settings
    [Header("Dodge Settings")]
    //UPDATE FUNCTIONALITY (COOLDOWN)
    public float dodgeCooldown = 0.8f; // Cooldown timer for player to wait until next available dodge
    private bool canDodge = true; // Check if player can dodge

    [Header("Gravity and Ground Check")]
    public float gravity = -9.81f;
    private Vector3 playerVelocity; // Store velocity for gravity - Changes velocity over time when falling


    // These are used for the movement calculations
    private Vector3 currentDirection;
    private Vector3 smoothMoveVelocity;
    private Vector3 targetDirection;

    #endregion
    #region Unity Starter Methods

    private void Awake()
    {
        inputs = GetComponent<PlayerControllerInputs>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        ApplyGravity();
    } 
    #endregion

    private bool GroundCheck()
    {
          return characterController.isGrounded; // Uses built-in CharacterController method to check if player is grounded
    }

    private void ApplyGravity()
    {
        if (!GroundCheck())
        {
            playerVelocity.y += gravity * Time.fixedDeltaTime;
        }

        characterController.Move(playerVelocity * Time.fixedDeltaTime); // Apply the velocity to the player 
    }

    private void HandleMovement()
    {
        // Move the character based on input
        Vector2 moveInput = inputs.move;
        targetDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // Determine speed based on sprint state - (if/else)
        float speed = inputs.sprint ? sprintSpeed : moveSpeed;

        // Move the character - SmoothDamp is used to smooth out the movement
        currentDirection = Vector3.SmoothDamp(currentDirection, targetDirection * speed, ref smoothMoveVelocity, 0.1f);
        characterController.Move(currentDirection * Time.deltaTime);

        if (inputs.dodge && canDodge && GroundCheck())
        {
            speed = moveSpeed;
            StartDodge();
        }

    }

    private void UpdateAnimations()
    {
        // Set animation parameters based on player state
        animator.SetBool("isWalking", currentDirection.magnitude > 0.1f && !inputs.sprint);
        animator.SetBool("isSprinting", inputs.sprint);
    }

    // Rotate the player based on the movement direction - Slerp is used to smooth out the rotation
    private void HandleRotation()
    {
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothTime);
        }
    }

    // Dodge using root motion (triggered by animation)
    private void StartDodge()
    {
        
       
        // Enable root motion for dodge and trigger dodge animation
        animator.applyRootMotion = true;
        animator.SetBool("isDodging", true);
        canDodge = false;

        // Animation Event
        Invoke("EndDodge", animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void EndDodge()
    {
        
        // Reset dodge animation, disable root motion
        animator.SetBool("isDodging", false);
        animator.applyRootMotion = false;
        canDodge = true;

    }

}
