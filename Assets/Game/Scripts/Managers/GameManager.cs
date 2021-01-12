using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        // #if UNITY_EDITOR
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
        // #endif
        // Input.multiTouchEnabled = false;
    }
}
