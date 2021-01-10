using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using ControlFreak2;
using Cinemachine;
using UnityEngine.AI;
using Panda;

public class Character : MonoBehaviour
{
    [Header("---Components---")]
    public Transform tf_Owner;
    public Animator anim_Onwer;
    public NavMeshAgent nav_Agent;

    [Header("---Animation Rigging---")]
    public Rig r_AimLayer;
    public MultiAimConstraint m_MultiAimConstraint;

    [Header("---Camera---")]
    Camera cam_Main;
    public CinemachineFreeLook m_CinemachineFreeLook;
    public float m_TurnSpd = 15f;

    [Header("---Shoot---")]
    public GameObject g_Bullet;
    public float m_ShootCd;
    public float m_MaxShootCd;
    public int m_ShootBullet;
    public int m_MaxBullet;
    public int m_CurrentBullet;
    public bool m_AI;
    public Transform tf_FirePoint;

    [Header("---Test---")]
    public Transform tf_Target;
    public Transform tf_CrosshairOwner;

    [Header("---Range---")]
    public float m_ShootRange;
    public float m_ChaseRange;

    private void OnEnable()
    {
        LoadCharacterConfig();
        StartListenToEvent();
    }

    private void OnDisable()
    {
        StopListenToEvent();
    }

    public void StartListenToEvent()
    {
        EventManagerWithParam<Vector3>.AddListener(GameEvent.SET_CHAR_CROSSHAIR_POS, SetOwnerCrosshairPos);
    }

    public void StopListenToEvent()
    {
        EventManagerWithParam<Vector3>.RemoveListener(GameEvent.SET_CHAR_CROSSHAIR_POS, SetOwnerCrosshairPos);
    }

    void Start()
    {
        cam_Main = Camera.main;
    }

    private void Update()
    {
        SetMovingInput();
        SetAimingInput();

        if (m_ShootCd < m_MaxShootCd)
        {
            m_ShootCd += Time.deltaTime;
        }

        if (CanShoot())
        {
            OnShooting();
        }
    }

    public void LoadCharacterConfig()
    {
        m_ShootRange = 6.5f;
        m_ChaseRange = 13f;
    }

    #region PLAYER INPUT

    [Task]
    public void SetMovingInput()
    {
        Vector2 moveInput = new Vector2(CF2Input.GetAxis("Joystick Move X"), CF2Input.GetAxis("Joystick Move Y"));

        anim_Onwer.SetFloat("InputX", moveInput.x);
        anim_Onwer.SetFloat("InputY", moveInput.y);
    }

    [Task]
    public void SetAimingInput()
    {
        // #if UNITY_ANDROID
        // Vector2 mouseInput = new Vector2(CF2Input.GetAxis("Mouse X"), CF2Input.GetAxis("Mouse Y")) * 0.35f;

        // if (mouseInput.magnitude > 0.015f)
        // {
        //     m_CinemachineFreeLook.m_XAxis.m_InputAxisValue = mouseInput.x;
        //     m_CinemachineFreeLook.m_YAxis.m_InputAxisValue = mouseInput.y;
        // }
        // else
        // {
        //     m_CinemachineFreeLook.m_XAxis.m_InputAxisValue = 0f;
        //     m_CinemachineFreeLook.m_YAxis.m_InputAxisValue = 0f;
        // }

        // if (m_ShootCd < m_MaxShootCd)
        // {
        //     m_ShootCd += Time.deltaTime;
        // }

        // if (CanShoot())
        // {
        //     OnShooting();
        // }

        // #elif UNITY_EDITOR

        m_CinemachineFreeLook.m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X");
        m_CinemachineFreeLook.m_YAxis.m_InputAxisValue = Input.GetAxis("Mouse Y");

        // if (Input.GetMouseButtonDown(0))
        // {
        //     Instantiate(g_Bullet, tf_FirePoint.position, tf_FirePoint.rotation);
        // }
        // #endif
    }

    #endregion

    public void SetOwnerCrosshairPos(Vector3 _pos)
    {
        if (!m_AI)
        {
            tf_CrosshairOwner.position = _pos;
        }
    }

    [Task]
    public bool IsAI()
    {
        return m_AI;
    }

    #region CHASING
    [Task]
    public void OnChasing()
    {
        nav_Agent.SetDestination(tf_Target.position);
    }

    [Task]
    public bool CanChase()
    {
        return Helper.InRange(tf_Owner.position, tf_Target.position, m_ChaseRange);
    }
    #endregion

    #region SHOOTING
    [Task]
    public void OnShooting()
    {
        for (int i = 0; i < m_ShootBullet; i++)
        {
            Instantiate(g_Bullet, tf_FirePoint.position, tf_FirePoint.rotation);
        }

        m_ShootCd = 0f;
        m_ShootBullet = 1;
    }

    [Task]
    public bool CanShoot()
    {
        RaycastHit hit;
        Debug.DrawRay(tf_FirePoint.position, tf_FirePoint.forward * 10f, Color.red);
        // Debug.DrawLine(tf_Owner.position, tf_CamCrosshair.position, Color.green);

        if (Physics.Raycast(tf_FirePoint.position, tf_FirePoint.forward, out hit))
        {
            ITakenDamage iTaken = hit.transform.GetComponent<ITakenDamage>();

            if (iTaken != null && (m_ShootCd >= m_MaxShootCd) && Helper.InRange(tf_Owner.position, hit.transform.position, m_ShootRange))
            {
                Collider col = hit.transform.GetComponent<Collider>();
                tf_CrosshairOwner.position = hit.point;
                return true;
            }

            return false;
        }

        return false;
    }
    #endregion

    #region  AIMING

    [Task]
    public void OnAiming()
    {
        tf_CrosshairOwner.position = tf_Target.position;

        var lookPos = tf_Target.position - tf_Owner.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        tf_Owner.rotation = Quaternion.Slerp(tf_Owner.rotation, rotation, Time.deltaTime * 5f);
    }

    #endregion

    void FixedUpdate()
    {
        float camMain = cam_Main.transform.rotation.eulerAngles.y;
        tf_Owner.rotation = Quaternion.Slerp(tf_Owner.rotation, Quaternion.Euler(0f, camMain, 0f), m_TurnSpd * Time.fixedDeltaTime);
    }
}
