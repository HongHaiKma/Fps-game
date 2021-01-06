using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    //DISTANCE
    public static float CalDistance(Vector2 _origin, Vector2 _des)
    {
        return Vector2.Distance(_origin, _des);
    }

    public static float CalDistance(Vector3 _origin, Vector3 _des)
    {
        return Vector3.Distance(_origin, _des);
    }

    // public static float CalDistanceXZ(Vector3 _origin, Vector3 _des)
    // {
    //     Vector3 origin = new Vector3();
    //     Vector3 des = _des;

    //     return Vector3.Distance(origin, des);
    // }

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

    //ROTATION
    public static float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if (min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }

        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }

    //DEBUG
    public static void DebugLog(string mess)
    {
        Debug.Log(mess);
    }
}
