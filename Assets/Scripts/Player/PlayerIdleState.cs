using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerIdleState : BasePlayerState
{
    public Transform idlePoint;

    public override void EnterState()
    {
        Animator.SetBool(Walk, true);
        Agent.SetDestination(idlePoint.position);
    }

    public override void HandleUpdate()
    {
        if (Agent.remainingDistance < 0.1f)
        {
            Animator.SetBool(Walk, false);
            Animator.SetBool(Idle, true);
        }

        if (Collectables.Count > 0)
        {
            Animator.SetBool(Idle, false);
            StateMachine.ChangeState(PlayerState.Collecting.ToString());
        }
    }
}