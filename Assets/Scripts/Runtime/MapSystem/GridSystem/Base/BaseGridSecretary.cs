using UnityEngine;

public abstract class BaseGridSecretary : MonoBehaviour
{
    protected GridManager gridManager;

    public virtual void BindManager(GridManager manager)
    {
        gridManager = manager;

        if (gridManager == null)
        {
            Debug.LogError("Grid Manager is not found");
            return;
        }
    }
}
