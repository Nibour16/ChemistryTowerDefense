using UnityEngine;

[RequireComponent(typeof(TowerStat))]
public abstract class BaseTower : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool drawGizmos = false;
    [SerializeField] private Color debugColour = Color.red;

    [Header("Advanced Setting")]
    [SerializeField] private bool checkModuleValidation = true;

    protected TowerStat stat;
    public TowerStat Stat => stat;

    protected float coolingTimer = 0;

    protected BaseTargetDetector detector;
    protected BaseTargetSelector selector;
    protected BaseAttackModule attacker;

    protected virtual void Awake()
    {
        stat = GetComponent<TowerStat>();
        //coolingTimer = stat.AttackDelay;

        SetupModules();

        if (checkModuleValidation)
            CheckModules();
    }

    private void Update()
    {
        HandleAttackLogic();
    }

    protected abstract void SetupModules();

    protected virtual void HandleAttackLogic()
    {
        coolingTimer -= Time.deltaTime;

        if (coolingTimer > 0f) return;

        var targets = detector?.Detect(this);

        if (targets == null || targets.Count == 0) return;

        var selectedTarget = selector?.Select(targets);

        if (selectedTarget == null) return;

        attacker?.Attack(selectedTarget);

        coolingTimer = stat.AttackDelay;
    }

    private void CheckModules()
    {
        if (detector == null)
        {
            Debug.LogWarning($"{name}: Detector is not assigned, this tower will unable to find target and attack");
            return;
        }
            
        if (selector == null)
        {
            Debug.LogWarning($"{name}: Selector is not assigned, this tower will unable to attack");
            return;
        }
            
        if (attacker == null)
            Debug.LogWarning($"{name}: Attack Module is not assigned");
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (drawGizmos)
            detector?.DrawGizmos(this, debugColour);
    }
    #endif
}
