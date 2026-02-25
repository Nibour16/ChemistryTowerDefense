using System.Collections.Generic;
using UnityEngine;

public class SphereDetector : BaseTargetDetector
{
    private readonly List<EnemyCharacter> _results = new();

    protected override List<EnemyCharacter> OnDetect(BaseTower tower)
    {
        _results.Clear();

        float range = tower.Stat.AttackRange;
        Vector3 pos = tower.transform.position;

        foreach (var enemy in enemyManager.ActiveEnemies)
        {
            if (RangeMath.InSphere(pos, enemy.transform.position, range))
                _results.Add(enemy);
        }

        return _results;
    }

    public override void DrawGizmos(BaseTower tower, Color debugColour)
    {
        base.DrawGizmos(tower, debugColour);
        Gizmos.DrawWireSphere(tower.transform.position, tower.Stat.AttackRange);
    }
}
