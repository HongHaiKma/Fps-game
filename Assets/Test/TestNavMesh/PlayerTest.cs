using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTest : MonoBehaviour
{
    public NavMeshAgent m_Agent;
    public GameObject m_Enemy;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Agent.SetDestination(m_Enemy.transform.position);
            Debug.Log("Keyco");
        }
    }
}
