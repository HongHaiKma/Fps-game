using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using ControlFreak2;
using Cinemachine;
using UnityEngine.AI;
using Panda;

public class Character : InGameObject
{
    [Header("---Charcteristics---")]
    public TEAM m_Team;
    public bool m_AI;
    public BigNumber m_Dmg;
    public BigNumber m_Hp;

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

    [Header("---Range---")]
    public float m_ShootRange;
    public float m_ChaseRange;
    public float m_ChaseStopRange;

    [Header("---Cooldown---")]
    public float m_RotateCd;
    public float m_RotateCdMax;
    public float m_ShootCd;
    public float m_ShootCdMax;
    public float m_AimModelCd;
    public float m_AimModelCdMax;

    [Header("---Test---")]
    public Character m_Target;
    public SkinnedMeshRenderer m_SkinnedMesh;


    private void OnEnable()
    {
        ResetAllCooldown();
        // LoadCharacterConfig();
        // SetupComponents();
        StartListenToEvents();
    }

    private void OnDisable()
    {
        StopListenToEvents();
    }

    public void StartListenToEvents()
    {
        EventManagerWithParam<Vector3>.AddListener(GameEvent.SET_CHAR_CROSSHAIR_POS, SetOwnerCrosshairPos);
        EventManager.AddListener(GameEvent.SET_CHAR_TARGET, SetCharTarget);
    }

    public void StopListenToEvents()
    {
        EventManagerWithParam<Vector3>.RemoveListener(GameEvent.SET_CHAR_CROSSHAIR_POS, SetOwnerCrosshairPos);
        EventManager.RemoveListener(GameEvent.SET_CHAR_TARGET, SetCharTarget);
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
        m_Dmg = 10;
        m_Hp = 20;

        if (!m_AI)
        {
            m_ShootRange = 6.5f;
        }
        else
        {
            m_ShootRange = 6.5f;
        }

        m_ChaseRange = 9f;
        // m_ChaseRange = Mathf.Infinity;
        m_ChaseStopRange = 6f;

        m_RotateCdMax = 2.5f;
        m_AimModelCdMax = 4f;

        if (!m_AI)
        {
            m_ShootCdMax = 2f;
        }
        else
        {
            m_ShootCdMax = 2f;
        }
    }

