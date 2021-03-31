using System.Collections;
using UnityEngine;

public class PlayerCollectingState : BasePlayerState
{
    public int impactAmount = 40;
    public GameObject wood;
    
    private ICollectable _currentTarget;
    private bool _isCollecting;
    
    public override void EnterState()
    {
        Animator.SetBool(Walk, true);
        _currentTarget = GetNearestCollectable();
        Agent.SetDestination(_currentTarget.Transform.position);
    }

    private ICollectable GetNearestCollectable()
    {
        var lowestDistance = Mathf.Infinity;
        ICollectable lowestDistanceCollectable = null;
        for (var i = 0; i < Collectables.Count; i++)
        {
            var collectable = Collectables[i];
            var distance = (collectable.Transform.position - transform.position).sqrMagnitude;
            if (!(distance < lowestDistance)) continue;
            lowestDistance = distance;
            lowestDistanceCollectable = collectable;
        }

        return lowestDistanceCollectable;
    }

    public override void HandleUpdate()
    {
        if (Agent.remainingDistance <= 1f && !_isCollecting)
        {
            Agent.isStopped = true;
            _isCollecting = true;
            Animator.SetBool(Walk, false);
            Animator.SetBool(Collect, true);
            transform.LookAt(_currentTarget.Transform);
            StartCoroutine(CollectTarget());
        }
    }

    private IEnumerator CollectTarget()
    {
        while (_currentTarget.IsAlive)
        {
            Animator.SetTrigger(Chop);
            yield return new WaitForSeconds(1.5f);
            _currentTarget.Impact(impactAmount);
        }
        wood.SetActive(true);
        Agent.isStopped = false;
        _isCollecting = false;
        Animator.SetBool(Collect, false);
        Animator.SetBool(Walk, true);
        StateMachine.ChangeState(PlayerState.Transferring.ToString());
    }
}