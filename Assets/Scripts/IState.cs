public interface IState
{
    StateMachine StateMachine { get; }
    void InitState(StateMachine stateMachine);
}