using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLizard : Character
{
    public bool m_Dash = true;

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!IsAI())
            {
                ChangeState(DashState.Instance);
            }
        }
    }
    public override void OnDashStateEnter()
    {
        base.OnDashStateEnter();
        m_Dash = true;
        StartCoroutine(Dash());
    }

    public override void OnDashStateExecute()
    {
        if (!m_Dash)
        {
            ChangeState(StandState.Instance);
        }
    }

    public override void OnDashStateExit()
    {
        m_Dash = true;
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        float dashTime = 0.25f;
        float dashSpd = 25f;

        while (Time.time < startTime + dashTime)
        {
            PPManager.Instance.SetMotionBlurDash(1f);
            cc_Owner.Move(tf_Owner.forward * dashSpd * Time.deltaTime);
            yield return null;
        }

        m_Dash = false;
        PPManager.Instance.SetMotionBlurDash(0f);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Character charrr = other.GetComponent<Character>();

        if (m_Dash && m_CharState == CharState.DASH)
        {
            if (charrr != null)
            {
                m_Dash = false;
            }
        }
    }
}
