using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float baseDamage = 20f;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.OnTakenDamage(baseDamage);
        }
    }
}
