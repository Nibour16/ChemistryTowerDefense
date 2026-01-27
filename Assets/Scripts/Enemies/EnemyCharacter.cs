using UnityEngine;

[RequireComponent(typeof(EnemyStat))]
public class EnemyCharacter : MonoBehaviour, IDamageable
{
    private EnemyStat _stat;

    private void Awake()
    {
        _stat = GetComponent<EnemyStat>();
    }

    public void OnTakenDamage(float basedamage)
    {
        _stat.health -= basedamage;
        if (_stat.health <= 0)
            OnDead();
    }

    public void OnDead()
    {
        Destroy(gameObject);
    }
}
