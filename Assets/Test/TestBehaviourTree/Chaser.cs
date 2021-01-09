using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    public NavMeshAgent nav_Agent;
    public Transform tf_Owner;
    public Transform tf_Target;

    private void Update()
    {
        float distance = Helper.CalDistance(tf_Owner.position, tf_Target.position);

        if (distance > nav_Agent.stoppingDistance)
        {
            nav_Agent.SetDestination(tf_Target.position);
        }
        else
        {
            Helper.DebugLog("Idle");
        }
    }
}
