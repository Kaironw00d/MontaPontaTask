public interface ILoopState : IState
{
    void EnterState();
    void HandleUpdate();
    void HandleFixedUpdate();
}