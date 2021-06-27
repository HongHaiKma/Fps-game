using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CdSkill : CharacterSkill
{
    public float m_Cooldown;
    public float m_CooldownMax;

    public override void OnEnable()
    {
        m_Cooldown = m_CooldownMax + 1;
        base.OnEnable();
    }

    public virtual void Update()
    {
        if (m_Cooldown <= m_CooldownMax)
        {
            m_Cooldown += Time.deltaTime;
            if (m_Cooldown >= m_CooldownMax && !m_Char.IsAI())
            {
                InGameManager.Instance.btn_Skill.interactable = true;
            }
        }
    }

    public override void Event_ACTIVATE_SKILL()
    {
        if (!m_Char.IsAI() && m_Cooldown >= m_CooldownMax && !m_IsActivated)
        {
            ActivateSkill();
        }
    }

    public virtual void ActivateSkill()
    {
        InGameManager.Instance.btn_Skill.interactable = false;
        m_IsActivated = true;
        m_Cooldown = 0f;
    }

    public override void DeactivateSkill()
    {

    }

    public override void SetSkillButton()
    {
        if (m_Cooldown <= m_CooldownMax)
        {
            InGameManager.Instance.btn_Skill.interactable = false;
        }
        else
        {
            InGameManager.Instance.btn_Skill.interactable = true;
        }
    }
}
