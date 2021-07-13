using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : Singleton<ObjectsManager>
{
    public MapController m_Map;

    public List<Character> m_Team1;
    public List<Character> m_Team2;

    public int m_SpawnTeam;

    public override void OnEnable()
    {
        SpawnTeam1(m_SpawnTeam);
        SpawnTeam2(m_SpawnTeam);

        EventManager.CallEvent(GameEvent.SET_CHAR_TARGET);
        EventManager.CallEvent(GameEvent.SET_HEALTH_BAR);
        StartCoroutine(TestSetChar());
        base.OnEnable();
    }

    IEnumerator TestSetChar()
    {
        yield return Yielders.EndOfFrame;
        EventManager1<bool>.CallEvent(GameEvent.SET_CMLOOK_TARGET, true);
    }

    public void Test()
    {
        EventManager.CallEvent(GameEvent.DESPAWN);
    }

    public void SpawnTeam1(int _number)
    {
        for (int i = 0; i < m_SpawnTeam; i++)
        {
            Vector3 pos = ConfigManager.Instance.m_Team1StartPos[Random.Range(0, ConfigManager.Instance.m_Team1StartPos.Count - 1)].position;
            int charRandom = Random.Range(0, PrefabManager.Instance.m_CharPrefabs.Length);
            Character charrr;
            if (m_Team1.Count < 5)
            {
                charrr = PrefabManager.Instance.SpawnCharPool(charRandom, pos).GetComponent<Character>();
            }
            else
            {
                return;
            }
            charrr.SetupCharacter(TEAM.Team1);
            m_Team1.Add(charrr);
        }
    }

    public void SpawnTeam2(int _number)
    {
        for (int i = 0; i < m_SpawnTeam; i++)
        {
            Vector3 pos = ConfigManager.Instance.m_Team2StartPos[Random.Range(0, ConfigManager.Instance.m_Team2StartPos.Count - 1)].position;
            int charRandom = Random.Range(0, PrefabManager.Instance.m_CharPrefabs.Length);
            Character charrr;
            if (m_Team2.Count < 5)
            {
                charrr = PrefabManager.Instance.SpawnCharPool(charRandom, pos).GetComponent<Character>();
            }
            else
            {
                return;
            }
            charrr.SetupCharacter(TEAM.Team2);
            m_Team2.Add(charrr);
        }
    }

    public Character GetRandomTeam1()
    {
        return m_Team1[Random.Range(0, m_Team1.Count - 1)];
    }

    public Character GetRandomTeam2()
    {
        return m_Team2[Random.Range(0, m_Team2.Count - 1)];
    }

    public void RemoveDeadChar(TEAM _team, Character _char)
    {
        if (_team == TEAM.Team1)
        {
            m_Team1.Remove(_char);
        }
        else if (_team == TEAM.Team2)
        {
            m_Team2.Remove(_char);
        }
    }
}

// public class EntityMap : Dictionary<double, InGameObject> { }
