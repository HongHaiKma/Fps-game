using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSkill : DurationSkill
{
    public HealthBar m_HealthBar;
    public override void HandleSkill()
    {
        BigNumber maxHp = m_Char.GetMaxHP();
        m_Char.m_Hp += (0.09 * maxHp);
        if (m_Char.m_Hp > maxHp)
        {
            m_Char.m_Hp = maxHp;
        }
        m_HealthBar.SetHpBar();
        GameObject go = PrefabManager.Instance.SpawnVFXPool("GreenBuff", m_Char.tf_Owner.position);
        go.GetComponent<VFXEffect>().SetFollow(m_Char.tf_Owner);
        go.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
}
