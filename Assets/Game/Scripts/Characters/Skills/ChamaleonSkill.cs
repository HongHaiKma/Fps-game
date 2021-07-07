using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamaleonSkill : NotDurationSkill
{
    public override void HandleSkill()
    {
        Helper.DebugLog("ChamaleonSkill handle skil");
        CharacterChameleon charrr = m_Char as CharacterChameleon;
        charrr.Skill_Dash();
    }
}
