using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTargetDetector
{
    private static readonly List<EnemyCharacter> _emptyResult = new();

    public List<EnemyCharacter> Detect(BaseTower tower)
    {
        if (tower == null)
            return _emptyResult;

        var results = OnDetect(tower);

        return results ?? _emptyResult;
    }

    protected abstract List<EnemyCharacter> OnDetect(BaseTower tower);

    public virtual void DrawGizmos(BaseTower tower, Color debugColour)
    {
        if (tower == null)  return;
        Gizmos.color = debugColour;
    }
}
