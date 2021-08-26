using UnityEngine;
using static Models;

public class Movement : MonoBehaviour
{
    private InputMaster controls;
    private float initialSpeed;
    private Vector3 initialPlayerScale;

    [SerializeField] private CharacterController controller;
    [SerializeField] public float speed = 15f;
    [SerializeField] private float jumpHeight = 30f;
    [SerializeField] private float gravity = -9.8f;

    [Header("References")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private LayerMask ground;

    [Header("Settings")]
    public PlayerSettings playerSettings;
    public float camClampXMax = 90f;
    public float camClampXMin = -90f;
    private float mouseX;
    private float mouseY;
    private Vector3 playerGravity;
    private Vector3 playerVelocity;
    private Vector3 CameraRotation;
    private Vector3 CharacterRotation;
    private Vector2 horizontalMovement;

    [Header("Checks")]
    private bool isGrounded;
    private bool isHeadBlocked;
    private bool isCrouching;
    private bool isWalking;
    private bool jump;

    public void Awake()
    {
        controls = new InputMaster();
        controls.GroundMovement.HorizontalMovement.performed += ctx => horizontalMovement = ctx.ReadValue<Vector2>();
        controls.GroundMovement.Jump.performed += _ => jump = true;
        controls.GroundMovement.MouseX.performed += ctx => mouseX = ctx.ReadValue<float>();
        controls.GroundMovement.MouseY.performed += ctx => mouseY = ctx.ReadValue<float>();
        controls.GroundMovement.Walk.performed += _ => isWalking = !isWalking;
        controls.GroundMovement.Crouch.performed += _ => isCrouching = !isCrouching;
        controls.Enable();
        initialSpeed = speed;
        initialPlayerScale = transform.localScale;
        CameraRotation = cameraHolder.localRotation.eulerAngles;
        CharacterRotation = transform.localRotation.eulerAngles;
    }

    public void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.55f, ground);
        isHeadBlocked = Physics.CheckSphere(ceilingCheck.position, 0.1f, ground);
        CalculateMovement();
        CalculateMouseLook();
        Jump();
        Crouch();
        Walk();
    }

    public void CalculateMovement()
    {
        playerVelocity = (transform.right * horizontalMovement.x + transform.forward * horizontalMovement.y) * speed;
        if (isGrounded)
        {
            playerGravity = Vector3.zero;
            speed = initialSpeed;
            controller.Move(playerVelocity * Time.deltaTime);
        }
        if (!isGrounded)
        {
            if (playerGravity.x < 5f && playerGravity.x > -5f && playerGravity.z < 5f && playerGravity.z > -5f)
            {
                playerGravity += (transform.right * horizontalMovement.x + transform.forward * horizontalMovement.y) * 0.1f;
            }

            playerGravity.y += gravity * Time.deltaTime;
            controller.Move(playerGravity * Time.deltaTime);
            speed = 7f;
        }
    }

    public void CalculateMouseLook()
    {
        CameraRotation.x += playerSettings.MouseSensitivityY * (playerSettings.InvertMouseY ? mouseY : -mouseY) * Time.deltaTime;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x, camClampXMin, camClampXMax);
        CharacterRotation.y += playerSettings.MouseSensitivityX * (playerSettings.InvertMouseX ? -mouseX : mouseX) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(CharacterRotation);
        cameraHolder.localRotation = Quaternion.Euler(CameraRotation);
    }

    public void Jump()
    {
        if (jump && isGrounded)
        {
            jump = false;
            playerGravity = playerVelocity;
            playerGravity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            controller.Move(playerGravity * Time.deltaTime);
        }
    }

    public void Walk()
    {
        if (isWalking)
        {
            speed /= 2;
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
            transform.localScale = crouchedHeight;
            return;
        }
        transform.localScale = initialPlayerScale;
    }
}