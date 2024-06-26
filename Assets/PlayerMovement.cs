using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Movement and animation settings
    public Camera playerCamera;
    public Animator playerAnim; // Animator component for animations
    public float walkSpeed = 6f; // Walking speed
    public float runSpeed = 12f; // Running speed
    public float jumpPower = 7f; // Jump power
    public float gravity = 10f; // Gravity force
    public float lookSpeed = 2f; // Mouse look speed
    public float lookXLimit = 45f; // Vertical look angle limit
    public float defaultHeight = 2f; // Default character height
    public float crouchHeight = 1f; // Crouch height
    public float crouchSpeed = 3f; // Crouch speed

    // Private state variables
    private Vector3 moveDirection = Vector3.zero; // Current movement direction
    private float rotationX = 0; // Vertical camera rotation
    private CharacterController characterController; // CharacterController component
    private bool isWalking = false; // Add definition and initialization of isWalking

    void Start()
    {
        // Initialize character controller and cursor settings
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Calculate forward and right directions for movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Check if running and calculate movement speed
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal");

        // Preserve vertical movement and update move direction
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Handle jumping
        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
            playerAnim.SetTrigger("jump"); // Trigger jump animation
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity when not grounded
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Handle crouching
        if (Input.GetKey(KeyCode.R))
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);

        // Handle animations based on movement
        HandleAnimations(curSpeedX, curSpeedY, isRunning);

        // Handle camera control
        HandleCameraControl();
    }

    void HandleAnimations(float curSpeedX, float curSpeedY, bool isRunning)
    {
        // Determine if the player is walking
        isWalking = curSpeedX != 0 || curSpeedY != 0;

        // Set animation triggers based on movement
        if (isWalking)
        {
            // Determine if running or walking forward
            if (isRunning)
            {
                playerAnim.SetTrigger("run");
                playerAnim.ResetTrigger("walk");
            }
            else
            {
                playerAnim.SetTrigger("walk");
                playerAnim.ResetTrigger("idle");
            }
        }
        else
        {
            // If not walking or running, set to idle state
            playerAnim.ResetTrigger("walk");
            playerAnim.ResetTrigger("run");
            playerAnim.SetTrigger("idle");
        }

        // Handle backward walking animation
        if (curSpeedX < 0)
        {
            playerAnim.SetTrigger("jogBack");
        }
        else
        {
            playerAnim.ResetTrigger("jogBack");
        }
    }

    // Handle camera control
    void HandleCameraControl()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
