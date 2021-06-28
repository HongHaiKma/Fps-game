using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill : MonoBehaviour
{
    public SkillType m_SkillType;
    public Character m_Char;

    public bool m_IsActivated = false;

    public virtual void OnEnable()
    {
        AddListener();
    }

    public virtual void OnDisable()
    {
        RemoveListener();
    }

    public virtual void OnDestroy()
    {
        RemoveListener();
    }

    public virtual void AddListener()
    {
        EventManager.AddListener(GameEvent.ACTIVATE_SKILL, Event_ACTIVATE_SKILL);
        EventManager.AddListener(GameEvent.SET_SKILL_BUTTON, Event_SET_SKILL_BUTTON);
    }

    public virtual void RemoveListener()
    {
        EventManager.RemoveListener(GameEvent.ACTIVATE_SKILL, Event_ACTIVATE_SKILL);
        EventManager.RemoveListener(GameEvent.SET_SKILL_BUTTON, Event_SET_SKILL_BUTTON);
    }

    public virtual void Event_ACTIVATE_SKILL()
    {

    }

    public virtual void DeactivateSkill()
    {
        if (!m_Char.IsAI())
        {
            InGameManager.Instance.btn_Skill.interactable = false;
        }
    }

    public virtual void HandleSkill()
    {

    }

    public virtual void Event_SET_SKILL_BUTTON()
    {
        if (!m_Char.IsAI())
        {
            SetSkillButton();
        }
    }

    public virtual void SetSkillButton()
    {

    }
}

public enum SkillType
{
    NotCdSkill,
    CdSkill,
}