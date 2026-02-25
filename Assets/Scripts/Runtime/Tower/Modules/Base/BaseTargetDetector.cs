using System.Collections.Generic;

public abstract class BaseTargetDetector
{
    public abstract List<EnemyCharacter> Detect(BaseTower tower);
}
