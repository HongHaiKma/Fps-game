using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAngelia : Character
{
    public override void TurnOnSkill()
    {
        m_ShootRange = 300f;
    }

    public override void TurnOffSkill()
    {
        m_ShootRange = m_ShootRange = 20.5f;
    }

    public override void HandleApplyDamageDead()
    {
        base.HandleApplyDamageDead();

        if (!IsAI())
        {
            CamController.Instance.m_CMFreeLook.m_Follow = null;
            EventManager.CallEvent(GameEvent.DEACTIVATE_SKILL);
            InGameManager.Instance.btn_Skill.interactable = false;
        }
    }
}
