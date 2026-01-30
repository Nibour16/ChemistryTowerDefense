using System.Collections;
using UnityEngine;

[System.Serializable]
public class PreparedEnemy
{
    [Min(1)] public int roundNumber = 1;
    public EnemyCharacter enemyChar;
}

public class EnemySpawnManager : Singleton<EnemySpawnManager>
{
    [Header("Spawn Inputs")]
    [SerializeField] private PreparedEnemy[] preparedEnemies;
    [SerializeField] private EnemySpawner[] enemySpawners;
    [SerializeField] private float waitTime = 3f;

    [Header("Test Inputs")]
    [SerializeField] private int testRound = 1;
    [SerializeField] private float testSpawnDelay = 0.5f;

    public PreparedEnemy[] PreparedEnemies => preparedEnemies;

    private void Start()
    {
        StartCoroutine(TestSpawn());
    }

    private IEnumerator TestSpawn()
    {
        //Testing only
        yield return new WaitForSeconds(waitTime);
        
        foreach (var spawner in enemySpawners)
            StartCoroutine(spawner.SpawnEnemies(testRound, testSpawnDelay));
    }
}
