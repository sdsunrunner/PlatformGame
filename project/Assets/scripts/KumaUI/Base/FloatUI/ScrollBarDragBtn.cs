
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
 
public class ScrollBarDragBtn : MonoBehaviour
{	   
	public void InitScrollBarBtn(UI_Event BtnEV,Transform ParentScrollRectTrans)
	{
		BtnEV.onBeginDrag =  OnScrollBeginDrag;
		BtnEV.onEndDrag =  OnScrollEndDrag;
		BtnEV.onDrag =  OnScrollDrag;  
		BtnEV.SetData("T",ParentScrollRectTrans);  
	}

	void OnScrollDrag( PointerEventData eventData , UI_Event ev )  
	{ 
		Transform Obj = ev.GetData<Transform>("T");  
		Obj.GetComponentInChildren<ScrollRect>().OnDrag(eventData);
		ev.DisableClick = true;
	}

	void OnScrollBeginDrag( PointerEventData eventData , UI_Event ev )  
	{ 
		Transform Obj = ev.GetData<Transform>("T");   
		Obj.GetComponentInChildren<ScrollRect>().OnBeginDrag(eventData);
		ev.DisableClick = true;
	}

	void OnScrollEndDrag( PointerEventData eventData , UI_Event ev )  
	{
		Transform Obj = ev.GetData<Transform>("T");    
		Obj.GetComponentInChildren<ScrollRect>().OnEndDrag(eventData); 
		StartCoroutine(DelayResetBtn(0.8f,ev));
	}
 
	IEnumerator DelayResetBtn(float DelayTime, UI_Event ev)
	{
		yield return new WaitForSeconds(DelayTime); 
		ev.DisableClick = false;
	}

}
