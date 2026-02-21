using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private PlayerInputs _playerInput;  //Create player input class reference

    private Vector2 _pointPosition;
    private bool _selectThisFrame;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        _playerInput = new PlayerInputs();  //Initialize player input class reference
    }

    private void OnEnable()
    {
        _playerInput.Enable();

        //Record new value to update in the point position variable (i.e. mouse is moved)
        _playerInput.Player.Point.performed += ctx =>
            _pointPosition = ctx.ReadValue<Vector2>();

        //Record new value to update in the select this frame variable (i.e. mouse is clicked)
        _playerInput.Player.Select.performed += ctx =>
            _selectThisFrame = true;
    }
    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void LateUpdate()
    {
        _selectThisFrame = false;
    }
    #endregion

    #region Inputs
    //Return the current point position (i.e. mouse position)
    public Vector2 PointPosition() => _pointPosition;

    //Return the current boolean whether it is selected or not (i.e. mouse click)
    public bool IsSelected() => _selectThisFrame;
    #endregion
}
