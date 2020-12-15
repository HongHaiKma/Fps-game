using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform m_TargetTf;
    public Transform tf_Owner;

    private void LateUpdate()
    {
        tf_Owner.position = m_TargetTf.position;
        tf_Owner.rotation = m_TargetTf.rotation;
    }
}
