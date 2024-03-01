using UnityEngine.AI;
public class CharacterState : State
{
    protected CorruptedCharacter _character;
    protected NavMeshAgent _agent;
    public CharacterState(StateMachine stateMachine, CorruptedCharacter character) : base(stateMachine)
    {
        _character = character;
        _agent = _character.GetComponent<NavMeshAgent>();
    }
}