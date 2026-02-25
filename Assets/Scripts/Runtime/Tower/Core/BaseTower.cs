using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(TowerStat))]
public abstract class BaseTower : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool drawGizmos = false;
    [SerializeField] private Color debugColour = Color.red;
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private bool isGroundTower = true;

    [Header("Advanced Setting")]
    [SerializeField] private float aimingTolerance = 5f;
    [SerializeField] private bool checkModuleValidation = true;

    private EnemyManager _enemyManager;
    private EnemyCharacter _currentTarget;

    protected TowerStat stat;
    public TowerStat Stat => stat;
    
    protected float coolingTimer = 0;

    protected BaseTargetDetector detector;
    protected BaseTargetSelector selector;
    protected BaseAttackModule attacker;

    protected virtual void Awake()
    {
        _enemyManager = EnemyManager.Instance;
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

    protected abstract void SetupModules();

    protected virtual void HandleAttackLogic()
    {
        // If the logic becomes very complex, use state machine instead
        
        coolingTimer -= Time.deltaTime;

        var targets = detector.Detect(_enemyManager, this);

        if (_currentTarget != null && !targets.Contains(_currentTarget))
            _currentTarget = null;

        if (_currentTarget == null)
        {
            if (targets.Count == 0) return;

            _currentTarget = selector.Select(targets);
        }

        RotateToTarget(_currentTarget);

        if (!IsFacingTarget(_currentTarget)) return;

        if (coolingTimer <= 0f)
        {
            attacker?.Attack(_currentTarget);
            coolingTimer = stat.AttackDelay;
        }
    }

    protected virtual void RotateToTarget(EnemyCharacter target)
    {
        GetDirection(target, out Vector3 dir);

        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            var rot = rotationSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rot);
        }
    }

    protected virtual bool IsFacingTarget(EnemyCharacter target)
    {
        GetDirection(target, out Vector3 dir);

        if (dir == Vector3.zero) return true;

        float angle = Vector3.Angle(transform.forward, dir);
        return angle < aimingTolerance;
    }

    private void GetDirection(EnemyCharacter target, out Vector3 dir)
    {
        dir = target.transform.position - transform.position;
        if (isGroundTower) dir.y = 0f; // if it is ground tower
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (drawGizmos)
            detector?.DrawGizmos(this, debugColour);
    }
    #endif
}
