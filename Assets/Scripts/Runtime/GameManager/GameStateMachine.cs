using UnityEngine;

public class GameStateMachine : BaseStateMachine
{
    #region Keep track of all states
    private PreparationState _preparationState;
    private BattleState _battleState;
    #endregion

    #region Referencing all of states
    public PreparationState PreparationState => _preparationState;
    public BattleState BattleState => _battleState;
    #endregion

    #region Keep track of all supporting components
    private GameManager _gameManager;
    #endregion

    #region Referencing all supporting components
    public GameManager GameManager => _gameManager;
    #endregion

    private void Awake()
    {
        // Initialize components
        _gameManager = GameManager.Instance;

        // Initialize states
        _preparationState = new PreparationState(this);
        _battleState = new BattleState(this);
    }

    private void Start()
    {
        // Setup starting state
        SetState(_preparationState);
    }
}