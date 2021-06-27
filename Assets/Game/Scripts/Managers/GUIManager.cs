using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-91)]
public class GUIManager : MonoBehaviour
{
    internal class GUIMap : Dictionary<int, UICanvas>
    {
        public static int m_NextID = 0;
        public int m_ID = 0;
        public GUIMap()
        {
            //Debug.Log("Create new GUI MAP " + m_NextID);
            m_ID = m_NextID;
            m_NextID++;
        }
    }

    AsyncOperation async;
    bool isLoadInitScene = false;
    private GUIMap m_GUIMap;

    private Vector3 m_CenterPos;

    private float m_OffsetTop = 0;
    public bool IsLongDevice = false;

    private UICanvas m_PreviousPopup = null;
    private UICanvas m_PreviousPanel = null;
    public GameObject m_MainCanvas;


    private bool IsHoldBackkey = false;

    public List<UICanvas> m_CurrentOpenedPopup = new List<UICanvas>();
    public List<UICanvas> m_CurrentOpenedPanel = new List<UICanvas>();

    [Header("Get Panels")]
    // private PanelLoadingAds m_PanelLoadingAds;
    public GameObject g_SubCanvas;
    // public PanelLoading m_PanelLoading;
    public GameObject g_PanelLoading;
    public GameObject g_IngameLoading;
    // public PanelInGame m_PanelInGame;

    private static GUIManager m_Instance;
    public static GUIManager Instance
    {
        get
        {
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
            this.m_GUIMap = new GUIMap();
            // if (m_SubCanvas != null)
            // {
            //     DontDestroyOnLoad(m_SubCanvas);
            // }
            FindMainCanvas();
            // float ratio = (float)Screen.height / (float)Screen.width;
            // if (ratio > 2.1f)
            // {
            //     m_OffsetTop = -50f;
            // }
            // if (ratio > 1.8f)
            // {
            //     IsLongDevice = true;
            // }
            // m_CenterPos = Vector3.zero + new Vector3(0, m_OffsetTop);
        }

        if (g_SubCanvas != null)
        {
            DontDestroyOnLoad(g_SubCanvas);
        }

        int maxScreenHeight = 1080;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }

    void Start()
    {
        int maxScreenHeight = 1080;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }

