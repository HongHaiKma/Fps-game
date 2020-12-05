using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform m_TargetTf;
    public Transform m_Tf;

    private void LateUpdate()
    {
        m_Tf.position = m_TargetTf.position;
        m_Tf.rotation = m_TargetTf.rotation;
    }
}
