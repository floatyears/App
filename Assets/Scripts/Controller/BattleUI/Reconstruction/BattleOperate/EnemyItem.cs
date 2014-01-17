using UnityEngine;
using System.Collections;

public class EnemyItem : UIBaseUnity {
	private ShowEnemyUtility enemyInfo;
	private UITexture texture;
	private UILabel bloodLabel;
	private UILabel nextLabel;

	void OnEnable() {
		MsgCenter.Instance.AddListener (CommandEnum.ShowEnemy, EnemyInfo);
	}

	void OnDisable () {
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowEnemy, EnemyInfo);
	}

	public void Init(ShowEnemyUtility te) {
		texture 	= FindChild<UITexture> ("Texture");
		bloodLabel 	= FindChild<UILabel> ("BloodLabel");
		nextLabel 	= FindChild<UILabel> ("NextLabel");
		enemyInfo = new ShowEnemyUtility ();
		enemyInfo.enemyID = te.enemyID;
		SetData (te);
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
		Destroy (gameObject);
	}

	void SetData (ShowEnemyUtility seu) {
		SetBloodLabel (seu.enemyBlood);
		SetNextLabel (seu.attackRound);
	}

	void SetBloodLabel (int seu) {
		if (seu != enemyInfo.enemyBlood) {
			bloodLabel.text = seu.ToString();	
			enemyInfo.enemyBlood = seu;
		}
	}

	void SetNextLabel (int seu) {
		if (seu != enemyInfo.attackRound) {
			nextLabel.text = "Next : " + seu;
			enemyInfo.attackRound = seu;
		}
	}

	void EnemyInfo (object data) {
		ShowEnemyUtility te = (ShowEnemyUtility)data;
		if (te.enemyID != enemyInfo.enemyID) {
			return;
		}
		SetData(te);
	}
}
