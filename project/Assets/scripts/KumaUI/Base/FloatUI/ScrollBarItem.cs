
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ScrollBarItem : MonoBehaviour
{	
	public GameObject ScrollItem;

	float MinX;
	float MinY;
	Vector3 OriginPos;
	void Start()
	{ 
		MinX = 	GetComponent<RectTransform>().sizeDelta.x;
		MinY = 	GetComponent<RectTransform>().sizeDelta.y;
		OriginPos = transform.localPosition;
	} 

	public float ScaleRate;  
	public int XNum;  
	public int YNum;

	public float GetMinY(){return MinY;}
	public float GetMinX(){return MinX;}

	public void AjustHeight(int Directon,bool AddDelay) // 1-->> Horizontal  2-->>> vertical
	{ 
		if(Directon == 2)
		{
			RectTransform RT = GetComponent<RectTransform>();
			if(!AddDelay)RT.localPosition = OriginPos;
			ScrollListItem[] ScrollListY = gameObject.GetComponentsInChildren<ScrollListItem>();
			float NewScaleY = (Mathf.CeilToInt((float)ScrollListY.Length/(float)YNum)+1) * (GetComponent<GridLayoutGroup>().cellSize.y + GetComponent<GridLayoutGroup>().spacing.y);

			//Debug.LogError("ScrollListY.Length/YNu-->" + (float)ScrollListY.Length/(float)YNum + ">>-->>" + Mathf.CeilToInt((float)ScrollListY.Length/(float)YNum));
			if(NewScaleY < MinY)NewScaleY = MinY;
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,NewScaleY); 
		}
		else if(Directon == 1)
		{
			RectTransform RT = GetComponent<RectTransform>();
			if(!AddDelay)RT.localPosition = OriginPos;
			ScrollListItem[] ScrollListX = gameObject.GetComponentsInChildren<ScrollListItem>();
			float NewScaleX = (Mathf.CeilToInt((float)ScrollListX.Length/(float)XNum)+1) * (GetComponent<GridLayoutGroup>().cellSize.x + GetComponent<GridLayoutGroup>().spacing.x);

			//Debug.LogError("ScrollListX.Length/YNu-->" + (float)ScrollListX.Length/(float)XNum + ">>-->>" + Mathf.CeilToInt((float)ScrollListX.Length/(float)YNum));
			if(NewScaleX < MinX)NewScaleX = MinX;
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,NewScaleX); 
		}
	}

	public void AjustRectSpecial(int Directon) // 1-->> Horizontal  2-->>> vertical
	{ 
		if(Directon == 1)
		{
			RectTransform RT = GetComponent<RectTransform>();
			RT.localPosition = OriginPos;
			LayoutElement[] LayoutElementX = gameObject.GetComponentsInChildren<LayoutElement>();
			float NewScaleX = 0.0f;
			foreach(LayoutElement LE in LayoutElementX){NewScaleX += LE.minWidth;} 
			NewScaleX += LayoutElementX[0].minHeight;
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,NewScaleX); 
		}
		else if(Directon == 2)
		{
			RectTransform RT = GetComponent<RectTransform>();
			RT.localPosition = OriginPos;
			LayoutElement[] LayoutElementX = gameObject.GetComponentsInChildren<LayoutElement>();
			float NewScaleY= 0.0f;
			foreach(LayoutElement LE in LayoutElementX){NewScaleY += LE.minHeight;} 
			NewScaleY += LayoutElementX[0].minHeight;
			//Debug.LogError("GetComponent<RectTransform>().sizeDelta.y-->>" + GetComponent<RectTransform>().sizeDelta.y);
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,NewScaleY); 
		} 
	}
}
