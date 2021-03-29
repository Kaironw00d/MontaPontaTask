using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTransferringState : MonoBehaviour, ILoopState
{
    public Transform transferringPoint;
    public GameObject wood;
    public StateMachine StateMachine { get; private set; }
    
    private Animator _animator;
    private NavMeshAgent _agent;
    private ICollectiblesProvider _collectiblesProvider;
    private List<ICollectable> _collectables = new List<ICollectable>();
    
    private static readonly int WalkHash = Animator.StringToHash("Walk");

    public void SetDependencies(Animator animator, NavMeshAgent agent, ICollectiblesProvider collectiblesProvider)
    {
        _animator = animator;
        _agent = agent;
        _collectiblesProvider = collectiblesProvider;
        _collectiblesProvider.OnListChanged += UpdateCollectiblesList;
    }
    
    public void InitState(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    
    private void UpdateCollectiblesList() => _collectables = _collectiblesProvider.Get();

    public void EnterState()
    {
        _animator.SetBool(WalkHash, true);
        _agent.SetDestination(transferringPoint.position);
    }

    public void HandleUpdate()
    {
        if (_agent.remainingDistance < 1f)
        {
            _animator.SetBool(WalkHash, false);
            wood.SetActive(false);
            StateMachine.ChangeState(_collectables.Count > 0
                ? PlayerState.Collecting.ToString()
                : PlayerState.Idle.ToString());
        }
    }

    public void HandleFixedUpdate()
    {
        
    }
}