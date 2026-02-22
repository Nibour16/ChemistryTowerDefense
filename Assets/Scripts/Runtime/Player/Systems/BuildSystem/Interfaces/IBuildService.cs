using UnityEngine;

public interface IBuildService
{
    void OnTowerSelected(GameObject selectedTowerObj);
    void OnBuildFinished();
}
