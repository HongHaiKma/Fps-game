using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class PPManager : Singleton<PPManager>
{
    public Volume m_Volume;
    private LensDistortion m_LensDistortion;

    float maxLensIntensity = -1f;
    float vignIntensity = 0;
    Tween lensTween;

    public void AnimateVignette(bool value)
    {
        DOTween.To(
            () =>
            vignIntensity,
            x => vignIntensity = x, -0.8f, 0.5f).OnUpdate(OnUpdateLensDistortion).
            OnComplete(() =>
            {
                if (m_Volume.profile.TryGet(out m_LensDistortion))
                {
                    m_LensDistortion.intensity.Override(0f);
                }
            });
    }


    void OnUpdateLensDistortion()
    {
        if (m_Volume.profile.TryGet(out m_LensDistortion))
        {
            m_LensDistortion.intensity.Override(vignIntensity);
        }
    }

    public void DashLens()
    {
        AnimateVignette(false);
    }

    IEnumerator IEDashLens()
    {
        if (m_Volume.profile.TryGet(out m_LensDistortion))
        {
            float time = 0f;
            float timeMax = -1f;
            while (time > timeMax)
            {
                time -= Time.deltaTime;
                m_LensDistortion.intensity.Override(time);
            }
            yield return Yielders.Get(1f);
            m_LensDistortion.intensity.Override(0f);
            Helper.DebugLog("Get lens distorrion");
        }
    }
}
