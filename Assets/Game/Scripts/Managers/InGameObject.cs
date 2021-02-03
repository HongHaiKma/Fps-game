using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObject : MonoBehaviour, ITakenDamage
{
    public virtual void OnHit() { }
    public virtual void OnHit(BigNumber _dmg, float _crit) { }

    public virtual void OnHit(BigNumber _dmg) { }

    public virtual void OnHit(string _targetName) { }

    public virtual void OnHit(GameObject _go) { }
}
