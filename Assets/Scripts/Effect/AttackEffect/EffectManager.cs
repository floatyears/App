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


	private Dictionary<string,Type> effectCommand = new Dictionary<string, Type> ();
	private EffectManager() {
		effectPanel = ViewManager.Instance.BottomPanel.transform.parent.Find("EffectPanel").gameObject;
		effectCommand.Add (EffectConstValue.NormalFire1, typeof(NormalFireEffect));
		AddListener ();

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
