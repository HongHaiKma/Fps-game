﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{
    public Transform tf_Owner;
    public Transform tf_CamCrosshair;
    public LayerMask m_LayerMask;

    public CinemachineFreeLook m_CMFreeLook;

    private void OnEnable()
    {
        StartListenToEvents();
    }

    private void OnDisable()
    {
        StopListenToEvents();
    }

    public void StartListenToEvents()
    {
        EventManagerWithParam<Vector2>.AddListener(GameEvent.Set_CMLOOK_VALUE, SetCMLookAxisValue);
    }

    public void StopListenToEvents()
    {
        EventManagerWithParam<Vector2>.RemoveListener(GameEvent.Set_CMLOOK_VALUE, SetCMLookAxisValue);
    }

    void FixedUpdate()
    {
        // Debug.DrawLine(tf_Owner.position, tf_CamCrosshair.position, Color.green);
        // Debug.DrawLine(tf_Owner.position, tf_Owner.forward * 80f, Color.green);

        CheckShoot();
    }

    public bool CheckShoot()
    {
        Debug.DrawLine(tf_Owner.position, tf_CamCrosshair.position, Color.green);
        RaycastHit[] hit = Physics.RaycastAll(tf_Owner.position, tf_Owner.forward * 80f);
        EventManagerWithParam<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_POS, tf_CamCrosshair.position);

        int hitCount = hit.Length;

        if (hitCount <= 0)
        {
            return false;
        }

        int index = 0;
        for (int i = 0; i < hitCount; i++)
        {
            if ((m_LayerMask.value & (1 << hit[i].transform.gameObject.layer)) > 0)
            {
                index = i;
            }
        }

        Transform tf = hit[index].transform;
        Collider col = tf.GetComponent<Collider>();

        if (col != null)
        {
            tf_CamCrosshair.position = hit[index].point;
            EventManagerWithParam<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_POS, tf_CamCrosshair.position);
            return true;
        }

        return false;
    }

    public void SetCMLookAxisValue(Vector2 _mouseInput)
    {
        if (_mouseInput.magnitude > 0.015f)
        {
            m_CMFreeLook.m_XAxis.m_InputAxisValue = _mouseInput.x;
            m_CMFreeLook.m_YAxis.m_InputAxisValue = _mouseInput.y;
        }
        else
        {
            m_CMFreeLook.m_XAxis.m_InputAxisValue = 0f;
            m_CMFreeLook.m_YAxis.m_InputAxisValue = 0f;
        }
    }
}