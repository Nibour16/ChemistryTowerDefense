using System;
using UnityEngine;

public class ShootingAttackModule : BaseAttackModule
{
    private readonly StandardProjectile _projectile;
    private readonly Transform _shootingSource;
    private readonly PoolManager _manager;

    public ShootingAttackModule(StandardProjectile projectile, Transform shootingSource)
    {
        _manager = PoolManager.Instance;

        _projectile = projectile;
        _shootingSource = shootingSource;
    }

    public override void Attack(EnemyCharacter target)
    {
        var proj = _manager.Get(
            _projectile, _shootingSource.position, _shootingSource.rotation);

        proj.Initialize(target);
    }
}
