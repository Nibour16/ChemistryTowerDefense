using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float moveSpeed = 1f;

    public float Health => health;
    public float MoveSpeed => moveSpeed;
}
