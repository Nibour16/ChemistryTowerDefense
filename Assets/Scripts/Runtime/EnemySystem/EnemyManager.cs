using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private readonly List<EnemyCharacter> _activeEnemies = new();

    public IReadOnlyList<EnemyCharacter> ActiveEnemies => _activeEnemies;

    public void Register(EnemyCharacter enemy)
    {
        if (!_activeEnemies.Contains(enemy))
            _activeEnemies.Add(enemy);
    }

    public void Unregister(EnemyCharacter enemy)
    {
        _activeEnemies.Remove(enemy);
    }
}
