using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemytest : MonoBehaviour
{
    public virtual void OnHit()
    {

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

    private void OnTriggerEnter(Collider other)
    {
        // Helper.DebugLog("Enemy hit!!!");
    }
}
