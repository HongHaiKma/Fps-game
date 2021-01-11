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

    public static Quaternion Random8Direction(Vector3 _ownerPos)
    {
        // Vector3 dir = new Vector3();
        // int a = Random.Range(0, 8);
        // switch (a)
        // {
        //     case 0:
        //         dir = new Vector3(1, 0, 0);
        //         break;
        //     case 1:
        //         dir = new Vector3(-1, 0, 0);
        //         break;
        //     case 2:
        //         dir = new Vector3(0, 0, 1);
        //         break;
        //     case 3:
        //         dir = new Vector3(0, 0, -1);
        //         break;
        //     case 4:
        //         dir = new Vector3(1, 0, 1);
        //         break;
        //     case 5:
        //         dir = new Vector3(-1, 0, -1);
        //         break;
        //     case 6:
        //         dir = new Vector3(1, 0, -1);
        //         break;
        //     case 7:
        //         dir = new Vector3(-1, 0, 1);
        //         break;

        // }

        Vector3 dir = new Vector3();
        int a = Random.Range(0, 4);
        switch (a)
        {
            case 0:
                dir = Vector3.left;
                break;
            case 1:
                dir = Vector3.right;
                break;
            case 2:
                dir = Vector3.forward;
                break;
            case 3:
                dir = Vector3.back;
                break;
        }

        // dir = tf_Owner.position + new Vector3(5, 0, 0);

        Vector3 dir2 = _ownerPos + dir;

        Vector3 lookPos = dir2 - _ownerPos;

        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);

        return rotation;
    }

    //DEBUG
    public static void DebugLog(string mess)
    {
        Debug.Log(mess);
    }

    public static void DebugLog(float mess)
    {
        Debug.Log(mess);
    }

    public static void DebugLog(int mess)
    {
        Debug.Log(mess);
    }
}
