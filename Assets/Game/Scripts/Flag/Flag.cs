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
        m_HpMax = 200f;
        m_Hp = m_HpMax;
        SetBarColor();
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0f, 90f, 0f);
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
    }

    public override void OnHit(BigNumber _dmg)
    {
        m_Hp -= _dmg;
        Debug.Log("m_Hp = " + m_Hp);
        Debug.Log("m_HpMax = " + m_HpMax);
        SetHpBar();
    }

    // On
}
