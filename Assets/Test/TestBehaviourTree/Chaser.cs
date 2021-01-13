using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class Chaser : MonoBehaviour
{
    public NavMeshAgent nav_Agent;
    public Transform tf_Owner;
    public Transform tf_Target;

    [Task]
    public bool IsInRange()
    {
        float distance = Helper.CalDistance(tf_Owner.position, tf_Target.position);

        return (distance <= nav_Agent.stoppingDistance);
    }

    [Task]
    public void OnChasing()
    {
        nav_Agent.SetDestination(tf_Target.position);
    }

    [Task]
    public void OnIdling()
    {
        // Helper.DebugLog("Idle");
    }
}
