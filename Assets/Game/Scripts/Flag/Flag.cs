using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : InGameObject
{
    public BigNumber m_Hp;
    public BigNumber m_HpMax;
    public TEAM m_Team;
    public Image img_Health;
    public Transform tf_ShootPoint;

    private void OnEnable()
    {
        m_HpMax = 1500f;
        m_Hp = m_HpMax;
        SetBarColor();

        StartListenToEvents();
    }

    private void OnDisable()
    {
        StopListenToEvents();
    }

    public void StartListenToEvents()
    {
        EventManager.AddListener(GameEvent.RESET_FLAG_HEALTH_BAR, ResetBar);
    }

    public void StopListenToEvents()
    {
        EventManager.RemoveListener(GameEvent.RESET_FLAG_HEALTH_BAR, ResetBar);
    }


    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0f, 90f, 0f);
    }

    public void ResetBar()
    {
        m_HpMax = 1500f;
        m_Hp = m_HpMax;
        SetBarColor();
    }

    public void SetHpBar()
    {
        img_Health.fillAmount = (m_Hp / m_HpMax).ToFloat();
    }

    public void SetBarColor()
    {
        if (m_Team == TEAM.Team1)
        {
            img_Health.color = Color.blue;
        }
        else if (m_Team == TEAM.Team2)
        {
            img_Health.color = Color.red;
        }

        img_Health.fillAmount = 1f;
    }

    public override void OnHit(BigNumber _dmg)
    {
        m_Hp -= _dmg;
        Debug.Log("m_Hp = " + m_Hp);
        Debug.Log("m_HpMax = " + m_HpMax);
        SetHpBar();

        if (m_Hp <= 0f)
        {
            EventManager.CallEvent(GameEvent.RESET_FLAG_HEALTH_BAR);

            // int team1Count = InGameObjectsManager.Instance.m_Team1.Count;
            // int team2Count = InGameObjectsManager.Instance.m_Team2.Count;

            List<Character> list1 = InGameObjectsManager.Instance.m_Team1;
            List<Character> list2 = InGameObjectsManager.Instance.m_Team2;

            for (int i = 0; i < list1.Count; i++)
            {
                Destroy(list1[i].gameObject);
            }
            list1.Clear();

            for (int i = 0; i < list2.Count; i++)
            {
                Destroy(list2[i].gameObject);
            }
            list2.Clear();

            InGameObjectsManager.Instance.SpawnTeam1(5);
            InGameObjectsManager.Instance.SpawnTeam2(5);

            EventManager.CallEvent(GameEvent.SET_CHAR_TARGET);
            EventManagerWithParam<bool>.CallEvent(GameEvent.SET_CMLOOK_TARGET, true);
            EventManager.CallEvent(GameEvent.SET_HEALTH_BAR);
        }
    }
}
