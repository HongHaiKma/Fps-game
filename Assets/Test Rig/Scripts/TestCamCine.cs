using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamCine : MonoBehaviour
{
    public Transform tf_Owner;
    public Transform tf_End;
    public Transform tf_CamCrosshair;

    void Update()
    {
        Debug.DrawLine(tf_Owner.position, tf_End.position, Color.green);

        CheckShoot();
    }

    public bool CheckShoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(tf_Owner.position, tf_Owner.forward, out hit))
        {
            // // ITakenDamage iTaken = hit.transform.GetComponent<ITakenDamage>();
            // // if (iTaken != null && CanShot(hit.transform.position))
            // ITakenDamage iTaken = hit.transform.GetComponent<ITakenDamage>();
            // if (iTaken != null && CanShot(hit.transform.position))
            // {
            //     // tf_Onwer.LookAt(hit.transform);
            //     // tf_CamPoint.LookAt(hit.transform);
            //     // tf_FirePoint.LookAt(hit.transform);
            //     return true;
            // }

            Collider col = hit.transform.GetComponent<Collider>();

            if (col != null)
            {
                // tf_CamCrosshair.position = hit.transform.position;
                tf_CamCrosshair.position = hit.point;
                Debug.Log("Hit name: " + hit.transform.gameObject.name);
            }

            return false;
        }

        return false;
    }

    // public bool CanShot(Vector3 _des)
    // {
    //     // return ((m_ShootCd >= m_MaxShootCd) && Helper.InRange(tf_Owner.position, _des, 15f));
    //     // return Helper.InRange(tf_Owner.position, _des, 15f));
    // }
}
