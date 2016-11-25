using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//menu manager
public class MenuMrg : MenuManagerKumaBase
{
    static MenuMrg sInstance = null;
    
    public static MenuMrg instance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = new MenuMrg();
            }
            return sInstance;
        }
    }

    protected Dictionary<string, GameObject> _resScreens = new Dictionary<string,GameObject>();

    public MenuMrg()
        :base()
    {
        //
    }

    public void Init()
    {
        //PreLoad<Screen_CardManager>();
        //PreLoad<ScreenHub>();
        //PreLoad<ScreenMission>();
        //PreLoad<WorldSphereHandler>();
        //PreLoad<ScreenMapPveHandler>();
        //// PreLoad<Screen_Card_EditCity>();
        //// PreLoad<ScreenHandler_BattleUI>();
        //PreLoad<ScreenWaitingCover>();
        //PreLoad<ScreenWaitingBoard>();
        //PreLoad<Screen2DDisableBoard>();
    }

    protected void PreLoad<T>()
        where T : ScreenHandlerUI5
    {
        string key = typeof(T).ToString();
        if(_resScreens.ContainsKey(key))
        {
            return;
        }
        GameObject obj = GetPrefabFromType(typeof(T));
        _resScreens.Add(key,obj);
    }

    protected override GameObject GetPrefabFromType(System.Type _type)
    {
        string key = _type.ToString();
        if(_resScreens.ContainsKey(key))
        {
            return _resScreens[key];
        }
        if (_type.Equals(typeof(ScreenEntry)))
            return Resources.Load("UIEntry") as GameObject;    

        if (_type.Equals(typeof(ScreenInitLoading)))
            return Resources.Load("UIloading") as GameObject;       

        return null;

    }
}