public abstract class State : IState
{
    protected StateMachine _stateMachine;
    
    protected State(StateMachine stateMachine) { }
    public virtual void Enter()
    {
    }
    public virtual void Exit()
    {
    }
    public virtual void LogicUpdate()
    {
    }
    public virtual void PhysicsUpdate()
    {
    }
}