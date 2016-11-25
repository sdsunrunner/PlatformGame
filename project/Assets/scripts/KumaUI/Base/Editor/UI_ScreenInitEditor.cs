using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ScreenHandlerUI5))]
public class UI_ScreenInitEditor : Editor
{
	public override void OnInspectorGUI()
	{
		ScreenHandlerUI5 screen = target as ScreenHandlerUI5;

		if(GUILayout.Button("Initview"))
		{
			screen.InitView();
		}
	}
}



