using UnityEngine;
using System.Collections;

public class EnemyItem : UIBaseUnity {
	private ShowEnemyUtility enemyInfo;
	private UITexture texture;
	private UILabel bloodLabel;
	private UILabel nextLabel;
	private UIPanel effect;
	private Vector3 attackPosition;
	private Vector3 localPosition;

	void OnEnable() {
		MsgCenter.Instance.AddListener (CommandEnum.ShowEnemy, EnemyInfo);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttack, EnemyAttack);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyRefresh, EnemyRefresh);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyDead, EnemyDead);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, Attack);
		MsgCenter.Instance.AddListener (CommandEnum.SkillPosion, SkillPosion);
		MsgCenter.Instance.AddListener (CommandEnum.BePosion, BePosion);
		MsgCenter.Instance.AddListener (CommandEnum.ReduceDefense, ReduceDefense);


	}

	void OnDisable () {
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowEnemy, EnemyInfo);
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
		if (ai == null || ai.EnemyID != enemyInfo.enemyID) {
			return;
		}
		if (prevObject != null) {
			Destroy(prevObject);
		}
		GameObject obj = GlobalData.GetEffect (ai.AttackType) as GameObject;
		DGTools.PlayAttackSound (ai.AttackType);
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
	
	public void Init(ShowEnemyUtility te) {
		texture 	= FindChild<UITexture> ("Texture");
		localPosition = texture.transform.localPosition;
		attackPosition = new Vector3 (localPosition.x, BattleBackground.ActorPosition.y , localPosition.z);
//		Debug.LogError (attackPosition + " --   -- " + localPosition + "       ......     " + BattleBackground.ActorPosition.y);
		bloodLabel 	= FindChild<UILabel> ("BloodLabel");
		nextLabel 	= FindChild<UILabel> ("NextLabel");
		effect		= FindChild<UIPanel> ("Effect");
		enemyInfo 	= te;
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
		if (te == null || te.GetID () != enemyInfo.enemyID) {
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

		if (te.GetID () != enemyInfo.enemyID) {
			return;		
		}
		if (enemyInfo.enemyBlood > te.GetBlood ()) {
			InjuredShake();
		}
		enemyInfo.enemyBlood = te.GetBlood ();
		if (enemyInfo.enemyBlood < 0) {
			enemyInfo.enemyBlood = 0;
		}
		enemyInfo.attackRound = te.GetRound ();
		SetBloodLabel (enemyInfo.enemyBlood);
		SetNextLabel (enemyInfo.attackRound);
	}

	void EnemyAttack (object data) {
		uint id = (uint) data;
		if (id == enemyInfo.enemyID) {
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

	void SetData (ShowEnemyUtility seu) {

		SetBloodLabel (seu.enemyBlood);
		SetNextLabel (seu.attackRound);
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

	void EnemyInfo (object data) {
		ShowEnemyUtility te = (ShowEnemyUtility)data;
		if (te.enemyID != enemyInfo.enemyID) {
			return;
		}
		SetData(te);
	}
}
