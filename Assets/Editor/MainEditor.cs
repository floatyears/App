using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Main))]
public class MainEditor : Editor {

	public override void OnInspectorGUI(){
		Main main = target as Main;
		main.DebugEnable [0] = EditorGUILayout.Toggle ("Debug0",main.DebugEnable [0]);
		if (GUI.changed)
			EditorUtility.SetDirty (target);
	}
}
