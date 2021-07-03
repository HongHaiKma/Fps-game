using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChameleon : Character
{
    public bool m_IsDash = true;
    public bool m_IsDashDam = true;

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

    public override void AddListener()
    {
        base.AddListener();
        EventManager.AddListener(GameEvent.TEST_LIZARD_SKILL, Event_TEST_LIZARD_DASH);
    }

    public override void RemoveListener()
    {
        base.RemoveListener();
        EventManager.RemoveListener(GameEvent.TEST_LIZARD_SKILL, Event_TEST_LIZARD_DASH);
    }

    public void Event_TEST_LIZARD_DASH()
    {
        if (!IsAI())
        {
            Helper.DebugLog("Event_TEST_LIZARD_DASH");
            ChangeState(DashState.Instance);
        }
    }

    public override void OnDashStateEnter()
    {
        base.OnDashStateEnter();
        m_IsDash = true;
        m_IsDashDam = true;
        StartCoroutine(Dash());
    }

    public override void OnDashStateExecute()
    {
        if (!m_IsDash)
        {
            ChangeState(StandState.Instance);
        }
    }

    public override void OnDashStateExit()
    {
        m_IsDash = true;
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        float dashTime = 0.25f;
        float dashSpd = 25f;
        // m_Dash = false;
        while (Time.time < startTime + dashTime)
        {
            PPManager.Instance.SetMotionBlurDash(1f);
            if (!IsAI())
            {
                cc_Owner.Move(tf_Owner.forward * dashSpd * Time.deltaTime);
            }
            else
            {
                cc_Owner.Move(tf_Owner.forward * dashSpd * Time.deltaTime);
            }
            yield return null;
        }

        m_IsDash = false;
        PPManager.Instance.SetMotionBlurDash(0f);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    // public override void OnTriggerEnter(Collider hit)
    {
        // base.OnTriggerEnter(hit);

        ITakenDamage iTaken = hit.gameObject.GetComponent<ITakenDamage>();

        Character charrr = iTaken as Character;

        if (m_IsDash && (m_CharState == CharState.DASH))
        {
            if ((iTaken != null) && (iTaken.GetInGameObjectType() == InGameObjectType.CHARACTER) && (iTaken.GetTeam() != GetTeam()))
            // if ((iTaken != null) && (iTaken.GetInGameObjectType() == InGameObjectType.CHARACTER))
            {
                m_IsDash = false;

                if (m_IsDashDam)
                {
                    Helper.DebugLog("m_Dash = " + m_IsDash);
                    iTaken.OnHit(150f, 1f);
                    charrr.v3_KnockBackDir = tf_Owner.position;
                    charrr.ChangeState(KnockBackState.Instance);
                }

                m_IsDashDam = false;

            }
        }
    }
}
