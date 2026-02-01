using UnityEngine;
using UnityEditor;
using System;

public class StateMachineNameWindow : BaseCreatorWindow
{
    protected override float VerticalSpace => 2f;
    protected override Vector2 MinimumWindowSize => new(300f, 50f);

    private Action<string> _onCreate;

    public static void ShowWindow(Action<string> onCreate) 
    { 
        var window = GetWindow<StateMachineNameWindow>("Create State Machine"); 
        window._onCreate = onCreate;
        window.primaryName = "NewStateMachine";
    }

    protected override void DrawFields()
    {
        primaryName = EditorGUILayout.TextField("State Machine Name", primaryName);
    }

    protected override void OnCreateClicked()
    {
        _onCreate?.Invoke(primaryName);
    }
}
