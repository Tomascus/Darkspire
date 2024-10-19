using InputSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementController : MonoBehaviour
{
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


    // Audio Settings
    public AudioClip[] footstepAudioClips; // For Walking, Running, etc.
    public AudioClip dodgeAudioClip;
    [Range(0, 1)] public float footstepAudioVolume = 0.5f;


    [Header("Gravity and Ground Check")]
    public float gravity = -9.81f;
    public float groundedOffset = -0.14f; // Offset for ground check - EXPERIMENT WITH THIS VALUE!
    public float groundedRadius = 0.5f; // Radius of the overlap sphere 
    public LayerMask groundLayers; // Specifies which counts as ground layer - Flexible for future iterations
    private bool isGrounded;
    private Vector3 playerVelocity; // Store velocity for gravity - Changes velocity over time when falling


    // These are used for the movement calculations
    private Vector3 currentDirection;
    private Vector3 smoothMoveVelocity;
    private Vector3 targetDirection;

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
        PlayFootsteps();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        ApplyGravity();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position + Vector3.up * groundedOffset, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore); // QueryTriggerInteraction ensures that it only collides with solid objects (not triggers)
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f; // Reset the velocity when grounded
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
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

        if (inputs.dodge && canDodge && isGrounded)
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

        PlayDodgeSound();

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

    private void PlayFootsteps()
    {
        // Simple check to play footsteps - adjust the condition as needed
        if (characterController.isGrounded && currentDirection.magnitude > 0.1f)
        {
            // ADD LOGIC FOR PLAYING FOOTSTEPS 
            PlayFootstepSound();

        }
    }

    private void PlayFootstepSound()
    {
        if (footstepAudioClips.Length > 0)
        {
            int index = Random.Range(0, footstepAudioClips.Length);
            AudioSource.PlayClipAtPoint(footstepAudioClips[index], transform.position, footstepAudioVolume);
        }
    }

    private void PlayDodgeSound()
    {
        if (dodgeAudioClip != null)
        {
            AudioSource.PlayClipAtPoint(dodgeAudioClip, transform.position, footstepAudioVolume);
        }
    }

}
