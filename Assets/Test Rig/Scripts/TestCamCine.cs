using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamCine : MonoBehaviour
{
    public Transform tf_Owner;
    public Transform tf_End;

    void Update()
    {
        Debug.DrawLine(tf_Owner.position, tf_End.position, Color.green);
    }
}
