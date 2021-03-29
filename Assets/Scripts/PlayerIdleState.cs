using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerIdleState : MonoBehaviour, ILoopState
{
    public Transform idlePoint;
    public StateMachine StateMachine { get; private set; }
    
    private Animator _animator;
    private NavMeshAgent _agent;
    private ICollectiblesProvider _collectiblesProvider;
    private List<ICollectable> _collectables = new List<ICollectable>();
    
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int WalkHash = Animator.StringToHash("Walk");

    public void SetDependencies(Animator animator, NavMeshAgent agent, ICollectiblesProvider collectiblesProvider)
    {
        _animator = animator;
        _agent = agent;
        _collectiblesProvider = collectiblesProvider;
        _collectiblesProvider.OnListChanged += UpdateCollectiblesList;
    }

    private void UpdateCollectiblesList() => _collectables = _collectiblesProvider.Get();

    public void InitState(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public void EnterState()
    {
        _animator.SetBool(WalkHash, true);
        _agent.SetDestination(idlePoint.position);
    }

    public void HandleUpdate()
    {
        if (_agent.remainingDistance < 0.1f)
        {
            _animator.SetBool(WalkHash, false);
            _animator.SetBool(IdleHash, true);
        }
        if (_collectables.Count > 0)
        {
            _animator.SetBool(IdleHash, false);
            StateMachine.ChangeState(PlayerState.Collecting.ToString());
        }
    }

    public void HandleFixedUpdate()
    {
        
    }
}