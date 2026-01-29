using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotatingSpeed = 2f;

    public float Health => health;
    public float MoveSpeed => moveSpeed;
    public float RotatingSpeed => rotatingSpeed;
}
