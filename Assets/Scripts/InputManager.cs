using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum InputType
{
    Gameplay,
    UI
}

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private Controls _controls;

    public Vector2 MovementInput { get; private set; }
    public Vector2 MovementInputNormalized { get; private set; }
    public float RotationInput { get; set; }
    public UnityEvent<int> CameraRotationInputEvent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        _controls = new Controls();
    }

    private void Start()
    {
        _controls.Gameplay.Move.started += OnMovementInput;
        _controls.Gameplay.Move.performed += OnMovementInput;
        _controls.Gameplay.Move.canceled += OnMovementInput;

        _controls.Gameplay.Rotate.started += OnRotationInput;
        _controls.Gameplay.Rotate.performed += OnRotationInput;
        _controls.Gameplay.Rotate.canceled += OnRotationInput;
        
        _controls.Gameplay.CameraRotate.performed += OnCameraRotationInput;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void OnMovementInput(InputAction.CallbackContext ctx)
    {
        MovementInput = ctx.ReadValue<Vector2>();
        MovementInputNormalized = MovementInput.normalized;
    }

    private void OnRotationInput(InputAction.CallbackContext ctx)
    {
        RotationInput = ctx.ReadValue<float>();
    }
    
    private void OnCameraRotationInput(InputAction.CallbackContext ctx)
    {
        CameraRotationInputEvent.Invoke((int)Mathf.Sign(ctx.ReadValue<float>()));
    }

    public void SwitchInput(InputType inputType)
    {
        switch (inputType)
        {
            case InputType.Gameplay:
                _controls.Gameplay.Enable();
                _controls.UI.Disable();
                break;
            case InputType.UI:
                _controls.Gameplay.Disable();
                _controls.UI.Enable();
                break;
        }
    }
}