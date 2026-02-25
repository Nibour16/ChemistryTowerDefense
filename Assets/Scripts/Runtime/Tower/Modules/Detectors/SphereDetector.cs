using System.Collections.Generic;
using UnityEngine;

public class SphereDetector : BaseTargetDetector
{
    public override List<EnemyCharacter> Detect(BaseTower tower)
    {
        var results = new List<EnemyCharacter>();

        float range = tower.Stat.AttackRange;

        Collider[] hits = Physics.OverlapSphere(
            tower.transform.position, range/*, targetLayer*/);

        foreach (var hit in hits)
        {
            if (hit.transform.IsChildOf(tower.transform))
                continue;

            if (hit.TryGetComponent<EnemyCharacter>(out var enemy))
            {
                results.Add(enemy);
            }
        }

        return results;
    }

    public void DrawGizmos(BaseTower tower)
    {
        if (Application.isPlaying == false)
            return;

        if (tower == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(tower.transform.position, tower.Stat.AttackRange);
    }
}
