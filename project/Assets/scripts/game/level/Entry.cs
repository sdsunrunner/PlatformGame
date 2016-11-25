using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entry : MonoBehaviour
{
    byte[] mStartMem;

	void Start()
	{
        Init();
	}

    void Init()
    {
        mStartMem = new byte[1024*1024*20*8];
        mStartMem = null;
#if RUNTIME_DEBUG
        Reporter.Init();
#endif
        GameObject ui_parent = GameObject.Find("UI");
        MenuMrg.instance.SetTransform(ui_parent.transform);

        ScreenEntry screen = MenuMrg.instance.CreateMenu<ScreenEntry>();
        if(screen != null)
        {
            screen.ShowScreen();
        }
    }

    void Update()
    {
        DebugAssetbundle();
    }

    #region DebugAssetbundle
    //////////////////////////////////////////////////////////////////

    bool resolutionBypassTriggered = false;
    bool assetBundleVerifierTriggered = false;
    string tapInput = "";

    void DebugAssetbundle()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            GetTouchInput();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            GetMouseInput();

        }
        checkTapInput();
    }

    void GetTouchInput()
    {
        GetTapInput(Input.GetTouch(0).position);
    }

    void GetMouseInput()
    {
        GetTapInput(Input.mousePosition);
    }

    void GetTapInput(Vector2 screenPos)
    {
        bool left = screenPos.x < Screen.width / 2.0f;
        bool top = screenPos.y > Screen.height / 2.0f;

        // topleft = 1, topright = 2, botleft= 3, botright = 4
        if (top)
        {
            if (left)
                tapInput += "1";
            else 
                tapInput += "2";
        }
        else
        {
            if (left)
                tapInput += "3";
            else
                tapInput += "4";
        }

        Debug.LogError("tapInput = " + tapInput);
    }

    void checkTapInput()
    {
        // if (assetBundleVerifierTriggered == false && tapInput.Contains("114343"))
        if (resolutionBypassTriggered == false && tapInput.Contains("114343"))
        {
            Debug.LogError("assetBundleVerifierTriggered");
            //BuildVerifier.Enabled = true;
            assetBundleVerifierTriggered = true;
            // debugLog += "Initializing Asset Bundle Verifier...\n";
        }
    }

    void OnGUI()
    {
        //if (BuildVerifier.Enabled)
        //{
        //    BuildVerifier.DrawLog();
        //}
    }
    //////////////////////////////////////////////////////////////////
    #endregion
}