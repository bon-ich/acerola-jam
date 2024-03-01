using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AI;

public enum PatrolType
{
    PingPong,
    Loop
}

public class CorruptedCharacter : MonoBehaviour
{
    [Header("Patrolling")]
    [SerializeField] private List<Transform> _patrolPoints;
    [SerializeField] private PatrolType _patrolType;
    public ReadOnlyCollection<Transform> PatrolPoints => _patrolPoints.AsReadOnly();
    public PatrolType PatrolType => _patrolType;
    
    private CharacterAnimationsController _animationsController;
    private InputManager _inputManager;
    private Vector3 _movementDirection;
    private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    private NavMeshAgent _agent;

    private StateMachine _stateMachine;
    public PatrolState PatrolState { get; private set; }

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animationsController = GetComponent<CharacterAnimationsController>();
        _agent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;

        InitStateMachine();
    }
    private void InitStateMachine()
    {
        _stateMachine = new StateMachine();
        PatrolState = new PatrolState(_stateMachine, this);

        _stateMachine.SetState(PatrolState);
    }
    private void Update()
    {
        _stateMachine?.Update();
        SetAnimationVariables();
        ApplyCameraBasedRotation();
    }
    private void FixedUpdate()
    {
        _stateMachine?.PhysicsUpdate();
    }
    private void ApplyCameraBasedRotation()
    {
        // makes sprite always look at camera
        _spriteRenderer.transform.LookAt(_camera.transform);
        _spriteRenderer.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
    }
    private void SetAnimationVariables()
    {
        var velocity = _agent.velocity;
        _animationsController.speed = velocity.sqrMagnitude > 0.05f ? velocity.sqrMagnitude : 0;
        Vector2 movement = new Vector2();

        var angle = (int)Mathf.Round(Vector3.SignedAngle(_camera.transform.forward.normalized, transform.forward, Vector3.up));
        int nAngle = 45;
        int sAngle = 135;
        int wAngle = -90;
        int eAngle = 90;
        int nwAngle = -60;
        int neAngle = 60;
        int swAngle = -120;
        int seAngle = 120;
        int threshold = 15;

        if (angle == -nAngle) angle = -nAngle;
        if (angle == -sAngle) angle = sAngle;

        if (angle <= nAngle + threshold && angle >= nAngle - threshold)
            movement = new Vector2(0, 1);
        else if (angle <= sAngle + threshold && angle >= sAngle - threshold)
            movement = new Vector2(0, -1);
        else if (angle <= wAngle + threshold && angle >= wAngle - threshold)
            movement = new Vector2(-1, 0);
        else if (angle <= eAngle + threshold && angle >= eAngle - threshold)
            movement = new Vector2(1, 0);
        else if (angle <= nwAngle + threshold && angle >= nwAngle - threshold)
            movement = new Vector2(-1, 1);
        else if (angle <= neAngle + threshold && angle >= neAngle - threshold)
            movement = new Vector2(1, 1);
        else if (angle <= swAngle + threshold && angle >= swAngle - threshold)
            movement = new Vector2(-1, -1);
        else if (angle <= seAngle + threshold && angle >= seAngle - threshold)
            movement = new Vector2(1, -1);

        _animationsController.movement = movement;
    }
}
