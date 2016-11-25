using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;



// single toggle
[AddComponentMenu("UI/Single Toggle")]
public class SingleToggle : UI_Event
{
	public Image Target;
	public Sprite OnSprite;
	public Sprite OffSprite;

	[SerializeField]
	private bool mIsOn;
	public bool IsOn
	{
		get
		{
			return mIsOn;
		}
		set
		{
			mIsOn = value;
			if(Target != null)
			{
				if(mIsOn)
				{
					Target.sprite = OnSprite;
				}
				else
				{
					Target.sprite = OffSprite;
				}
			}
		}
	}

	// Use this for initialization
	void Start ()
	{
		if(Target != null)
		{
			if(mIsOn)
			{
				Target.sprite = OnSprite;
			}
			else
			{
				Target.sprite = OffSprite;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Toggle()
	{
		IsOn = !IsOn;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		if(mAnyMove) return;
		if(sDisableEvent2 && !mIgnoreDisable) return;
        if(sDisableEvent && !mIgnoreDisable) return;
        sIsEvent = true;
		if(IsOn)
		{
			IsOn = false;
		}
		else
		{
			IsOn = true;
		}
	}
}
