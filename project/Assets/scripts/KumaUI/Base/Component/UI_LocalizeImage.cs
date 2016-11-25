
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//  UI_FontNum.cs
//  Author: Lu Zexi
//  2015-03-31


//font num
[AddComponentMenu("UI/UI Localize Image")]
[RequireComponent (typeof(Image))]
public class UI_LocalizeImage : MonoBehaviour
{
	private Image mImage;
    //public Sprite[] mLocalizes = new Sprite[(int)TextManager.LANGUAGE.NA];

    void Awake()
    {
        this.mImage = this.GetComponent<Image>();
		SetLocalize();
    }

	public void SetLocalize()
	{
        //mImage.sprite = mLocalizes[(int)TextManager.instance.currentLanguage];
	}
}

