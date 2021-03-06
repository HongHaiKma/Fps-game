using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagShootPoint : MonoBehaviour, ITakenDamage
{
    public InGameObjectType m_InGameObjectType;

    public TEAM m_Team;
    public Flag m_Flag;

    public void OnHit() { }
    public void OnHit(BigNumber _dmg, float _crit) { }

    public void OnHit(BigNumber _dmg)
    {
        m_Flag.OnHit(_dmg);
    }

    public void OnHit(string _targetName) { }

    public void OnHit(GameObject _go) { }

    public virtual InGameObjectType GetInGameObjectType()
    {
        return m_InGameObjectType;
    }

    public virtual TEAM GetTeam()
    {
        return m_Team;
    }
}
