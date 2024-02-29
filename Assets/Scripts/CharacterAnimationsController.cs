using System;
using UnityEngine;

public class CharacterAnimationsController : MonoBehaviour
{
    private Animator _animator;
    
    private static readonly int _horizontalMovementHash = Animator.StringToHash("HorizontalMovement");
    private static readonly int _verticalMovementHash = Animator.StringToHash("VerticalMovement");
    private static readonly int _speedhash = Animator.StringToHash("Speed");

    public float speed;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleMovementAnimations();
    }
    private void HandleMovementAnimations()
    {
        _animator.SetFloat(_speedhash, speed);
        if(InputManager.Instance.MovementInput != Vector2.zero)
        {
            _animator.SetFloat(_horizontalMovementHash, InputManager.Instance.MovementInput.x);
            _animator.SetFloat(_verticalMovementHash, InputManager.Instance.MovementInput.y);
        }
    }
}
