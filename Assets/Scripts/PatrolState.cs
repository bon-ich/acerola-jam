using UnityEngine;
public class PatrolState : CharacterState
{
    private Transform _target;
    private int _positionSelectionDirection = 1;
    public PatrolState(StateMachine stateMachine, CorruptedCharacter character) : base(stateMachine, character)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("entered Patrol State");
        _target = _character.PatrolPoints[0];
        _agent.SetDestination(_target.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Vector3.Distance(_character.transform.position, _target.position) <= _agent.stoppingDistance + 0.1f)
        {
            SelectNextTarget();
            _agent.SetDestination(_target.position);
        }
    }
    private void SelectNextTarget()
    {
        var positions = _character.PatrolPoints;
        int currentPositionIndex = positions.IndexOf(_target);
        bool isTargetSelected = false;

        int nextPositionIndex = 0;
        if (_character.PatrolType == PatrolType.Loop)
        {
            if (currentPositionIndex + 1 >= positions.Count)
            {
                nextPositionIndex = _character.PatrolType == PatrolType.Loop ? 0 : positions.Count - 1;
                isTargetSelected = true;
            }
        }
        else if (_character.PatrolType == PatrolType.PingPong)
        {
            if (currentPositionIndex + 1 >= positions.Count)
                _positionSelectionDirection = -1;
            else if (currentPositionIndex - 1 < 0)
                _positionSelectionDirection = 1;
        }

        if (!isTargetSelected)
            nextPositionIndex = currentPositionIndex + _positionSelectionDirection;
        _target = positions[nextPositionIndex];
    }
}
