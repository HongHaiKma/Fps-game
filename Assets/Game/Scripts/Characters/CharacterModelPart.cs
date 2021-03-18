using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelPart : MonoBehaviour, ITakenDamage
{
    public Character m_OwnerChar;
    public BodyPart m_BodyPart;

    public virtual void OnHit()
    {
        // Debug.Log("Char is: " + m_OwnerChar.gameObject.name);
        // Debug.Log("Body is: " + gameObject.name);

    }

    public void OnHit(BigNumber _dmg)
    {
        if (m_BodyPart == BodyPart.HEAD)
        {
            m_OwnerChar.OnHit(_dmg, 2f);
            Debug.Log("1111111111111111111111");
        }
        else if (m_BodyPart == BodyPart.BODY)
        {
            m_OwnerChar.OnHit(_dmg, 1f);
            Debug.Log("2222222222222222222222");
        }
    }

    public virtual void OnHit(BigNumber _dmg, float _crit)
    {

    }

    public virtual void OnHit(string _targetName)
    {

    }

    public virtual void OnHit(GameObject _go)
    {

    }
}

public enum BodyPart
{
    HEAD = 0,
    BODY = 1
}