using UnityEditor;

public class StateCreator : BaseScriptCreator
{
    private string _stateName;
    private string _stateMachineName;

    protected override string Template =>
@"using UnityEngine;

public class {0} : BaseState
{{
    public {0}({1} stateMachine) : base(stateMachine) {{ }}

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

