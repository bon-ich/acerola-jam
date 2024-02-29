public class StateMachine
{
    public State PreviousState { get; private set; }
    public State CurrentState { get; private set; }

    public void SetState(State targetState)
    {
        if (PreviousState != null)
        {
            PreviousState.Exit();
            PreviousState = CurrentState;
        }

        CurrentState = targetState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.LogicUpdate();
    }

    public void PhysicsUpdate()
    {
        CurrentState?.PhysicsUpdate();
    }
}