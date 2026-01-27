using UnityEngine;

[RequireComponent(typeof(TowerStat))]
public class BaseTower : MonoBehaviour
{
    protected TowerStat stat;

    protected virtual void Awake()
    {
        stat = GetComponent<TowerStat>();
    }
}
