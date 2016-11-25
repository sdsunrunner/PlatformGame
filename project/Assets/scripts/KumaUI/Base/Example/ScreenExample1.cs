using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ScreenExample1 : ScreenHandlerUI5
{
	Transform node1 = null;
	Transform btn1 = null;
	Image img1 = null;

	public override void InitView()
	{
		img1 = GUIUtil.Find<Image>(this,"Canvas/img1");
		btn1 = GUIUtil.Find(this,"Canvas/btn1");
		node1 = GUIUtil.Find(this,"Canvas/node1");
		Debug.Log("init 1");
	}

	public override void AddListener()
	{
		UI_Event ev = UI_Event.Get(btn1);
		ev.onClick = OnBtnClick;
		Debug.Log("init 2");
	}

	public override void Init()
	{
		base.Init();
		Debug.Log("init 3");
	}

	void OnBtnClick(PointerEventData eventData , UI_Event ev)
	{
		node1.gameObject.SetActive(!node1.gameObject.activeSelf);
	}
}
