using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Constants")] [SerializeField] private CharacterController controller;

    [SerializeField] private float speed = 15f;
    [SerializeField] private float walkSpeed = 15f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float terminalVelocity = 9.8f;

    [Header("References")] [SerializeField]
    private LayerMask groundMask;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;

    [Header("Movement Settings")] [SerializeField]
    private float jumpHeight;

    [SerializeField] private float jumpTime;
    public Vector2 horizontalMovementInput;
    private InputMaster _controls;

    [Header("InitialValues")] private float _initialSpeed;

    private bool _isCrouching;

    [Header("Checks")] private bool _isGrounded;

    private bool _isHeadBlocked;
    private bool _isWalking;
    private Vector3 _jumpingForce;
    private Vector3 _jumpingVelocity;
    private Vector3 _movementVelocity;
    private float _playerGravity;

    public Controls CurrentInputs;

    public void Awake()
    {
        _controls = InputMaster.GetInstance();
        _controls.GroundMovement.HorizontalMovement.performed +=
            ctx => horizontalMovementInput = ctx.ReadValue<Vector2>();
        _controls.GroundMovement.Jump.started += _ => Jump();
        _controls.GroundMovement.Walk.started += _ => Walk();
        _controls.GroundMovement.Crouch.started += _ => Crouch();
        _controls.Enable();
        _initialSpeed = speed;
    }

    public void Update()
    {
        _isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.55f, groundMask);
        _isHeadBlocked = Physics.Raycast(ceilingCheck.position, Vector3.up, 0.55f, groundMask);
        CalculateControlsToSend();
        CalculateGroundMovement(CurrentInputs);
        controller.Move(_movementVelocity * Time.deltaTime);
        CalculateJump();
    }

    private void CalculateControlsToSend()
    {
        CurrentInputs.Walk = _isWalking;
        CurrentInputs.Crouch = _isCrouching;
        CurrentInputs.HorizontalMovement = horizontalMovementInput;
    }

    public Vector3 CalculateGroundMovement(Controls input)
    {
        if (_playerGravity > terminalVelocity && _jumpingVelocity.y < 0.1f) _playerGravity -= gravity * Time.deltaTime;
        if (_playerGravity < 0.01f && _isGrounded)
        {
            var horizontalMovement = input.HorizontalMovement.x * speed;
            var verticalMovement = input.HorizontalMovement.y * speed;
            _movementVelocity = new Vector3(horizontalMovement, 0f, verticalMovement);
            _movementVelocity = transform.TransformDirection(_movementVelocity);
            _playerGravity = -0.01f;
        }
        else
        {
            var horizontalMovement = input.HorizontalMovement.x * 8f * Time.deltaTime;
            var verticalMovement = input.HorizontalMovement.y * 8f * Time.deltaTime;
            var airVelocity = new Vector3(horizontalMovement, 0f, verticalMovement);
            airVelocity = transform.TransformDirection(airVelocity);
            if (airVelocity.magnitude < 0.1f)
                _movementVelocity += airVelocity;
        }

        _movementVelocity.y = _playerGravity;
        _movementVelocity += _jumpingVelocity;
        return _movementVelocity;
    }

    private void CalculateJump()
    {
        _jumpingVelocity = Vector3.SmoothDamp(_jumpingVelocity, Vector3.zero, ref _jumpingForce, jumpTime);
    }

    private void Jump()
    {
        if (!_isGrounded) return;
        CurrentInputs.Jump = true;
        _jumpingVelocity = Vector3.up * jumpHeight;
        _playerGravity = 0f;
    }

    private void Walk()
    {
        _isWalking = !_isWalking;
        if (_isWalking)
        {
            speed = walkSpeed;
            return;
        }

        speed = _initialSpeed;
    }

    private void Crouch()
    {
        _isCrouching = !_isCrouching;
    }
}