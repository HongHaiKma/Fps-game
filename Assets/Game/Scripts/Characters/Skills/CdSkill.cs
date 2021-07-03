using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            Button btn = InGameManager.Instance.btn_Skill;
            m_Cooldown += Time.deltaTime;

            if (!m_Char.IsAI())
            {
                if (m_Cooldown >= m_CooldownMax)
                {
                    btn.GetComponent<Image>().fillAmount = 1f;
                    btn.interactable = true;
                }
                else
                {
                    btn.GetComponent<Image>().fillAmount = (m_Cooldown / m_CooldownMax);
                }
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
        Button btn = InGameManager.Instance.btn_Skill;
        if (m_Cooldown <= m_CooldownMax)
        {

            btn.interactable = false;
            btn.GetComponent<Image>().fillAmount = (m_Cooldown / m_CooldownMax);
        }
        else
        {
            btn.interactable = true;
            btn.GetComponent<Image>().fillAmount = 1f;
        }
    }
}
