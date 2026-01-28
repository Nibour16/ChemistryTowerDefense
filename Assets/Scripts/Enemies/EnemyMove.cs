using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float reachDistance = 0.05f;
    private bool _callGameOverIfReachEnd = true;

    private LineRenderer _enemyPath; //Enemy path reference that enemy can use to follow
    private EnemyPath _enemyPathClass;   //Enemy path class reference (from _enemyPath)
    private EnemyCharacter _enemy;  //Enemy character reference

    private Vector3[] _pathPoints;  //Path points data that gets from the enemy path
    private int _currentPathPointIndex = 0; //Enemy's current path point arrived

    private void Awake()
    {
        _enemy = GetComponent<EnemyCharacter>();
        enabled = false; // Close by default
    }

    #region Path Binding & Data Initialization
    public void BindEnemyPath(LineRenderer enemyPath, bool resetPathPointIndex)
    {
        if (_enemyPath == null)
        {
            Debug.LogError("Enemy path is null");
            return;
        }//Prevent giving null reference, usually for safety case

        _enemyPath = enemyPath; //Bind enemy path from input path (line renderer)

        HandleCallGameOverIfReachEnd();
        CachePathPoints();  //Set end of path behaviour

        // If reset bool is true then we reset the current path point back to the start (i.e. 0)
        if (resetPathPointIndex) _currentPathPointIndex = 0;
    }

    private void HandleCallGameOverIfReachEnd()
    {
        // Initialize enemy path class
        _enemyPathClass = _enemyPath.GetComponent<EnemyPath>();

        // If enemy path script component is exist
        if (_enemyPathClass != null)
        {
            // Then this script component will tell if the enemy can "kill" the player
            _callGameOverIfReachEnd = _enemyPathClass.callGameOverIfReachEnd;
        }
        else
        {
            // Otherwise by default the end of the path is the enemy's final goal
            _callGameOverIfReachEnd = true;
        }
    }

    private void CachePathPoints()
    {
        // Cache all path points so movement logic can directly access
        // any waypoint by index (e.g. _pathPoints[i])
        _pathPoints = new Vector3[_enemyPath.positionCount];
        _enemyPath.GetPositions(_pathPoints);
    }
    #endregion

    #region Move Logic
    private void Update()
    {
        // If path point data is empty enemy will stop moving immediately
        if (_pathPoints == null || _currentPathPointIndex >= _pathPoints.Length)
            enabled = false;

        // Otherwise move to the next point
        MoveToNextPoint();
    }

    private void MoveToNextPoint()
    {
        Vector3 target = _pathPoints[_currentPathPointIndex];   //Get target position 
        Vector3 dir = target - transform.position;  //Get target direction

        // Calculate the size of the moving step
        float step = _enemy.GetCurrentMoveSpeed() * Time.deltaTime; 

        if (dir.magnitude <= reachDistance) //If reach the target point
        {
            //Next target will be the next point
            _currentPathPointIndex++;

            // If reach the last point
            if (_currentPathPointIndex >= _pathPoints.Length)
                OnReachEnd();   //Handle reach end logic
        }
        else
        {
            transform.position += dir.normalized * step;    //Moving logic
        }
    }

    private void OnReachEnd()
    {
        enabled = false;    //Stop moving

        var reachEnd = GetComponentInChildren<IEnemyReachEnd>();
        // Call method from the interface component if the enemy object has
        reachEnd?.OnEnemyReachEnd(this);

        // Call Game Over Method if the end of the path is the enemy's goal
        if(_callGameOverIfReachEnd)  Debug.Log("Game over");
    }
    #endregion
}
