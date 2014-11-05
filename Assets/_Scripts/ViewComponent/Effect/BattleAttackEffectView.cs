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
			Debug.Log("attack effect: [[[[[[[[[++++++++++" + aei.GetInstanceID());
//			iTween.MoveTo(aei.gameObject,iTween.Hash("y",300f,"time", 0.4f,"easetype",iTween.EaseType.easeInOutQuart,"islocal",true,"oncomplete","AniEnd","oncompletetarget",gameObject));

			StartCoroutine(DelayAni());
			attackEffectStack.Enqueue (aei);
			AttackEffectItem[] temp = attackEffectStack.ToArray ();
			int len = temp.Length;
			Debug.Log("temp leng: " + temp.Length);
			for (int i = 0; i< len; i++){
//				iTween.Stop(temp[i].gameObject);
				iTween.MoveTo(temp[i].gameObject,iTween.Hash("y",300f - (len - i - 1)*90f,"time", 0.4f,"easetype",iTween.EaseType.easeInOutQuart,"islocal",true));
			}

		}, (int)ai.attackValue, recoverHP);
	}

//	void AniEnd(){
//		StartCoroutine (DelayAni ());
//	}

	IEnumerator DelayAni(){
		yield return new WaitForSeconds(1.25f);

		AttackEffectItem item = attackEffectStack.Dequeue ();
		item.gameObject.SetActive (false);
		itemPool.Enqueue (item);
		Debug.Log ("effect length: " + attackEffectStack.Count);

		AttackEffectItem[] temp = attackEffectStack.ToArray ();
		int len = temp.Length;
		for (int i = 0; i< len; i++){
//			iTween.Stop(temp[i].gameObject);
			iTween.MoveTo(temp[i].gameObject,iTween.Hash("y",300f - (len - i - 1)*90f,"time", 0.4f,"easetype",iTween.EaseType.easeInOutQuart,"islocal",true));
		}
	}

	string skillName = "";

	void PlayActiveSkill(AttackInfoProto ai) {
		activeEffect.SetActive (true);
		activeEffect.transform.localPosition = BattleManipulationView.startPosition;
		UserUnit tuu = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit(ai.userUnitID);
		ResourceManager.Instance.GetAvatarAtlas (tuu.UnitInfo.id, avatarTexture);
		SkillBase sbi = DataCenter.Instance.BattleData.GetSkill (ai.userUnitID, ai.skillID, SkillType.ActiveSkill);
		skillName = sbi == null ? "" : TextCenter.GetText (SkillBase.SkillNamePrefix + sbi.id);//sbi.SkillName;
		iTween.RotateTo (activeEffect, iTween.Hash ("rotation",new Vector3(0,0,1080f), "time", activeSkillEffectTime - 0.5f, "oncompletetarget", gameObject, "oncomplete", "ActiveSkillEnd","oncompleteparams",ai ,"islocal", true,"easetype",iTween.EaseType.linear));  
//		Debug.LogError ("PlayActiveSkill MoveTo");
		AudioManager.Instance.PlayAudio (AudioEnum.sound_as_fly);
	}

	void ActiveSkillEnd(object data) {
//		AttackEffectItem aei = GetAttackEffectItem ();
//		Debug.LogError ("ActiveSkillEnd");
//		aei.ShowActiveSkill (skillName, End);
//		avatarTexture.mainTexture = null;
		activeEffect.SetActive (false);
		RefreshItem (data as AttackInfoProto);
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
