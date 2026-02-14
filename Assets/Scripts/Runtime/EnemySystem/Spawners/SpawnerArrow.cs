using UnityEngine;

public class SpawnerArrow : MonoBehaviour
{
    [SerializeField] private float length = 2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 start = transform.position;
        Vector3 dir = transform.forward * length;

        Gizmos.DrawLine(start, start + dir);

        Vector3 right = Quaternion.Euler(0, 150, 0) * dir * 0.2f;
        Vector3 left = Quaternion.Euler(0, -150, 0) * dir * 0.2f;

        Gizmos.DrawLine(start + dir, start + dir + right);
        Gizmos.DrawLine(start + dir, start + dir + left);
    }
}
