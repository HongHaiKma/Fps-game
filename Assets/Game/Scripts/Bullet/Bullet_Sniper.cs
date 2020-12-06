using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Sniper : Bullet
{
    [Header("---VFX---")]
    public GameObject m_ImpactEffect;

    public override void VFXEffect()
    {
        Instantiate(m_ImpactEffect, tf_Onwer.position, tf_Onwer.rotation);
    }
}
