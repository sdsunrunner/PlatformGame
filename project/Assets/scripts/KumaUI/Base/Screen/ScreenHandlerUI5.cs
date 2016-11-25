using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ScreenHandlerUI5 : MonoBehaviour
{
    public bool m_DisableSceneCamOnShow = false;
    public bool m_DestroyOnClose = true;
    public bool m_disableCamera = false;

    protected Camera m_SceneCam;
    protected bool m_Initialized = false; // Prevent unintentional, double initialization.
    public bool mIsShow = false;


    public delegate void OnScreenHandlerEventHandler(ScreenHandlerUI5 screenHandler);
    public event OnScreenHandlerEventHandler OnCloseAndDestroy;
    public event OnScreenHandlerEventHandler OnShowScreen;
    public event OnScreenHandlerEventHandler OnHideScreen;

    // Use this for initialization
    void Awake()
    {
        if (m_DisableSceneCamOnShow)
        {
            GameObject go = GameObject.Find("SceneCam");
            if(go != null)
            m_SceneCam = go.GetComponent<Camera>();
        }

        if (m_Initialized == false)
        {
            InitView();
            AddListener();
		    Init();
        }
    }

    public virtual void InitView()
    {
        //
    }

    public virtual void AddListener()
    {
        //
    }

	public virtual void Init()
	{	
        m_Initialized = true;
	}

    public virtual void HideScreen()
    {
        mIsShow = false;
        CheckEnableSceneCam();

        if (OnCloseAndDestroy != null)
            OnCloseAndDestroy(this);

        if (OnHideScreen != null)
            OnHideScreen(this);

        //if (m_disableCamera)
        //    GlobalObject.gMainController.enableCamera = true;
    }

    public virtual void ShowScreen()
    {
        mIsShow = true;
        if (OnShowScreen != null)
            OnShowScreen(this);

        CheckDisableSceneCam();

        //if (m_disableCamera)
        //    GlobalObject.gMainController.enableCamera = false;
    }

    public GameObject screen { get { return this.gameObject; } }

    void CheckEnableSceneCam()
    {
        if (m_DisableSceneCamOnShow)
        {
            if (m_SceneCam != null)
                m_SceneCam.gameObject.SetActive(true);
        }
    }

    void CheckDisableSceneCam()
    {
        if (m_DisableSceneCamOnShow)
        {
            if (m_SceneCam != null)
                m_SceneCam.gameObject.SetActive(false);
        }
    }

}
