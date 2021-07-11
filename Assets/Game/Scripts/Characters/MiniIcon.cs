using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiniIcon : MonoBehaviour
{
    public Character m_Char;
    public Button btn_MiniIcon;
    public Canvas m_Canvas;
    public Image img_Icon;
    public GameObject g_UI;

    private IEnumerator Start()
    {
        g_UI.SetActive(true);
        m_Canvas.worldCamera = InGameManager.Instance.m_MiniMapCam;
        GUIManager.Instance.AddClickEvent(btn_MiniIcon, OnChooseChar);

        yield return Yielders.EndOfFrame;

        if (m_Char.GetTeam() == TEAM.Team1)
        {
            img_Icon.color = Color.blue;
        }
        else if (m_Char.GetTeam() == TEAM.Team2)
        {
            img_Icon.color = Color.red;
        }
    }

    // private void OnEnable()
    // {
    //     if (m_Char.GetTeam() == TEAM.Team1)
    //     {
    //         img_Icon.color = Color.blue;
    //     }
    //     else if (m_Char.GetTeam() == TEAM.Team2)
    //     {
    //         img_Icon.color = Color.red;
    //         Helper.DebugLog("Team 2");
    //     }
    // }

    public void OnChooseChar()
    {
        if (m_Char.GetTeam() == TEAM.Team1)
        {
            InGameManager.Instance.OnUnClickMiniMap();
            CamController.Instance.SelectCharMiniMap(m_Char);
        }
    }
}
