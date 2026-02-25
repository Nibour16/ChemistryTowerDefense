using System.Collections.Generic;
using UnityEngine;

public class SphereDetector : BaseTargetDetector
{
    private List<EnemyCharacter> _results = new();

    protected override List<EnemyCharacter> OnDetect(BaseTower tower)
    {
        _results.Clear();

        float range = tower.Stat.AttackRange;

        Collider[] hits = Physics.OverlapSphere(
            tower.transform.position, range/*, targetLayer*/);

        foreach (var hit in hits)
        {
            if (hit.transform.IsChildOf(tower.transform))
                continue;

            if (hit.TryGetComponent<EnemyCharacter>(out var enemy))
            {
                _results.Add(enemy);
            }
        }

        return _results;
    }

    public override void DrawGizmos(BaseTower tower, Color debugColour)
    {
        base.DrawGizmos(tower, debugColour);
        Gizmos.DrawWireSphere(tower.transform.position, tower.Stat.AttackRange);
    }
}
