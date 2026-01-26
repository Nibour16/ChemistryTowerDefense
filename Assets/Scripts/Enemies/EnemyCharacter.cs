using UnityEngine;

[RequireComponent(typeof(EnemyStat))]
public class EnemyCharacter : MonoBehaviour
{
    private EnemyStat _stat;

    #region Ingame Stats
    private float _currentHealth = 0;
    private float _moveSpeed = 0;
    #endregion

    private void Awake()
    {
        _stat = GetComponent<EnemyStat>();
        _currentHealth = _stat.health;
        _moveSpeed = _stat.moveSpeed;
    }

    public void ReceiveDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
