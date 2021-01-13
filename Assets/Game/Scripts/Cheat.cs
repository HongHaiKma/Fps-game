using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public void OpenDebugger()
    {
        SRDebug.Instance.ShowDebugPanel();
    }
}
