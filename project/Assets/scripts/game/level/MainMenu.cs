using UnityEngine;
using System.Collections;
using GAF.Core;
using GAFInternal.Core;

public enum EState
{
    eNone = 0,
    eOpening,
    eStay,    
    eShowMenu,
    eEnd,
}

public class MainMenu : MonoBehaviour
{
    public GAFMovieClip MC_MenuBG;
	// Use this for initialization

    private EState mCurState = EState.eNone;
    private EState mPreState = EState.eNone;
    private float mStateTimer;

    private const float WAIT_FOR_SHOW_MENU = 1.5f;
	void Start ()
    {
        state = EState.eOpening;
	}
	
	// Update is called once per frame
	void Update () 
    {
        switch (mCurState)
        {
            case EState.eOpening:               
                if(!MC_MenuBG.isPlaying())
                     state = EState.eStay;
                break;
            case EState.eStay:                
                mStateTimer += Time.deltaTime;
                if(mStateTimer >= WAIT_FOR_SHOW_MENU)
                    state = EState.eShowMenu;               
                break;
        }
	}
   

    public EState state
    {
        get { return mCurState; }
        set { _OnStateChange(value); }
    }
    private void _OnStateChange(EState _state)
    {
        mStateTimer = 0.0f;
        if (_state != mCurState)
        {           
            mPreState = mCurState;
            mCurState = _state;
            _StateEnter(mPreState);
        }
    }

    private void _StateEnter(EState _prevState)
    {
        switch (mCurState)
        {
            case EState.eOpening:
                ShowOpening();
                break;
            case EState.eStay:
                ShowStay();
                break;
            case EState.eShowMenu:
                ShowMenu();
                break;      
        }
    }

    void ShowOpening()
    {
        MC_MenuBG.setSequence("opening", true);
        MC_MenuBG.setAnimationWrapMode(GAFWrapMode.Once);
    }

    void ShowStay()
    {
        MC_MenuBG.setSequence("stay", true);
        MC_MenuBG.setAnimationWrapMode(GAFWrapMode.Loop);
    }
    void ShowMenu()
    {
        Debug.LogError("-----------showmenu-----");
        GameObject ui_parent = GameObject.Find("UI");
        MenuMrg.instance.SetTransform(ui_parent.transform);
        ScreenMainMenu mainMenu = MenuMrg.instance.CreateMenu<ScreenMainMenu>();
        mainMenu.ShowScreen();
    }

}
