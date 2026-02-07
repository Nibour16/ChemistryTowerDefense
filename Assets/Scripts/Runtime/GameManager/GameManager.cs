using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private EnemySpawnManager _enemySpawnManager;

    #region Preparation Inputs
    [SerializeField] private float prepareDuration = 20f;
    private float _currentDuration = 20f;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _enemySpawnManager = EnemySpawnManager.Instance;
    }

    #region Preparation
    public float CurrentCountDown => _currentDuration;

    public void StartCountDown()
    {
        _currentDuration = prepareDuration;
    }
    public void UpdateCountdown()
    {
        _currentDuration = Mathf.Max(0, _currentDuration - Time.deltaTime);
    }
    #endregion

    #region Battle
    public void OnBattleStart()
    {
        if (_enemySpawnManager != null) 
        {
            _enemySpawnManager.StartSpawning();
        }
    }

    public void OnBattleEnd()
    {
        if (_enemySpawnManager != null)
        {
            _enemySpawnManager.StopSpawning();
        }
    }
    #endregion

    public void OnEnemyReachFinalEnd()
    {
        Debug.Log("Game Over!!");
    }
}
