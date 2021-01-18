using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform tf_Owner;
    public Transform tf_CamCrosshair;

    void FixedUpdate()
    {
        // Debug.DrawLine(tf_Owner.position, tf_CamCrosshair.position, Color.green);
        // Debug.DrawLine(tf_Owner.position, tf_Owner.forward * 80f, Color.green);

        CheckShoot();
    }

    public bool CheckShoot()
    {
        // Debug.DrawLine(tf_Owner.position, tf_Owner.forward * 80f, Color.green);
        // Debug.DrawLine(tf_Owner.position, tf_Owner.forward * 80f, Color.green);
        // Debug.DrawLine(tf_Owner.position, tf_CamCrosshair.position, Color.green);
        // Debug.DrawRay(tf_Owner.position, tf_Owner.forward * 80f);
        // RaycastHit[] hit = Physics.RaycastAll(tf_Owner.position, tf_Owner.forward * 80f); ;

        Debug.DrawLine(tf_Owner.position, tf_CamCrosshair.position, Color.green);
        RaycastHit[] hit = Physics.RaycastAll(tf_Owner.position, tf_Owner.forward * 80f); ;
        EventManagerWithParam<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_POS, tf_CamCrosshair.position);

        if (hit.Length <= 0)
        {
            // Debug.Log("Raycast not hit!!!");
            return false;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    Debug.Log("Hit name: " + hit[0].transform.name);
                }
            }
        }

        // Debug.Log("All hits: ");
        // for (int i = 0; i < hit.Length; i++)
        // {
        //     Debug.Log("Hit name: " + hit[0].transform.name);
        // }

        // if (Physics.Raycast(tf_Owner.position, tf_Owner.forward * 80f))
        // {
        // Transform tf = hit[0].transform;
        Transform tf = hit[0].transform;
        // Transform tf = hit[hit.Length - 1].transform;
        Collider col = tf.GetComponent<Collider>();

        if (col != null)
        {
            // Debug.Log("Raycast hit name: " + hit[0].transform.name);
            tf_CamCrosshair.position = hit[0].point;
            // tf_CamCrosshair.position = hit[hit.Length - 1].point;
            EventManagerWithParam<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_POS, tf_CamCrosshair.position);
            return true;
        }

        return false;
        // }

        // return false;
    }

    // public bool CanShot(Vector3 _des)
    // {
    //     // return ((m_ShootCd >= m_MaxShootCd) && Helper.InRange(tf_Owner.position, _des, 15f));
    //     // return Helper.InRange(tf_Owner.position, _des, 15f));
    // }
}
