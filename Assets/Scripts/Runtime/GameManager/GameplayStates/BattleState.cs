using UnityEngine;

public class BattleState : BaseState
{
    private GameStateMachine _stateMachine;

    #region Initialization
    public BattleState(BaseStateMachine  stateMachine) : base(stateMachine) 
    {
        ResolveDependencies();
    }

    private void ResolveDependencies()
    {
        _stateMachine = GetStateMachine<GameStateMachine>();
        if (_stateMachine == null)
        {
            Debug.LogError($"{nameof(BattleState)} requires {nameof(GameStateMachine)}");
        }
    }
    #endregion

    #region State Lifecycle
    public override void EnterState() 
    {
        _stateMachine.GameManager.OnBattleStart();
    }
    public override void UpdateState() 
    {
        
    }
    public override void ExitState() 
    {
        
    }
    #endregion
}