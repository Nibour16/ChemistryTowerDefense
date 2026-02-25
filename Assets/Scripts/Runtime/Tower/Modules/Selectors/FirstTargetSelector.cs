using System.Collections.Generic;

public class FirstTargetSelector : BaseTargetSelector
{
    public override EnemyCharacter Select(List<EnemyCharacter> targets)
    {
        if (targets == null || targets.Count == 0)
            return null;

        return targets[0];
    }
}
