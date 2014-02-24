using UnityEngine;
using System.Collections.Generic;

public class EnemyItem : UIBaseUnity {
	private TempEnemy enemyInfo;
	private UITexture texture;
	private UILabel bloodLabel;
	private UILabel nextLabel;
	private UIPanel effect;
	private Vector3 attackPosition;
	private Vector3 localPosition;

	private UILabel hurtValueLabel;
	private Queue<GameObject> hurtValueQueue = new Queue<GameObject>();
	private Vector3 hurtLabelPosition = Vector3.zero;
	private Vector3 initHurtLabelPosition = Vector3.zero;

	void OnEnable() {
//		MsgCenter.Instance.AddListener (CommandEnum.ShowEnemy, EnemyInfo);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttack, EnemyAttack);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyRefresh, EnemyRefresh);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyDead, EnemyDead);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, Attack);
		MsgCenter.Instance.AddListener (CommandEnum.SkillPosion, SkillPosion);
		MsgCenter.Instance.AddListener (CommandEnum.BePosion, BePosion);
		MsgCenter.Instance.AddListener (CommandEnum.ReduceDefense, ReduceDefense);


	}

	void OnDisable () {
//		MsgCenter.Instance.RemoveListener (CommandEnum.ShowEnemy, EnemyInfo);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttack, EnemyAttack);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyRefresh, EnemyRefresh);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyDead, EnemyDead);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, Attack);
		MsgCenter.Instance.RemoveListener (CommandEnum.SkillPosion, SkillPosion);
		MsgCenter.Instance.RemoveListener (CommandEnum.BePosion, BePosion);
		MsgCenter.Instance.RemoveListener (CommandEnum.ReduceDefense, ReduceDefense);
	}

	GameObject prevObject = null;

	void ReduceDefense(object data)  {
		TClass<int,int,float> tc = data as TClass<int,int,float>;
		if (tc == null) {
			return;	
		}
	}

	void Attack (object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null || ai.EnemyID != enemyInfo.GetID()) {
			return;
		}
		if (prevObject != null) {
			Destroy(prevObject);
		}

