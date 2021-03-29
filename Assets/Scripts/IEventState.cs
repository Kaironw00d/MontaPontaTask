public interface IEventState : IState
{
    void HandleState(string stateName);
    bool CanHandleState(string stateName);
}