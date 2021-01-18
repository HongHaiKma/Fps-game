using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using ControlFreak2;
using Cinemachine;
using UnityEngine.AI;
using Panda;

public class Character : MonoBehaviour, ITakenDamage
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

    [Header("---Hit parts---")]
    public Transform tf_Head;
    public Transform tf_Body;
    public AimBodyPart m_AimBodyPart;

    [Header("---Shoot---")]
    public GameObject g_Bullet;
    public Transform tf_FirePoint;
    public Transform tf_Crosshair;
    public int m_ShootBullet;
    public int m_MaxBullet;
    public int m_CurrentBullet;
    public bool m_AI;

    [Header("---Range---")]
    public float m_ShootRange;
    public float m_ChaseRange;

    [Header("---Cooldown---")]
    public float m_RotateCd;
    public float m_RotateCdMax;
    public float m_ShootCd;
    public float m_MaxShootCd;
    public float m_AimModelCd;
    public float m_AimModelCdMax;

    [Header("---Test---")]
    public Transform tf_Target;


    private void OnEnable()
    {
        ResetAllCooldown();
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
        // SetMovingInput();
        // SetAimingInput();

        if (m_ShootCd < m_MaxShootCd)
        {
            m_ShootCd += Time.deltaTime;
        }

        if (m_RotateCd < m_RotateCdMax)
        {
            m_RotateCd += Time.deltaTime;
        }

        if (m_AimModelCd < m_AimModelCdMax)
        {
            m_AimModelCd += Time.deltaTime;
        }

        // if (CanShoot())
        // {
        //     OnShooting();
        // }

    }

    void FixedUpdate()
    {
        if (!m_AI)
        {
            float camMain = cam_Main.transform.rotation.eulerAngles.y;
            tf_Owner.rotation = Quaternion.Slerp(tf_Owner.rotation, Quaternion.Euler(0f, camMain, 0f), m_TurnSpd * Time.fixedDeltaTime);
        }
    }

    public void LoadCharacterConfig()
    {
        m_ShootRange = 6.5f;
        m_ChaseRange = 9f;

        m_RotateCdMax = 2.5f;
        m_AimModelCdMax = 4f;
    }

    public void ResetAllCooldown()
    {
        m_RotateCd = 0f;
        m_AimModelCd = 0f;
    }

    public void SetOwnerCrosshairPos(Vector3 _pos)
    {
        if (!m_AI)
        {
            tf_Crosshair.position = _pos;
        }
    }

    [Task]
    public bool IsAI()
    {
        return m_AI;
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

    #region CHASING
    [Task]
    public void OnChasing()
    {
        nav_Agent.SetDestination(tf_Target.position);
        // Debug.Log("ON CHASING!!!!");
    }

    [Task]
    public bool CanChase()
    {
        // if (m_AI)
        // {
        //     Debug.Log(gameObject.name);
        //     Debug.Log("CAN CHASE!!!!!!!!!");
        // }
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
                Character charTarget = hit.transform.GetComponent<Character>();
                // tf_Crosshair.position = charTarget.tf_Head.position;
                // Debug.Log("CANSHOOTTTTTTT");
                // tf_Crosshair.position = charTarget.tf_Body.position;
                // tf_Crosshair.position = tf_Body.position;

                // if (m_AimModelCd >= m_AimModelCdMax)
                // {
                //     if (Helper.Random2Probability(10))
                //     {
                //         tf_Crosshair.position = tf_Head.position;
                //     }

                //     m_AimModelCd = 0f;
                // }

                // if (!m_AI)
                // {
                //     Debug.Log("CAN SHOOT!!!");
                // }

                return true;
            }

            // if (iTaken != null)
            // {
            //     if (!m_AI)
            //     {
            //         Debug.Log("CAN SHOOT!!!");
            //     }
            // }

            return false;
        }

        return false;
    }
    #endregion

    #region AIMING

    [Task]
    public void OnAiming()
    {
        SetTargetBodyPart();
        SetCrosshairToBodyPartPos();

        Vector3 lookPos = tf_Target.position - tf_Owner.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        tf_Owner.rotation = Quaternion.Slerp(tf_Owner.rotation, rotation, Time.deltaTime * 5f);
    }

    public void SetTargetBodyPart()
    {
        if (m_AimModelCd >= m_AimModelCdMax)
        {
            if (Helper.Random2Probability(20))
            {
                m_AimBodyPart = AimBodyPart.HEAD;
            }
            else
            {
                m_AimBodyPart = AimBodyPart.BODY;
            }

            m_AimModelCd = 0f;
        }
    }

    public void SetCrosshairToBodyPartPos()
    {
        Character charTarget = tf_Target.GetComponent<Character>();

        switch (m_AimBodyPart)
        {
            case AimBodyPart.BODY:
                tf_Crosshair.position = charTarget.tf_Body.position;
                break;
            case AimBodyPart.HEAD:
                tf_Crosshair.position = charTarget.tf_Head.position;
                break;
        }
    }

    #endregion

    #region IDLING

    public float bbb = 0f;
    [Task]
    public void OnIdling()
    {
        // if (m_RotateCd >= m_RotateCdMax)
        // {
        //     Quaternion rotation = Helper.Random8Direction(tf_Owner.position);
        //     tf_Owner.rotation = Quaternion.Slerp(tf_Owner.rotation, rotation, Time.deltaTime * 5f);
        //     m_RotateCd = 0f;
        // }

        // if (m_RotateCd >= m_RotateCdMax)
        // {
        //     float angle = Random.Range(0f, 359f);
        //     // int a = Random.Range(0, 4);
        //     // switch (a)
        //     // {
        //     //     case 0:
        //     //         angle = Random.Range(0f, 90f);
        //     //         break;
        //     //     case 1:
        //     //         angle = Random.Range(0f, 90f);
        //     //         break;
        //     //     case 2:
        //     //         angle = Random.Range(0f, 90f);
        //     //         break;
        //     //     case 3:
        //     //         angle = Random.Range(0f, 90f);
        //     //         break;
        //     // }

        //     Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);



        //     if (bbb < 1)
        //     {
        //         bbb += Time.deltaTime;
        //         // tf_Owner.rotation = Quaternion.Slerp(tf_Owner.rotation, rotation, Time.deltaTime * 10);
        //         // tf_Owner.rotation = Quaternion.
        //     }
        //     else
        //     {
        //         bbb = 0f;
        //         m_RotateCd = 0f;
        //     }
        // }

        if (m_RotateCd >= m_RotateCdMax)
        {
            float angle = Random.Range(0f, 359f);
            StartCoroutine(RotateMe(angle, 1f));
            m_RotateCd = 0f;
        }

        // Debug.Log("ON IDLING!!!!!!!!!");
    }

    IEnumerator RotateMe(float byAngles, float inTime)
    {
        var fromAngle = tf_Owner.rotation;
        // var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        Quaternion toAngle = Quaternion.AngleAxis(byAngles, Vector3.up);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
    }

    public virtual void OnHit()
    {

    }

    public virtual void OnHit(string _targetName)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // Helper.DebugLog("Enemy hit!!!");
    }

    #endregion
}

public interface ITakenDamage
{
    void OnHit();
    void OnHit(string _targetName);
}

public enum AimBodyPart
{
    HEAD = 0,
    BODY = 1
}