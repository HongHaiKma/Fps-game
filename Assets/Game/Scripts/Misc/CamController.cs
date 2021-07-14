using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : Singleton<CamController>
{
    public CameraShaker m_CamShaker;

    public Character m_Char;
    public Transform tf_Owner;
    public Transform tf_CamCrosshairAim;
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
        // m_CMChanelPerlin = m_CMFreeLook.GetComponent<CinemachineBasicMultiChannelPerlin>();
        // m_CMChanelPerlin = m_CMFreeLook.Add<CinemachineBasicMultiChannelPerlin>();
    }

    public override void AddListener()
    {
        EventManager1<Vector2>.AddListener(GameEvent.SET_CMLOOK_VALUE, SetCMLookAxisValue);
        EventManager1<bool>.AddListener(GameEvent.SET_CMLOOK_TARGET, ChangeCameraToAnotherChar);
    }

    public override void RemoveListener()
    {
        EventManager1<Vector2>.RemoveListener(GameEvent.SET_CMLOOK_VALUE, SetCMLookAxisValue);
        EventManager1<bool>.RemoveListener(GameEvent.SET_CMLOOK_TARGET, ChangeCameraToAnotherChar);
    }

    void FixedUpdate()
    {
        // Debug.DrawLine(tf_Owner.position, tf_CamCrosshair.position, Color.green);
        // Debug.DrawLine(tf_Owner.position, tf_Owner.forward * 80f, Color.green);

        EventManager1<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_POS, tf_CamCrosshair.position);

        CheckShoot();
    }

    public void Shake()
    {
        m_CamShaker.Shake();
    }

    public void CheckShoot()
    {
        RaycastHit[] hit = Physics.RaycastAll(tf_Owner.position, tf_Owner.forward * 80f);
        EventManager1<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_AIM_POS, tf_CamCrosshairAim.position);

        int hitCount = hit.Length;

        if (hitCount <= 0)
        {
            return;
        }

        for (int i = 0; i < hitCount; i++)
        {
            if ((lm_Char.value & (1 << hit[i].transform.gameObject.layer)) > 0)
            {
                tf_CamCrosshairAim.position = hit[i].point;
                EventManager1<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_AIM_POS, tf_CamCrosshairAim.position);
                break;
            }

            if (m_Char != null)
            {
                float hitPointDistance = Helper.CalDistance(hit[0].point, CamController.Instance.tf_Owner.position);
                float charDistance = Helper.CalDistance(m_Char.tf_Owner.position, CamController.Instance.tf_Owner.position);

                if (hitPointDistance < charDistance)
                {
                    continue;
                }
            }

            tf_CamCrosshairAim.position = hit[0].point;
            EventManager1<Vector3>.CallEvent(GameEvent.SET_CHAR_CROSSHAIR_AIM_POS, tf_CamCrosshairAim.position);
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

    public void SelectCharMiniMap(Character _character)
    {
        if (m_Char != null)
        {
            m_Char.m_AI = true;
        }

        _character.m_AI = false;
        m_CMFreeLook.Follow = _character.tf_Owner;
        m_CMFreeLook.LookAt = _character.tf_Head;

        m_Char = null;
        m_Char = _character;
        m_Char.m_AI = false;

        // EventManager.CallEvent(GameEvent.DEACTIVATE_SKILL);
        EventManager.CallEvent(GameEvent.SET_HP_BAR_UI);
        EventManager.CallEvent(GameEvent.SET_SKILL_BUTTON);
        EventManager.CallEvent(GameEvent.SET_HEALTH_BAR);
    }

    public void ChangeCameraToAnotherChar(bool _init)
    {
        if (_init)
        {
            if (m_Char != null)
            {
                m_Char.m_AI = true;
            }

            List<Character> chars = ObjectsManager.Instance.m_Team1;

            int a = Random.Range(0, chars.Count);
            chars[a].m_AI = false;

            m_CMFreeLook.Follow = chars[a].tf_Owner;
            m_CMFreeLook.LookAt = chars[a].tf_Head;

            m_Char = null;
            m_Char = chars[a];
            m_Char.m_AI = false;

            EventManager.CallEvent(GameEvent.SET_HEALTH_BAR);

            return;
        }
        else
        {
            if (m_Char != null)
            {
                m_Char.m_AI = true;
            }

            List<Character> chars = ObjectsManager.Instance.m_Team1;

            int a = Random.Range(0, chars.Count - 1);
            chars[a].m_AI = false;
            m_CMFreeLook.Follow = chars[a].tf_Owner;
            m_CMFreeLook.LookAt = chars[a].tf_Head;

            m_Char = null;
            m_Char = chars[a];
            m_Char.m_AI = false;
        }

        EventManager.CallEvent(GameEvent.SET_HP_BAR_UI);
        EventManager.CallEvent(GameEvent.SET_SKILL_BUTTON);
        EventManager.CallEvent(GameEvent.SET_HEALTH_BAR);
    }

    public void Test()
    {
        EventManager.CallEvent(GameEvent.DEACTIVATE_SKILL);
        EventManager1<bool>.CallEvent(GameEvent.SET_CMLOOK_TARGET, true);
        EventManager.CallEvent(GameEvent.SET_SKILL_BUTTON);
        EventManager.CallEvent(GameEvent.SET_HP_BAR_UI);
    }
}