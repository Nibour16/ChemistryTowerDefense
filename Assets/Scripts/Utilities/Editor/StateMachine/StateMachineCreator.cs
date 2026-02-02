using UnityEditor;

public class StateMachineCreator : BaseScriptCreator
{
    private string _stateMachineName = "NewStateMachine";

    protected override string Template =>
@"using UnityEngine;

public class {0} : BaseStateMachine
{{
    #region Keep track of all states
    // TODO: Create the desired states here, ensure they are all non-serialized and private
    #endregion

    #region Referencing all of states
    // TODO: Create the desired state properties here
    #endregion

    #region Keep track of all supporting components
    //  TODO: Create supporting components here, ensure they are all private
    #endregion

    #region Referencing all supporting components
    //  TODO: Create supporting components here
    #endregion

    private void Awake()
    {{
        // TODO: Initialize states and components here
        // Example: _someState = new SomeState(this);
    }}

    private void Start()
    {{
        // TODO: Set initial state
        // Format will be like this: SetState(new SomeState(this));
    }}
}}";

    protected override string GetFileName()
    {
        return _stateMachineName;
    }

    protected override object[] GetTemplateArgs()
    {
        // {0} ¡ú class name
        return new object[]
        {
            GetFileName()
        };
    }

    [MenuItem("Assets/Create/Scripting/State Machine")]
    private static void CreateStateMachine()
    {
        var creator = new StateMachineCreator();

        StateMachineNameWindow.ShowWindow
        (
            name =>
            {
                creator._stateMachineName = name;
                creator.CreateScript();
            }
        );
    }
}
