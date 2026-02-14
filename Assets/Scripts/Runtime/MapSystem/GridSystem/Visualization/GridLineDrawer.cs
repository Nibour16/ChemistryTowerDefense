using UnityEngine;

public class GridLineDrawer : MonoBehaviour
{
    [Header("Grid Line Visual")]
    [SerializeField] private Color gridLineColor = Color.black;
    [Range(0.01f, 0.2f)]
    [SerializeField] private float gridLineWidth = 0.03f;

    [Header("Advanced Setting")]
    [SerializeField] private string lineXNamePrefix = "Line_X_";
    [SerializeField] private string lineZNamePrefix = "Line_Z_";

    public void DrawLines(Transform root, Vector3 corner, float y, float cell, int width, int height)
    {
        // Lines parallel to Z
        for (int x = 0; x <= width; x++)
        {
            var a = new Vector3(corner.x + x * cell, y, corner.z);
            var b = new Vector3(corner.x + x * cell, y, corner.z + height * cell);
            var lineName = GetLineName(lineXNamePrefix, x);

            CreateLine(root, lineName, a, b);
        }

        // Lines parallel to X
        for (int z = 0; z <= height; z++)
        {
            var a = new Vector3(corner.x, y, corner.z + z * cell);
            var b = new Vector3(corner.x + width * cell, y, corner.z + z * cell);
            var lineName = GetLineName(lineZNamePrefix, z);

            CreateLine(root, lineName, a, b);
        }
    }

    private string GetLineName(string prefix, int value)
    {
        return prefix + value;
    }

    private void CreateLine(Transform parent, string name, Vector3 a, Vector3 b)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);

        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.positionCount = 2;
        lr.SetPosition(0, a);
        lr.SetPosition(1, b);

        lr.startWidth = gridLineWidth;
        lr.endWidth = gridLineWidth;

        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = gridLineColor;

        lr.gameObject.layer = parent.gameObject.layer;
    }
}
