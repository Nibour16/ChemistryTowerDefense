using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("Advanced")]
    // Allow small buffer to tell that enemy has reached a point
    [Min(0f)]
    [SerializeField] private float reachDistance = 0.05f;
    [SerializeField] private bool neverAcrossPointWhileReaching = false;

    // Variables that are ready to cache
    private bool _callGameOverIfReachEnd = true;
    private bool _isGroundEnemy = true;

    private GameManager _gameManager; // Game Manager reference
    private EnemyPath _enemyPath; // Enemy path reference that enemy can use to follow
    private EnemyCharacter _enemy;  // Enemy character reference that can share the stats

    private Vector3[] _pathPoints;  // Path points data that gets from the enemy path
    private int _currentPathPointIndex = 0; // Enemy's current path point arrived
    private Vector3 _currentPoint;
    private Vector3 _dir;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _enemy = GetComponent<EnemyCharacter>(); // Assign enemy character class reference

        // Cache variables during awake
        _isGroundEnemy = _enemy.IsGroundEnemy;

        enabled = false; //Close by default
    }

    #region Path Binding & Data Initialization
    public void BindEnemyPath(EnemyPath enemyPath, bool resetPathPointIndex)
    {
        if (enemyPath == null)
        {
            Debug.LogError("Enemy path is null");
            return;
        }// Prevent giving null reference, usually for safety case

        _enemyPath = enemyPath; // Bind enemy path from input path (line renderer)

        // Cache path points data
        _pathPoints = _enemyPath.PathPoints;

        // Cache Call Game Over If Reach End boolean
        _callGameOverIfReachEnd = _enemyPath.callGameOverIfReachEnd;

        // If reset bool is true then we reset the current path point back to the start (i.e. 0)
        if (resetPathPointIndex) _currentPathPointIndex = 0;
    }
    #endregion

    #region Move Logic
    private void Update()
    {
        // If path point data is empty enemy will stop moving immediately
        if (_pathPoints == null || _currentPathPointIndex >= _pathPoints.Length)
        {
            enabled = false;
            return;
        }

        // Otherwise move to the next point
        MoveToNextPoint();
    }

    private void MoveToNextPoint()
    {
        UpdateDirectionAndTarget();
        Vector3 detectDir = _dir;

        HandleRotation(_dir); // Enemy always look at front, so they will look definitely "walking

        // Calculate the size of the moving step
        float step = _enemy.GetCurrentMoveSpeed() * Time.deltaTime;
        float reachDistance = this.reachDistance;

        bool isReachPathEnd = false;

        float reachDistanceCurrent = reachDistance; // Middle points, can be a tiny space
        if (_currentPathPointIndex >= _pathPoints.Length - 1)
        {
            isReachPathEnd = true;

            if (_callGameOverIfReachEnd)
                reachDistanceCurrent = 0f;  // Final point, must be precise
        }

        if (detectDir.magnitude <= reachDistance) // If reach the target point
        {
            // Next target will be the next point
            _currentPathPointIndex++;

            if (!isReachPathEnd)
                UpdateDirectionAndTarget();

            var reachPoint = GetComponentInChildren<IEnemyReachPoint>();
            reachPoint?.OnEnemyReachPoint(this, _currentPathPointIndex);

            // If reach the last point
            if (isReachPathEnd)
            {
                OnReachEnd();   // Handle reach end logic
                return; // Stop moving
            } 
        }

        if (neverAcrossPointWhileReaching)
            transform.position = Vector3.MoveTowards(transform.position, _currentPoint, step);
        else
            transform.position += transform.forward * step; //Moving Logic
    }

    private void UpdateDirectionAndTarget()
    {
        _currentPoint = _pathPoints[_currentPathPointIndex];   // Get target position 
        _dir = _currentPoint - transform.position;  // Get target direction"
        if (_isGroundEnemy) _dir.y = 0f; // Lock y direction to 0 for 3d ground tower defense
    }

    private void HandleRotation(Vector3 dir)
    {
        // Rotate to look at the target point
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _enemy.GetCurrentRotateSpeed() * Time.deltaTime
            );
        }
    }

    private void OnReachEnd()
    {
        enabled = false;    // Stop moving

        var reachPoint = GetComponentInChildren<IEnemyReachEnd>();
        // Call method from the interface component if the enemy object has
        reachPoint?.OnEnemyReachEnd(this);

        // Call Game Over Method if the end of the path is the enemy's goal
        if (_callGameOverIfReachEnd)
        {
            Destroy(gameObject);
            _gameManager.OnEnemyReachFinalEnd();
        }
    }
    #endregion
}
