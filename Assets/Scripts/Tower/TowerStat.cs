using UnityEngine;

public class TowerStat : MonoBehaviour
{
    public float price = 50f;
    public float attackDelay = 2f;
    public float attackRange = 5f;

    public float Price => price;
    public float AttackDelay => attackDelay;
    public float AttackRange => attackRange;
}
