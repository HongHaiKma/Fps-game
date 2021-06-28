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

    // public override void HandleApplyDamageDead(BigNumber _dmg, float _crit)
    // {
    //     if (!m_AI)
    //     {
    //         CamController.Instance.m_CMFreeLook.m_Follow = null;
    //         EventManager.CallEvent(GameEvent.DEACTIVATE_SKILL);
    //     }
    //     m_HeadPart.gameObject.SetActive(false);
    //     m_BodyPart.gameObject.SetActive(false);
    //     ChangeState(DeathState.Instance);
    // }
}
