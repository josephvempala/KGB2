using System.Buffers;
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
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    private InputMaster controls;

    [Header("Movement Settings")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpTime;
    private Vector2 horizontalMovementInput;
    private Vector3 movementVelocity;
    public float playerGravity;
    public Vector3 jumpingForce;
    public Vector3 jumpingVelocity;

    [Header("Checks")]
    public bool isGrounded;
    private bool isCrouching;
    private bool isWalking;
    private bool jump;
    private bool isHeadBlocked;

    [Header("InitialValues")]
    private float initialSpeed;
    private Vector3 initialPlayerScale;

    [Header("Network")]
    ArrayPool<byte> NetworkControlsArrayPool;

    public void Awake()
    {
        NetworkControlsArrayPool = ArrayPool<byte>.Create();
        controls = InputMaster.GetInstance();
        controls.GroundMovement.HorizontalMovement.performed += ctx => horizontalMovementInput = ctx.ReadValue<Vector2>();
        controls.GroundMovement.Jump.performed += _ => jump = !jump;
        controls.GroundMovement.Walk.performed += _ => Walk();
        controls.GroundMovement.Crouch.performed += _ => Crouch();
        controls.Enable();
        initialPlayerScale = transform.localScale;
        initialSpeed = speed;
    }

    public void Update()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.55f, groundMask);
        isHeadBlocked = Physics.CheckSphere(ceilingCheck.position, 0.55f, groundMask);
        CalculateGroundMovement();
        CalculateJump();
        Jump();
    }

    public void FixedUpdate()
    {
        byte[] NetworkControlsArray = NetworkControlsArrayPool.Rent(12);
        using(Packet p = new Packet(NetworkControlsArray))
        {
            p.Write(horizontalMovementInput);
            p.Write(jump);
            p.Write(isWalking);
            p.Write(isCrouching);

            ClientSend.SendControls(p.ToArray());
        }
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
        if (isGrounded && jump)
        {
            jump = !jump;
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
        else
        {
            speed = initialSpeed;
        }
    }

    public void Crouch()
    {
        isCrouching = !isCrouching;
        if (isCrouching)
        {
            Vector3 crouchedHeight = initialPlayerScale;
            crouchedHeight.y /= 2f;
            transform.localScale = crouchedHeight;
            return;
        }
        else
        {
            transform.localScale = initialPlayerScale;
        }
    }
}