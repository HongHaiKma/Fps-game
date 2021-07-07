using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestCallback : MonoBehaviour
{
    private void Start()
    {
        TestFunc(

        );
    }

    public void TestFunc(UnityAction _callback = null)
    {
        _callback();
    }
}
