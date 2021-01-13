using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemytest : MonoBehaviour, ITakenDamage
{
    public virtual void OnHit()
    {

    }

    public virtual void OnHit(string _targetName)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // Helper.DebugLog("Enemy hit!!!");
    }
}
