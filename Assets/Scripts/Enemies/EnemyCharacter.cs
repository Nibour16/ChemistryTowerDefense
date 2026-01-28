using UnityEngine;

[RequireComponent(typeof(EnemyStat))]
public class EnemyCharacter : MonoBehaviour, IDamageable
{
    protected EnemyStat stat;

    protected float currentHealth;
    protected float currentMoveSpeed;

    protected virtual void Awake()
    {
        stat = GetComponent<EnemyStat>();

        currentHealth = stat.Health;
        currentMoveSpeed = stat.MoveSpeed;
    }

    public virtual void OnTakenDamage(float basedamage)
    {
        currentHealth -= basedamage;
        Debug.Log(
            $"Received {basedamage} damage, now current Health is {currentHealth}, " +
            $"original health is {stat.Health}"
        );

        if (currentHealth <= 0)
            OnDead();
    }

    public virtual void OnDead()
    {
        Debug.Log("I died");
        Destroy(gameObject);
    }
}
