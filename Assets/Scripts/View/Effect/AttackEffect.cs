using UnityEngine;
using System.Collections.Generic;
using bbproto;
using System.Collections;

public class AttackEffect : MonoBehaviour {

	private GameObject effect;
	private Queue<AttackEffectItem> inactiveEffect = new Queue<AttackEffectItem>();
	private Queue<AttackEffectItem> attackEffectQueue = new Queue<AttackEffectItem>();

	private GameObject activeEffect;
	private UITexture avatarTexture;

	public const float activeSkillEffectTime = 2f;

	void Awake() {
		effect = transform.Find ("AE").gameObject;
		effect.SetActive (false);
		activeEffect = transform.Find ("ActiveSkill").gameObject;
		avatarTexture = activeEffect.transform.Find ("Avatar").GetComponent<UITexture> ();
		activeEffect.SetActive (false);
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

	public void PlayActiveSkill(TUserUnit tuu) {
		activeEffect.SetActive (true);
		activeEffect.transform.localPosition = BattleCardArea.activeSkillStartPosition;
		avatarTexture.mainTexture = tuu.UnitInfo.GetAsset (UnitAssetType.Avatar);
		iTween.MoveTo (activeEffect, iTween.Hash ("position", BattleCardArea.startPosition, "time", activeSkillEffectTime, "oncompletetarget", gameObject, "oncomplete", "ActiveSkillEnd", "islocal", true));
	}

	void ActiveSkillEnd() {
		avatarTexture.mainTexture = null;
		activeEffect.SetActive (false);
	}
}
