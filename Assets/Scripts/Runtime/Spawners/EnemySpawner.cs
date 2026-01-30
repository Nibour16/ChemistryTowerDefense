using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private EnemyPath relatedEnemyPath;
    
    private List<List<EnemyCharacter>> _incomingEnemies = new();
    private EnemySpawnManager _spawnManager;

    #region Initialization
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
    #endregion

    #region Spawn Enemy Logic
    public IEnumerator SpawnEnemies(int roundIndex)
    {
        if (roundIndex < 0 || roundIndex >= _incomingEnemies.Count)
            yield break;

        foreach (var i in _incomingEnemies[roundIndex])
        {
            EnemyCharacter spawnEnemy = Instantiate(i, spawnTrans.position, spawnTrans.rotation);
            spawnEnemy.defaultPath = relatedEnemyPath;

            var spawnDelay = _spawnManager.Rounds[roundIndex].spawnDelay;
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    #endregion
}
