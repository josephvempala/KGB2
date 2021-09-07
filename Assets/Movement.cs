using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Constants")]
    [SerializeField] private CharacterController controller;
    [SerializeField] public float speed = 15f;
    [SerializeField] public float walkSpeed = 15f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float terminalVelocity = 9.8f;

    [Header("References")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    private InputMaster controls;

    [Header("Camera Settings")]
    public float camClampXMax = 90f;
    public float camClampXMin = -90f;
    private float mouseX;
    private float mouseY;
    private Vector3 CameraRotation;
    private Vector3 CharacterRotation;
    public float MouseSensitivityX;
    public float MouseSensitivityY;
    public bool InvertMouseX;
    public bool InvertMouseY;

    [Header("Movement Settings")]
    private Vector2 horizontalMovementInput;
    private Vector3 movementVelocity;
    public float playerGravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpTime;
    public Vector3 jumpingForce;
    public Vector3 jumpingVelocity;
    private Vector3 airmovement;

    [Header("Checks")]
    public bool isGrounded;
    private bool isCrouching;
    private bool isWalking;
    private bool isHeadBlocked;

    [Header("InitialValues")]
    private float initialSpeed;
    private Vector3 initialPlayerScale;

    public void Awake()
    {
        controls = new InputMaster();
        controls.GroundMovement.HorizontalMovement.performed += ctx => horizontalMovementInput = ctx.ReadValue<Vector2>();
        controls.GroundMovement.Jump.performed += _ => Jump();
        controls.GroundMovement.MouseX.performed += ctx => mouseX = ctx.ReadValue<float>();
        controls.GroundMovement.MouseY.performed += ctx => mouseY = ctx.ReadValue<float>();
        controls.GroundMovement.Walk.performed += _ => Walk();
        controls.GroundMovement.Crouch.performed += _ => Crouch();
        controls.Enable();
        CameraRotation = cameraHolder.localRotation.eulerAngles;
        CharacterRotation = transform.localRotation.eulerAngles;
        initialPlayerScale = transform.localScale;
        initialSpeed = speed;
    }

    public void Update()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.55f, groundMask);
        isHeadBlocked = Physics.CheckSphere(ceilingCheck.position, 0.55f, groundMask);
        CalculateMouseLook();
        CalculateGroundMovement();
        CalculateJump();
    }

    public void FixedUpdate()
    {
        
    }

    public void CalculateMouseLook()
    {
        CameraRotation.x += MouseSensitivityY * (InvertMouseY ? mouseY : -mouseY) * Time.deltaTime;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x, camClampXMin, camClampXMax);
        CharacterRotation.y += MouseSensitivityX * (InvertMouseX ? -mouseX : mouseX) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(CharacterRotation);
        cameraHolder.localRotation = Quaternion.Euler(CameraRotation);
    }

    private void CalculateGroundMovement()
    {
        

        if (playerGravity > terminalVelocity && jumpingVelocity.y < 0.1f)
        {
            playerGravity -= gravity * Time.deltaTime;
        }
        if (playerGravity < 0.01f && isGrounded)
        {
            var horizontalMovement = horizontalMovementInput.x * speed;
            var verticalMovement = horizontalMovementInput.y * speed;
            movementVelocity = new Vector3(horizontalMovement, 0f, verticalMovement);
            movementVelocity = transform.TransformDirection(movementVelocity);
            playerGravity = -0.01f;
        }
        else
        {
            var horizontalMovement = horizontalMovementInput.x * 8f * Time.deltaTime;
            var verticalMovement = horizontalMovementInput.y * 8f * Time.deltaTime;
            var airVelocity = new Vector3(horizontalMovement, 0f, verticalMovement);
            airVelocity = transform.TransformDirection(airVelocity);
            if(airVelocity.magnitude < 0.1f)
                movementVelocity += airVelocity;
        }
        movementVelocity.y = playerGravity;
        movementVelocity += jumpingVelocity;
        controller.Move(movementVelocity * Time.deltaTime);
    }

    private void CalculateJump()
    {
        jumpingVelocity = Vector3.SmoothDamp(jumpingVelocity, Vector3.zero, ref jumpingForce, jumpTime);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            jumpingVelocity = Vector3.up * jumpHeight;
            playerGravity = 0f;
        }
    }

    public void Walk()
    {
        isWalking = !isWalking;
        if (isWalking)
        {
            speed = walkSpeed;
            return;
        }
        speed = initialSpeed;
    }

    public void Crouch()
    {
        isCrouching = !isCrouching;
        if (isCrouching)
        {
            Vector3 crouchedHeight = initialPlayerScale;
            crouchedHeight.y /= 2f;
            speed = walkSpeed;
            transform.localScale = crouchedHeight;
            return;
        }
        speed = initialSpeed;
        transform.localScale = initialPlayerScale;
    }
}