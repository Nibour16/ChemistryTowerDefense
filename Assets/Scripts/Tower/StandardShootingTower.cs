using UnityEngine;

public class StandardShootingTower : BaseTower
{
    [SerializeField] protected StandardProjectile projectile;

    [SerializeField] protected Transform shootingSource;
    [SerializeField] protected Vector3 boxHalfExtents = new Vector3(1f, 1f, 2f);

    public StandardProjectile Projectile => projectile;

    protected EnemyCharacter targetEnemy;
    private float _coolingTimer = 0;

    private void Update()
    {
        if (projectile == null)
            Debug.LogError("Projectile reference is not assigned!");
        
        if (IsEnemyInRange())
        {
            transform.LookAt(targetEnemy.transform);
            Debug.Log("Threat detected, attack mode engaged!");

            if (_coolingTimer <= 0f)
            {
                Shoot();
                _coolingTimer = stat.AttackDelay;
            }

            _coolingTimer -= Time.deltaTime;
        }
    }

    protected virtual bool IsEnemyInRange()
    {
        RaycastHit[] hits = Physics.BoxCastAll(
            shootingSource.position, boxHalfExtents,
            transform.forward, transform.rotation,
            stat.AttackRange, LayerMask.GetMask("Character")
        );

        foreach (var hit in hits)
        {
            if (hit.collider.transform.IsChildOf(transform))
                continue;

            targetEnemy = hit.collider.GetComponent<EnemyCharacter>();
            return (targetEnemy != null);
        }

        targetEnemy = null;
        return false;
    }

    protected virtual void Shoot()
    {
        var projectileShoot =
            Instantiate(projectile, shootingSource.position, transform.rotation);
    }
}
