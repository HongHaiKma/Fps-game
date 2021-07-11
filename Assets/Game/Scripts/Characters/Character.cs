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
    [Header("---Components---")]
    public Rigidbody rb_Owner;
    public Transform tf_Owner;
    public Animator anim_Onwer;
    public NavMeshAgent nav_Agent;
    public LayerMask m_LayerMask;
    public HealthBar m_HealthBar;
    public CharacterController cc_Owner;
    // public MiniIcon m_MiniIcon;

    [Header("---Charcteristics---")]
    // public TEAM m_Team;
    public bool m_AI;
    private float m_MoveSpd;
    public CharState m_CharState;
    public BigNumber m_Dmg;
    public BigNumber m_Hp;
    public BigNumber m_HpMax;
    public StateMachine<Character> m_StateMachine;
    public Flag m_Flag;
    public CharacterModelPart m_HeadPart;
    public CharacterModelPart m_BodyPart;

    [Header("---Camera---")]
    Camera cam_Main;
    public float m_TurnSpd = 15f;

    [Header("---Hit parts---")]
    public Transform tf_Head;
    public Transform tf_Body;
    public AimBodyPart m_AimBodyPart;

    [Header("---Shoot---")]
    public Transform tf_CheckShootPoint;
    public Transform tf_FirePoint;
    public Transform tf_Crosshair;
    public Transform tf_CrosshairAim;
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

    public float m_StopChaseCd;
    public float m_StopChaseCdMax;

    public float m_ShootFlagCd;
    public float m_ShootFlagCdMax;

    public float m_DeathCd;
    public float m_DeathCdMax;

    [Header("---Test---")]
    public Character m_Target;
    private Vector3 m_OldPos;
    private Vector3 m_NewPos;
    public Vector3 v3_KnockBackDir;


    public float m_DistanceToTarget;


    public virtual void OnEnable()
    {
        m_StateMachine = new StateMachine<Character>(this);
        m_StateMachine.Init(StandState.Instance);

        m_OldPos = tf_Owner.position;
        m_NewPos = tf_Owner.position;

        ResetAllCooldown();
        AddListener();
    }

    public virtual void OnDisable()
    {
        RemoveListener();
    }

    public virtual void AddListener()
    {
        EventManager1<Vector3>.AddListener(GameEvent.SET_CHAR_CROSSHAIR_AIM_POS, SetOwnerCrosshairAimPos);
        EventManager1<Vector3>.AddListener(GameEvent.SET_CHAR_CROSSHAIR_POS, SetOwnerCrosshairPos);
        EventManager.AddListener(GameEvent.SET_CHAR_TARGET, SetCharTarget);
        EventManager.AddListener(GameEvent.DESPAWN, Despawn);
        EventManager.AddListener(GameEvent.SET_HP_BAR_UI, Event_SET_HP_BAR_UI);
    }

    public virtual void RemoveListener()
    {
        EventManager1<Vector3>.RemoveListener(GameEvent.SET_CHAR_CROSSHAIR_AIM_POS, SetOwnerCrosshairAimPos);
        EventManager1<Vector3>.RemoveListener(GameEvent.SET_CHAR_CROSSHAIR_POS, SetOwnerCrosshairPos);
        EventManager.RemoveListener(GameEvent.SET_CHAR_TARGET, SetCharTarget);
        EventManager.RemoveListener(GameEvent.DESPAWN, Despawn);
        EventManager.RemoveListener(GameEvent.SET_HP_BAR_UI, Event_SET_HP_BAR_UI);
    }

    public void Despawn()
    {
        PrefabManager.Instance.DespawnPool(gameObject);
    }

    void Start()
    {
        cam_Main = Camera.main;
    }

    public virtual void Update()
    {
        HandleCooldown();

        m_StateMachine.ExecuteStateUpdate();

        if (m_Target != null)
        {
            m_DistanceToTarget = Helper.CalDistance(tf_Owner.position, m_Target.tf_Owner.position);
        }
    }

    public void Event_SET_HP_BAR_UI()
    {
        if (!IsAI())
        {
            InGameManager.Instance.m_HealthBarUI.SetHealthBar(GetHpPercentage());
        }
    }

    public void HandleCooldown()
    {
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

    public bool IsMoving()
    {
        m_OldPos = tf_Owner.position;

        if ((m_OldPos - m_NewPos).magnitude > 0.2f)
        {
            m_NewPos = m_OldPos;
            return true;
        }

        return false;
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
        m_Dmg = 0f;
        m_HpMax = GetMaxHP();
        m_Hp = m_HpMax;

        m_ShootRange = 20.5f;

        m_ChaseRange = 14f;
        // m_ChaseRange = Mathf.Infinity;
        m_ChaseStopRange = 15f;

        m_RotateCdMax = 2.5f;
        m_AimModelCdMax = 4f;

        m_ShootCd = 0.29f;
        m_ShootCdMax = 0.5f;

        m_StopChaseCdMax = 3f;

        m_ShootFlagCd = 0f;
        m_ShootFlagCdMax = 12f;

        m_DeathCdMax = 2f;

        m_MoveSpd = 5f;
    }

    public void SetupComponents()
    {
        if (m_Team == TEAM.Team1)
        {
            m_Flag = ObjectsManager.Instance.m_Map.m_Flag2;
        }
        else if (m_Team == TEAM.Team2)
        {
            m_Flag = ObjectsManager.Instance.m_Map.m_Flag1;
        }

        m_AI = true;
        SetNavMeshStopRange(m_ChaseStopRange);
        SetNavMeshSpeed(m_MoveSpd);
    }

    public void ResetAllCooldown()
    {
        m_RotateCd = 0f;
        m_AimModelCd = 0f;
        m_ShootCd = 0f;
        m_StopChaseCd = 0f;
    }

    public void SetOwnerCrosshairAimPos(Vector3 _pos)
    {
        if (!m_AI)
        {
            tf_CrosshairAim.position = _pos;
            // tf_Crosshair.position = _pos;
        }
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
        Vector3 moveInput = new Vector3(CF2Input.GetAxis("Joystick Move X"), Physics.gravity.y, CF2Input.GetAxis("Joystick Move Y"));

        anim_Onwer.SetFloat("InputX", moveInput.x);
        anim_Onwer.SetFloat("InputY", moveInput.z);

        moveInput = moveInput.normalized;
        moveInput = tf_Owner.TransformDirection(moveInput);

        HandleMoving(moveInput);
    }

    public void HandleMoving(Vector3 _moveInput)
    {
        float gravity = 0f;

        gravity -= 9.81f * Time.deltaTime;
        cc_Owner.Move(new Vector3(_moveInput.x, gravity, _moveInput.z * 1.2f));
        if (cc_Owner.isGrounded)
        {
            gravity = 0;
        }
    }

    [Task]
    public virtual void SetAimingInput()
    {
        Vector2 mouseInput = new Vector2(CF2Input.GetAxis("Mouse X"), CF2Input.GetAxis("Mouse Y")) * 0.35f;

        EventManager1<Vector2>.CallEvent(GameEvent.SET_CMLOOK_VALUE, mouseInput);

        ShootByMouse();
    }

    public virtual void ShootByMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            string bulletS = ConfigName.m_Bullet;
            BulletConfigData infor;
            if (!m_AI)
            {
                infor = new BulletConfigData(m_Team, m_Dmg, bulletS, tf_CrosshairAim);
            }
            else
            {
                infor = new BulletConfigData(m_Team, m_Dmg, bulletS, tf_Crosshair);
            }
            GameObject go = PrefabManager.Instance.SpawnBulletPool(infor.m_PrefabName, tf_FirePoint.position);
            Bullet bullet = go.GetComponent<Bullet>();
            bullet.SetupBullet(infor);
        }
    }

    #endregion

    public void ChangeState(IState<Character> _state)
    {
        m_StateMachine.ChangeState(_state);
    }

    #region CHASING

    [Task]
    public bool CanChase()
    {
        if (m_Target == null)
        {
            SetCharTarget();
        }
        if (!gameObject.activeInHierarchy)
        {
            return false;
        }
        return Helper.InRange(tf_Owner.position, m_Target.tf_Owner.position, m_ChaseRange);
    }

    public virtual void OnChaseEnter()
    {
        if (!m_AI)
        {
            ChangeState(NothingState.Instance);
            return;
        }

        nav_Agent.isStopped = false;
        nav_Agent.Warp(tf_Owner.position);
        SetNavMeshUpdatePosition(true);

        m_CharState = CharState.CHASE;
        anim_Onwer.SetFloat("InputY", 1);

        if (!m_Target.IsDead())
        {
            SetNavMeshDestination(m_Target.tf_Owner.position);
        }
        else
        {
            ChangeState(StandState.Instance);
        }
    }

    public virtual void OnChaseExecute()
    {
        if (!m_AI)
        {
            ChangeState(NothingState.Instance);
            return;
        }

        // SetNavMeshUpdatePosition(true);

        if (CanStopChasing() && !m_Target.IsMoving())
        {
            ChangeState(StandState.Instance);
            return;
        }

        m_ShootFlagCd += Time.deltaTime;

        // if (m_ShootFlagCd >= m_ShootFlagCdMax)
        // {
        //     ChangeState(ShootFlagState.Instance);
        //     return;
        // }

        if (CanChase() || m_Target.IsMoving())
        {
            SetNavMeshDestination(m_Target.tf_Owner.position);
            return;
        }
        else
        {
            SetNavMeshDestination(tf_Owner.position);
            ChangeState(StandState.Instance);
        }
    }

    public virtual void OnChaseExit()
    {

    }

    public void SetCharTarget()
    {
        if (m_Team == TEAM.Team1)
        {
            m_Target = ObjectsManager.Instance.GetRandomTeam2();
        }
        else if (m_Team == TEAM.Team2)
        {
            m_Target = ObjectsManager.Instance.GetRandomTeam1();
        }
    }
    #endregion

    #region SHOOTING
    [Task]
    public virtual void OnShooting()
    {
        for (int i = 0; i < m_ShootBullet; i++)
        {
            string bulletS = ConfigName.m_BulletValkyrja;
            // BulletConfigData infor = new BulletConfigData(m_Team, m_Dmg, bulletS, tf_CrosshairAim);
            BulletConfigData infor = new BulletConfigData(m_Team, m_Dmg, bulletS, tf_Crosshair);
            GameObject go = PrefabManager.Instance.SpawnBulletPool(infor.m_PrefabName, tf_FirePoint.position);
            Bullet bullet = go.GetComponent<Bullet>();
            bullet.SetupBullet(infor);
        }

        if (!m_AI)
        {
            CamController.Instance.Shake();
        }

        m_ShootCd = 0f;
        m_ShootBullet = 1;
    }

    [Task]
    public bool CanShoot()
    {
        // Vector3 direction = (tf_CheckShootPoint.position - tf_Crosshair.position);
        tf_CheckShootPoint.LookAt(tf_Crosshair);
        // Debug.DrawRay(tf_FirePoint.position, tf_FirePoint.forward * 90f, Color.red);
        Debug.DrawRay(tf_CheckShootPoint.position, tf_CheckShootPoint.forward * 90f, Color.red);

        if (m_CharState == CharState.DEATH || m_CharState == CharState.EMPTY)
        {
            return false;
        }

        // RaycastHit[] hit = Physics.RaycastAll(tf_FirePoint.position, tf_FirePoint.forward * 10f);
        RaycastHit[] hit = Physics.RaycastAll(tf_CheckShootPoint.position, tf_CheckShootPoint.forward * 10f);
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
            // else
            // {
            //     return false;
            // }
        }

        List<RaycastHit> hits = new List<RaycastHit>();

        for (int i = 0; i < hitCount; i++)
        {
            if (true)
            {

            }
            hits.Add(hit[i]);
        }

        hits.Sort(delegate (RaycastHit a, RaycastHit b)
        {
            return Vector3.Distance(tf_FirePoint.position, a.point)
        .CompareTo(
            Vector3.Distance(tf_FirePoint.position, b.point));
        });

        if (Input.GetKeyDown(KeyCode.F) && !IsAI())
        {
            // hitCount.
            for (int i = 0; i < hits.Count; i++)
            {
                Helper.DebugLog(hits[i].transform.name);
            }
        }

        ITakenDamage iTaken = hits[index].transform.GetComponent<ITakenDamage>();

        if (iTaken != null && (m_ShootCd >= m_ShootCdMax) && Helper.InRange(tf_Owner.position, hits[index].transform.position, m_ShootRange))
        {
            if (m_CharState != CharState.SHOOT_FLAG)
            {
                if (iTaken.GetInGameObjectType() == InGameObjectType.CHARACTER)
                {
                    if (m_Team != iTaken.GetTeam())
                    {
                        if (GetTeam() == TEAM.Team1)
                        {
                            if (iTaken != null && m_Team != iTaken.GetTeam())
                            {
                                Helper.DebugLog("CanSHOTTTTTTTTTTTTTTTTTT");
                                Vector3 v3_CollisionPoint = hits[index].transform.position;
                                string vfx = ConfigName.vfx1;
                                PrefabManager.Instance.SpawnVFXPool(vfx, v3_CollisionPoint);
                                iTaken.OnHit(m_Dmg, 1f);
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (iTaken.GetInGameObjectType() == InGameObjectType.FLAG)
                {
                    if (m_Team != iTaken.GetTeam())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
        Vector3 lookPos = new Vector3();
        if (m_CharState == CharState.SHOOT_FLAG)
        {
            lookPos = m_Flag.transform.position - tf_Owner.position;
            SetCrosshairTarget();
        }
        else
        {
            SetTargetBodyPart();
            SetCrosshairTarget();

            lookPos = m_Target.tf_Owner.position - tf_Owner.position;
        }

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

    public void SetCrosshairTarget()
    {
        if (m_CharState == CharState.SHOOT_FLAG)
        {
            tf_Crosshair.position = m_Flag.tf_ShootPoint.position;
            return;
        }

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

    #region NOTHING

    public virtual void OnNothingEnter()
    {
        if (m_AI)
        {
            SetNavMeshNextPosition(tf_Owner.position);
            ChangeState(StandState.Instance);
            return;
        }

        SetNavMeshUpdatePosition(false);
    }

    public virtual void OnNothingExecute()
    {
        if (m_AI)
        {
            ChangeState(StandState.Instance);
            return;
        }

        SetNavMeshNextPosition(tf_Owner.position);
        m_CharState = CharState.NOTHING;
    }

    public virtual void OnNothingExit()
    {

    }

    #endregion

    #region DASHSTATE
    public virtual void OnDashStateEnter()
    {
        m_CharState = CharState.DASH;
    }

    public virtual void OnDashStateExecute()
    {

    }

    public virtual void OnDashStateExit()
    {

    }

    #endregion

    #region STAND_STATE

    // [Task]
    public bool CanStopChasing()
    {
        if (m_Target == null)
        {
            SetCharTarget();
        }
        return Helper.InRange(tf_Owner.position, m_Target.tf_Owner.position, m_ChaseStopRange);
    }

    public virtual void OnStandEnter()
    {
        if (!m_AI)
        {
            ChangeState(NothingState.Instance);
            return;
        }

        if (cc_Owner.isGrounded)
        {
            nav_Agent.isStopped = true;
            nav_Agent.Warp(tf_Owner.position);
            SetNavMeshUpdatePosition(false);
        }

        m_CharState = CharState.STAND;
        anim_Onwer.SetFloat("InputY", 0);
        anim_Onwer.SetFloat("InputX", 0);

        // float gravity = 0f;
        // gravity -= 9.81f * Time.deltaTime;
        // if (cc_Owner.isGrounded)
        // {
        //     gravity = 0f;
        // }
        // Vector3 dir = new Vector3(0f, gravity, 0f);
        // cc_Owner.Move(dir);
    }

    public virtual void OnStandExecute()
    {
        if (!m_AI)
        {
            ChangeState(NothingState.Instance);
            return;
        }

        // SetNavMeshUpdatePosition(false);

        if (!cc_Owner.isGrounded)
        {
            float gravity = 0f;
            gravity -= 9.81f * Time.deltaTime;
            Vector3 dir = new Vector3(0f, gravity, 0f);
            cc_Owner.Move(dir);
        }

        m_ShootFlagCd += Time.deltaTime;

        // if (m_ShootFlagCd >= m_ShootFlagCdMax)
        // {
        //     ChangeState(ShootFlagState.Instance);
        //     return;
        // }

        if (CanChase() && !CanStopChasing())
        {
            ChangeState(ChaseState.Instance);
        }
    }

    public virtual void OnStandExit()
    {

    }

    #endregion

    #region SHOOT_FLAG_STATE

    public virtual void OnShootFlagEnter()
    {
        if (!m_AI)
        {
            ChangeState(NothingState.Instance);
            return;
        }

        SetNavMeshUpdatePosition(true);

        m_CharState = CharState.SHOOT_FLAG;
        anim_Onwer.SetFloat("InputY", 1);
        m_ChaseStopRange = 0f;
        m_ShootFlagCd = 20f;
        SetNavMeshStopRange(m_ChaseStopRange);
        SetNavMeshDestination(m_Flag.tf_Stop.position);
    }

    public virtual void OnShootFlagExecute()
    {
        if (!m_AI)
        {
            ChangeState(NothingState.Instance);
            return;
        }

        SetNavMeshUpdatePosition(true);

        m_ShootFlagCd -= Time.deltaTime;

        if (m_ShootFlagCd <= 0f)
        {
            ChangeState(ChaseState.Instance);
        }

        if (Helper.InRange(tf_Owner.position, m_Flag.tf_Stop.position, 0f))
        {
            anim_Onwer.SetFloat("InputY", 0);
            SetNavMeshDestination(tf_Owner.position);
        }
        else
        {
            anim_Onwer.SetFloat("InputY", 1);
            SetNavMeshDestination(m_Flag.tf_Stop.position);
        }
    }

    public virtual void OnShootFlagExit()
    {
        m_ChaseStopRange = 15f;
        SetNavMeshStopRange(m_ChaseStopRange);
    }

    #endregion

    #region EMPTY_STATE

    public virtual void OnEmptyStateEnter()
    {
        m_CharState = CharState.EMPTY;
    }

    public virtual void OnEmptyStateExecute()
    {

    }

    public virtual void OnEmptyStateExit()
    {

    }

    #endregion

    #region  DEATH

    public virtual void OnDeathStateEnter()
    {
        m_CharState = CharState.DEATH;
        anim_Onwer.SetTrigger("Death");
        m_DeathCd = 0f;
    }

    public virtual void OnDeathStateExecute()
    {
        m_DeathCd += Time.deltaTime;
        if (m_DeathCd > m_DeathCdMax)
        {
            ChangeState(EmptyState.Instance);
        }
    }

    public virtual void OnDeathStateExit()
    {
        HandleDeath();
    }

    #endregion

    #region CC

    public virtual void OnKnockBackStateEnter()
    {
        m_CharState = CharState.KNOCKBACK;
        KnockBack();
    }

    public virtual void OnKnockBackStateExecute()
    {

    }

    public virtual void OnKnockBackStateExit()
    {

    }

    // public void KnockBack(Vector3 _dir)
    public void KnockBack()
    {
        StartCoroutine(IEKnockBack(v3_KnockBackDir));
    }

    IEnumerator IEKnockBack(Vector3 _dir)
    {
        float startTime = Time.time;
        float dashTime = 0.25f;
        float dashSpd = 7.5f;

        nav_Agent.enabled = false;

        while (Time.time < startTime + dashTime)
        {
            Vector3 dir = new Vector3(tf_Owner.position.x - _dir.x, 0f, tf_Owner.position.z - _dir.z);
            cc_Owner.Move(dir * dashSpd * Time.deltaTime);
            yield return null;
        }

        while (!cc_Owner.isGrounded)
        {
            float gravity = 0f;
            gravity -= 9.81f * Time.deltaTime;
            Vector3 dir = new Vector3(0f, gravity, 0f);
            cc_Owner.Move(dir);
            // tf_Owner.position = new Vector3(tf_Owner.position.x, gravity, tf_Owner.position.z);
            yield return null;
        }

        nav_Agent.enabled = true;

        ChangeState(StandState.Instance);
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
        if (GetTeam() == TEAM.Team2)
        {
            Helper.DebugLog("zvdkvnadjkbvadjkbvadjkmbvadujadvihadvui");
        }
        if (!IsDead())
        {
            HandleApplyDamageAlive();
        }
        else
        {
            HandleApplyDamageDead();
        }
    }

    public virtual void HandleApplyDamageAlive()
    {
        // m_Hp -= _dmg * _crit;
        m_HealthBar.SetHpBar();
    }

    public virtual void HandleApplyDamageDead()
    {
        if (!m_AI)
        {
            CamController.Instance.m_CMFreeLook.m_Follow = null;
            InGameManager.Instance.btn_Skill.interactable = false;
        }
        m_HeadPart.gameObject.SetActive(false);
        m_BodyPart.gameObject.SetActive(false);
        ChangeState(DeathState.Instance);
    }

    public virtual void HandleDeath()
    {
        ObjectsManager.Instance.RemoveDeadChar(m_Team, this);
        PrefabManager.Instance.DespawnPool(gameObject);

        if (m_Team == TEAM.Team1)
        {
            ObjectsManager.Instance.SpawnTeam1(1);
        }
        else if (m_Team == TEAM.Team2)
        {
            ObjectsManager.Instance.SpawnTeam2(1);
        }

        if (!m_AI)
        {
            EventManager1<bool>.CallEvent(GameEvent.SET_CMLOOK_TARGET, false);
        }

        EventManager.CallEvent(GameEvent.SET_HEALTH_BAR);
    }

    public BigNumber GetMaxHP()
    {
        return 1000;
    }

    public BigNumber GetHpPercentage()
    {
        return (m_Hp / m_HpMax);
    }

    [Task]
    public bool IsDead()
    {
        return (m_Hp <= 0);
    }

    #region NavMesh

    public void SetNavMeshDestination(Vector3 _des)
    {
        nav_Agent.SetDestination(_des);
    }

    public void SetNavMeshUpdatePosition(bool _value)
    {
        nav_Agent.updatePosition = _value;
        nav_Agent.updateRotation = _value;
    }

    public void SetNavMeshNextPosition(Vector3 _value)
    {
        nav_Agent.nextPosition = _value;
    }

    public void SetNavMeshStopRange(float _value)
    {
        nav_Agent.stoppingDistance = _value;
    }

    public void SetNavMeshSpeed(float _value)
    {
        nav_Agent.speed = _value;
    }

    #endregion

    public virtual void HandleSkill()
    {

    }

    public virtual void HandleSkill(bool _activated)
    {
        if (_activated)
        {
            TurnOnSkill();
        }
        else
        {
            TurnOffSkill();
        }
    }

    public virtual void TurnOnSkill()
    {

    }

    public virtual void TurnOffSkill()
    {

    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("DeadPlane"))
        {
            m_Hp = 0;
            HandleApplyDamageDead();
        }
    }
}

public interface ITakenDamage
{
    void OnHit();
    void OnHit(BigNumber _dmg);
    void OnHit(BigNumber _dmg, float _crit);
    void OnHit(string _targetName);
    void OnHit(GameObject _go);
    InGameObjectType GetInGameObjectType();
    TEAM GetTeam();
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

public enum CharState
{
    NOTHING = 0,
    STAND,
    CHASE,
    SHOOT_FLAG,
    DEATH,
    EMPTY,
    DASH,
    KNOCKBACK,
}