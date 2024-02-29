using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedCharacter : MonoBehaviour
{
    private CharacterAnimationsController _animationsController;
    private InputManager _inputManager;
    private Vector3 _movementDirection;
    private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animationsController = GetComponent<CharacterAnimationsController>();
        _camera = Camera.main;
    }
    void Update()
    {
        ApplyCameraBasedRotation();

    }
    private void ApplyCameraBasedRotation()
    {
        _spriteRenderer.transform.LookAt(_camera.transform);
        _spriteRenderer.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
    }
}
