using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image img_HealthBar;
    public void SetHealthBar(BigNumber _percent)
    {
        img_HealthBar.fillAmount = _percent.ToFloat();
    }
}
