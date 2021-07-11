using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelPart : MonoBehaviour, ITakenDamage
{
    public TEAM m_Team;
    public InGameObjectType m_InGameObjectType;
    public Character m_OwnerChar;
    public BodyPart m_BodyPart;
    public Collider col_Owner;

    private void OnEnable()
    {
        m_Team = m_OwnerChar.m_Team;
    }

    public virtual void OnHit()
    {
        // Debug.Log("Char is: " + m_OwnerChar.gameObject.name);
        // Debug.Log("Body is: " + gameObject.name);

    }

    public void OnHit(BigNumber _dmg)
    {
        Helper.DebugLog("SHOTTTTTTTTT BODY PARTTTTT");
        if (m_BodyPart == BodyPart.HEAD)
        {
            m_OwnerChar.OnHit(_dmg, 2f);
            Helper.DebugLog("Shoot HEAD");
        }
        else if (m_BodyPart == BodyPart.BODY)
        {
            m_OwnerChar.OnHit(_dmg, 1f);
            Helper.DebugLog("Shoot BODY");
        }
    }

    public virtual void OnHit(BigNumber _dmg, float _crit) { }

    public virtual void OnHit(string _targetName) { }

    public virtual void OnHit(GameObject _go) { }

    public virtual InGameObjectType GetInGameObjectType()
    {
        return m_InGameObjectType;
    }

    public virtual TEAM GetTeam()
    {
        return m_Team;
    }
}

public enum BodyPart
{
    HEAD = 0,
    BODY = 1
}