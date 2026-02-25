using UnityEngine;

[RequireComponent(typeof(TowerStat))]
public abstract class BaseTower : MonoBehaviour
{
    protected TowerStat stat;
    public TowerStat Stat => stat;

    protected float coolingTimer;

    protected BaseTargetDetector detector;
    protected BaseTargetSelector selector;
    protected BaseAttackModule attacker;

    protected virtual void Awake()
    {
        stat = GetComponent<TowerStat>();
        coolingTimer = stat.AttackDelay;
    }

    private void Update()
    {
        UpdateTowerSystem();
    }

    protected abstract void SetupModules();

    protected virtual void UpdateTowerSystem()
    {
        coolingTimer -= Time.deltaTime;

        if (coolingTimer > 0f) return;

        var targets = detector?.Detect(this);

        if (targets == null || targets.Count == 0) return;

        var selectedTarget = selector?.Select(targets);

        if (selectedTarget == null) return;

        attacker?.Attack(this/*, selectedTarget*/);

        coolingTimer = stat.AttackDelay;
    }
}
