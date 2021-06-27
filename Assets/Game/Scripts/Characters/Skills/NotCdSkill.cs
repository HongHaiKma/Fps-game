using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotCdSkill : CharacterSkill
{
    public override void Event_ACTIVATE_SKILL()
    {
        if (!m_Char.IsAI())
        {
            if (!m_IsActivated)
            {
                m_IsActivated = true;
                HandleSkill();
            }
            else
            {
                DeactivateSkill();
            }
        }
    }

    public override void DeactivateSkill()
    {
        base.DeactivateSkill();
        m_IsActivated = false;
        HandleSkill();
    }

    public override void SetSkillButton()
    {
        InGameManager.Instance.btn_Skill.interactable = true;
    }
}
