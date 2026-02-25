using System.Collections.Generic;

public abstract class BaseTargetSelector
{
    public abstract EnemyCharacter Select(List<EnemyCharacter> targets);
}
