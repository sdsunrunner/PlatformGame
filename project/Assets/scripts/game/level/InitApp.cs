using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class InitApp : MonoBehaviour 
{
    public enum EState
    {
        eNone = 0,
        eRegisNotification,
        eInitGameSetting,
        eInitTableConfig,
        eStart,
        eGo,
        eEnd,
    }

    private EState mCurState = EState.eNone;
    private EState mPreState = EState.eNone;

    private float mStateTimer;
    private const float WAIT_TIME_AND_START = 1.5f;
    private long mStartTime;

    private ScreenInitLoading mLoadingHandler;

    private AppFacade mCommandFacade = null;    
	void Start () 
    {
        Debug.LogError("start init app");
        state = EState.eRegisNotification;

        GameObject ui_parent = GameObject.Find("UI");
        MenuMrg.instance.SetTransform(ui_parent.transform);
        mLoadingHandler = MenuMrg.instance.CreateMenu<ScreenInitLoading>();       
        mLoadingHandler.ShowScreen();       
	}
    void Update()
    {
        switch (mCurState)
        {
            case EState.eStart:
                mStateTimer += Time.deltaTime;
                if (mStateTimer > WAIT_TIME_AND_START)
                    state = EState.eGo;
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
            _StateExit(_state);
            mPreState = mCurState;
            mCurState = _state;
            _StateEnter(mPreState);
        }
    }
    private void _StateEnter(EState _prevState)
    {
        switch (mCurState)
        {
            case EState.eRegisNotification:
                RegisNotification();
                break;
            case EState.eInitGameSetting:
                InitGameSetting();
                break;
            case EState.eInitTableConfig:
                InitTableConfig();
                break;
            case EState.eGo:
                StartGame();
                break;
        }
    }
    private void _StateExit(EState _nextState)
    {
        //do nothing
    }
    
    void RegisNotification()
    {
        mCommandFacade = AppFacade.instance;
        mCommandFacade.addCommand(CommandInteracType.STARTAPP_COMMAND, typeof(StartAppMacroCommand));
        //mCommandFacade.addCommand(CommandInteracType.STARTAPP_ASYNC_COMMAND, StartAppAnsyCommand);
        this.notify(CommandInteracType.STARTAPP_COMMAND);
        state = EState.eInitGameSetting;       
    }
    void InitGameSetting()
    {
        Debug.LogError("InitGameSetting");
        Application.targetFrameRate = 30;
        Time.timeScale = 1.0f;
        mStartTime = GameTime.GetMilSecTime();
        state = EState.eInitTableConfig;
    }

    void InitTableConfig()
    {
        Debug.LogError("InitTableConfig");
        ClientTableDataManager.Reset();
        ClientTableDataManager.Instance.Init();
        state = EState.eStart;
    }

    void StartGame()
    {
        Debug.LogError("StartGame");
        state = EState.eEnd;
        LevelManager.Instance.LoadLevelAsync(Level.Opening);
        SceneManager.LoadScene(LevelManager.Instance.GetLevelName(LevelManager.Instance.GetCurLevel()));
    }

    private void notify(string type, object data = null)
	{
		INotification notificaion = new BaseNotification();
		notificaion.data = data;
			
		AppNotification note = new AppNotification(type,notificaion);
		note.dispatch();
	}
}
