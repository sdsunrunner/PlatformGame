using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ScreenMainMenu : ScreenHandlerUI5
{
    public GameObject btn_play;
	// Use this for initialization
    public override void Init()
    {
        base.Init();
    }
    public override void AddListener()
    {
        UI_Event ev = UI_Event.Get(btn_play);
        ev.onClick = OnClickBtnPlay;
    }
    void OnClickBtnPlay(PointerEventData eventData, UI_Event ev)
    {
        Debug.LogError("---OnClickBtnPlay-");

    }
}

