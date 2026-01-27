using UnityEngine;

[RequireComponent(typeof(EnemyStat))]
public class EnemyCharacter : MonoBehaviour, IDamageable
{
    private EnemyStat _stat;

    private float _currentHealth;
    private float _currentMoveSpeed;

    private void Awake()
    {
        _stat = GetComponent<EnemyStat>();

        _currentHealth = _stat.Health;
        _currentMoveSpeed = _stat.moveSpeed;
    }

    public void OnTakenDamage(float basedamage)
    {
        _currentHealth -= basedamage;
        Debug.Log(
            $"Received {basedamage} damage, now current Health is {_currentHealth}, " +
            $"original health is {_stat.health}"
        );

        if (_currentHealth <= 0)
            OnDead();
    }

    public void OnDead()
    {
        Debug.Log("I died");
        Destroy(gameObject);
    }
}
