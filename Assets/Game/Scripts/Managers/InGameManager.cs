using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : Singleton<InGameManager>
{
    public HealthBarUI m_HealthBarUI;
    public Button btn_Skill;
    public Button btn_TestLizardSkill;

    public Camera m_MiniMapCam;
    public RenderTexture m_MiniMapTexture;
    public Button btn_MiniMap;
    public GameObject g_Joystick;

    private void Awake()
    {
        GUIManager.Instance.AddClickEvent(btn_Skill, ActivateSkill);
        GUIManager.Instance.AddClickEvent(btn_MiniMap, OnClickMiniMap);
        // GUIManager.Instance.AddClickEvent(btn_TestLizardSkill, Event_TEST_LIZARD_DASH);
    }

    public void ActivateSkill()
    {
        EventManager.CallEvent(GameEvent.ACTIVATE_SKILL);
    }

    public void OnClickMiniMap()
    {
        EventManager.CallEvent(GameEvent.DEACTIVATE_SKILL);
        g_Joystick.SetActive(false);
        btn_Skill.gameObject.SetActive(false);
        m_MiniMapCam.targetTexture = null;
        btn_MiniMap.gameObject.SetActive(false);
    }

    public void OnUnClickMiniMap()
    {
        g_Joystick.SetActive(true);
        btn_Skill.gameObject.SetActive(true);
        m_MiniMapCam.targetTexture = m_MiniMapTexture;
        btn_MiniMap.gameObject.SetActive(true);
    }
}
