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

        InitializeInComingEnemies();
    }

    private void InitializeInComingEnemies()
    {
        foreach (var enemyClass in _spawnManager.PreparedEnemies)
        {
            int index = enemyClass.roundNumber - 1; //Set round index

            // Make sure the volume of the 2D list is enough to add, otherwise we increase that volume
            while (_incomingEnemies.Count <= index)
                _incomingEnemies.Add(new List<EnemyCharacter>());

            _incomingEnemies[index].Add(enemyClass.enemyChar);  //Add enemy into the row
        }
    }

    // Spawn Enemy Logic
    public IEnumerator SpawnEnemies(int roundNumber, float spawnDelay)
    {
        foreach(var i in _incomingEnemies[roundNumber - 1])
        {
            EnemyCharacter spawnEnemy = Instantiate(i, spawnTrans.position, spawnTrans.rotation);
            spawnEnemy.defaultPath = relatedEnemyPath;

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
