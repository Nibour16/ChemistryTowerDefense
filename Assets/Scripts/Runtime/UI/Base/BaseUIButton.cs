using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUIButton : MonoBehaviour, IInteractable
{
    protected Button button;
    protected InteractionManager interactionManager;

    // Initialization
    protected virtual void Awake()
    {
        interactionManager = InteractionManager.Instance;

        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnButtonClicked);
    }

    // Handle Unity's button click and interaction system connection
    protected virtual void OnButtonClicked()
    {
        // Simulate mouse hover
        interactionManager.SetCurrentHover(this);

        // Execute Interaction Manager's Handle Select logic
        interactionManager.ForceSelectCurrentHover();
    }

    #region Interactable Interface Connections
    public abstract void OnHovered();
    public abstract void OnHoverExit();
    public abstract void OnSelected();
    #endregion
}
