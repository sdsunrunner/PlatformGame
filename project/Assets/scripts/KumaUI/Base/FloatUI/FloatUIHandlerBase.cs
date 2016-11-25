using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatUIHandlerBase : UI_Follow3D
{
	public override void Init()
	{
		base.Init();
		GameObject obj = GameObject.Find("SceneCam");
		if (obj)
        	cam_3d = obj.GetComponent<Camera>();
	}
	
	public void ShowScreen()
	{
		gameObject.SetActive(true);
	}
	
	public void HideScreen()
	{
		gameObject.SetActive(false);
	}
	
	public void CloseScreen()
	{
		GameObject.Destroy(this.gameObject);
	}
}

