using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasePlayerState : MonoBehaviour, ILoopState
{
    protected static readonly int Idle = Animator.StringToHash("isIdling");
    protected static readonly int Walk = Animator.StringToHash("isWalking");
    protected static readonly int Collect = Animator.StringToHash("isCollecting");
    protected static readonly int Chop = Animator.StringToHash("isChopping");
    
    public StateMachine StateMachine { get; private set; }

    protected Animator Animator { get; private set; }
    protected NavMeshAgent Agent { get; private set; }
    private ICollectiblesProvider CollectiblesProvider { get; set; }
    protected List<ICollectable> Collectables = new List<ICollectable>();
    
    public virtual void InitState(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public virtual void SetDependencies(Animator animator, NavMeshAgent agent,
        ICollectiblesProvider collectiblesProvider)
    {
        Animator = animator;
        Agent = agent;
        CollectiblesProvider = collectiblesProvider;
        CollectiblesProvider.OnListChanged += UpdateCollectablesList;
    }

    protected virtual void UpdateCollectablesList() => Collectables = CollectiblesProvider.Get();
    
    public virtual void EnterState()
    {
        
    }

    public virtual void HandleUpdate()
    {
        
    }

    public virtual void HandleFixedUpdate()
    {
        
    }
}