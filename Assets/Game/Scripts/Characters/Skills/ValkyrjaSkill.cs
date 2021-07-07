using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrjaSkill : NotDurationSkill
{
    public override void HandleSkill()
    {
        // Helper.DebugLog("ValkyrjaSkill");
        CharacterValkyrja charrr = m_Char as CharacterValkyrja;
        charrr.Skill_LaunchRockets();
    }
}
