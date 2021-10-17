using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator _animator;
    private int _isMovingHash;
    private Movement _movement;
    private int _moveXHash;
    private int _moveYHash;
    private int _crouchHash;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _movement = GetComponent<Movement>();
        _moveXHash = Animator.StringToHash("MoveX");
        _moveYHash = Animator.StringToHash("MoveY");
        _isMovingHash = Animator.StringToHash("IsMoving");
        _crouchHash = Animator.StringToHash("IsCrouched");
    }

    private void Update()
    {
        CheckIsMoving();
        Run();
        Crouch();
    }

    private void CheckIsMoving()
    {
        if (_movement.horizontalMovementInput.x == 0f && _movement.horizontalMovementInput.y == 0f)
        {
            _animator.SetBool(_isMovingHash, false);
            return;
        }

        _animator.SetBool(_isMovingHash, true);
    }

    private void Run()
    {
        if (!_animator.GetBool(_isMovingHash)) return;
        _animator.SetFloat(_moveXHash, _movement.horizontalMovementInput.x);
        _animator.SetFloat(_moveYHash, _movement.horizontalMovementInput.y);
    }

    private void Crouch()
    {
        if (_movement.CurrentInputs.Crouch)
        {
            _animator.SetBool(_crouchHash, true);
            return;
        }

        _animator.SetBool(_crouchHash, false);
    }
}