using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCollectingState : MonoBehaviour, ILoopState
{
    public int impactAmount = 40;
    public GameObject wood;
    public StateMachine StateMachine { get; private set; }

    private Animator _animator;
    private NavMeshAgent _agent;
    private ICollectiblesProvider _collectiblesProvider;
    private List<ICollectable> _collectables = new List<ICollectable>();
    private ICollectable _currentTarget;
    private bool _isCollecting;
    
    private static readonly int WalkHash = Animator.StringToHash("Walk");
    private static readonly int CollectHash = Animator.StringToHash("Collect");
    private static readonly int ChopHash = Animator.StringToHash("Chop");

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
        _currentTarget = GetNearestCollectable();
        _agent.SetDestination(_currentTarget.Transform.position);
    }

    private ICollectable GetNearestCollectable()
    {
        var lowestDistance = Mathf.Infinity;
        ICollectable lowestDistanceCollectable = null;
        for (var i = 0; i < _collectables.Count; i++)
        {
            var collectable = _collectables[i];
            var distance = (collectable.Transform.position - transform.position).sqrMagnitude;
            if (!(distance < lowestDistance)) continue;
            lowestDistance = distance;
            lowestDistanceCollectable = collectable;
        }

        return lowestDistanceCollectable;
    }

    public void HandleUpdate()
    {
        if (_agent.remainingDistance <= 1f && !_isCollecting)
        {
            _agent.isStopped = true;
            _isCollecting = true;
            _animator.SetBool(WalkHash, false);
            _animator.SetBool(CollectHash, true);
            transform.LookAt(_currentTarget.Transform);
            StartCoroutine(Collect());
        }
    }

    private IEnumerator Collect()
    {
        while (_currentTarget.IsAlive)
        {
            _animator.SetTrigger(ChopHash);
            yield return new WaitForSeconds(1.5f);
            _currentTarget.Impact(impactAmount);
        }
        wood.SetActive(true);
        _agent.isStopped = false;
        _isCollecting = false;
        _animator.SetBool(CollectHash, false);
        _animator.SetBool(WalkHash, true);
        StateMachine.ChangeState(PlayerState.Transferring.ToString());
    }

    public void HandleFixedUpdate()
    {
        
    }
}