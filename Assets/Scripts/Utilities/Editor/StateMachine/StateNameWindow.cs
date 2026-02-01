using UnityEngine;
using UnityEditor;

public class StateNameWindow : BaseCreatorWindow
{
    protected override float VerticalSpace => 2f;
    protected override Vector2 MinimumWindowSize => new(300f, 50f);

    private System.Action<string, string> _onCreate;

    public static void ShowWindow(System.Action<string, string> onCreate)
    {
        var window = GetWindow<StateNameWindow>("Create State");
        window._onCreate = onCreate;
    }

    protected override void DrawFields()
    {
        primaryName = EditorGUILayout.TextField("State Name", primaryName);
        secondaryName = EditorGUILayout.TextField("State Machine Name", secondaryName);
    }

    protected override void OnCreateClicked()
    {
        _onCreate?.Invoke(primaryName, secondaryName);
    }
}
