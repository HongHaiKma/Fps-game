using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAngelia : Character
{
    public override void TurnOnSkill()
    {
        m_ShootRange = 300f;
    }

    public override void TurnOffSkill()
    {
        m_ShootRange = m_ShootRange = 20.5f;
    }
}
