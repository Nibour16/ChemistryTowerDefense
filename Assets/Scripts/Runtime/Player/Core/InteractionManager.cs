using UnityEngine;

public class InteractionManager : Singleton<InteractionManager>
{
    private Camera _camera; // Create game camera reference
    private InputManager _inputManager; // Create input manager reference
    private IInteractable _currentHover; // Create currenr hover interface object reference

    private bool _currentHoverHandledThisFrame = false;

    // Initialization
    protected override void Awake()
    {
        base.Awake();

        _camera = Camera.main;  // By default, we use the main camera
        _inputManager = InputManager.Instance;  // Assign input manager reference by getting instance
        
        // Tell people to prevent if they forget to put input manager inside the scene
        Debug.Assert(_inputManager != null, "InputManager not found!");
    }

    #region Private Logic Handle
    private void Update()
    {
        HandleHover();
        HandleSelect();
    }

    private void HandleHover()
    {
        // Construct ray by setting the origin by point position
        Ray ray = _camera.ScreenPointToRay(_inputManager.PointPosition());

        if (Physics.Raycast(ray, out RaycastHit hit))   //If hit something by the ray
        {
            var interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != _currentHover)  // Ensure the hit object is interactable
            {
                // Then we execute the hover event
                SetCurrentHover(interactable);
            }
        }
        else
        {
            // Execute hover exit event when not hovered
            _currentHover?.OnHoverExit();
            _currentHover = null;
        }
    }

    private void HandleSelect()
    {
        // When the select input is pressed, execute select event
        // Only execute if current hover object is assigned, otherwise nothing will happen
        if (!InputManager.Instance.IsSelected())
            return;
        
        SelectCurrentHover();
    }
    
    private void SelectCurrentHover()
    {
        // This check prevents select event repeats again in one frame
        if (_currentHoverHandledThisFrame) return;

        // Setup current hover
        _currentHover?.OnSelected();
        _currentHoverHandledThisFrame = true;
    }

    private void LateUpdate()
    {
        // Reset the select check during each of the end of the frame
        _currentHoverHandledThisFrame = false;
    }
    #endregion

    #region Public Methods
    // Force select current hover, for special cases
    // This property coding just makes the script reading more understandable
    public void ForceSelectCurrentHover() => SelectCurrentHover();

    public void SetCurrentHover(IInteractable interactable)
    {
        // Hover Logic
        _currentHover?.OnHoverExit();
        _currentHover = interactable;
        _currentHover?.OnHovered();
    }
    #endregion
}
