
using System;
using UnityEngine;
using System.Collections;

public class AnimationSendMessage : MonoBehaviour
{
	public GameObject mTarget;
	public void SendMessageEx(string method)
	{
		if(mTarget!=null)
			mTarget.SendMessage(method);
	}
}

