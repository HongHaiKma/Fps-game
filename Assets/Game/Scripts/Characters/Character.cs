using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using ControlFreak2;
using Cinemachine;

public class Character : MonoBehaviour
{
    public float m_TurnSpd = 15f;

    [Header("---Animation Rigging---")]
    public Rig r_AimLayer;
    public MultiAimConstraint m_MultiAimConstraint;

    [Header("---Camera---")]
    Camera cam_Main;
    public CinemachineFreeLook m_CinemachineFreeLook;

    [Header("---Components---")]
    public Transform tf_Owner;
    public Animator anim_Onwer;

    [Header("---Shoot---")]
    public GameObject g_Bullet;
    public float m_ShootCd;
    public float m_MaxShootCd;
    public int m_ShotBulet;
    public Transform tf_FirePoint;

    void Start()
    {
        cam_Main = Camera.main;

#if UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }

    private void Update()
    {
        // Debug.DrawRay(tf_FirePoint.position, tf_FirePoint.forward * 40f, Color.red);

        SetMovingInput();

        SetAimingInput();
    }

    public void SetMovingInput()
    {
        Vector2 moveInput = new Vector2(CF2Input.GetAxis("Joystick Move X"), CF2Input.GetAxis("Joystick Move Y"));

        anim_Onwer.SetFloat("InputX", moveInput.x);
        anim_Onwer.SetFloat("InputY", moveInput.y);
    }

    public void SetAimingInput()
    {
#if UNITY_ANDROID
        Vector2 mouseInput = new Vector2(CF2Input.GetAxis("Mouse X"), CF2Input.GetAxis("Mouse Y")) * 0.35f;

        if (mouseInput.magnitude > 0.015f)
        {
            m_CinemachineFreeLook.m_XAxis.m_InputAxisValue = mouseInput.x;
            m_CinemachineFreeLook.m_YAxis.m_InputAxisValue = mouseInput.y;
        }
        else
        {
            m_CinemachineFreeLook.m_XAxis.m_InputAxisValue = 0f;
            m_CinemachineFreeLook.m_YAxis.m_InputAxisValue = 0f;
        }

        if (m_ShootCd < m_MaxShootCd)
        {
            m_ShootCd += Time.deltaTime;
        }

        if (CheckShoot())
        {
            OnShooting();
        }
#endif

#if UNITY_EDITOR
        m_CinemachineFreeLook.m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X");
        m_CinemachineFreeLook.m_YAxis.m_InputAxisValue = Input.GetAxis("Mouse Y");

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(g_Bullet, tf_FirePoint.position, tf_FirePoint.rotation);
        }
#endif
    }

    public bool CanShoot(Vector3 _des)
    {
        return ((m_ShootCd >= m_MaxShootCd) && Helper.InRange(tf_Owner.position, _des, 50f));
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
        Debug.DrawRay(tf_FirePoint.position, tf_FirePoint.forward * 80f, Color.red);

        if (Physics.Raycast(tf_FirePoint.position, tf_FirePoint.forward * 80f, out hit))
        {
            ITakenDamage iTaken = hit.transform.GetComponent<ITakenDamage>();
            if (iTaken != null && CanShoot(hit.transform.position))
            {
                Collider col = hit.transform.GetComponent<Collider>();
                return true;
            }

            return false;
        }

        return false;
    }

    void FixedUpdate()
    {
        float camMain = cam_Main.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, camMain, 0f), m_TurnSpd * Time.fixedDeltaTime);
    }
}
