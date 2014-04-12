using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class AttackEffect : MonoBehaviour {

	private GameObject effect;
	private Queue<AttackEffectItem> inactiveEffect = new Queue<AttackEffectItem>();
	private Queue<AttackEffectItem> attackEffectQueue = new Queue<AttackEffectItem>();

	void Awake() {
		effect = transform.Find ("AE").gameObject;
		effect.SetActive (false);
	}

	public void RefreshItem (AttackInfo data) {
		AttackEffectItem aei;
		if (inactiveEffect.Count == 0) {
			GameObject go = NGUITools.AddChild (gameObject, effect);
			aei = go.GetComponent<AttackEffectItem> ();
		} else {
			aei = inactiveEffect.Dequeue();
		}
		aei.gameObject.SetActive (true);
		aei.RefreshInfo(data,End);
		attackEffectQueue.Enqueue(aei);
	}

	void End() {
		AttackEffectItem aei = attackEffectQueue.Dequeue ();
		if (inactiveEffect.Count < 3) {
			aei.gameObject.SetActive (false);
			inactiveEffect.Enqueue (aei);
		} else {
			Destroy(aei.gameObject);
		}
	}
}
