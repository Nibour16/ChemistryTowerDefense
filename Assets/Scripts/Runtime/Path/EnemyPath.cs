using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [Header("Gameplay")]
    public bool callGameOverIfReachEnd = true;  //Prevent if the end of the path is not the final end

    [Header("Debug/Visual")]
    [SerializeField] private bool showPathInGame = false;   //Should display the line during the gameplay

    [SerializeField] private Vector3[] pathPoints;  //Editable path points

    //Property that allowed other classes to use the path points data but not allowed to change
    public Vector3[] PathPoints => pathPoints;

    private LineRenderer _pathLine;  //Path line reference
    private Renderer[] _childRenderers; //This will contain all of the renderers in the children object

    private void Awake()
    {
        CacheRenderers();
        ApplyVisual(Application.isPlaying);
    }

    private void OnEnable()
    {
        ApplyVisual(Application.isPlaying);
    }

    public void SetPoints(Vector3[] newPoints)  //Handle for when points are updated
    {
        pathPoints = newPoints;
        SyncLineRenderer();
    }

    public void SyncLineRenderer()
    {
        if (pathPoints == null || pathPoints.Length == 0)   //If no path point
        {
            // Tell path line that it does not have point (positionCount = 0), then do nothing
            _pathLine.positionCount = 0;
            return;
        }

        // Setup path line
        _pathLine.positionCount = pathPoints.Length;
        _pathLine.SetPositions(pathPoints);
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (!_pathLine) //If this class forgot to assign the path line reference
            _pathLine = GetComponent<LineRenderer>(); //Then assign this reference immediately

        SyncLineRenderer(); //And then initialize all of the data in this class
        UpdatePathLineRenderer();
    }

    private void UpdatePathLineRenderer()
    {
        // Prevent invalid call to the path line renderer
        if (!gameObject.scene.IsValid())
            return;

        // This method handles path visibility
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this == null) return;
            ApplyVisual(Application.isPlaying);
        };
    }
    #endif

    private void CacheRenderers()
    {
        //Cache all renderers
        
        _pathLine = GetComponent<LineRenderer>();
        _childRenderers = GetComponentsInChildren<Renderer>(true);

        // Make sure all of the path point positions are all used world space
        _pathLine.useWorldSpace = true;
    }

    private void ApplyVisual(bool isPlaying)
    {
        //If the game is playing, check show path in game boolean, it will always display in editor
        bool visible = !isPlaying || showPathInGame;

        //Handle visibility by check
        if (_pathLine != null)
            _pathLine.enabled = visible;

        if (_childRenderers != null)
        {
            foreach (var r in _childRenderers)
                r.enabled = visible;
        }
    }
}
