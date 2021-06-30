using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : Singleton<InGameManager>
{
    public HealthBarUI m_HealthBarUI;
    public Button btn_Skill;
    public Button btn_TestLizardSkill;

    private void Awake()
    {
        // GUIManager.Instance.AddClickEvent(btn_Skill, ActivateSkill);
        GUIManager.Instance.AddClickEvent(btn_TestLizardSkill, Event_TEST_LIZARD_DASH);
    }

    public void ActivateSkill()
    {
        EventManager.CallEvent(GameEvent.ACTIVATE_SKILL);
    }

    public void Event_TEST_LIZARD_DASH()
    {
        EventManager.CallEvent(GameEvent.TEST_LIZARD_SKILL);
    }
}