    public void SetupComponents()
    {
        m_AI = true;
        nav_Agent.enabled = true;
        nav_Agent.stoppingDistance = m_ChaseStopRange;

        if (m_Team == TEAM.Team1)
        {
            m_SkinnedMesh.material = SpriteManager.Instance.m_MatsTest[0];
        }
        else if (m_Team == TEAM.Team2)
        {
            m_SkinnedMesh.material = SpriteManager.Instance.m_MatsTest[1];
        }
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

    [Task]
    public bool HasTarget()
    {
        return (m_Target != null);
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
        Vector2 mouseInput = new Vector2(CF2Input.GetAxis("Mouse X"), CF2Input.GetAxis("Mouse Y")) * 0.35f;
        // Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 0.35f;

        EventManagerWithParam<Vector2>.CallEvent(GameEvent.SET_CMLOOK_VALUE, mouseInput);

        if (Input.GetMouseButtonDown(0))
        {
            string bulletS = ConfigName.bullet1;
            BulletConfigData infor = new BulletConfigData(m_Dmg, bulletS, tf_FirePoint.rotation);
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
        anim_Onwer.SetFloat("InputY", 1);
        nav_Agent.SetDestination(m_Target.tf_Owner.position);
    }

    [Task]
    public bool CanChase()
    {
        // if (m_AI)
        // {
        //     Debug.Log(gameObject.name);
        //     Debug.Log("CAN CHASE!!!!!!!!!");
        // }
        // anim_Onwer.SetFloat("InputY", 1);
        // if (m_Team == TEAM1)
        // {

        // }

        // if (tf_Target == null)
        // {
        //     return false
        // }

        return Helper.InRange(tf_Owner.position, m_Target.tf_Owner.position, m_ChaseRange);
    }

    public void SetCharTarget()
    {
        if (m_Team == TEAM.Team1)
        {
            m_Target = InGameObjectsManager.Instance.GetRandomTeam2();
        }
        else if (m_Team == TEAM.Team2)
        {
            m_Target = InGameObjectsManager.Instance.GetRandomTeam1();
        }
    }
    #endregion

    #region SHOOTING
    [Task]
    public void OnShooting()
    {
        for (int i = 0; i < m_ShootBullet; i++)
        {
            string bulletS = ConfigName.bullet1;
            BulletConfigData infor = new BulletConfigData(m_Dmg, bulletS, tf_FirePoint.rotation);
            GameObject go = PrefabManager.Instance.SpawnBulletPool(infor.m_PrefabName, tf_FirePoint.position);
            Bullet bullet = go.GetComponent<Bullet>();
            bullet.SetupBullet(infor);

            if (!m_AI)
            {
                Debug.Log("OnShooting!!!");
            }
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

        // if (Input.GetKeyDown(KeyCode.B) && !m_AI)
        // {
        //     if (iTaken == null)
        //     {
        //         Debug.Log("Itaken null!");
        //         Debug.Log("Itaken name: " + hit[index].transform.name);
        //     }

        //     if (!Helper.InRange(tf_Owner.position, hit[index].transform.position, m_ShootRange))
        //     {
        //         Debug.Log("Not in range!");
        //     }
        // }

        Character charrr = hit[index].transform.GetComponent<Character>();



        if (iTaken != null && (m_ShootCd >= m_ShootCdMax) && Helper.InRange(tf_Owner.position, hit[index].transform.position, m_ShootRange))
        {
            if (charrr != null)
            {
                if (m_Team != charrr.m_Team)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

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

        Vector3 lookPos = m_Target.tf_Owner.position - tf_Owner.position;
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
        Character charTarget = m_Target;

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

    #region IDLING STAND

    [Task]
    public void OnIdlingStand()
    {
        // anim_Onwer.SetTrigger("AI_Idle");
        anim_Onwer.SetFloat("InputY", 0);
        anim_Onwer.SetFloat("InputX", 0);

        if (m_RotateCd >= m_RotateCdMax)
        {
            float angle = Random.Range(0f, 359f);
            StartCoroutine(RotateMe(angle, 1f));
            m_RotateCd = 0f;
        }
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

    #endregion

    #region IDLING ATTACK

    [Task]
    public bool CanStopChasing()
    {
        return Helper.InRange(tf_Owner.position, m_Target.tf_Owner.position, m_ChaseStopRange);
    }

    [Task]
    public void OnIdlingAttack()
    {
        anim_Onwer.SetFloat("InputY", 0);
        anim_Onwer.SetFloat("InputX", 0);
    }

    #endregion

    public override void OnHit()
    {

    }

    public override void OnHit(string _targetName)
    {

    }

    public override void OnHit(BigNumber _dmg, float _crit)
    {
        ApplyDamage(_dmg, _crit);
    }

    public void ApplyDamage(BigNumber _dmg, float _crit)
    {
        EventManager.CallEvent(GameEvent.SET_CHAR_TARGET);

        m_Hp -= _dmg * _crit;

        if (IsDead())
        {
            InGameObjectsManager.Instance.RemoveDeadChar(m_Team, this);
            PrefabManager.Instance.DespawnPool(gameObject);

            if (m_Team == TEAM.Team1)
            {
                Vector3 pos = ConfigManager.Instance.m_Team1StartPos[Random.Range(0, ConfigManager.Instance.m_Team1StartPos.Count - 1)];
                Character charrr = PrefabManager.Instance.SpawnCharPool(ConfigName.char1, pos).GetComponent<Character>();
                charrr.m_Team = TEAM.Team1;
                charrr.LoadCharacterConfig();
                charrr.SetupComponents();
                InGameObjectsManager.Instance.m_Team1.Add(charrr);
                // charrr.tf_Target = InGameObjectsManager.Instance.m_Team2[Random.Range(0, InGameObjectsManager.Instance.m_Team2.Count - 1)].tf_Owner;
            }
            else if (m_Team == TEAM.Team2)
            {
                Vector3 pos = ConfigManager.Instance.m_Team2StartPos[Random.Range(0, ConfigManager.Instance.m_Team2StartPos.Count - 1)];
                Character charrr = PrefabManager.Instance.SpawnCharPool(ConfigName.char1, pos).GetComponent<Character>();
                charrr.m_Team = TEAM.Team2;
                charrr.LoadCharacterConfig();
                charrr.SetupComponents();
                InGameObjectsManager.Instance.m_Team2.Add(charrr);
                // charrr.tf_Target = InGameObjectsManager.Instance.m_Team1[Random.Range(0, InGameObjectsManager.Instance.m_Team1.Count - 1)].tf_Owner;
            }

            if (!m_AI)
            {
                EventManagerWithParam<bool>.CallEvent(GameEvent.SET_CMLOOK_TARGET, false);
            }
        }
    }

    public bool IsDead()
    {
        return (m_Hp <= 0);
    }
}

public interface ITakenDamage
{
    void OnHit();
    void OnHit(BigNumber _dmg);
    void OnHit(BigNumber _dmg, float _crit);
    void OnHit(string _targetName);
    void OnHit(GameObject _go);
}

public enum AimBodyPart
{
    HEAD = 0,
    BODY = 1
}

public enum TEAM
{
    Team1 = 1,
    Team2 = 2
}