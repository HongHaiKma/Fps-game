using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigDataClass
{

}

public class BulletConfigData
{
    public BigNumber m_Dmg;
    public string m_PrefabName;
    public Quaternion m_Rotation;

    public BulletConfigData(BigNumber _dmg, string _prefabName, Quaternion _rotation)
    {
        m_Dmg = _dmg;
        m_PrefabName = _prefabName;
        m_Rotation = _rotation;
    }
}