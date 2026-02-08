using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PreparedEnemies
{
    public EnemyCharacter preparedEnemy;
    [Min(1)] public int number = 1;
}

[System.Serializable]
public class RoundData
{
    public PreparedEnemies[] preparedEnemies;
    public EnemySpawner[] relatedEnemySpawners;

    [Min(0)] public float waitTimeBeforeSpawn = 20f;
    [Min(0)] public float spawnDelay = 0;

    public void EnsureEnemySpawners(List<EnemySpawner> activeSpawners)
    {
        if (relatedEnemySpawners == null ||
            relatedEnemySpawners.Length == 0)
        {
            relatedEnemySpawners = activeSpawners.ToArray();
            //If not explicitly assigned, use all active spawners as default
        }
    }
}

public class EnemySpawnManager : Singleton<EnemySpawnManager>
{
    [SerializeField] private RoundData[] rounds;
    
    public RoundData[] Rounds => rounds;
    private readonly List<EnemySpawner> _enemySpawners = new();

    private int _currentRound = 0;
    private bool _isFianlRound = false;
    private Coroutine _spawnRoutine;

    public int CurrentRound => _currentRound;
    public bool IsFinalRound => _isFianlRound;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        enabled = false; //By default will disable it first
    }

    private void OnEnable()
    {
        var spawners = _enemySpawners.ToArray();

        foreach (var enemySpawner in spawners)
            if (enemySpawner) enemySpawner.enabled = true;

        var rounds = this.rounds;

        foreach (var round in rounds)
            round?.EnsureEnemySpawners(_enemySpawners);
    }

    private void OnDisable()
    {
        var spawners = _enemySpawners.ToArray();

        foreach (var enemySpawner in spawners)
            if(enemySpawner) enemySpawner.enabled = false;
    }

    public void AddEnemySpawnerToList(EnemySpawner spawner)
    {
        if (!_enemySpawners.Contains(spawner))
            _enemySpawners.Add(spawner);
    }

    public void RemoveEnemySpawnerFromList(EnemySpawner spawner)
    {
        _enemySpawners.Remove(spawner);
    }
    #endregion

    #region Game Manager Connections
    public void StartSpawning()
    {
        if (_spawnRoutine != null)  //If spawning system is already triggered, do nothing
            return;

        // Enable spawning logic
        if (!enabled) enabled = true;
        _spawnRoutine = StartCoroutine(SpawnInRound());
    }

    public void StopSpawning()
    {
        // Stop all coroutines that will stop spawning enemies,
        
        if (_spawnRoutine != null) //If the spawning system has been triggered
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }

        enabled = false; //Always disable this manager while stopping
    }
    #endregion

    #region Spawn enemy Logic
    private IEnumerator SpawnInRound()
    {
        while (_currentRound < Rounds.Length)
        {
            int roundIndex = _currentRound;
            RoundData currentRoundData = Rounds[_currentRound];

            // Wait for the round start
            yield return new WaitForSeconds(currentRoundData.waitTimeBeforeSpawn);

            // Update the current round
            _currentRound++;

            // Spawn enemies during the round
            if (currentRoundData.relatedEnemySpawners != null)
            {
                foreach (var spawner in currentRoundData.relatedEnemySpawners)
                {
                    if (spawner != null)
                    {
                        Debug.Log("Spawn enemy");
                        StartCoroutine(spawner.SpawnEnemies(roundIndex));
                    }
                }
            }

            // Is the final round?
            if (_currentRound == Rounds.Length)
            {
                _isFianlRound = true;
                Debug.Log("Final round!");
            }
        }
    }
    #endregion
}
