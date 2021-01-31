using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObject : MonoBehaviour, ITakenDamage
{
    public virtual void OnHit()
    {

    }

    public virtual void OnHit(string _targetName)
    {

    }

    public virtual void OnHit(GameObject _go)
    {

    }
}
