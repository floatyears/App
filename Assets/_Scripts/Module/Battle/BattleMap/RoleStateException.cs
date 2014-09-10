using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class RoleStateException {
	private GameObject srcObject;
	public RoleStateException () {
		ResourceManager.Instance.LoadLocalAsset("Prefabs/Fight/State",o =>srcObject = o as GameObject);
	}

	private Dictionary<StateEnum, GameObject> roleStateDic = new Dictionary<StateEnum, GameObject>();

	public void AddListener() {
		MsgCenter.Instance.AddListener (CommandEnum.PlayerPosion, PlayerPosion);
		MsgCenter.Instance.AddListener (CommandEnum.ShieldMap, ShieldMap);
	}

	public void RemoveListener() {
		MsgCenter.Instance.RemoveListener (CommandEnum.PlayerPosion, PlayerPosion);
		MsgCenter.Instance.RemoveListener (CommandEnum.ShieldMap, ShieldMap);
	}

	void PlayerPosion(object data) {
		int round = (int)data;
		TrapInfo (round, TrapBase.poisonTrapSpriteName, StateEnum.Poison);
	}

	void ShieldMap(object data) {
		int round = (int)data;
		TrapInfo (round, TrapBase.environmentSpriteName, StateEnum.TrapEnvironment);
	}

	void TrapInfo (int round, string spriteName, StateEnum trapState) {
		GameObject go = null;
		UILabel label = null;
		if (roleStateDic.TryGetValue(trapState, out go)) {
			if(round == 0) {
				GameObject.Destroy(go);
				roleStateDic.Remove(trapState);
			} else {
				label = go.transform.Find("Label").GetComponent<UILabel>();
				label.text = round.ToString();
			}
			return;
		}

		if (round == 0) {
			return;		
		}

		GameObject ins = NGUITools.AddChild (ViewManager.Instance.BottomLeftPanel, srcObject);
		UISprite sprite = ins.transform.Find ("Sprite").GetComponent<UISprite> ();
		label = ins.transform.Find("Label").GetComponent<UILabel>();
//		sprite.spriteName = spriteName; 
		label.text = round.ToString();
		ins.transform.localPosition = srcObject.transform.localPosition;

		DGTools.SortStateItem (roleStateDic, ins.transform, 40f);
		roleStateDic.Add (trapState, ins);
	}


}
