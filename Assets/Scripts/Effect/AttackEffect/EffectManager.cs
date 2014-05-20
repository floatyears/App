﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class EffectManager {
	private static EffectManager instance;
	public static EffectManager Instance {
		get{
			if(instance == null) {
				instance = new EffectManager();
			}
			return instance;
		}
	}

	public GameObject effectPanel;

	public const int DragCardEffect = -1000;

	private Dictionary<int,string> effectName = new Dictionary<int, string>();
	private Dictionary<int, GameObject> effectObject = new Dictionary<int, GameObject>();
	public GameObject GetEffectObject(int skillID) {
		GameObject go = null;
		effectObject.TryGetValue (skillID, out go);
		return go;
	}

	void SetName() {
		effectName.Add (400, "BOOM");
		effectName.Add (401, "daoguang");
		effectName.Add (402, "fire");
		effectName.Add (403, "fire1");
		effectName.Add (404, "fire2");
		effectName.Add (405, "firerain");
		effectName.Add (406, "ice1");
		effectName.Add (407, "ice2");
		effectName.Add (408, "jiufeng");
		effectName.Add (409, "water");
		effectName.Add (410, "wind1");
		effectName.Add (411, "wind2");

		//config effect no skill.
		effectName.Add (DragCardEffect, "card_effect");
	}

	private Dictionary<string,Type> effectCommand = new Dictionary<string, Type> ();
	private EffectManager() {
		SetName ();
		foreach (var item in effectName) {
			GameObject go = Resources.Load("Effect/"+item.Value) as GameObject;
			effectObject.Add(item.Key,go);
		}
	}

	public static GameObject InstantiateEffect(GameObject parent, GameObject obj) {
		Vector3 localScale = obj.transform.localScale;
		Vector3 rotation = obj.transform.eulerAngles;
		GameObject effectIns =  NGUITools.AddChild(parent, obj);
		effectIns.transform.localScale = localScale;
		effectIns.transform.eulerAngles = rotation;
		return effectIns;
	}

}
