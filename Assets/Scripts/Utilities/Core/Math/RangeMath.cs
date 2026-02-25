using UnityEngine;

public static class RangeMath
{
    public static bool InSphere(Vector3 center, Vector3 point, float radius)
    {
        return (point - center).sqrMagnitude <= radius * radius;
    }
}

