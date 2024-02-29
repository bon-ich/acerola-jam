using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private CharacterController _characterController;
    private PlayerAnimationsController _animationsController;
    private InputManager _inputManager;
    private Vector3 _movementDirection;
    private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    private float _velocity;
    private float _gravityMultiplier = 1f;
    private float _currentSpeed;
    private readonly float _gravity = -9.81f;

    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _fallMultiplier = 3f;
    public bool CanMove = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animationsController = GetComponent<PlayerAnimationsController>();
        _inputManager = InputManager.Instance;
        _camera = Camera.main;
        _gravityMultiplier = _fallMultiplier;
        _currentSpeed = _speed;
    }
    void Update()
    {
        ApplyGravity();
        ApplyCameraBasedRotation();

    }

    private void FixedUpdate()
    {
        ApplyMovementRelativeToCamera();
    }

    private void ApplyMovement()
    {
        if (!CanMove)
            return;

        _movementDirection = new Vector3(_inputManager.MovementInput.x, _velocity, _inputManager.MovementInput.y);
        _characterController.Move(_movementDirection * (_currentSpeed * Time.deltaTime));
    }
    private void ApplyMovementRelativeToCamera()
    {
        if (!CanMove)
            return;

        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardRelativeVerticalInput = _inputManager.MovementInput.y * forward;
        Vector3 rightRelativeVerticalInput = _inputManager.MovementInput.x * right;

        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;
        _animationsController.speed = cameraRelativeMovement.sqrMagnitude;
        cameraRelativeMovement.y = _velocity;
        _characterController.Move(cameraRelativeMovement * (_speed * 0.01f));
    }
    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity < 0)
            _velocity = -1;
        else
            _velocity += _gravity * _gravityMultiplier * Time.deltaTime;
    }
    private void ApplyCameraBasedRotation()
    {
        _spriteRenderer.transform.LookAt(_camera.transform);
        _spriteRenderer.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
    }
}
