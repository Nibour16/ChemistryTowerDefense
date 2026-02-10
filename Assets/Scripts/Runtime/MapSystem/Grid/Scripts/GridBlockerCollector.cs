using System.Collections.Generic;
using UnityEngine;

public class GridBlockerCollector : BaseGridSecretary
{
    [SerializeField] private Transform blockingRoot;

    private readonly List<Bounds> _blockingBounds = new();
    public IReadOnlyList<Bounds> BlockingBounds => _blockingBounds;

    private void Awake()
    {
        if (blockingRoot == null)
            blockingRoot = transform;
    }

    /// <summary>
    /// Collect blocking bounds under blockingRoot
    /// </summary>
    public void Collect()
    {
        _blockingBounds.Clear();

        if (blockingRoot == null)
            return;

        var colliders = blockingRoot.GetComponentsInChildren<Collider>();

        foreach (var col in colliders)
        {
            if (col == null || col.isTrigger)
                continue;

            _blockingBounds.Add(col.bounds);
        }
    }
}
