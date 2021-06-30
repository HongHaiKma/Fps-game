using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLizard : Character
{
    public bool m_Dash = true;

    // public override void Update()
    // {
    //     base.Update();

    //     if (Input.GetKeyDown(KeyCode.F))
    //     {
    //         if (!IsAI())
    //         {
    //             ChangeState(DashState.Instance);
    //         }
    //     }
    // }

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
            ChangeState(DashState.Instance);
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        ITakenDamage iTaken = hit.gameObject.GetComponent<ITakenDamage>();

        Character charrr = iTaken as Character;

        if (m_Dash && (m_CharState == CharState.DASH))
        {
            if ((iTaken != null) && (iTaken.GetInGameObjectType() == InGameObjectType.CHARACTER) && (iTaken.GetTeam() != GetTeam()))
            // if ((iTaken != null) && (iTaken.GetInGameObjectType() == InGameObjectType.CHARACTER))
            {
                m_Dash = false;
                iTaken.OnHit(200f, 1f);
                charrr.KnockBack(tf_Owner.position);
            }
        }
    }
}
