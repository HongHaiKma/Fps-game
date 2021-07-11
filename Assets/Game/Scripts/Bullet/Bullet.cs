using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : InGameObject
{
    [Header("---Charcteristics---")]
    public BigNumber m_Dmg;
    // public TEAM m_Team;

    [Header("---Components---")]
    public Transform tf_Onwer;
    public Collider col_Onwer;
    public Vector3 v3_CollisionPoint;


    [Header("---Movements---")]
    public float m_MoveSpd;
    public float m_FlyingTime;
    public float m_FlyingTimeMax;
    public bool m_Collided;

    [Header("Test Rocket")]
    public Vector3 v3_Rotation;

    public virtual void OnEnable()
    {
        m_Collided = false;
        // col_Onwer.enabled = true;

        m_FlyingTime = 0f;
        m_FlyingTimeMax = 10f;
    }

    private void OnDisable()
    {
        m_Collided = true;
        // col_Onwer.enabled = false;
    }

    public virtual void SetupBullet(BulletConfigData _bulletInfor)
    {
        ConfigBullet(_bulletInfor);
    }

    public virtual void SetupBullet(BulletConfigData _bulletInfor, Vector3 _offset, float _speed = 5000f)
    {
        ConfigBullet(_bulletInfor, _speed);
    }

    public void ConfigBullet(BulletConfigData _bulletInfor, float _speed = 5000f)
    {
        m_Team = _bulletInfor.m_Team;
        m_Dmg = _bulletInfor.m_Dmg;
        m_MoveSpd = _speed;
        v3_Rotation = _bulletInfor.m_Rotation.position;
        tf_Onwer.LookAt(_bulletInfor.m_Rotation.position);
    }

    public virtual void FixedUpdate()
    {
        tf_Onwer.position += tf_Onwer.forward * m_MoveSpd * Time.fixedDeltaTime;


        m_FlyingTime += Time.fixedDeltaTime;

        if (m_FlyingTime >= m_FlyingTimeMax)
        {
            PrefabManager.Instance.DespawnPool(gameObject);
        }
    }

    public virtual void SpawnVFX(Vector3 _collisionPoint)
    {
        string vfx = ConfigName.vfx1;
        PrefabManager.Instance.SpawnVFXPool(vfx, _collisionPoint);
    }

    public virtual void SpawnVFX()
    {

    }



    public override void OnHit()
    {
        if (!m_Collided)
        {
            m_Collided = true;

            SpawnVFX(v3_CollisionPoint);
            PrefabManager.Instance.DespawnPool(gameObject);
        }
    }

    public override void OnHit(string _targetName)
    {
        PrefabManager.Instance.DespawnPool(gameObject);
    }

    public override void OnHit(GameObject _go)
    {
        if (!m_Collided)
        {
            m_Collided = true;

            // SpawnVFX(v3_CollisionPoint);
            PrefabManager.Instance.DespawnPool(gameObject);

            // ITakenDamage iTaken = _go.GetComponent<ITakenDamage>();

            // if (iTaken != null && m_Team != iTaken.GetTeam())
            // {
            //     // m_Collided = true;

            //     // SpawnVFX(v3_CollisionPoint);
            //     // PrefabManager.Instance.DespawnPool(gameObject);


            //     iTaken.OnHit(m_Dmg);
            // }
        }
    }
}

public class BulletConfigData
{
    public TEAM m_Team;
    public BigNumber m_Dmg;
    public string m_PrefabName;
    public Transform m_Rotation;

    public BulletConfigData(TEAM _team, BigNumber _dmg, string _prefabName, Transform _lookAt)
    {
        m_Team = _team;
        m_Dmg = _dmg;
        m_PrefabName = _prefabName;
        m_Rotation = _lookAt;
    }
}