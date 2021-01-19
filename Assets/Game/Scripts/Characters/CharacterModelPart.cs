using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelPart : MonoBehaviour, ITakenDamage
{
    public Character m_OwnerChar;
    public BodyPart m_BodyPart;

    public virtual void OnHit()
    {

    }

    public virtual void OnHit(string _targetName)
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }
}

public enum BodyPart
{
    HEAD = 0,
    BODY = 1
}