using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDurationSkill : CdSkill
{
    // public override void Event_ACTIVATE_SKILL()
    // {
    //     if (!m_Char.IsAI())
    //     {
    //         if (!m_IsActivated)
    //         {
    //             HandleSkill();
    //         }
    //     }
    // }

    public override void Update()
    {
        base.Update();

        if (m_Cooldown >= m_CooldownMax)
        {
            m_IsActivated = false;
        }
    }

    public override void ActivateSkill()
    {
        InGameManager.Instance.btn_Skill.interactable = false;
        m_IsActivated = true;
        m_Cooldown = 0f;
        HandleSkill();
    }
}
