using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    public float m_TurnSpd = 15f;
    Camera cam_Main;

    // Start is called before the first frame update
    void Start()
    {
        cam_Main = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yawCam = cam_Main.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, yawCam, 0f), m_TurnSpd * Time.fixedDeltaTime);
    }
}
