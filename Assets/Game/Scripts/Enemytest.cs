using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemytest : MonoBehaviour, ITakenDamage
{
    public virtual void OnHit()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy hit!!!");
    }
}
