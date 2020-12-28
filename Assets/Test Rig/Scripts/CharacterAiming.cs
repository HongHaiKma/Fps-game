using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using ControlFreak2;
using Cinemachine;

public class CharacterAiming : MonoBehaviour
{
    public float m_TurnSpd = 15f;
    Camera cam_Main;
    public Rig r_AimLayer;

    [Header("---Components---")]
    public Transform tf_Owner;

    [Header("---Shoot---")]
    public GameObject g_Bullet;
    public float m_ShootCd;
    public float m_MaxShootCd;
    public int m_ShotBulet;

    [Header("Test")]
    public Transform tf_Start;
    public Transform tf_End;

    public Transform tf_Start1;
    public Transform tf_Start2;

    [Header("Test")]
    public Transform tf_FirePoint;
    public CinemachineFreeLook aaa;

    void Start()
    {
        cam_Main = Camera.main;
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Debug.DrawRay(tf_FirePoint.position, tf_FirePoint.forward * 40f, Color.red);

        Vector2 mouseInput = new Vector2(CF2Input.GetAxis("Mouse X"), CF2Input.GetAxis("Mouse Y")) * 0.5f;

        // aaa.m_XAxis.m_InputAxisValue = CF2Input.GetAxis("Mouse X") * 0.5f;
        // aaa.m_YAxis.m_InputAxisValue = CF2Input.GetAxis("Mouse Y") * 0.5f;

        if (mouseInput.magnitude > 0.1f)
        {
            aaa.m_XAxis.m_InputAxisValue = mouseInput.x;
            aaa.m_YAxis.m_InputAxisValue = mouseInput.y;
        }
        else
        {
            aaa.m_XAxis.m_InputAxisValue = 0f;
            aaa.m_YAxis.m_InputAxisValue = 0f;
        }

        // aaa.m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X");
        // aaa.m_YAxis.m_InputAxisValue = Input.GetAxis("Mouse Y");

        // if (Input.GetMouseButtonDown(0))
        // {
        //     Instantiate(g_Bullet, tf_FirePoint.position, tf_FirePoint.rotation);
        // }

        if (m_ShootCd < m_MaxShootCd)
        {
            m_ShootCd += Time.deltaTime;
        }

        if (CheckShoot())
        {
            OnShooting();
        }
    }

    public bool CanShoot(Vector3 _des)
    {
        return ((m_ShootCd >= m_MaxShootCd) && Helper.InRange(tf_Owner.position, _des, 15f));
    }

    public void OnShooting()
    {
        for (int i = 0; i < m_ShotBulet; i++)
        {
            Instantiate(g_Bullet, tf_FirePoint.position, tf_FirePoint.rotation);
        }

        m_ShootCd = 0f;
        m_ShotBulet = 1;
    }

    public bool CheckShoot()
    {
        RaycastHit hit;
        Debug.DrawRay(tf_FirePoint.position, tf_FirePoint.forward * 40f, Color.red);

        if (Physics.Raycast(tf_FirePoint.position, tf_FirePoint.forward * 40f, out hit))
        {
            ITakenDamage iTaken = hit.transform.GetComponent<ITakenDamage>();
            if (iTaken != null && CanShoot(hit.transform.position))
            {
                Debug.Log("Can shot!!!!!");
                Collider col = hit.transform.GetComponent<Collider>();
                return true;
            }

            return false;
        }

        return false;
    }

    // public bool CanShot(Vector3 _des)
    // {
    //     // return ((m_ShootCd >= m_MaxShootCd) && Helper.InRange(tf_Owner.position, _des, 15f));
    //     // return Helper.InRange(tf_Owner.position, _des, 15f));
    // }

    void FixedUpdate()
    {
        float camMain = cam_Main.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, camMain, 0f), m_TurnSpd * Time.fixedDeltaTime);
    }
}
