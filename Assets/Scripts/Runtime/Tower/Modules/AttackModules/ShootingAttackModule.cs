using UnityEngine;

public class ShootingAttackModule : BaseAttackModule
{
    private readonly StandardProjectile _projectile;
    private readonly Transform _shootingSource;

    public ShootingAttackModule(StandardProjectile projectile, Transform shootingSource)
    {
        _projectile = projectile;
        _shootingSource = shootingSource;
    }

    public override void Attack(BaseTower tower)
    {
        var proj = Object.Instantiate(
            _projectile, _shootingSource.position, _shootingSource.rotation);
    }
}
