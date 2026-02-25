using UnityEngine;

public class StandardProjectile : PooledMonoBehaviour<StandardProjectile>
{
    [SerializeField] protected float baseDamage = 20f;
    [SerializeField] protected float speed = 5f;

    public float BaseDamage => baseDamage;

    protected EnemyCharacter target;

    public void Initialize(EnemyCharacter target)
    {
        this.target = target;
    }

    protected virtual void Update()
    {
        if (target == null)
            ReturnToPool();

        // Projectile Movement Logic
        transform.position += 10f * Time.deltaTime * transform.forward;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.OnTakenDamage(baseDamage);
            ReturnToPool();
        }
    }
}
