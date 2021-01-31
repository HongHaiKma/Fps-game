using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : InGameObject
{
    [Header("---Components---")]
    public Transform tf_Onwer;
    public Collider col_Onwer;


    [Header("---Movements---")]
    public float m_MoveSpd;
    public float m_FlyingTime;
    public float m_FlyingTimeMax;

    private void OnEnable()
    {
        m_FlyingTime = 0f;
        m_FlyingTimeMax = 3f;
    }

    public void SetupBullet(BulletInfor _bulletInfor)
    {
        tf_Onwer.rotation = _bulletInfor.m_Rotation;
    }

    public virtual void FixedUpdate()
    {
        // rb_Owner.velocity = tf_Onwer.forward * m_MoveSpd;
        tf_Onwer.position += tf_Onwer.forward * m_MoveSpd * Time.deltaTime;

        m_FlyingTime += Time.deltaTime;

        if (m_FlyingTime >= m_FlyingTimeMax)
        {
            PrefabManager.Instance.DespawnPool(gameObject);
        }
    }

    public virtual void VFXEffect()
    {

    }

    // public void OnTriggerEnter(Collider other)
    // {
    //     OnHit(other.gameObject.name);
    //     VFXEffect();
    // }

    public override void OnHit()
    {
        PrefabManager.Instance.DespawnPool(gameObject);
        Debug.Log("Bullet just despawn no damage!!!");
    }

    public override void OnHit(string _targetName)
    {
        PrefabManager.Instance.DespawnPool(gameObject);
        Helper.DebugLog("Hit: " + _targetName);
        // Debug.Log("Hit: " + _targetName);
    }

    public override void OnHit(GameObject _go)
    {
        PrefabManager.Instance.DespawnPool(gameObject);
        Debug.Log("bullet called!!!");

        ITakenDamage iTaken = _go.GetComponent<ITakenDamage>();

        if (iTaken != null)
        {
            iTaken.OnHit();
            Debug.Log("Target != null");
        }
        else
        {
            Debug.Log("Target == null");
        }
    }
}

public class BulletInfor
{
    public string m_PrefabName;
    public Quaternion m_Rotation;

    public BulletInfor(string _prefabName, Quaternion _rotation)
    {
        m_PrefabName = _prefabName;
        m_Rotation = _rotation;
    }
}