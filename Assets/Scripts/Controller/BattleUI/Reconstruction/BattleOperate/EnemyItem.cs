using UnityEngine;
using System.Collections.Generic;

public class EnemyItem : UIBaseUnity {
	private TEnemyInfo enemyInfo;
	[HideInInspector]
	public UITexture texture;
	private UITexture dropTexture;

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
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttack, EnemyAttack);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyRefresh, EnemyRefresh);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyDead, EnemyDead);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, Attack);
		MsgCenter.Instance.AddListener (CommandEnum.SkillPosion, SkillPosion);
		MsgCenter.Instance.AddListener (CommandEnum.BePosion, BePosion);
		MsgCenter.Instance.AddListener (CommandEnum.ReduceDefense, ReduceDefense);
	}

	void OnDisable () {
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

	Queue<AttackInfo> attackQueue = new Queue<AttackInfo> ();
	void Attack (object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null || ai.EnemyID != enemyInfo.EnemyID) {
			return;
		}
		if (prevObject != null) {
			Destroy(prevObject);
		}

		attackQueue.Enqueue (ai);
		GameTimer.GetInstance ().AddCountDown (1f, Effect);

	}

	void Effect() {
		ShowHurtInfo (attackQueue.Dequeue().InjuryValue);
		InjuredShake ();
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
		iTween.ShakeScale (texture.gameObject, iTween.Hash ("amount", new Vector3 (0.5f, 0.5f, 0.5f), "time", 0.2f));
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
	
	public void Init(TEnemyInfo te) {
		texture 				= FindChild<UITexture> ("Texture");
		TUnitInfo tui 			= GlobalData.unitInfo [te.UnitID ];
		texture.mainTexture 	= tui.GetAsset (UnitAssetType.Profile);
		dropTexture 			= FindChild<UITexture>("Drop");
		dropTexture.enabled 	= false;
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

	public override void DestoryUI () {
		base.DestoryUI ();
		OnDisable ();
		Destroy (gameObject);
	}

	public void DropItem () {
		dropTexture.enabled = true;
		iTween.ShakeRotation (dropTexture.gameObject, iTween.Hash ("z",20,"time",0.5f));  //"oncomplete","DorpEnd","oncompletetarget",gameObject
		GameTimer.GetInstance ().AddCountDown (1f, DorpEnd);
	}

	void DorpEnd () {
		DestoryUI ();
	}

	void EnemyDead(object data) {
		TEnemyInfo te = data as TEnemyInfo;
		if (te == null || te.EnemyID != enemyInfo.EnemyID) {
			return;		
		}
		AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_die);
		//DestoryUI ();
		texture.enabled = false;
		DropItem ();
	}

	void EnemyRefresh(object data) {
		TEnemyInfo te = data as TEnemyInfo;
		if (te == null) {
			return;		
		}

		if (te.EnemyID != enemyInfo.EnemyID) {
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
		if (id == enemyInfo.EnemyID) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_enemy_attack);
			iTween.ScaleTo(gameObject,new Vector3(1.5f,1.5f,1f),0.2f);
			iTween.MoveTo (texture.gameObject,iTween.Hash("position",attackPosition,"time",0.2f,"oncomplete","MoveBack","oncompletetarget",gameObject, "islocal",true ,"easetype",iTween.EaseType.easeInCubic));
		}
	}

	void MoveBack () {
		iTween.ScaleTo(gameObject,new Vector3(1f,1f,1f),0.2f);
		iTween.MoveTo (texture.gameObject,iTween.Hash("position",localPosition,"time",0.2f,"islocal",true,"easetype",iTween.EaseType.easeOutCubic));
	}

	void SetData (TEnemyInfo seu) {
		SetBloodLabel (seu.GetBlood());
		SetNextLabel (seu.GetRound());
	}

	void SetBloodLabel (int seu) {
			bloodLabel.text = seu.ToString();	
	}

	void SetNextLabel (int seu) {
			nextLabel.text = "Next : " + seu;
	}


}
