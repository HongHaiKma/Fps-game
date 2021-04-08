using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    public CinemachineCameraOffset m_CineCamOffSet;

    public Transform camTransform;
    public float shakeDuration = 0f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    public void Shake()
    {
        shakeDuration = 0.3f;
        Helper.DebugLog("Camera Shakiiiiiiiiiiiiiiing!!!!");
    }
    void Update()
    {
        if (shakeDuration > 0)
        {
            // Vector3 rand = camTransform.position + Random.insideUnitSphere * shakeAmount;
            Vector3 rand = m_CineCamOffSet.m_Offset + Random.insideUnitSphere * shakeAmount;
            // camTransform.position = new Vector3(rand.x, camTransform.position.y, rand.z);
            m_CineCamOffSet.m_Offset = new Vector3(m_CineCamOffSet.m_Offset.x, m_CineCamOffSet.m_Offset.y, rand.z);

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            m_CineCamOffSet.m_Offset = new Vector3(0.4f, 0.05f, 0.22f);
        }
    }
}