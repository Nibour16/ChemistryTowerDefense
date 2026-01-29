using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private Camera _camera; //Create game camera reference
    private InputManager _inputManager; //Create input manager reference
    private IInteractable _currentHover; //Create currenr hover interface object reference

    private void Awake()
    {
        _camera = Camera.main;  //By default, we use the main camera

        _inputManager = InputManager.Instance;  //Assign input manager reference by getting instance
        //Tell people to prevent if they forget to put input manager inside the scene
        Debug.Assert(_inputManager != null, "InputManager not found!");
    }

    private void Update()
    {
        HandleHover();
        HandleSelect();
    }

    private void HandleHover()
    {
        //Construct ray by setting the origin by point position
        Ray ray = _camera.ScreenPointToRay(_inputManager.PointPosition());

        if (Physics.Raycast(ray, out RaycastHit hit))   //If hit something by the ray
        {
            var interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != _currentHover)  //Ensure the hit object is interactable
            {
                //Then we execute the hover event
                _currentHover?.OnHoverExit();
                _currentHover = interactable;
                _currentHover?.OnHovered();
            }
        }
        else
        {
            //Execute hover exit event when not hovered
            _currentHover?.OnHoverExit();
            _currentHover = null;
        }
    }

    private void HandleSelect()
    {
        //When the select input is pressed, execute select event

        if (!InputManager.Instance.IsSelected())
            return;

        //Only execute if current hover object is assigned, otherwise nothing will happen
        _currentHover?.OnSelected();
    }
}
