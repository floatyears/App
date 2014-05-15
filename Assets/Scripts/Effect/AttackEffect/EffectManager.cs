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
	}

	private Dictionary<string,Type> effectCommand = new Dictionary<string, Type> ();
	private EffectManager() {
		SetName ();
		foreach (var item in effectName) {
			GameObject go = Resources.Load("Effect/"+item.Value) as GameObject;
			effectObject.Add(item.Key,go);
		}

//		effectPanel = ViewManager.Instance.BottomPanel.transform.parent.Find("EffectPanel").gameObject;
//		effectCommand.Add (EffectConstValue.NormalFire1, typeof(NormalFireEffect));
//		AddListener ();

	}

	public void PlayAttackEffect(string command,List<GameObject> effect,AttackInfo ai) {
		Type ty = effectCommand [command];
		IEffectConcrete eb = Activator.CreateInstance (ty) as IEffectConcrete;
		eb.Play (effect, ai);
	}

	void AddListener() {
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
	}
	
	void RemoveListener () {
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
	}

	void AttackEnemy(object data) {
//		AttackInfo ai = data as AttackInfo;
//		if (ai == null) {
//			return;	
//		}
//		List<GameObject> effect = EffectConstValue.Instance.GetEffect (ai);
//		if (effect == null || effect.Count == 0) {
//			return;	
//		}
//		PlayAttackEffect (EffectConstValue.NormalFire1, effect, ai);


	}


	public Vector3 DisposeActorPosition(Transform temp) {
		Vector3 tempPosition = temp.localPosition; 
		return tempPosition;
	}

	public Vector3 DisposeEnemyPosition(Transform enemy) {
		Vector3 tempPosition = enemy.localPosition + enemy.parent.localPosition + enemy.parent.parent.localPosition;
		return tempPosition;
	}

	public List<GameObject> InitGameObject (List<GameObject> sourceObject) {
		List<GameObject> temp = new List<GameObject> ();
	
		for (int i = 0; i < sourceObject.Count; i++) {
			GameObject go = NGUITools.AddChild(effectPanel,sourceObject[i]);
			go.transform.localScale = new Vector3(100f,100f,100f);
			go.SetActive(false);
			temp.Add(go);
		}
		return temp;
	}
}
