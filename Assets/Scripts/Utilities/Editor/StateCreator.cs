using UnityEngine;
using UnityEditor;
using System.IO;

public class StateCreator
{
    // In C#, if you use single curly brackets (e.g. {}), that is placeholder.
    // We need actual brackets look when the creator construct new state class.
    // The correct format in c# will be using double curly bracket (e.g. {{}}),
    // So that the output will be like this: {}
    // Therefore initial state class format will look like this:
    private const string TEMPLATE = @"using UnityEngine;

public class {0} : BaseState
{{
    public {0}({1} stateMachine) : base(stateMachine) {{ }}

    public override void EnterState()
    {{
        
    }}

    public override void UpdateState()
    {{
        
    }}

    public override void ExitState()
    {{
        
    }}
}}
";  

    [MenuItem("Assets/Create/NewState")]
    public static void CreateNewState()
    {
        // Setup default input name
        string stateName = "NewState";
        string machineName = "StateMachine";

        // Display input window
        StateNameWindow.ShowWindow((name, machine) =>
        {
            stateName = name;
            machineName = machine;

            string folder = "Assets";
            if (Selection.activeObject != null)
            {
                // Define file location
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                if (Directory.Exists(path)) //is an exist folder?
                    folder = path;
                else
                    folder = Path.GetDirectoryName(path);
            }

            // Refresh and create file
            string pathToCreate = Path.Combine(folder, stateName + ".cs");

            if (File.Exists(pathToCreate))  //If current state is already exist
            {
                EditorUtility.DisplayDialog("Error", "File already exists!", "OK");
                return;
            }

            string content = string.Format(TEMPLATE, stateName, machineName);
            File.WriteAllText(pathToCreate, content);
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(pathToCreate);
        });
    }

    //This class handles window UI display that allows user to interact with
    public class StateNameWindow : EditorWindow
    {
        //Initial inputs
        private string stateName = "NewState";
        private string machineName = "StateMachine";
        private System.Action<string, string> callback;

        public static void ShowWindow(System.Action<string, string> callback)   //Display window method
        {
            StateNameWindow window = ScriptableObject.CreateInstance<StateNameWindow>();
            window.titleContent = new GUIContent("Create State");
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 100);
            window.callback = callback;
            window.ShowUtility();
        }

        private void OnGUI()    //Window UI logic and update
        {
            GUILayout.Label("State Name", EditorStyles.boldLabel);
            stateName = EditorGUILayout.TextField(stateName);

            GUILayout.Label("StateMachine Name", EditorStyles.boldLabel);
            machineName = EditorGUILayout.TextField(machineName);

            GUILayout.Space(10);

            if (GUILayout.Button("Create"))
            {
                if (!string.IsNullOrEmpty(stateName) && !string.IsNullOrEmpty(machineName))
                {
                    callback?.Invoke(stateName, machineName);
                    Close();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please enter valid names", "OK");
                }
            }
        }
    }
}

