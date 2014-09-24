using UnityEngine;
using System.Collections.Generic;
using bbproto;
using System.Collections;

public class BattleAttackEffectView : ViewBase {
	private GameObject effect;
	private Queue<AttackEffectItem> inactiveEffect = new Queue<AttackEffectItem>();
	private Queue<AttackEffectItem> attackEffectQueue = new Queue<AttackEffectItem>();

	private GameObject activeEffect;
	private UISprite avatarTexture;

	private Vector3 activeEnd = Vector3.zero;

	public const float activeSkillEffectTime = 1.5f;
	public const float activeSkillShowNameTime = 0.5f;

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);
		effect = transform.Find ("AE").gameObject;
		effect.SetActive (false);
		activeEffect = transform.Find ("ActiveSkill").gameObject;
		avatarTexture = activeEffect.transform.Find ("Avatar").GetComponent<UISprite> ();
		activeEffect.SetActive (false);
	}

	public override void CallbackView (params object[] args)
	{
		switch (args[0].ToString()) {
		case "refresh_item":
			RefreshItem(args[1] as AttackInfo, (bool)args[2]);
			break;
		case "active_skill":
			PlayActiveSkill(args[1] as AttackInfo);
			break;
		default:
				break;
		}
	}

	void RefreshItem (AttackInfo ai, bool recoverHP = false) {
		AttackEffectItem aei = GetAttackEffectItem ();
		aei.RefreshInfo(ai.UserUnitID, ai.SkillID, End, (int)ai.AttackValue, recoverHP);
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

	string skillName = "";

	void PlayActiveSkill(AttackInfo ai) {
		activeEffect.SetActive (true);
		activeEffect.transform.localPosition = BattleManipulationView.activeSkillStartPosition;
		UserUnit tuu = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit(ai.UserUnitID);
		ResourceManager.Instance.GetAvatarAtlas (tuu.UnitInfo.id, avatarTexture);
		SkillBase sbi = DataCenter.Instance.BattleData.GetSkill (ai.UserUnitID, ai.SkillID, SkillType.ActiveSkill);
		skillName = sbi == null ? "" : TextCenter.GetText (SkillBase.SkillNamePrefix + sbi.id);//sbi.SkillName;
		iTween.MoveTo (activeEffect, iTween.Hash ("position", BattleManipulationView.startPosition, "time", activeSkillEffectTime - 0.5f, "oncompletetarget", gameObject, "oncomplete", "ActiveSkillEnd", "islocal", true,"easetype", iTween.EaseType.easeInOutQuad));  
//		Debug.LogError ("PlayActiveSkill MoveTo");
		AudioManager.Instance.PlayAudio (AudioEnum.sound_as_fly);
	}

	void ActiveSkillEnd() {
		AttackEffectItem aei = GetAttackEffectItem ();
//		Debug.LogError ("ActiveSkillEnd");
//		aei.ShowActiveSkill (avatarTexture, skillName, End);
//		avatarTexture.mainTexture = null;
		activeEffect.SetActive (false);
	}

	AttackEffectItem GetAttackEffectItem () {
		AttackEffectItem aei;
		if (inactiveEffect.Count == 0) {
			GameObject go = NGUITools.AddChild (gameObject, effect);
			aei = go.GetComponent<AttackEffectItem> ();
		} else {
			aei = inactiveEffect.Dequeue();
		}
		
		aei.gameObject.SetActive (true);
		attackEffectQueue.Enqueue(aei);

		return aei;
	}
}
