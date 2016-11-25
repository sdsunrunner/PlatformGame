using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SingleToggle))]
public class UI_SingleToggleEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		SingleToggle toggle = target as SingleToggle;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Target"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("OnSprite"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("OffSprite"));
		toggle.IsOn = EditorGUILayout.Toggle("IsOn",toggle.IsOn);
		serializedObject.ApplyModifiedProperties();
	}
}



