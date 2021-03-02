using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : InGameObject
{
    [Header("---Charcteristics---")]
    public BigNumber m_Dmg;

    [Header("---Components---")]
    public Transform tf_Onwer;
    public Collider col_Onwer;
    public Vector3 v3_CollisionPoint;


    [Header("---Movements---")]
    public float m_MoveSpd;
    public float m_FlyingTime;
    public float m_FlyingTimeMax;
    public bool m_Collided;

    private void OnEnable()
    {
        m_Collided = false;
        // col_Onwer.enabled = true;

        m_FlyingTime = 0f;
        m_FlyingTimeMax = 3f;
    }

    private void OnDisable()
    {
        m_Collided = true;
        // col_Onwer.enabled = false;
    }

    public void SetupBullet(BulletConfigData _bulletInfor)
    {
        m_Dmg = _bulletInfor.m_Dmg;
        tf_Onwer.rotation = _bulletInfor.m_Rotation;
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

    }

    public virtual void SpawnVFX()
    {

    }

    // public void OnTriggerEnter(Collider other)
    // {
    //     OnHit(other.gameObject.name);
    //     VFXEffect();
    // }

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

            SpawnVFX(v3_CollisionPoint);
            PrefabManager.Instance.DespawnPool(gameObject);

            ITakenDamage iTaken = _go.GetComponent<ITakenDamage>();

            iTaken.OnHit(m_Dmg);
        }
    }
}