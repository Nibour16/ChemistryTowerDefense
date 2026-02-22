using UnityEditor;

public class StateCreator : BaseScriptCreator
{
    private string _stateName;
    private string _stateMachineName;

    protected override string Template =>
@"using UnityEngine;

public class {0} : BaseState
{{
    private {1} _stateMachine;

    #region Initialization
    public {0}(BaseStateMachine  stateMachine) : base(stateMachine) 
    {{
        ResolveDependencies();
    }}

    private void ResolveDependencies()
    {{
        _stateMachine = GetStateMachine<{1}>();
        if (_stateMachine == null)
        {{
            Debug.LogError($""{{nameof({0})}} requires {{nameof({1})}}"");
        }}
    }}
    #endregion

    #region State Lifecycle
    public override void EnterState() 
    {{
        // TODO: write logic when entering this state here
    }}

    public override void UpdateState() 
    {{
        // TODO: write logic during this state update here
    }}

    public override void ExitState() 
    {{
        // TODO: write logic when exiting this state here
    }}
    #endregion
}}";

    [MenuItem("Assets/Create/Scripting/State", priority = 80)]
    private static void OpenWindow()
    {
        StateNameWindow.ShowWindow((state, machine) =>
        {
            var creator = new StateCreator
            {
                _stateName = state,
                _stateMachineName = machine
            };

            creator.CreateScript();
        });
    }

    protected override string GetFileName() => _stateName;

    protected override object[] GetTemplateArgs()
        => new object[] { _stateName, _stateMachineName };
}

