using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static float CalDistance(Vector2 origin, Vector2 des)
    {
        return Vector2.Distance(origin, des);
    }

    public static float CalDistance(Vector3 origin, Vector3 des)
    {
        return Vector3.Distance(origin, des);
    }

    public static bool InRange(Vector3 _origin, Vector3 _des, float _maxDistance)
    {
        return (Vector3.Distance(_origin, _des) <= _maxDistance);
    }

    public float CalDistance2(Vector2 origin, Vector2 des)
    {
        return (origin - des).magnitude;
    }

    public float CalDistance2(Vector3 origin, Vector3 des)
    {
        return Vector3.Distance(origin, des);
    }
}
