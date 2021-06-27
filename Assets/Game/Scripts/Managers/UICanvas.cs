using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UICanvas : MonoBehaviour
{
    public UIID m_ID;
    public UIID ID()
    {
        return m_ID;
    }

    protected RectTransform m_RectTransform;
    public RectTransform RectTransform
    {
        get { return m_RectTransform; }
        set { m_RectTransform = value; }
    }

    protected CanvasGroup m_CanvasGroup;
    protected bool IsClosing = false;
    public bool IsOpenPrevious = true;
    public bool IsAutoRemove = false;
    public bool IsAvoidBackKey = false;

    public Button btn_Close;

    private void Start()
    {
        if (btn_Close != null)
        {
            GUIManager.Instance.AddClickEvent(btn_Close, OnClose);
        }
    }

    protected void Init(bool isActive = false)
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        if (m_CanvasGroup == null)
        {
            m_CanvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        if (GUIManager.Instance == null)
        {
            StartCoroutine(OnWaitingRegister());
        }
        else
        {
            GUIManager.Instance.RegisterUI(this);
            gameObject.SetActive(isActive);
        }
    }

    public virtual void OnEnable()
    {
        AddListener();
    }

    public virtual void OnDisable()
    {
        RemoveListener();
    }

    public virtual void OnDestroy()
    {
        RemoveListener();
    }

    public virtual void AddListener()
    {

    }

    public virtual void RemoveListener()
    {

    }

    public virtual void Setup()
    {

    }

    public virtual void OnStartOpen()
    {
        IsClosing = false;
        transform.localEulerAngles = new Vector3(0, 0, 0);
        FadeIn();
    }

    public virtual void OnClose()
    {
        if (IsClosing) return;
        GUIManager.Instance.HideUIPopup(this, IsAutoRemove, IsOpenPrevious);
        IsClosing = true;
    }

    public virtual void OnBack()
    {
        OnClose();
    }

    IEnumerator OnWaitingRegister()
    {
        yield return Yielders.EndOfFrame;
        GUIManager.Instance.RegisterUI(this);
        gameObject.SetActive(false);
    }


    public void ShowPopup(bool isSetup = true)
    {
        // gameObject.SetActive(false);
        gameObject.SetActive(true);
        if (isSetup)
        {
            Setup();
        }
        OnStartOpen();
        //if (m_CanvasGroup != null)
        //    m_CanvasGroup.alpha = 0;
    }

    // public virtual void OnClose()
    // {
    //     if (IsClosing) return;
    //     GUIManager.Instance.HideUIPopup(this, IsAutoRemove, IsOpenPrevious);
    //     IsClosing = true;
    // }

    public void HidePopup()
    {
        FadeOut();
    }

    public void ShowPanel()
    {
        //Debug.Log("Start Show ID " + ID());
        gameObject.SetActive(true);
    }

    public void SetPosition(Vector3 position)
    {
        if (m_RectTransform != null)
            m_RectTransform.position = position;
    }
    public void SetLocalPosition(Vector3 position)
    {
        if (m_RectTransform != null)
            m_RectTransform.localPosition = position;
    }

    public virtual void FadeOut()
    {
        m_CanvasGroup.DOFade(0, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true); ;
        transform.DOScale(1.05f, 0.2f).SetEase(Ease.Flash).OnComplete(() => { gameObject.SetActive(false); }).SetUpdate(UpdateType.Late, true);
    }
    public void FadeIn()
    {
        if (m_CanvasGroup != null)
        {
            m_CanvasGroup.alpha = 0;
            transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            m_CanvasGroup.DOFade(1, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true);
            transform.DOScale(1, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true); ;
        }
    }

    public void FadeIn(float _timeDuration)
    {
        if (m_CanvasGroup != null)
        {
            m_CanvasGroup.alpha = 0;
            transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            m_CanvasGroup.DOFade(1, _timeDuration).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true);
            transform.DOScale(1, _timeDuration).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true); ;
        }
    }
}
