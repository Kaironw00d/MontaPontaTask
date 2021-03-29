using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public delegate void OnStateChangeHandler(string name);
    public event OnStateChangeHandler OnStateChange;

    private IState _currentState;
    private bool _isEventBased;
    private Dictionary<string, IState> _states;

    public StateMachine(bool isEventBased, params StateConstructor[] stateConstructors)
    {
        _states = new Dictionary<string, IState>();
        _isEventBased = isEventBased;
        
        InitStates(stateConstructors);
    }

    public void ChangeState(string name)
    {
        if (_states.TryGetValue(name, out _currentState))
        {
            if(!_isEventBased)
                (_currentState as ILoopState)?.EnterState();
            OnStateChange?.Invoke(name);
        }
    }

    public void ProcessUpdate()
    {
        if(_isEventBased) return;
        (_currentState as ILoopState)?.HandleUpdate();
    }

    public void ProcessFixedUpdate()
    {
        if(_isEventBased) return;
        (_currentState as ILoopState)?.HandleFixedUpdate();
    }
    private void InitStates(IReadOnlyList<StateConstructor> stateConstructors)
    {
        for (var i = 0; i < stateConstructors.Count; i++)
        {
            var stateConstructor = stateConstructors[i];
            if (!_isEventBased && !(stateConstructor.State is ILoopState))
                throw new System.Exception("Not all of the given states implemented ILoopState");

            stateConstructor.State.InitState(this);
            _states.Add(stateConstructor.Name, stateConstructor.State);
        }
    }
}

public readonly struct StateConstructor
{
    public string Name { get; }
    public IState State { get; }
    public StateConstructor(string name, IState state)
    {
        Name = name;
        State = state;
    }
}