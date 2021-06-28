using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMain : Character
{
    public override void HandleDeath()
    {
        ObjectsManager.Instance.RemoveDeadChar(m_Team, this);
        PrefabManager.Instance.DespawnPool(gameObject);

        if (m_Team == TEAM.Team1)
        {
            ObjectsManager.Instance.SpawnTeam1(1);
        }
        else if (m_Team == TEAM.Team2)
        {
            ObjectsManager.Instance.SpawnTeam2(1);
        }

        if (!m_AI)
        {
            EventManager1<bool>.CallEvent(GameEvent.SET_CMLOOK_TARGET, false);
        }

        EventManager.CallEvent(GameEvent.SET_HEALTH_BAR);
    }
}
