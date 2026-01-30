using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private EnemyPath relatedEnemyPath;
    
    private List<List<EnemyCharacter>> _incomingEnemies = new();
    private EnemySpawnManager _spawnManager;

    private void Awake()
    {
        _spawnManager = EnemySpawnManager.Instance;
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is not exist!");
            return;
        }

        if (spawnTrans == null)
            spawnTrans = transform;
    }

    private void OnEnable()
    {
        _spawnManager.AddEnemySpawnerToList(this);
        InitializeIncomingEnemies();
    }

    private void OnDisable()
    {
        _spawnManager.RemoveEnemySpawnerFromList(this);
    }

    private void InitializeIncomingEnemies()
    {
        foreach (var round in _spawnManager.Rounds)
        {
            var roundEnemies = new List<EnemyCharacter>();

            foreach(var roundEnemy in round.preparedEnemies)
            {
                for(int i = 0; i < roundEnemy.number; i++)
                    roundEnemies.Add(roundEnemy.preparedEnemy);
            }

            _incomingEnemies.Add(roundEnemies);
        }
    }

    // Spawn Enemy Logic
    public IEnumerator SpawnEnemies()
    {
        int currentRound = _spawnManager.CurrentRound;

        foreach (var i in _incomingEnemies[currentRound - 1])
        {
            EnemyCharacter spawnEnemy = Instantiate(i, spawnTrans.position, spawnTrans.rotation);
            spawnEnemy.defaultPath = relatedEnemyPath;

            var spawnDelay = _spawnManager.Rounds[currentRound - 1].spawnDelay;
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
