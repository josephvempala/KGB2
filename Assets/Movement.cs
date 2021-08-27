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
    private Vector3 verticalVelocity;
    [SerializeField] private float jumpHeight;

    [Header("Checks")]
    private bool isGrounded;
    private bool jump;
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
        controls.GroundMovement.Jump.performed += _ => jump = !jump;
        controls.GroundMovement.MouseX.performed += ctx => mouseX = ctx.ReadValue<float>();
        controls.GroundMovement.MouseY.performed += ctx => mouseY = ctx.ReadValue<float>();
        controls.GroundMovement.Walk.performed += _ => isWalking = !isWalking;
        controls.GroundMovement.Crouch.performed += _ => isCrouching = !isCrouching;
        controls.Enable();
        CameraRotation = cameraHolder.localRotation.eulerAngles;
        CharacterRotation = transform.localRotation.eulerAngles;
    }

    public void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.55f, groundMask);
        isHeadBlocked = Physics.CheckSphere(ceilingCheck.position, 0.55f, groundMask);
        CalculateMouseLook();
        CalculateMovement();
    }


    public void CalculateMouseLook()
    {
        CameraRotation.x += MouseSensitivityY * (InvertMouseY ? mouseY : -mouseY) * Time.deltaTime;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x, camClampXMin, camClampXMax);
        CharacterRotation.y += MouseSensitivityX * (InvertMouseX ? -mouseX : mouseX) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(CharacterRotation);
        cameraHolder.localRotation = Quaternion.Euler(CameraRotation);
    }

    private void CalculateMovement()
    {
        Vector3 horizontalVelocity = new Vector3(horizontalMovementInput.x, 0f, horizontalMovementInput.y) * speed;
        horizontalVelocity = transform.TransformDirection(horizontalVelocity);
        if (isGrounded)
        {
            verticalVelocity.x = 0f;
            verticalVelocity.y = 0f;
            verticalVelocity.z = 0f;
            controller.Move(horizontalVelocity * Time.deltaTime);
        }
        CalculateJump(horizontalVelocity);
    }

    private void CalculateJump(Vector3 horizontalVelocity)
    {
        if (jump)
        {
            verticalVelocity.x = horizontalVelocity.x;
            verticalVelocity.z = horizontalVelocity.z;
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(2f * jumpHeight * gravity);
            }
            jump = false;
        }
        verticalVelocity.x += horizontalVelocity.x * 0.001f;
        verticalVelocity.z += horizontalVelocity.z * 0.001f;
        verticalVelocity.y -= gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    public void Walk()
    {
        if (isWalking)
        {
            speed = walkSpeed;
            return;
        }
        speed = initialSpeed;
    }

    public void Crouch()
    {
        if (isCrouching)
        {
            Vector3 crouchedHeight = initialPlayerScale;
            crouchedHeight.y /= 2;
            speed = walkSpeed;
            transform.localScale = crouchedHeight;
            return;
        }
        speed = initialSpeed;
        transform.localScale = initialPlayerScale;
    }
}