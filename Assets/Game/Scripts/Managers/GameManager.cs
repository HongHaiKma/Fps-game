using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<Character> m_Team1;
    public List<Character> m_Team2;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        // #if UNITY_EDITOR
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
        // #endif
        // Input.multiTouchEnabled = false;
    }

    // public Character GetNearestEnemyInRange(Transform finder, float range)
    // {
    //     Vector3 finderPos = finder.position;
    //     Character nearestCharacter = null;
    //     float nearestDistance = 9999999;
    //     foreach (int key in m_IngameEnemyMap.Keys)
    //     {
    //         Character c;
    //         if (m_IngameEnemyMap.TryGetValue(key, out c))
    //         {
    //             if (c != null && !c.IsDead() && !c.IsHideOnRadar)
    //             {
    //                 Vector3 cPos = c.transform.position;
    //                 float distance = (cPos - finderPos).magnitude;
    //                 if (distance < nearestDistance && distance <= range)
    //                 {
    //                     nearestCharacter = c;
    //                     nearestDistance = distance;
    //                 }
    //             }
    //         }
    //     }
    //     return nearestCharacter;
    // }
}
