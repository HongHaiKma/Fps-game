using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : Singleton<InGameManager>
{
    public HealthBarUI m_HealthBarUI;
    public Button btn_Skill;

    private void Awake()
    {
        GUIManager.Instance.AddClickEvent(btn_Skill, ActivateSkill);
    }

    public void ActivateSkill()
    {
        EventManager.CallEvent(GameEvent.ACTIVATE_SKILL);
    }
}
