using UnityEngine;

public class BuildFacade : BaseFacade<IBuildService, BuildManager>, IBuildService
{
    public void OnTowerSelected(GameObject prefab)
        => Manager.OnTowerSelected(prefab);

    public void OnBuildFinished()
        => Manager.OnBuildFinished();
}
