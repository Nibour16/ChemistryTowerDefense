using UnityEngine;

public class StandardShootingTower : BaseTower
{
    [SerializeField] private StandardProjectile projectile;
    [SerializeField] private Transform shootingSource;

    protected override void SetupModules()
    {
        detector = new SphereDetector();
        selector = new FirstTargetSelector();
        attacker = new ShootingAttackModule(projectile, shootingSource);
    }
}
