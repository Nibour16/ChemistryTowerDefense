using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Leave a tiny space as when telling enemy reach a point
    [SerializeField] private float reachDistance = 0.05f;

    // Variables that are ready to cache
    private bool _callGameOverIfReachEnd = true;
    private bool _isGroundEnemy = true;

    private EnemyPath _enemyPath; //Enemy path reference that enemy can use to follow
    private EnemyCharacter _enemy;  //Enemy character reference that can share the stats

    private Vector3[] _pathPoints;  //Path points data that gets from the enemy path
    private int _currentPathPointIndex = 0; //Enemy's current path point arrived

    private void Awake()
    {
        _enemy = GetComponent<EnemyCharacter>(); //Assign enemy character class reference

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
        }//Prevent giving null reference, usually for safety case

        _enemyPath = enemyPath; //Bind enemy path from input path (line renderer)

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
        Vector3 target = _pathPoints[_currentPathPointIndex];   //Get target position 
        Vector3 dir = target - transform.position;  //Get target direction
        if (_isGroundEnemy) dir.y = 0f; //Lock y direction to 0 for 3d ground tower defense

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
            HandleRotation(dir);
            transform.position += transform.forward * step;    //Moving logic
        }
    }

    private void HandleRotation(Vector3 dir)
    {
        //Rotate to look at the target point
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
        enabled = false;    //Stop moving

        var reachEnd = GetComponentInChildren<IEnemyReachEnd>();
        // Call method from the interface component if the enemy object has
        reachEnd?.OnEnemyReachEnd(this);

        // Call Game Over Method if the end of the path is the enemy's goal
        if(_callGameOverIfReachEnd)  Debug.Log("Game over");
    }
    #endregion
}
