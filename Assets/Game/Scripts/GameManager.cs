using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;
        // Input.multiTouchEnabled = false;
    }
}
