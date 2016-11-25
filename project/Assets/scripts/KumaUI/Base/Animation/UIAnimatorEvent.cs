using UnityEngine;
using System.Collections;

public class UIAnimatorEvent : MonoBehaviour
{
	public System.Action mOnEnter = null;
	public System.Action mOnExit = null;

	public virtual void OnAnimationEnter()
	{
		if(this.mOnEnter != null)
		{
			this.mOnEnter();
		}
	}

	public virtual void OnAnimationExit()
	{
		if(this.mOnExit != null)
		{
			this.mOnExit();
		}
	}
}
