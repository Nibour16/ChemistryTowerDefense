using UnityEditor;
using UnityEngine;

public abstract class BaseCreatorWindow : EditorWindow
{
    protected string primaryName;
    protected string secondaryName;

    /// <summary>
    /// All children are allowed to edit the vertical space
    /// By default is 10 pixels
    /// </summary>
    protected virtual float VerticalSpace => 10f;

    /// <summary>
    /// All children are allowed to edit the default window size
    /// By default the width is 300 pixels and height is 70 pixels
    /// </summary>
    protected virtual Vector2 MinimumWindowSize => new(300f, 70f);

    private void OnEnable()
    {
        // Overwrite minimum window size
        minSize = MinimumWindowSize;
    }

    protected virtual void OnGUI()
    {
        DrawFields();
        GUILayout.Space(VerticalSpace);

        if (GUILayout.Button("Create"))
        {
            OnCreateClicked();
            Close();
        }
    }

    protected abstract void DrawFields();
    protected abstract void OnCreateClicked();
}
