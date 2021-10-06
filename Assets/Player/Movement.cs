using System;
using System.Buffers;
using System.IO;
using Unity.Collections;
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
    public Vector2 horizontalMovementInput;
    private Vector3 movementVelocity;
    public float playerGravity;
    public Vector3 jumpingForce;
    public Vector3 jumpingVelocity;

    [Header("Checks")]
    public bool isGrounded;
    private bool isCrouching;
    private bool isWalking;
    public bool isJumping;
    private bool isHeadBlocked;

    [Header("InitialValues")]
    private float initialSpeed;
    private Vector3 initialPlayerScale;

    public Controls CurentInputs;

    public void Awake()
    {
        controls = InputMaster.GetInstance();
        controls.GroundMovement.HorizontalMovement.performed += ctx => horizontalMovementInput = ctx.ReadValue<Vector2>();
        controls.GroundMovement.Jump.started += _ => Jump();
        controls.GroundMovement.Walk.started += _ => Walk();
        controls.GroundMovement.Crouch.started += _ => Crouch();
        controls.Enable();
        initialPlayerScale = transform.localScale;
        initialSpeed = speed;
    }

    public void Update()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.55f, groundMask);
        isHeadBlocked = Physics.CheckSphere(ceilingCheck.position, 0.55f, groundMask);
        CalculateControlsToSend();
        CalculateGroundMovement(CurentInputs);
        controller.Move(movementVelocity * Time.deltaTime);
        CalculateJump();
    }

    public void CalculateControlsToSend()
    {
        CurentInputs.walk = isWalking;
        CurentInputs.crouch = isCrouching;
        CurentInputs.horizontalMovement = horizontalMovementInput;
    }

    public Vector3 CalculateGroundMovement(Controls input)
    {
        if (playerGravity > terminalVelocity && jumpingVelocity.y < 0.1f)
        {
            playerGravity -= gravity * Time.deltaTime;
        }
        if (playerGravity < 0.01f && isGrounded)
        {
            var horizontalMovement = input.horizontalMovement.x * speed;
            var verticalMovement = input.horizontalMovement.y * speed;
            movementVelocity = new Vector3(horizontalMovement, 0f, verticalMovement);
            movementVelocity = transform.TransformDirection(movementVelocity);
            playerGravity = -0.01f;
        }
        else
        {
            var horizontalMovement = input.horizontalMovement.x * 8f * Time.deltaTime;
            var verticalMovement = input.horizontalMovement.y * 8f * Time.deltaTime;
            var airVelocity = new Vector3(horizontalMovement, 0f, verticalMovement);
            airVelocity = transform.TransformDirection(airVelocity);
            if (airVelocity.magnitude < 0.1f)
                movementVelocity += airVelocity;
        }
        movementVelocity.y = playerGravity;
        movementVelocity += jumpingVelocity;
        return movementVelocity;
    }

    //private void CalculateGroundMovement()
    //{
    //    if (playerGravity > terminalVelocity && jumpingVelocity.y < 0.1f)
    //    {
    //        playerGravity -= gravity * Time.deltaTime;
    //    }
    //    if (playerGravity < 0.01f && isGrounded)
    //    {
    //        var horizontalMovement = horizontalMovementInput.x * speed;
    //        var verticalMovement = horizontalMovementInput.y * speed;
    //        movementVelocity = new Vector3(horizontalMovement, 0f, verticalMovement);
    //        movementVelocity = transform.TransformDirection(movementVelocity);
    //        playerGravity = -0.01f;
    //    }
    //    else
    //    {
    //        var horizontalMovement = horizontalMovementInput.x * 8f * Time.deltaTime;
    //        var verticalMovement = horizontalMovementInput.y * 8f * Time.deltaTime;
    //        var airVelocity = new Vector3(horizontalMovement, 0f, verticalMovement);
    //        airVelocity = transform.TransformDirection(airVelocity);
    //        if(airVelocity.magnitude < 0.1f)
    //            movementVelocity += airVelocity;
    //    }
    //    movementVelocity.y = playerGravity;
    //    movementVelocity += jumpingVelocity;
    //    controller.Move(movementVelocity * Time.deltaTime);
    //}

    private void CalculateJump()
    {
        jumpingVelocity = Vector3.SmoothDamp(jumpingVelocity, Vector3.zero, ref jumpingForce, jumpTime);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            CurentInputs.jump = true;
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