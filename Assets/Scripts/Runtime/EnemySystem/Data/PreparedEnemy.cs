using UnityEngine;

[System.Serializable]
public class PreparedEnemy
{
    public EnemyCharacter preparedEnemy;
    [Min(1)] public int number = 1;
}
