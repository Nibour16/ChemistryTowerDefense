using UnityEngine;

[RequireComponent(typeof(EnemyStat))]
public class EnemyCharacter : MonoBehaviour, IDamageable
{
    public LineRenderer defaultPath;
    
    protected EnemyStat stat;
    protected EnemyMove enemyMove;

    protected float currentHealth;
    protected float currentMoveSpeed;

    protected virtual void Awake()
    {
        stat = GetComponent<EnemyStat>();
        enemyMove = GetComponent<EnemyMove>();

        currentHealth = stat.Health;
        currentMoveSpeed = stat.MoveSpeed;
    }

    protected virtual void Start()
    {
        InitializeMovement();
    }

    private void InitializeMovement()
    {
        if (defaultPath != null)
        {
            //Bind enemy path during the start
            enemyMove.enabled = true;
            enemyMove.BindEnemyPath(defaultPath, true);
        }
        else
        {
            enemyMove.enabled = false;
            Debug.LogWarning("Enemy path is not found, unable to move");
        }
    }

    public float GetCurrentMoveSpeed() => currentMoveSpeed;

    public virtual void OnTakenDamage(float basedamage)
    {
        currentHealth -= basedamage;
        Debug.Log(
            $"Received {basedamage} damage, now current Health is {currentHealth}, " +
            $"original health is {stat.Health}"
        );

        if (currentHealth <= 0)
            OnDead();
    }

    public virtual void OnDead()
    {
        Debug.Log("I died");
        Destroy(gameObject);
    }
}
