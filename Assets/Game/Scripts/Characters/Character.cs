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
    public LayerMask m_LayerMask;

    [Header("---Animation Rigging---")]
    public Rig r_AimLayer;
    public MultiAimConstraint m_MultiAimConstraint;

    [Header("---Camera---")]
    Camera cam_Main;
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
    public float m_ShootCdMax;
    public float m_AimModelCd;
    public float m_AimModelCdMax;

    [Header("---Test---")]
    public Transform tf_Target;


    private void OnEnable()
    {
        ResetAllCooldown();
        LoadCharacterConfig();
        StartListenToEvents();
    }

    private void OnDisable()
    {
        StopListenToEvents();
    }

    public void StartListenToEvents()
    {
        EventManagerWithParam<Vector3>.AddListener(GameEvent.SET_CHAR_CROSSHAIR_POS, SetOwnerCrosshairPos);
    }

    public void StopListenToEvents()
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

        if (m_ShootCd < m_ShootCdMax)
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

        m_ShootCdMax = 2f;
    }

    public void ResetAllCooldown()
    {
        m_RotateCd = 0f;
        m_AimModelCd = 0f;
        m_ShootCd = 0f;
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

        tf_Owner.position += (tf_Owner.right * moveInput.x + tf_Owner.forward * moveInput.y) * Time.deltaTime * 5f;
        // tf_Owner.position += tf_Owner.forward * Time.deltaTime * moveInput.y * 5f;

        // if (moveInput.x > 0)
        // {
        //     tf_Owner.position += tf_Owner.right * Time.deltaTime * 5f;
        // }
        // if (moveInput.x < 0)
        // {
        //     tf_Owner.position -= tf_Owner.right * Time.deltaTime * 5f;
        // }
        // if (moveInput.y > 0)
        // {
        //     tf_Owner.position += tf_Owner.forward * Time.deltaTime * 5f;
        // }
        // if (moveInput.y < 0)
        // {
        //     tf_Owner.position -= tf_Owner.forward * Time.deltaTime * 5f;
        // }
    }

    [Task]
    public void SetAimingInput()
    {
        // Vector2 mouseInput = new Vector2(CF2Input.GetAxis("Mouse X"), CF2Input.GetAxis("Mouse Y")) * 0.35f;
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 0.35f;

        EventManagerWithParam<Vector2>.CallEvent(GameEvent.Set_CMLOOK_VALUE, mouseInput);

        if (Input.GetMouseButtonDown(0))
        {
            BulletInfor infor = new BulletInfor("Bullet_Sniper", tf_FirePoint.rotation);
            GameObject go = PrefabManager.Instance.SpawnBulletPool(infor.m_PrefabName, tf_FirePoint.position);
            Bullet bullet = go.GetComponent<Bullet>();
            bullet.SetupBullet(infor);
        }
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
            BulletInfor infor = new BulletInfor("Bullet_Sniper", tf_FirePoint.rotation);
            GameObject go = PrefabManager.Instance.SpawnBulletPool(infor.m_PrefabName, tf_FirePoint.position);
            Bullet bullet = go.GetComponent<Bullet>();
            bullet.SetupBullet(infor);

            // Debug.Log("OnShooting!!!");
        }

        m_ShootCd = 0f;
        m_ShootBullet = 1;
    }

    [Task]
    public bool CanShoot()
    {
        Debug.DrawRay(tf_FirePoint.position, tf_FirePoint.forward * 10f, Color.red);
        RaycastHit[] hit = Physics.RaycastAll(tf_FirePoint.position, tf_FirePoint.forward * 10f);
        int hitCount = hit.Length;

        if (hitCount <= 0)
        {
            return false;
        }

        int index = 0;
        for (int i = 0; i < hitCount; i++)
        {
            if ((m_LayerMask.value & (1 << hit[i].transform.gameObject.layer)) > 0)
            {
                index = i;
            }
        }

        ITakenDamage iTaken = hit[index].transform.GetComponent<ITakenDamage>();

        if (Input.GetKeyDown(KeyCode.B) && !m_AI)
        {
            if (iTaken == null)
            {
                Debug.Log("Itaken null!");
                Debug.Log("Itaken name: " + hit[index].transform.name);
            }

            if (!Helper.InRange(tf_Owner.position, hit[index].transform.position, m_ShootRange))
            {
                Debug.Log("Not in range!");
            }
        }

        if (iTaken != null && (m_ShootCd >= m_ShootCdMax) && Helper.InRange(tf_Owner.position, hit[index].transform.position, m_ShootRange))
        {
            return true;
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