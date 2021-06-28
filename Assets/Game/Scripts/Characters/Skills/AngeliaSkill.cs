using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngeliaSkill : NotCdSkill
{
    public override void AddListener()
    {
        base.AddListener();
        EventManager.AddListener(GameEvent.DEACTIVATE_SKILL, Event_DEACTIVATE_SKILL);
    }

    public override void RemoveListener()
    {
        base.RemoveListener();
        EventManager.RemoveListener(GameEvent.DEACTIVATE_SKILL, Event_DEACTIVATE_SKILL);
    }

    private void Update()
    {
        if (!m_Char.IsAI())
        {

        }
    }

    public void Event_DEACTIVATE_SKILL()
    {
        if (!m_Char.IsAI())
        {
            DeactivateSkill();
        }
    }

    public override void DeactivateSkill()
    {
        base.DeactivateSkill();
    }

    public override void HandleSkill()
    {
        if (m_IsActivated)
        {
            CamController.Instance.m_CMFreeLook.m_Lens.FieldOfView = 10f;
            m_Char.HandleSkill(true);
        }
        else
        {
            CamController.Instance.m_CMFreeLook.m_Lens.FieldOfView = 40f;
            m_Char.HandleSkill(false);
        }
    }
}
