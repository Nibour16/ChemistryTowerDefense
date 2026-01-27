using UnityEngine;

public abstract class BaseShootingTower : BaseTower
{
    [SerializeField] protected BaseProjectile projectile;
    [SerializeField] protected Transform shootingSource;

    public BaseProjectile Projectile => projectile;

    private float _coolingTimer = 0;

    private void Update()
    {
        if (IsEnemyInRange())
        {
            if (_coolingTimer <= 0f)
            {
                Shoot();
                _coolingTimer = stat.attackDelay;
            }

            _coolingTimer -= Time.deltaTime;
        }
    }

    protected virtual bool IsEnemyInRange()
    {
        return true;
    }

    protected virtual void Shoot()
    {

    }
}
