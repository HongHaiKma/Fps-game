using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObjectsManager : Singleton<InGameObjectsManager>
{
    public List<Character> m_Team1;
    public List<Character> m_Team2;

    private void OnEnable()
    {
        SpawnTeam1(2);
        SpawnTeam2(1);

        EventManager.CallEvent(GameEvent.SET_CHAR_TARGET);
        EventManagerWithParam<bool>.CallEvent(GameEvent.SET_CMLOOK_TARGET, true);
        EventManager.CallEvent(GameEvent.SET_HEALTH_BAR);
        // Debug.Log("InGameObjectsManager awake!!!");
    }

    public void SpawnTeam1(int _number)
    {
        for (int i = 0; i < _number; i++)
        {
            Vector3 pos = ConfigManager.Instance.m_Team1StartPos[Random.Range(0, ConfigManager.Instance.m_Team1StartPos.Count - 1)];
            Character charrr = PrefabManager.Instance.SpawnCharPool(ConfigName.char1, pos).GetComponent<Character>();
            charrr.m_Team = TEAM.Team1;
            charrr.LoadCharacterConfig();
            charrr.SetupComponents();
            charrr.m_HealthBar.SetHpBar();
            m_Team1.Add(charrr);
        }
    }

    public void SpawnTeam2(int _number)
    {
        for (int i = 0; i < _number; i++)
        {
            Vector3 pos = ConfigManager.Instance.m_Team2StartPos[Random.Range(0, ConfigManager.Instance.m_Team2StartPos.Count - 1)];
            Character charrr = PrefabManager.Instance.SpawnCharPool(ConfigName.char1, pos).GetComponent<Character>();
            charrr.m_Team = TEAM.Team2;
            charrr.LoadCharacterConfig();
            charrr.SetupComponents();
            charrr.m_HealthBar.SetHpBar();
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

public class EntityMap : Dictionary<double, InGameObject> { }
