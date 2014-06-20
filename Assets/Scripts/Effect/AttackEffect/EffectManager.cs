using UnityEngine;
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
//		Debug.LogError ("skillID : " + skillID + " go : " + go); 
		return go;
	}

	void SetName() {
//		effectName.Add (4015, "CFX3_Hit_Fire_A_Air");				//fire single
//		effectName.Add (4026, "firerain"); 							//fire all
//		effectName.Add (4061, "firerain"); 							//fire all
//		effectName.Add (4018, "linhunqiu2"); 						//light single
//		effectName.Add (4004, "CFXM3_Hit_Ice_B_Air");				//water single
//		effectName.Add (4017, "zhua");								//wind singele
//		effectName.Add (4028, "zhua");								//wind single
//		effectName.Add (4062, "liandao");							//wind single

		effectName.Add (4021, "effect/ns-all-1-fire");		
		effectName.Add (4038, "effect/ns-all-2-fire");	

		effectName.Add (4003, "effect/ns-single-2-fire");	
		effectName.Add (4005, "effect/ns-single-2-water");	

		effectName.Add (1024, "effect/as-all-1-fire");
		effectName.Add (1067, "effect/as-single-1-fire02");
		effectName.Add (1055, "effect/as-single-1-fire02");	
		effectName.Add (1097, "effect/as-reduce-def03");	

		effectName.Add (DragCardEffect, "card_effect");
	}

	private Dictionary<string,Type> effectCommand = new Dictionary<string, Type> ();
	private EffectManager() {
		SetName ();
		foreach (var item in effectName) {
			ResourceManager.Instance.LoadLocalAsset("Effect/"+item.Value,o => effectObject.Add(item.Key,o as GameObject));

		}
	}

	public static GameObject InstantiateEffect(GameObject parent, GameObject obj) {
		Vector3 localScale = obj.transform.localScale;
		Vector3 rotation = obj.transform.eulerAngles;
		GameObject effectIns =  NGUITools.AddChild(parent, obj);
//		effectIns.layer = GameLayer.EffectLayer;
		effectIns.transform.localScale = localScale;
		effectIns.transform.eulerAngles = rotation;
		return effectIns;
	}
}
