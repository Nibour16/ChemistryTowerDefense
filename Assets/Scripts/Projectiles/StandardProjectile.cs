using UnityEngine;

public class StandardProjectile : MonoBehaviour
{
    [SerializeField] protected float baseDamage = 20f;
    [SerializeField] protected float speed = 5f;
    
    public float BaseDamage => baseDamage;

    protected virtual void Update()
    {
        //By default it will always moving forward with a constant speed
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    
    protected virtual void OnTriggerEnter(Collider other)   //Handle hit
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.OnTakenDamage(baseDamage);
            Destroy(gameObject);
        }
    }
}
