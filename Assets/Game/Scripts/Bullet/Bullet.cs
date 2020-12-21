using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ITakenDamage
{
    [Header("---Components---")]
    public Transform tf_Onwer;
    public Collider col_Onwer;


    [Header("---Movements---")]
    public float m_MoveSpd;
    public float m_MaxFlyingTime;

    public virtual void Update()
    {
        // rb_Owner.velocity = tf_Onwer.forward * m_MoveSpd;
        tf_Onwer.position += tf_Onwer.forward * m_MoveSpd * Time.deltaTime;

        m_MaxFlyingTime -= Time.deltaTime;

        if (m_MaxFlyingTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void VFXEffect()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        OnHit(other.gameObject.name);
        VFXEffect();
    }

    public virtual void OnHit()
    {

    }

    public virtual void OnHit(string _targetName)
    {
        Destroy(gameObject);
        Debug.Log("Hit: " + _targetName);
    }
}

public class BulletInfor
{

}