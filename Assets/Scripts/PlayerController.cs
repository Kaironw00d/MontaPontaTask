using UnityEngine;
using UnityEngine.AI;

public enum PlayerState
{
    Idle,
    Collecting,
    Transferring
}
public class PlayerController : MonoBehaviour
{
    private StateMachine _stateMachine;
    private PlayerIdleState _idleState;
    private PlayerCollectingState _collectingState;
    private PlayerTransferringState _transferringState;
    private Animator _animator;
    private NavMeshAgent _agent;
    private ICollectiblesProvider _collectiblesProvider;

    private void Awake()
    {
        _idleState = GetComponent<PlayerIdleState>();
        _collectingState = GetComponent<PlayerCollectingState>();
        _transferringState = GetComponent<PlayerTransferringState>();
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _collectiblesProvider = GetComponent<ICollectiblesProvider>();
        
        _idleState.SetDependencies(_animator, _agent, _collectiblesProvider);
        _collectingState.SetDependencies(_animator, _agent, _collectiblesProvider);
        _transferringState.SetDependencies(_animator, _agent, _collectiblesProvider);
        _stateMachine = new StateMachine(false, new StateConstructor(PlayerState.Idle.ToString(), _idleState),
            new StateConstructor(PlayerState.Collecting.ToString(), _collectingState),
            new StateConstructor(PlayerState.Transferring.ToString(), _transferringState));
    }

    private void Start()
    {
        _stateMachine.ChangeState(PlayerState.Idle.ToString());
    }

    private void Update()
    {
        _stateMachine.ProcessUpdate();
    }
}