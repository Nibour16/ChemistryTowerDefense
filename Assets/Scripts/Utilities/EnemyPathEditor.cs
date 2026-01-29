#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(EnemyPath))]
public class EnemyPathEditor : Editor
{
    private EnemyPath _path;

    private void OnEnable()
    {
        _path = (EnemyPath)target;
    }

    private void OnSceneGUI()
    {
        if (_path.PathPoints == null)
            return;

        Handles.color = Color.cyan;

        for (int i = 0; i < _path.PathPoints.Length; i++)
        {
            EditorGUI.BeginChangeCheck();

            Vector3 newPos = Handles.PositionHandle(
                _path.PathPoints[i],
                Quaternion.identity
            );

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_path, "Move Path Point");

                var pts = _path.PathPoints;
                pts[i] = newPos;
                _path.SetPoints(pts);

                EditorUtility.SetDirty(_path);
            }

            Handles.Label(newPos + Vector3.up * 0.2f, $"Point {i}");

            if (i < _path.PathPoints.Length - 1)
                Handles.DrawLine(newPos, _path.PathPoints[i + 1]);
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);

        if (GUILayout.Button("Add Point"))
        {
            Undo.RecordObject(_path, "Add Path Point");

            var list = _path.PathPoints?.ToList() ?? new System.Collections.Generic.List<Vector3>();

            Vector3 newPoint =
                list.Count > 0 ? list[list.Count - 1] + Vector3.forward : _path.transform.position;

            list.Add(newPoint);
            _path.SetPoints(list.ToArray());

            EditorUtility.SetDirty(_path);
        }

        if (GUILayout.Button("Remove Last Point"))
        {
            if (_path.PathPoints == null || _path.PathPoints.Length == 0)
                return;

            Undo.RecordObject(_path, "Remove Path Point");

            var list = _path.PathPoints.ToList();
            list.RemoveAt(list.Count - 1);
            _path.SetPoints(list.ToArray());

            EditorUtility.SetDirty(_path);
        }
    }
}
#endif