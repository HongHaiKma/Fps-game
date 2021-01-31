using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelPart : MonoBehaviour, ITakenDamage
{
    public Character m_OwnerChar;
    public BodyPart m_BodyPart;

    public virtual void OnHit()
    {
        Debug.Log("Char is: " + gameObject.name);
    }

    public virtual void OnHit(string _targetName)
    {

    }

    public virtual void OnHit(GameObject _go)
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