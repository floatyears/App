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

	private GameObject effectPanel;

	private Dictionary<string,Type> effectCommand = new Dictionary<string, Type> ();
	private EffectManager() {
		effectPanel = ViewManager.Instance.BottomPanel.transform.parent.Find("EffectPanel").gameObject;
		effectCommand.Add (EffectConstValue.NormalFire1, typeof(MoveLineAndBoomEffect));
		AddListener ();
	}

	public void PlayAttackEffect(string command,List<Vector3> position,List<GameObject> obj) {
		Type ty = effectCommand [command];
		IEffectBehavior eb = Activator.CreateInstance (ty) as IEffectBehavior;
		eb.EffectAssetList = obj;
		eb.Excute (position);
	}

	void AddListener() {
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
	}
	
	void RemoveListener () {
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
	}

	void AttackEnemy(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;	
		}
		
		List<GameObject> effect = EffectConstValue.Instance.GetEffect (ai);
		if (effect == null || effect.Count == 0) {
			return;	
		}

//		if (ai.AttackType == 1) {
			GameObject go = NGUITools.AddChild(effectPanel,effect[0]);
//			Debug.LogError("go : " + go);
			go.transform.localScale = new Vector3(100f,100f,100f);
			go.SetActive(false);
			List<GameObject> tempGo = new List<GameObject>();
			tempGo.Add(go);
			Debug.LogError("tempGo : " + tempGo.Count);
			List<Vector3> position = new List<Vector3>();
			Debug.LogError("ai.UserUnitID : " + ai.UserUnitID);
			Vector3 temp = DisposeActorPosition(BattleBackground.ActorTransform[ai.UserUnitID]);
			position.Add(temp);
			temp = DisposeEnemyPosition(BattleEnemy.Monster[ai.EnemyID].transform);
			position.Add(temp);
			PlayAttackEffect(EffectConstValue.NormalFire1,position,tempGo);
//		}
	}

	Vector3 DisposeActorPosition(Transform temp) {
		Vector3 tempPosition = temp.localPosition; //- temp.parent.localPosition;
		return tempPosition;
	}

	Vector3 DisposeEnemyPosition(Transform enemy) {
//		return Vector3.zero;
		Vector3 tempPosition = enemy.localPosition + enemy.parent.localPosition + enemy.parent.parent.localPosition;
		return tempPosition;
	}
}
