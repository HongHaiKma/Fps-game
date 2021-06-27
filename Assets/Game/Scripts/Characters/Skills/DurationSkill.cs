using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationSkill : CdSkill
{
    public float m_Duration;
    public float m_DurationMax;

    public float m_CastCd;
    public float m_CastMax;

    public override void OnEnable()
    {
        m_IsActivated = false;
        m_Cooldown = 0f;
        m_Duration = 0f;
        m_CastCd = 0f;
        base.OnEnable();
    }

    public override void Update()
    {
        base.Update();

        if (m_IsActivated)
        {
            m_Duration += Time.deltaTime;
            m_CastCd += Time.deltaTime;

            if (m_Duration >= m_DurationMax)
            {
                m_IsActivated = false;
            }
            else
            {
                if (m_CastCd >= m_CastMax)
                {
                    HandleSkill();
                    m_CastCd = 0f;
                }
            }
        }
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        m_Duration = 0f;
        m_CastCd = 0f;
    }
}
