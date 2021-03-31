using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTransferringState : BasePlayerState
{
    public Transform transferringPoint;
    public GameObject wood;

    public override void EnterState()
    {
        Animator.SetBool(Walk, true);
        Agent.SetDestination(transferringPoint.position);
    }

    public override void HandleUpdate()
    {
        if (Agent.remainingDistance < 1f)
        {
            Animator.SetBool(Walk, false);
            wood.SetActive(false);
            StateMachine.ChangeState(Collectables.Count > 0
                ? PlayerState.Collecting.ToString()
                : PlayerState.Idle.ToString());
        }
    }
}