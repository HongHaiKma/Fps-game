using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigDataClass
{

}

public class BulletConfigData
{
    public TEAM m_Team;
    public BigNumber m_Dmg;
    public string m_PrefabName;
    public Quaternion m_Rotation;

    public BulletConfigData(TEAM _team, BigNumber _dmg, string _prefabName, Quaternion _rotation)
    {
        m_Team = _team;
        m_Dmg = _dmg;
        m_PrefabName = _prefabName;
        m_Rotation = _rotation;
    }
}