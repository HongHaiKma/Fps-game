using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Canon : Bullet
{
    [Header("---Bullet_Pistol Movements---")]
    public float m_GravityModifier;

    [Header("---VFX---")]
    public GameObject m_ImpactEffect;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        m_GravityModifier += (Physics.gravity.y * Time.deltaTime);
        tf_Onwer.position += new Vector3(0f, m_GravityModifier * Time.deltaTime, 0f);
    }

    public override void VFXEffect()
    {
        Instantiate(m_ImpactEffect, tf_Onwer.position, tf_Onwer.rotation);
    }
}
