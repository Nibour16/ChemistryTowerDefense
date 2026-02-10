using UnityEngine;

public abstract class BaseGridSystem : MonoBehaviour
{
    protected GridManager gridManager;
    protected GridGenerator3D gridGenerator;
    //protected GridDetector gridDetector;

    protected virtual void Awake()
    {
        gridManager = GridManager.Instance;
        if (gridManager == null)
        {
            Debug.LogError("Grid Manager is not found");
            return;
        }

        gridGenerator = gridManager.GridGenerator;
        if (gridGenerator == null)
        {
            Debug.LogError($"{GetType().Name}: GridGenerator is not assigned in GridManager");
        }

        /*gridDetector = gridManager.GridDetector;
        if (gridDetector == null)
        {
            Debug.LogError($"{GetType().Name}: GridDetector is not assigned in GridManager");
        }*/
    }
}
