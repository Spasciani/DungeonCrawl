using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Walking speed of the player.")]
    public float walkSpeed = 5f;
    
    [Tooltip("Sprinting speed of the player.")]
    public float sprintSpeed = 8f;
    
    [Tooltip("Force applied when the player jumps.")]
    public float jumpForce = 10f;

    [Header("Ground Check Settings")]
    [Tooltip("Transform used to check if the player is grounded.")]
    public Transform groundCheck;        // Assign a child object positioned at the player's feet
    
    [Tooltip("Radius of the ground check circle.")]
    public float groundCheckRadius = 0.2f;
    
    [Tooltip("Layer(s) considered as ground.")]
    public LayerMask groundLayer;        // Assign the layer(s) considered as ground

    [Header("Slope Settings")]
    [Tooltip("Maximum slope angle (in degrees) the player can stand on without sliding.")]
    public float maxSlopeAngle = 45f;     // Maximum slope angle the player can stand on without sliding
    
    [Tooltip("Force applied to counteract sliding on steep slopes.")]
    public float slidingForce = 5f;       // Force applied to counteract sliding on steep slopes

    [Header("Damage Settings")]
    [Tooltip("Duration for which the player is in damage state.")]
    public float damageDuration = 0.5f;  // Duration for which the player is in damage state

    public Collider2D groundCheckCollider;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool canJump;
    private bool isSprinting = false;
    private bool isTakingDamage = false;

    public GameObject virtualCameraPrefab; // Assign in Inspector


    // Parameters for ground detection raycasting
    [Header("Ground Detection Raycast Settings")]
    [Tooltip("Offset for left ground detection ray.")]
    public float leftRayOffset = 0.1f;    // Adjust based on player's width
    
    [Tooltip("Offset for right ground detection ray.")]
    public float rightRayOffset = 0.1f;   // Adjust based on player's width

    void Start()
    {
        // Initialize Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on this GameObject!");
        }

        // Initialize Animator
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject!");
        }

        // Initialize SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing on this GameObject!");
        }

        // Check if GroundCheck is assigned
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck Transform is not assigned in the Inspector.");
        }

        // Initialize animator parameters if Animator is present
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsGrounded", true);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsTakingDamage", false);
        }
    }

    void Update()
    {
        HandleMovementInput();
        UpdateAnimatorParameters();
    }

    /// <summary>
    /// Handles player movement and actions based on input.
    /// </summary>
    void HandleMovementInput()
    {
        // Horizontal movement input
        float moveInput = Input.GetAxis("Horizontal");

        // Sprinting input (e.g., holding Left Shift)
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Determine current speed based on sprinting state
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Apply horizontal movement
        if (rb != null)
        {
            rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);
        }

        // Flip sprite based on movement direction
        if (spriteRenderer != null && moveInput != 0)
        {
            spriteRenderer.flipX = moveInput < 0;
        }

        // Jumping input
        if (Input.GetButtonDown("Jump") && isGrounded && !isTakingDamage)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset Y velocity for consistent jumps
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        canJump = false; // Prevent further jumps until grounded again
        isGrounded = false; // Ensure grounded state is reset

        // Trigger jumping animation
        if (animator != null)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsGrounded", false);
        }
    }

    /// <summary>
    /// Updates animator parameters based on player state.
    /// </summary>
    void UpdateAnimatorParameters()
    {
        if (animator == null) return;

        // Calculate absolute horizontal speed
        float horizontalSpeed = Mathf.Abs(rb.velocity.x);

        // Update 'Speed' parameter
        animator.SetFloat("Speed", horizontalSpeed);

        // Update 'IsGrounded' parameter
        animator.SetBool("IsGrounded", isGrounded);

        // Determine animation state based on speed
        if (horizontalSpeed > 4f)
        {
            // Sprinting
            animator.SetFloat("Speed", horizontalSpeed);
            // You can set additional parameters or triggers here if needed
        }
        else if (horizontalSpeed > 0.1f && horizontalSpeed <= 4f)
        {
            // Walking
            animator.SetFloat("Speed", horizontalSpeed);
            // You can set additional parameters or triggers here if needed
        }
        else
        {
            // Idle
            animator.SetFloat("Speed", 0f);
            // You can set additional parameters or triggers here if needed
        }
    }

    /// <summary>
    /// Performs a ground check using raycasting in FixedUpdate.
    /// </summary>
    void FixedUpdate()
    {
        GroundCheck();
    }

    /// <summary>
    /// Checks if the player is grounded using raycasting.
    /// </summary>
    void GroundCheck()
    {
        if (groundCheck == null) return;

        // Check for overlap with ground layer using Physics2D.OverlapCircle
       isGrounded = groundCheckCollider.IsTouchingLayers(groundLayer);

        if (isGrounded)
        {
            canJump = true; // Allow jumping again once grounded
        }

        // Update animator if necessary
        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
            if (isGrounded)
            {
                animator.SetBool("IsJumping", false);
            }
        }
    }

    /// <summary>
    /// Handles collision events to detect slope angles and prevent sliding on steep slopes.
    /// </summary>
    /// <param name="collision">Collision data.</param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Only process collisions with ground layer
        if (((1 << collision.gameObject.layer) & groundLayer) == 0)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Calculate slope angle
            float slopeAngle = Vector2.Angle(contact.normal, Vector2.up);

            if (slopeAngle > maxSlopeAngle)
            {
                // Determine the direction to apply the sliding force
                Vector2 slidingDirection = new Vector2(contact.normal.x, -contact.normal.y).normalized;

                // Apply a force opposite to the slope to prevent sliding
                rb.AddForce(slidingDirection * slidingForce, ForceMode2D.Force);
            }
        }
    }

    /// <summary>
    /// Handles collision events to determine if the player is grounded.
    /// </summary>
    /// <param name="collision">Collision data.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // No need to handle ground detection here as it's managed by raycasting
        // This method can be removed or kept for other collision-related logic
    }

    /// <summary>
    /// Call this method to trigger the damage state.
    /// For example, when colliding with an enemy.
    /// </summary>
    public void TakeDamage()
    {
        if (!isTakingDamage)
        {
            StartCoroutine(DamageRoutine());
        }
    }

    /// <summary>
    /// Coroutine to handle damage state duration.
    /// </summary>
    /// <returns>IEnumerator for coroutine.</returns>
    IEnumerator DamageRoutine()
    {
        isTakingDamage = true;

        // Trigger damage animation
        if (animator != null)
        {
            animator.SetBool("IsTakingDamage", true);
        }

        // Implement damage logic here (e.g., reduce health)

        // Wait for the damage duration
        yield return new WaitForSeconds(damageDuration);

        isTakingDamage = false;

        // Reset damage animation
        if (animator != null)
        {
            animator.SetBool("IsTakingDamage", false);
        }
    }

    /// <summary>
    /// Visualizes the ground check area in the Scene view.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        // Draw additional gizmos for raycast origins
        if (groundCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(groundCheck.position + Vector3.left * leftRayOffset, groundCheck.position + Vector3.left * leftRayOffset + Vector3.down * (groundCheckRadius + 0.1f));
            Gizmos.DrawLine(groundCheck.position + Vector3.right * rightRayOffset, groundCheck.position + Vector3.right * rightRayOffset + Vector3.down * (groundCheckRadius + 0.1f));
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsLocalPlayer)
        {
            // Instantiate the Cinemachine Virtual Camera
            GameObject vCamGameObject = Instantiate(virtualCameraPrefab);

            // Get the CinemachineVirtualCamera component
            CinemachineVirtualCamera vCam = vCamGameObject.GetComponent<CinemachineVirtualCamera>();

            if (vCam != null)
            {
                // Set the camera to follow and look at this player's transform
                vCam.Follow = transform;
                vCam.LookAt = transform;
            }
            else
            {
                Debug.LogError("CinemachineVirtualCamera component not found on the instantiated prefab.");
            }
        }
    }
}
