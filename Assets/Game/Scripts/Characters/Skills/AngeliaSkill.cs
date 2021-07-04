using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngeliaSkill : NotCdSkill
{
    public override void AddListener()
    {
        base.AddListener();
        EventManager.AddListener(GameEvent.DEACTIVATE_SKILL, Event_DEACTIVATE_SKILL);
    }

    public override void RemoveListener()
    {
        base.RemoveListener();
        EventManager.RemoveListener(GameEvent.DEACTIVATE_SKILL, Event_DEACTIVATE_SKILL);
    }
    public void Event_DEACTIVATE_SKILL()
    {
        if (!m_Char.IsAI())
        {
            DeactivateSkill();
        }
    }

    public override void DeactivateSkill()
    {
        base.DeactivateSkill();
    }

    public override void HandleSkill()
    {
        if (m_IsActivated)
        {
            StartCoroutine(IEZoom(true));
            // CamController.Instance.m_CMFreeLook.m_Lens.FieldOfView = 10f;
            StartCoroutine(IEZoomFOV(true));
            m_Char.HandleSkill(true);
        }
        else
        {
            StartCoroutine(IEZoom(false));
            // CamController.Instance.m_CMFreeLook.m_Lens.FieldOfView = 40f;
            StartCoroutine(IEZoomFOV(false));
            m_Char.HandleSkill(false);
        }
        InGameManager.Instance.btn_Skill.interactable = true;
    }

    IEnumerator IEZoom(bool _value)
    {
        if (_value)
        {
            float zoom = 0f;
            while (zoom <= 0.45f)
            {
                zoom += Time.deltaTime;
                PPManager.Instance.SetZoomVinegtte(zoom);
                yield return null;
            }

            zoom = 0.45f;
            PPManager.Instance.SetZoomVinegtte(zoom);
        }
        else
        {
            float zoom = 0.45f;
            while (zoom > 0f)
            {
                zoom -= Time.deltaTime;
                PPManager.Instance.SetZoomVinegtte(zoom);
                yield return null;
            }

            zoom = 0f;
            PPManager.Instance.SetZoomVinegtte(zoom);
        }
    }

    IEnumerator IEZoomFOV(bool _value)
    {
        if (_value)
        {
            float zoom = 40f;
            while (zoom > 10f)
            {
                zoom -= Time.deltaTime * 120f;
                CamController.Instance.m_CMFreeLook.m_Lens.FieldOfView = zoom;
                yield return null;
            }

            zoom = 10f;
            CamController.Instance.m_CMFreeLook.m_Lens.FieldOfView = zoom;
        }
        else
        {
            float zoom = 10f;
            while (zoom < 40f)
            {
                zoom += Time.deltaTime * 120f;
                CamController.Instance.m_CMFreeLook.m_Lens.FieldOfView = zoom;
                yield return null;
            }

            zoom = 40f;
            CamController.Instance.m_CMFreeLook.m_Lens.FieldOfView = zoom;
        }
    }
}
