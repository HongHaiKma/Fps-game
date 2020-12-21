using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float m_TurnSpd = 15f;
    Camera cam_Main;
    public Rig r_AimLayer;

    [Header("Test")]
    public Transform tf_Start;
    public Transform tf_End;

    public Transform tf_Start1;
    public Transform tf_Start2;

    void Start()
    {
        cam_Main = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Debug.DrawLine(tf_Start.position, tf_End.position, Color.red);
        Debug.DrawLine(tf_Start1.position, tf_Start2.position, Color.blue);

        Ray ray = new Ray(transform.position, (tf_Start1.position - tf_Start2.position));
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
    }


    void FixedUpdate()
    {
        float yawCam = cam_Main.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, yawCam, 0f), m_TurnSpd * Time.fixedDeltaTime);
    }
}
