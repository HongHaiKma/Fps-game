using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform tf_Owner;
    public Transform tf_CamCrosshair;

    void Update()
    {
        Debug.DrawLine(tf_Owner.position, tf_CamCrosshair.position, Color.green);

        CheckShoot();
    }

    public bool CheckShoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(tf_Owner.position, tf_Owner.forward * 80f, out hit))
        {
            Collider col = hit.transform.GetComponent<Collider>();

            if (col != null)
            {
                tf_CamCrosshair.position = hit.point;
                return true;
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
