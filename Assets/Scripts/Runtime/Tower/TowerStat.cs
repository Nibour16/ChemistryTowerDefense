using UnityEngine;

public class TowerStat : MonoBehaviour
{
    [SerializeField] private float price = 50f;
    [SerializeField] private float attackDelay = 2f;
    [SerializeField] private float attackRange = 15f;

    public float Price => price;
    public float AttackDelay => attackDelay;
    public float AttackRange => attackRange;
}
