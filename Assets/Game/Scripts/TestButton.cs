using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PPManager.Instance.DashLens();
        }
    }
}
