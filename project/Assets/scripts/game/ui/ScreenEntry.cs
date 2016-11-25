using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//screen entry
public class ScreenEntry: ScreenHandlerUI5
{	
	public UIAnimator mAni;

	public override void Init()
	{
		base.Init();

		if(mAni != null)
		{
			mAni.Play("start",AniCallBack);
		}
    }

    void AniCallBack()
    {
    	HideScreen();
        //ResolutionManager.instance.Init();
    	LevelManager.Instance.LoadLevelAsync(Level.AppInit);       
    	Application.LoadLevel(LevelManager.Instance.GetLevelName(LevelManager.Instance.GetCurLevel()));
    }

}
