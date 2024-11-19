using InputSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerMovementController : MonoBehaviour
{
    #region Fields

    [Header("Player Controller References")]
    private PlayerControllerInputs inputs;
    private CharacterController characterController;
    private Animator animator;


    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    private bool canMove = true;

    // Useful for different weight limits - light, medium, heavy builds ingame
    [Tooltip("Rotation Speed")]
    [Range(0.0f, 0.3f)]
    [SerializeField] public float rotationSmoothTime = 0.12f;

    // Dodge Settings
    [Header("Dodge Settings")]
    private bool canDodge = true; // Check if player can dodge

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
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

        if (canMove)
        {
            // Determine speed based on sprint state - (if/else)
            float speed = inputs.sprint ? sprintSpeed : moveSpeed;
            Vector3 moveVelocity = targetDirection * speed;

            // Moves the player based velocity, time and direction as well as smooths out the movement
            currentDirection = Vector3.SmoothDamp(currentDirection, moveVelocity, ref smoothMoveVelocity, 0.1f);
            characterController.Move(currentDirection * Time.deltaTime);

            if (inputs.dodge && canDodge && GroundCheck())
            {
                speed = moveSpeed;
                StartDodge();
            }

        }
    }

    // Used to enable or disable movement during cutscenes, dialogue, combat, etc.
    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
    }

    private void UpdateAnimations()
    {
        // Set animation parameters based on player state
        animator.SetBool("isWalking", currentDirection.magnitude > 0.1f && !inputs.sprint);
        animator.SetBool("isSprinting", inputs.sprint);
    }

    // Rotate the player based on input
    private void HandleRotation()
{
    // Ignore rotation if no meaningful input
    if (inputs.move.sqrMagnitude < 0.1f) return;

    // Calculates the target direction based on input
    Vector3 inputDirection = new Vector3(inputs.move.x, 0, inputs.move.y).normalized;

    // Rotates the player towards the target direction if there is valid input
    if (inputDirection != Vector3.zero)
    {
        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
        
        // Smooth out the rotation using Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
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
