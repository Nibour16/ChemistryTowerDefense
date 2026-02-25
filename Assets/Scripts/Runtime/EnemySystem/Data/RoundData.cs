using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    public PreparedEnemy[] preparedEnemies;
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
