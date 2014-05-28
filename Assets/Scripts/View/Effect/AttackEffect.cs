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

	private Vector3 activeEnd = Vector3.zero;

	public const float activeSkillEffectTime = 1.5f;

	void Awake() {
		effect = transform.Find ("AE").gameObject;
		effect.SetActive (false);
		activeEffect = transform.Find ("ActiveSkill").gameObject;
		avatarTexture = activeEffect.transform.Find ("Avatar").GetComponent<UITexture> ();
		activeEffect.SetActive (false);
	}

	public void RefreshItem (string userUnitID) {
		AttackEffectItem aei;
		if (inactiveEffect.Count == 0) {
			GameObject go = NGUITools.AddChild (gameObject, effect);
			aei = go.GetComponent<AttackEffectItem> ();
		} else {
			aei = inactiveEffect.Dequeue();
		}

		aei.gameObject.SetActive (true);
		aei.RefreshInfo(userUnitID,End);
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
		if (activeEnd.x == 0) {
			activeEnd = new Vector3(BattleCardArea.activeSkillEndPosition.x - 320f, BattleCardArea.activeSkillEndPosition.y, BattleCardArea.activeSkillEndPosition.z);
		}
		iTween.MoveTo (activeEffect, iTween.Hash ("position", BattleCardArea.startPosition, "time", activeSkillEffectTime, "oncompletetarget", gameObject, "oncomplete", "ActiveSkillEnd", "islocal", true,"easetype", iTween.EaseType.easeInOutBack ));  
	}

//	public void PlayPassiveSkill(TUserUnit tuu) {
//
//	}

	void ActiveSkillEnd() {
		avatarTexture.mainTexture = null;
		activeEffect.SetActive (false);
	}
}
