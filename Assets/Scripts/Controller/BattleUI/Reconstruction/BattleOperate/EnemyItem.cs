using UnityEngine;
using System.Collections;

public class EnemyItem : UIBaseUnity {
	private ShowEnemyUtility enemyInfo;
	private UITexture texture;
	private UILabel bloodLabel;
	private UILabel nextLabel;

	void OnEnable() {
		MsgCenter.Instance.AddListener (CommandEnum.ShowEnemy, EnemyInfo);
		//MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttack, EnemyAttack);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyRefresh, EnemyRefresh);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyDead, EnemyDead);
	}

	void OnDisable () {
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowEnemy, EnemyInfo);
		//MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttack, EnemyAttack);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyRefresh, EnemyRefresh);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyDead, EnemyDead);
	}

	public void Init(ShowEnemyUtility te) {
		texture 	= FindChild<UITexture> ("Texture");
		bloodLabel 	= FindChild<UILabel> ("BloodLabel");
		nextLabel 	= FindChild<UILabel> ("NextLabel");
		enemyInfo = te;
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
		enemyInfo.enemyBlood = te.GetBlood ();
		enemyInfo.attackRound = te.GetRound ();
		SetBloodLabel (enemyInfo.enemyBlood);
		SetNextLabel (enemyInfo.attackRound);
	}

	void EnemyAttack (object data) {
		int id = (int) data;
		if (id == enemyInfo.enemyID) {
			iTween.ScaleFrom(gameObject,new Vector3(1.5f,1.5f,1f),0.5f);
		}
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
