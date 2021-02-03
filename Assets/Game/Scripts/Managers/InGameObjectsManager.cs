using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObjectsManager : Singleton<InGameObjectsManager>
{
    public List<Character> m_Team1;
    public List<Character> m_Team2;

    private void OnEnable()
    {
        for (int i = 0; i < 1; i++)
        {
            Vector3 pos = ConfigManager.Instance.m_Team1StartPos[Random.Range(0, ConfigManager.Instance.m_Team1StartPos.Count - 1)];
            Character charrr = PrefabManager.Instance.SpawnCharPool(ConfigName.char1, pos).GetComponent<Character>();
            charrr.m_Team = TEAM.Team1;
            m_Team1.Add(charrr);
            // charrr.tf_Target = m_Team2[Random.Range(0, m_Team2.Count - 1)].tf_Owner;
        }
        for (int i = 0; i < 1; i++)
        {
            Vector3 pos = ConfigManager.Instance.m_Team2StartPos[Random.Range(0, ConfigManager.Instance.m_Team2StartPos.Count - 1)];
            Character charrr = PrefabManager.Instance.SpawnCharPool(ConfigName.char1, pos).GetComponent<Character>();
            charrr.m_Team = TEAM.Team2;
            m_Team2.Add(charrr);
            charrr.tf_Target = m_Team1[Random.Range(0, m_Team1.Count - 1)].tf_Owner;
        }

        EventManagerWithParam<bool>.CallEvent(GameEvent.SET_CMLOOK_TARGET, true);
        Debug.Log("InGameObjectsManager awake!!!");
    }
}

public class EntityMap : Dictionary<double, InGameObject> { }