		ShowHurtInfo (ai.InjuryValue);
		ShowInjuredEffect (ai.AttackType);
	}

	void ShowHurtInfo(int injuredValue) {
		GameObject hurtLabel = NGUITools.AddChild (gameObject, hurtValueLabel.gameObject);
		hurtLabel.SetActive (true);
		hurtLabel.transform.localPosition = initHurtLabelPosition;
		hurtValueQueue.Enqueue (hurtLabel);
		UILabel info = hurtLabel.GetComponent<UILabel> ();
		info.text = injuredValue.ToString();
		iTween.MoveTo(hurtLabel,iTween.Hash("position",hurtLabelPosition,"time",1f,"easetype",iTween.EaseType.easeOutQuart,"oncomplete","RemoveHurtLabel","oncompletetarget",gameObject,"islocal",true));
	}

	void RemoveHurtLabel() {
		Destroy (hurtValueQueue.Dequeue ());
	}

	void ShowInjuredEffect (int attackType) {
		GameObject obj = GlobalData.GetEffect (attackType) as GameObject;
		DGTools.PlayAttackSound (attackType);
		if (obj != null) {
			prevObject = NGUITools.AddChild(effect.gameObject,obj);
			prevObject.transform.localScale = new Vector3(100f,100f,100f);
			InjuredShake();
		}
	}

	void InjuredShake(){
		iTween.ShakeScale(texture.gameObject, new Vector3(0.5f,0.5f,0.5f), 0.2f);
	}

	void BePosion(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;	
		}
		Debug.Log ("play posion animation");
		Debug.Log ("posion round : " + ai.AttackRound);
	}

	void SkillPosion(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;	
		}
		Debug.Log ("posion round : " + ai.AttackRound);
	}
	
	public void Init(TempEnemy te) {
		texture 				= FindChild<UITexture> ("Texture");
		TempUnitInfo tui 		= GlobalData.tempUnitInfo [te.GetID ()];
		texture.mainTexture 	= tui.GetAsset (UnitAssetType.Profile);
		localPosition 			= texture.transform.localPosition;
		attackPosition 			= new Vector3 (localPosition.x, BattleBackground.ActorPosition.y , localPosition.z);
		bloodLabel 				= FindChild<UILabel> ("BloodLabel");
		nextLabel 				= FindChild<UILabel> ("NextLabel");
		effect					= FindChild<UIPanel> ("Effect");
		hurtValueLabel			= FindChild<UILabel> ("HurtLabel");
		initHurtLabelPosition 	= hurtValueLabel.transform.localPosition;
		hurtLabelPosition 		= new Vector3 (initHurtLabelPosition.x, initHurtLabelPosition.y + hurtValueLabel.height * 3, initHurtLabelPosition.z);
		enemyInfo 				= te;
		hurtValueLabel.gameObject.SetActive (false);
		SetData (te);
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
		OnDisable ();
		Destroy (gameObject);
	//	Destroy (this);
	}

	void EnemyDead(object data) {
		TempEnemy te = data as TempEnemy;
		//Debug.LogError(te.GetID() + " EnemyDead : " + enemyInfo.enemyID);
		if (te == null || te.GetID () != enemyInfo.GetID()) {
			return;		
		}
		AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_die);
		DestoryUI ();
	}

	void EnemyRefresh(object data) {
		TempEnemy te = data as TempEnemy;
		if (te == null) {
			return;		
		}

		if (te.GetID () != enemyInfo.GetID()) {
			return;		
		}

		enemyInfo = te;

		if (enemyInfo.GetBlood() > te.GetBlood ()) {
			InjuredShake();
		}

		SetBloodLabel (enemyInfo.GetBlood());
		SetNextLabel (enemyInfo.GetRound());
	}

	void EnemyAttack (object data) {
		uint id = (uint) data;
		if (id == enemyInfo.GetID()) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_enemy_attack);
			iTween.ScaleTo(gameObject,new Vector3(1.5f,1.5f,1f),0.2f);
			iTween.MoveTo (texture.gameObject,iTween.Hash("position",attackPosition,"time",0.2f,"oncomplete","MoveBack","oncompletetarget",gameObject, "islocal",true ,"easetype",iTween.EaseType.easeInCubic));
		}
	}

	void MoveBack () {
		iTween.ScaleTo(gameObject,new Vector3(1f,1f,1f),0.2f);
		iTween.MoveTo (texture.gameObject,iTween.Hash("position",localPosition,"time",0.2f,"islocal",true,"easetype",iTween.EaseType.easeOutCubic));
	}

//	void AttackEnemy (object data) {
//		AttackInfo ai = data as AttackInfo;
//
//		if (ai == null) {
//			return;
//		}
//		if (ai.AttackRange != 1 && ai.EnemyID != enemyInfo.enemyID) {
//			return;		
//		}
//		enemyInfo.enemyBlood -= (int)ai.AttackValue;
//	
//		SetData (enemyInfo);
//	}

	void SetData (TempEnemy seu) {

		SetBloodLabel (seu.GetBlood());
		SetNextLabel (seu.GetRound());
	}

	void SetBloodLabel (int seu) {
			bloodLabel.text = seu.ToString();	
			//enemyInfo.enemyBlood = seu;
//		}
	}

	void SetNextLabel (int seu) {
//		if (seu != enemyInfo.attackRound) {
			nextLabel.text = "Next : " + seu;
			//enemyInfo.attackRound = seu;
//		}
	}

//	void EnemyInfo (object data) {
//		ShowEnemyUtility te = (ShowEnemyUtility)data;
//		if (te.enemyID != enemyInfo.GetID()) {
//			return;
//		}
//		SetData(te);
//	}
}
