using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(UI_LocalizeImage))]
public class UI_LocalizeImageEditor : Editor
{
	public Sprite[] mSprites;

	void OnEnable ()
	{
		serializedObject.Update();
		UI_LocalizeImage localImage = target as UI_LocalizeImage;
        //this.mSprites = localImage.mLocalizes;
	}

	public override void OnInspectorGUI()
	{
        //serializedObject.Update();
        //UI_LocalizeImage localImage = target as UI_LocalizeImage;
        //for(int i = 0 ; i<(int)TextManager.LANGUAGE.NA ; i++)
        //{
        //    EditorGUILayout.LabelField((TextManager.LANGUAGE.EN+i).ToString());
        //    mSprites[(int)TextManager.LANGUAGE.EN+i] = (Sprite) EditorGUILayout.ObjectField(mSprites[(int)TextManager.LANGUAGE.EN+i],typeof(Sprite),true);
        //}
        //serializedObject.ApplyModifiedProperties();
	}
}



