using UnityEngine;

[RequireComponent(typeof(TowerStat))]
public abstract class BaseTower : MonoBehaviour
{
    protected TowerStat stat;

    protected virtual void Awake()
    {
        stat = GetComponent<TowerStat>();
    }
}
