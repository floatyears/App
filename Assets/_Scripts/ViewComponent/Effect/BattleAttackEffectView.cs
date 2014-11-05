using UnityEngine;
using System.Collections.Generic;
using bbproto;
using System.Collections;

public class BattleAttackEffectView : ViewBase {
	private GameObject effect;
	private Queue<AttackEffectItem> itemPool = new Queue<AttackEffectItem>();
	private Queue<AttackEffectItem> attackEffectStack = new Queue<AttackEffectItem>();

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
			RefreshItem(args[1] as AttackInfoProto, (bool)args[2]);
			break;
		case "active_skill":
			PlayActiveSkill(args[1] as AttackInfoProto);
			break;
		default:
				break;
		}
	}

	void RefreshItem (AttackInfoProto ai, bool recoverHP = false) {
		AttackEffectItem aei = GetAttackEffectItem ();
		aei.RefreshInfo(ai.userUnitID, ai.skillID, ()=>{
			if(attackEffectStack.Count == 0){
				InvokeRepeating("DelayAni",2f,2f);
			}
			attackEffectStack.Enqueue (aei);
			Debug.Log("attack effect: [[[[[[[[[++++++++++" + aei.GetInstanceID());
			iTween.MoveTo(aei.gameObject,iTween.Hash("y",200f + attackEffectStack.Count*111f,"time", 0.4f,"easetype",iTween.EaseType.easeInOutQuart,"islocal",true,"oncomplete","AniEnd","oncompletetarget",gameObject));
		}, (int)ai.attackValue, recoverHP);
	}

//	void AniEnd(){
//		StartCoroutine (DelayAni ());
//	}

	void DelayAni(){
//		yield return new WaitForSeconds(1f);

		AttackEffectItem item = attackEffectStack.Dequeue ();
		if (attackEffectStack.Count == 0) {
			CancelInvoke("DelayAni");
		}
		item.gameObject.SetActive (false);
		itemPool.Enqueue (item);
		Debug.Log ("effect length: " + attackEffectStack.Count);

		AttackEffectItem[] temp = attackEffectStack.ToArray ();

		for (int i = 0; i< temp.Length; i++){
			iTween.Stop(temp[i].gameObject);
			iTween.MoveTo(temp[i].gameObject,iTween.Hash("y",200f + i*111f,"time", 0.4f,"easetype",iTween.EaseType.easeInOutQuart,"islocal",true));
		}
	}

	string skillName = "";

	void PlayActiveSkill(AttackInfoProto ai) {
		activeEffect.SetActive (true);
		activeEffect.transform.localPosition = Vector3.zero;
		UserUnit tuu = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit(ai.userUnitID);
		ResourceManager.Instance.GetAvatarAtlas (tuu.UnitInfo.id, avatarTexture);
		SkillBase sbi = DataCenter.Instance.BattleData.GetSkill (ai.userUnitID, ai.skillID, SkillType.ActiveSkill);
		skillName = sbi == null ? "" : TextCenter.GetText (SkillBase.SkillNamePrefix + sbi.id);//sbi.SkillName;
		iTween.MoveTo (activeEffect, iTween.Hash ("position", BattleManipulationView.startPosition, "time", activeSkillEffectTime - 0.5f, "oncompletetarget", gameObject, "oncomplete", "ActiveSkillEnd", "islocal", true,"easetype", iTween.EaseType.easeInOutQuad));  
//		Debug.LogError ("PlayActiveSkill MoveTo");
		AudioManager.Instance.PlayAudio (AudioEnum.sound_as_fly);
	}

	void ActiveSkillEnd() {
		AttackEffectItem aei = GetAttackEffectItem ();
//		Debug.LogError ("ActiveSkillEnd");
//		aei.ShowActiveSkill (skillName, End);
//		avatarTexture.mainTexture = null;
		activeEffect.SetActive (false);
	}

	AttackEffectItem GetAttackEffectItem () {
		AttackEffectItem aei;
		if (itemPool.Count == 0) {
			GameObject go = NGUITools.AddChild (gameObject, effect);
			aei = go.GetComponent<AttackEffectItem> ();
		} else {
			aei = itemPool.Dequeue();
		}
		
		aei.gameObject.SetActive (true);
//		attackEffectQueue.Enqueue(aei);

		return aei;
	}
}
