using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : Singleton<CamController>
{
    public CameraShaker m_CamShaker;

    public Character m_Char;
    public Transform tf_Owner;
    public Transform tf_CamCrosshair;
    public LayerMask lm_Char;
    public LayerMask lm_Ignore;

    [Header("Camera Free Look")]
    public CinemachineFreeLook m_CMFreeLook;
    // public CinemachineVirtualCamera m_CMFreeLook2;
    // public CinemachineBasicMultiChannelPerlin m_CMChanelPerlin;
    public float m_ShakeTime;

    private void Awake()
    {
        Helper.DebugLog("CamControler Awake Calledddd");
        // m_CMChanelPerlin = m_CMFreeLook.GetComponent<CinemachineBasicMultiChannelPerlin>();
        // m_CMChanelPerlin = m_CMFreeLook.Add<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnEnable()
    {
        // m_CMFreeLook.Follow = GameManager.Instance.m_Team1[0].tf_Owner;
        // m_CMFreeLook.LookAt = GameManager.Instance.m_Team1[0].tf_Head;
        // m_CMChanelPerlin = m_CMFreeLook.GetCinema<CinemachineBasicMultiChannelPerlin>();
        // m_CMFreeLook.m_ExcludedPropertiesInInspector
        // m_CMChanelPerlin = m_CMFreeLook2.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StartListenToEvents();
    }

    private void OnDisable()
    {
        StopListenToEvents();
    }

    public void StartListenToEvents()
    {
        EventManagerWithParam<Vector2>.AddListener(GameEvent.SET_CMLOOK_VALUE, SetCMLookAxisValue);
        EventManagerWithParam<bool>.AddListener(GameEvent.SET_CMLOOK_TARGET, ChangeCameraToAnotherChar);
    }

    public void StopListenToEvents()
    {
        EventManagerWithParam<Vector2>.RemoveListener(GameEvent.SET_CMLOOK_VALUE, SetCMLookAxisValue);
        EventManagerWithParam<bool>.RemoveListener(GameEvent.SET_CMLOOK_TARGET, ChangeCameraToAnotherChar);
    }

    private void Update()
    {
        if (m_Char != null)
        {
            if (m_Char.m_AI)
            {
                m_Char.m_AI = false;
            }
        }

        // if (m_ShakeTime > 0f)
        // {
        //     Helper.DebugLog("m_ShakeTime > 0f");
        //     m_ShakeTime -= Time.deltaTime;
        //     if (m_ShakeTime <= 0f)
        //     {
        //         m_CMChanelPerlin.m_AmplitudeGain = 0f;
        //     }
        // }
    }

    void FixedUpdate()
    {
        // Debug.DrawLine(tf_Owner.position, tf_CamCrosshair.position, Color.green);
        // Debug.DrawLine(tf_Owner.position, tf_Owner.forward * 80f, Color.green);

        CheckShoot();
    }

    public void Shake()
    {
        Helper.DebugLog("CamController shakeeeeeeee!!!!!!!!!!!!");
        m_CamShaker.Shake();
    }

    // public void Shake(float _amplitude, float _duration)
    // {
    //     m_CMChanelPerlin.m_AmplitudeGain = _amplitude;
    //     m_ShakeTime = _duration;
    //     Helper.DebugLog("Camera shake called!!!!!");
    // }

    public void CheckShoot()
    {
        Debug.DrawLine(tf_Owner.position, tf_CamCrosshair.position, Color.green);
        RaycastHit[] hit = Physics.RaycastAll(tf_Owner.position, tf_Owner.forward * 80f);
        // RaycastHit[] hit = Physics.RaycastAll(tf_Owner.position, tf_Owner.forward * 80f, Mathf.Infinity);
        // RaycastHit[] hit = Physics.RaycastAll(tf_Owner.position, tf_Owner.forward * 80f, Mathf.Infinity, lm_Ignore);
        EventManagerWithParam<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_POS, tf_CamCrosshair.position);

        int hitCount = hit.Length;

        if (hitCount <= 0)
        {
            return;
        }

        for (int i = 0; i < hitCount; i++)
        {
            if ((lm_Char.value & (1 << hit[i].transform.gameObject.layer)) > 0)
            {
                tf_CamCrosshair.position = hit[i].point;
                EventManagerWithParam<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_POS, tf_CamCrosshair.position);
                break;
            }

            // if ((lm_Ignore.value & (1 << hit[i].transform.gameObject.layer)) > 0)
            // {
            //     // tf_CamCrosshair.position = hit[0].point;
            //     // EventManagerWithParam<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_POS, tf_CamCrosshair.position);
            //     continue;
            // }

            tf_CamCrosshair.position = hit[0].point;
            EventManagerWithParam<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_POS, tf_CamCrosshair.position);
        }
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

    public void ChangeCameraToAnotherChar(bool _init)
    {
        if (_init)
        {
            if (m_Char != null)
            {
                m_Char.m_AI = true;
            }

            List<Character> chars = InGameObjectsManager.Instance.m_Team1;

            int a = Random.Range(0, chars.Count);
            chars[a].m_AI = false;

            m_CMFreeLook.Follow = chars[a].tf_Owner;
            m_CMFreeLook.LookAt = chars[a].tf_Head;

            m_Char = null;
            m_Char = chars[a];

            EventManager.CallEvent(GameEvent.SET_HEALTH_BAR);

            return;
        }

        if (m_Char.gameObject.activeInHierarchy)
        {
            if (m_Char != null)
            {
                m_Char.m_AI = true;
            }

            List<Character> chars = InGameObjectsManager.Instance.m_Team1;

            int a = Random.Range(0, chars.Count - 1);
            chars[a].m_AI = false;
            m_CMFreeLook.Follow = chars[a].tf_Owner;
            m_CMFreeLook.LookAt = chars[a].tf_Head;

            m_Char = null;
            m_Char = chars[a];
        }
        else
        {
            if (m_Char != null)
            {
                m_Char.m_AI = true;
            }

            List<Character> chars = InGameObjectsManager.Instance.m_Team1;

            int a = Random.Range(0, chars.Count - 1);
            chars[a].m_AI = false;
            m_CMFreeLook.Follow = chars[a].tf_Owner;
            m_CMFreeLook.LookAt = chars[a].tf_Head;

            m_Char = null;
            m_Char = chars[a];
        }

        EventManager.CallEvent(GameEvent.SET_HEALTH_BAR);
    }

    public void Test()
    {
        EventManagerWithParam<bool>.CallEvent(GameEvent.SET_CMLOOK_TARGET, true);
    }
}