using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // PrefabManager.Instance.SpawnVFXPool("GreenBuff", new Vector3(0f, 3.5f, 0f));
        }
    }
}
