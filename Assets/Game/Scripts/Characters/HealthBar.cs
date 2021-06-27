using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Character m_Char;
    public Image img_Health;
    public GameObject g_Bar;

    private void OnEnable()
    {
        StartListenToEvents();
    }

    private void OnDisable()
    {
        StopListenToEvents();
    }

    public void StartListenToEvents()
    {
        EventManager.AddListener(GameEvent.SET_HEALTH_BAR, SetEnableOrDisable);
        EventManager.AddListener(GameEvent.SET_HEALTH_BAR, SetBarColor);
    }

    public void StopListenToEvents()
    {
        EventManager.RemoveListener(GameEvent.SET_HEALTH_BAR, SetEnableOrDisable);
        EventManager.RemoveListener(GameEvent.SET_HEALTH_BAR, SetBarColor);
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0f, 180f, 0f);
    }

    public void SetHpBar()
    {
        img_Health.fillAmount = m_Char.GetHpPercentage().ToFloat();
        if (!m_Char.IsAI())
        {
            InGameManager.Instance.m_HealthBarUI.SetHealthBar(m_Char.GetHpPercentage());
        }
    }

    public void SetEnableOrDisable()
    {
        g_Bar.SetActive(m_Char.m_AI);
    }

    public void SetBarColor()
    {
        if (m_Char.m_Team == TEAM.Team1)
        {
            img_Health.color = Color.blue;
        }
        else if (m_Char.m_Team == TEAM.Team2)
        {
            img_Health.color = Color.red;
        }
    }
}
