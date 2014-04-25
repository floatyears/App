using UnityEngine;
using System.Collections;

public class BattleAttackInfo : UIBaseUnity {
	private UISprite firstSprite;
	private Vector3 fsInitPosition;

	private UISprite secondSprite;
	private Vector3 ssInitPosition;

	private UISprite handsSprite;
	private Vector3 hsInitPosition; 

	private UISprite hitFirstSprite;
	private UISprite hitSecondSprite;
	private UILabel rateLabel;
	private UILabel hitLabel;

	public void Init() {
		firstSprite = FindChild<UISprite>("FirstSprite");
		firstSprite.spriteName = string.Empty;
		fsInitPosition = firstSprite.transform.localPosition;

		secondSprite = FindChild<UISprite>("SecondSprite");
		secondSprite.spriteName = string.Empty;
		ssInitPosition = secondSprite.transform.localPosition;

		handsSprite = FindChild<UISprite>("HandsSprite");
		handsSprite.spriteName = string.Empty;
		hsInitPosition = handsSprite.transform.localPosition;

		hitFirstSprite = FindChild<UISprite>("HitFirstSprite");
		hitFirstSprite.spriteName = string.Empty;
		hitSecondSprite = FindChild<UISprite>("HitSecondSprite");
		hitSecondSprite.spriteName = string.Empty;
		rateLabel = FindChild<UILabel>("RateLabel");
		rateLabel.text = string.Empty;
		hitLabel = FindChild<UILabel>("Label");
		hitLabel.enabled = false;
	}

	public override void ShowUI () {
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, RefreshRate);
		MsgCenter.Instance.AddListener (CommandEnum.RecoverHP, RefreshRate);
		MsgCenter.Instance.AddListener (CommandEnum.ShowHands, StartAttack);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
	}

	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, RefreshRate);
		MsgCenter.Instance.RemoveListener (CommandEnum.RecoverHP, RefreshRate);
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowHands, StartAttack);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
	}

	public void StartAttack(object data) {
		int allAttack = (int)data;
		firstSprite.spriteName = (allAttack / 10).ToString ();
		secondSprite.spriteName =(allAttack % 10).ToString ();
		handsSprite.spriteName = "hands";
		Invoke ("MoveNumberSprite", 0.2f);
	}

	void MoveNumberSprite() {
		iTween.MoveTo(firstSprite.gameObject,iTween.Hash("y",72f,"time",0.3f,"islocal",true,"easetype",iTween.EaseType.easeInQuart));
		iTween.MoveTo(secondSprite.gameObject,iTween.Hash("y",72f,"time",0.3f,"islocal",true,"easetype",iTween.EaseType.easeInQuart));
		iTween.MoveTo (handsSprite.gameObject, iTween.Hash ("y", -50f, "time", 0.3f, "islocal",true, "easetype", iTween.EaseType.easeInQuart, "oncomplete", "StartMoveEnd", "oncompletetarget", gameObject));
	}

	void AttackEnemyEnd(object data) {
		RefreshRate (null, true);
		GameTimer.GetInstance ().AddCountDown (0.5f, ClearAttackInfo);
	}

	void ClearAttackInfo() {
		hitFirstSprite.spriteName = string.Empty;
		hitSecondSprite.spriteName = string.Empty;
		rateLabel.text = string.Empty;
		hitLabel.enabled = false;
	}

	void StartMoveEnd() {
		Invoke ("Clear", 0.2f);
	}

	void Clear() {
		firstSprite.transform.localPosition = fsInitPosition;
		secondSprite.transform.localPosition = ssInitPosition;
		handsSprite.transform.localPosition = hsInitPosition;
		firstSprite.spriteName = string.Empty;
		secondSprite.spriteName = string.Empty;
		handsSprite.spriteName = string.Empty;
	}

	AttackInfo prevAttack;
	void RefreshRate (object data,bool end = false) {
		if (hitLabel == null) {
			return;	
		}
		if(prevAttack != null){
			hitFirstSprite.spriteName = (prevAttack.ContinuAttackMultip / 10).ToString ();
			hitSecondSprite.spriteName = (prevAttack.ContinuAttackMultip % 10).ToString ();
			hitLabel.enabled = true;
			rateLabel.text = "rate x " + prevAttack.AttackRate;
		}
		prevAttack = data as AttackInfo;
	}
}
