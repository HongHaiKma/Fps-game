using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Button btn = InGameManager.Instance.btn_Skill;
        btn.GetComponent<Image>().fillAmount = 1f;
        btn.interactable = true;
    }
}