        // FindPanelLoading();
        // 
        // LoadPlayScene();
    }

    public void ClearAllOpenedPopupList()
    {
        m_CurrentOpenedPopup.Clear();
    }

    public void LoadPlayScene(bool _continue = false)
    {
        StartCoroutine(LoadScreen(_continue));
    }

    IEnumerator LoadScreen(bool _continue)
    {
        // ObjectsManager.Instance.DespawnAllPools();
        Debug.Log("Start Load");
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        // async = SceneManager.LoadSceneAsync("InitScene", LoadSceneMode.Single);
        async = SceneManager.LoadSceneAsync("PlayScene", LoadSceneMode.Single);
        async.allowSceneActivation = false;

        // float _loadProgress = 0;
        // while (_loadProgress <= 1)
        // {
        //     _loadProgress += 0.02f;
        //     m_PanelLoading.img_LoadingBar.fillAmount = _loadProgress;
        //     // img_LoadingBar.fillAmount = _loadProgress;
        //     int percent = (int)(_loadProgress * 100f);
        //     if (percent > 100) percent = 100;
        //     // m_TextLoadingPer.text = percent + "%";
        //     yield return new WaitForSeconds(Time.deltaTime);
        // }

        while (async.progress < 0.9f)
        {
            yield return null;
        }
        SimplePool.Release();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        // yield return Yielders.Get(0.1f);
        async.allowSceneActivation = true;

        yield return Yielders.Get(0.1f);

        // InGameObjectsManager.Instance.DespawnAllPools();
        // ObjectsManager.Instance.LoadMap();

        FindMainCanvas();

        yield return Yielders.Get(0.1f);

    }

    // public void FindPanelLoadingAds()
    // {
    //     m_PanelLoadingAds = FindObjectOfType<PanelLoadingAds>().GetComponent<PanelLoadingAds>();
    // }

    // public PanelLoading GetPanelLoading()
    // {
    //     return m_PanelLoading;
    // }

    public GameObject GetGOPanelLoading()
    {
        return g_PanelLoading;
    }

    // public PanelLoadingAds GetPanelLoadingAds()
    // {
    //     if (m_PanelLoadingAds == null)
    //     {
    //         FindPanelLoadingAds();
    //     }

    //     return m_PanelLoadingAds;
    // }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (!isLoadInitScene)
        {
            isLoadInitScene = true;
            // GameManager.Instance.ChangeToPlayScene(false);
        }
    }

    public void AddClickEvent(Button _bt, UnityAction _callback)
    {
        _bt.onClick.AddListener(() =>
        {
            // SoundManager.Instance.PlayButtonClick();
            if (_callback != null)
            {
                _callback();
            }
        });
    }

    public void RegisterUI(UICanvas uicanvas)
    {
        int id = (int)uicanvas.ID();

        UICanvas rUI = null;
        if (m_GUIMap.TryGetValue(id, out rUI))
        {
            m_GUIMap[id] = uicanvas;
            //Debug.Log(m_GUIMap.m_ID + " Register overlap " + uicanvas.ID());
        }
        else
        {
            m_GUIMap.Add((int)uicanvas.ID(), uicanvas);
            //Debug.Log(m_GUIMap.m_ID + " Register add " + uicanvas.ID());
        }
    }

    public UICanvas GetUICanvasByID(UIID uid)
    {
        return GetUICanvasByID((int)uid);
    }

    public void FindMainCanvas()
    {
        m_MainCanvas = GameObject.Find("MainCanvas");
    }

    // public void FindPanelLoading()
    // {
    //     m_PanelLoading = GameObject.FindObjectOfType<PanelLoading>().GetComponent<PanelLoading>();
    // }

    // public PanelInGame FindPanelInGame()
    // {
    //     if (m_PanelInGame == null)
    //     {
    //         m_PanelInGame = FindObjectOfType<PanelInGame>().GetComponent<PanelInGame>();
    //     }
    //     return m_PanelInGame;
    // }

    public UICanvas GetUICanvasByID(int id)
    {
        if (m_MainCanvas == null)
        {
            FindMainCanvas();
        }
        UICanvas rUI = null;
        m_GUIMap.TryGetValue(id, out rUI);
        if (rUI == null)
        {
            string name = GetUIName((UIID)id);
            GameObject go = FindObject(m_MainCanvas, name);
            if (go != null)
            {
                go.gameObject.SetActive(true);
                return go.GetComponent<UICanvas>();
            }
            else
            {
                GameObject prefab = GetPrefab((UIID)id);
                if (prefab == null) return null;
                GameObject goUICanvas = GameObject.Instantiate(prefab, m_MainCanvas.transform.position, Quaternion.identity, m_MainCanvas.transform);
                RectTransform rect = goUICanvas.GetComponent<RectTransform>();
                rect.SetParent(m_MainCanvas.GetComponent<RectTransform>());
                rect.localPosition = new Vector3(0, 0, 0);
                rect.localScale = new Vector3(1, 1, 1);
                return goUICanvas.GetComponent<UICanvas>();
            }
        }
        else
        {
            return rUI;
        }
    }

    public void HideUIPopup(UICanvas popup, bool isClosed, bool isOpenPrevious = true)
    {
        if (popup == null) return;
        popup.HidePopup();

        if (isClosed || isOpenPrevious)
        {
            if (m_CurrentOpenedPopup.Contains(popup))
            {
                m_CurrentOpenedPopup.Remove(popup);
                CleanOpenPopup();
            }
        }
        if (isOpenPrevious)
        {
            if (m_CurrentOpenedPopup.Count > 0)
            {
                UICanvas previusPopup = m_CurrentOpenedPopup[m_CurrentOpenedPopup.Count - 1];
                ShowUIPopup(previusPopup, false);
            }
        }
    }

    public void ShowUIPopup(UICanvas popup, bool isClosePrevious = true, bool isSetup = true)
    {
        ShowUIPopup(popup, GetCenterPosition(), isClosePrevious, isSetup);
    }

    // IEnumerator IEShowUIPopup(UICanvas popup, bool isClosePrevious = true)
    // {
    //     // Yielders.Get();
    // }

    public void ShowUIPopup(UICanvas popup, Vector3 position, bool isClosePreviousPopup = true, bool isSetup = true)
    {
        if (popup == null) return;
        if (m_CurrentOpenedPopup.Count > 0)
        {
            m_PreviousPopup = m_CurrentOpenedPopup[m_CurrentOpenedPopup.Count - 1];
            if (isClosePreviousPopup)
            {
                HideUIPopup(m_PreviousPopup, m_PreviousPopup.IsAutoRemove, false);
            }
        }
        popup.ShowPopup(isSetup);
        popup.SetLocalPosition(position);
        if (!m_CurrentOpenedPopup.Contains(popup))
        {
            m_CurrentOpenedPopup.Add(popup);
            CleanOpenPopup();
        }
    }
    public Vector3 GetCenterPosition()
    {
        return m_CenterPos;
    }

    public void ShowUIPanel(UIID id)
    {
        //Debug.Log(id.ToString());
        UICanvas panel = GetUICanvasByID((int)id);
        ShowUIPanel(panel, m_MainCanvas.transform.position);
    }
    public void ShowUIPanel(UIID id, Vector3 position)
    {
        UICanvas panel = GetUICanvasByID((int)id);
        ShowUIPanel(panel, position);
    }
    public void ShowUIPanel(UICanvas panel)
    {
        ShowUIPanel(panel, m_MainCanvas.transform.position);
    }
    public void ShowUIPanel(UICanvas panel, Vector3 position, bool isLocalPositon = false)
    {
        if (panel == null)
        {
            //Debug.Log(" NULL");
            return;
        }
        if (m_CurrentOpenedPanel.Count > 0)
        {
            m_PreviousPanel = m_CurrentOpenedPanel[m_CurrentOpenedPanel.Count - 1];
        }
        panel.ShowPanel();

        Vector3 v = Vector3.zero + new Vector3(0, m_OffsetTop);
        v.z = 0;
        panel.RectTransform.localPosition = v;
        if (!m_CurrentOpenedPanel.Contains(panel))
        {
            m_CurrentOpenedPanel.Add(panel);
        }
        //Debug.Log(" Complete Show");
    }

    public void CleanOpenPopup()
    {
        int num = 0;
        while (num < m_CurrentOpenedPopup.Count)
        {
            if (m_CurrentOpenedPopup[num] == null)
            {
                m_CurrentOpenedPopup.RemoveAt(num);
                continue;
            }
            num++;
        }
    }

    public GameObject GetPrefab(UIID uid)
    {
        string prefabName = "";
        GameObject prefab = null;
        switch (uid)
        {
            case UIID.POPUP_OUTFIT:
                prefabName = "PopupOutfit";
                break;
            case UIID.POPUP_WIN:
                prefabName = "PopupWin";
                break;
            case UIID.POPUP_BONUS_REWARD:
                prefabName = "PopupBonusReward";
                break;
            case UIID.POPUP_OUTFIT_REWARD:
                prefabName = "PopupOutfitReward";
                break;
            case UIID.POPUP_RATE:
                prefabName = "PopupRate";
                break;
        }
        prefab = GetPopupPrefabByName(prefabName);
        return prefab;
    }

    public UICanvas GetCurrentPopup()
    {
        int num = m_CurrentOpenedPopup.Count;
        if (num > 0)
            return m_CurrentOpenedPopup[num - 1];
        return null;
    }

    public GameObject GetPopupPrefabByName(string name)
    {
        GameObject go = Resources.Load<GameObject>("UI/Popups/" + name);
        return go;
    }

    public string GetUIName(UIID uiid)
    {
        string name = "";
        switch (uiid)
        {

        }
        return name;
    }

    public GameObject FindObject(GameObject parent, string name)
    {
        if (parent == null) return null;
        foreach (Transform t in parent.transform)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}

public enum UIID
{
    PANEL_INGAME = 0,
    POPUP_OUTFIT = 1,
    POPUP_WIN = 2,
    POPUP_BONUS_REWARD = 3,
    POPUP_OUTFIT_REWARD = 4,
    POPUP_RATE = 5,
}