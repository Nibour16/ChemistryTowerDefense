using UnityEngine;

public abstract class BaseBuildSecretary : MonoBehaviour
{
    protected BuildManager manager;
    protected IGridService gridService;

    public virtual void BindManager(BuildManager manager)
    {
        this.manager = manager;
        gridService = manager.GridService;
    }
}
