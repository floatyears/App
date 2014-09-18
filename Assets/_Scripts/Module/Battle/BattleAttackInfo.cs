using UnityEngine;
using System.Collections;

public class BattleAttackInfo : MonoBehaviour {
//	private UISprite firstSprite;
	private Vector3 fsInitPosition;

//	private UISprite secondSprite;
	private Vector3 ssInitPosition;

//	private UISprite handsSprite;
	private Vector3 hsInitPosition; 

//	private UISprite hitFirstSprite;
//	private UISprite hitSecondSprite;
	private UILabel rateLabel;
	private UILabel hitLabel;

	//---------new label ---------------
	private UILabel firstLabel;
//	private UILabel secondLabel;
	private UILabel handsLabel;
	private UILabel hitFirstLabel;
//	private UILabel hitSecondLabel;
	//----------------------------------

	public void Init() {
//		firstSprite = FindChild<UISprite>("FirstSprite");
//		firstSprite.spriteName = string.Empty;
//		fsInitPosition = firstSprite.transform.localPosition;
//
//		secondSprite = FindChild<UISprite>("SecondSprite");
//		secondSprite.spriteName = string.Empty;
//		ssInitPosition = secondSprite.transform.localPosition;
//
//		handsSprite = FindChild<UISprite>("HandsSprite");
//		handsSprite.spriteName = string.Empty;
//		hsInitPosition = handsSprite.transform.localPosition;
//
//		hitFirstSprite = FindChild<UISprite>("HitFirstSprite");
//		hitFirstSprite.spriteName = string.Empty;
//		hitSecondSprite = FindChild<UISprite>("HitSecondSprite");
//		hitSecondSprite.spriteName = string.Empty;

		firstLabel = transform.FindChild("FirstLabel").GetComponent<UILabel>();
		firstLabel.text = "";
		fsInitPosition = firstLabel.transform.localPosition;

//		secondLabel = FindChild<UILabel>("SecondLabel");
//		secondLabel.text = "";
//		ssInitPosition = secondLabel.transform.localPosition;

		handsLabel = transform.FindChild("HandsLabel").GetComponent<UILabel>();
		handsLabel.text = string.Empty;
		hsInitPosition = handsLabel.transform.localPosition;

		hitFirstLabel = transform.FindChild("HitFirstLabel").GetComponent<UILabel>();
		hitFirstLabel.text = string.Empty;
//		hitSecondLabel = FindChild<UILabel>("HitSecondLabel");
//		hitSecondLabel.text = "";

		rateLabel = transform.FindChild("RateLabel").GetComponent<UILabel>();
		rateLabel.text = string.Empty;
		hitLabel = transform.FindChild("Label").GetComponent<UILabel>();
		hitLabel.enabled = false;
	}

	public void ShowUI () {
//		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, RefreshRate);
//		MsgCenter.Instance.AddListener (CommandEnum.RecoverHP, RefreshRate);
//		MsgCenter.Instance.AddListener (CommandEnum.ShowHands, StartAttack);
//		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
	}

	public void HideUI () {
//		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, RefreshRate);
//		MsgCenter.Instance.RemoveListener (CommandEnum.RecoverHP, RefreshRate);
//		MsgCenter.Instance.RemoveListener (CommandEnum.ShowHands, StartAttack);
//		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
	}

	public void StartAttack(object data) {
		int allAttack = (int)data;

//		firstSprite.spriteName = (allAttack / 10).ToString ();
//		secondSprite.spriteName =(allAttack % 10).ToString ();
//		handsSprite.spriteName = "hands";

//		string info = allAttack < 10 ? ("0" + allAttack.ToString()) : allAttack.ToString();
		firstLabel.text = GetFormatAttackCount(allAttack); //(allAttack / 10).ToString ();
//		secondLabel.text = (allAttack % 10).ToString ();
		handsLabel.text = "HANDS !";

		Invoke ("MoveNumberSprite", 0.2f);
	}

	void MoveNumberSprite() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_combo);

//		iTween.MoveTo(firstSprite.gameObject,iTween.Hash("y",72f,"time",0.3f,"islocal",true,"easetype",iTween.EaseType.easeInQuart));
//		iTween.MoveTo(secondSprite.gameObject,iTween.Hash("y",72f,"time",0.3f,"islocal",true,"easetype",iTween.EaseType.easeInQuart));
//		iTween.MoveTo (handsSprite.gameObject, iTween.Hash ("y", -50f, "time", 0.3f, "islocal",true, "easetype", iTween.EaseType.easeInQuart, "oncomplete", "StartMoveEnd", "oncompletetarget", gameObject));

		iTween.MoveTo(firstLabel.gameObject,iTween.Hash("y",72f,"time",0.3f,"islocal",true,"easetype",iTween.EaseType.easeInQuart));
//		iTween.MoveTo(secondLabel.gameObject,iTween.Hash("y",72f,"time",0.3f,"islocal",true,"easetype",iTween.EaseType.easeInQuart));
		iTween.MoveTo (handsLabel.gameObject, iTween.Hash ("y", -50f, "time", 0.3f, "islocal",true, "easetype", iTween.EaseType.easeInQuart, "oncomplete", "StartMoveEnd", "oncompletetarget", gameObject));

	}

	public void AttackEnemyEnd(object data) {
		RefreshRate (null, true);
		GameTimer.GetInstance ().AddCountDown (0.5f, ClearAttackInfo);
	}

	void ClearAttackInfo() {
//		hitFirstSprite.spriteName = string.Empty;
//		hitSecondSprite.spriteName = string.Empty;

		hitFirstLabel.text = "";
//		hitSecondLabel.text = "";

		rateLabel.text = string.Empty;
		hitLabel.enabled = false;
	}

	void StartMoveEnd() {
		Invoke ("Clear", 0.2f);
	}

	void Clear() {
//		firstSprite.transform.localPosition = fsInitPosition;
//		secondSprite.transform.localPosition = ssInitPosition;
//		handsSprite.transform.localPosition = hsInitPosition;
//		firstSprite.spriteName = string.Empty;
//		secondSprite.spriteName = string.Empty;
//		handsSprite.spriteName = string.Empty;

		firstLabel.transform.localPosition = fsInitPosition;
//		secondLabel.transform.localPosition = ssInitPosition;
		handsLabel.transform.localPosition = hsInitPosition;
		firstLabel.text = string.Empty;
//		secondLabel.text = string.Empty;
		handsLabel.text = string.Empty;
	}

	AttackInfo prevAttack;
	public void RefreshRate (object data,bool end = false) {
		if (hitLabel == null) {
			return;	
		}
		if(prevAttack != null){
//			hitFirstSprite.spriteName = (prevAttack.ContinuAttackMultip / 10).ToString ();
//			hitSecondSprite.spriteName = (prevAttack.ContinuAttackMultip % 10).ToString ();

			hitFirstLabel.text = GetFormatAttackCount(prevAttack.ContinuAttackMultip); //prevAttack.ContinuAttackMultip.ToString(); //(prevAttack.ContinuAttackMultip / 10).ToString ();
//			hitSecondLabel.text = (prevAttack.ContinuAttackMultip % 10).ToString ();

			hitLabel.enabled = true;
			rateLabel.text = "Rate x " + prevAttack.AttackRate;
		}
		prevAttack = data as AttackInfo;
	}

	string GetFormatAttackCount (int count) {
		return count < 10 ? ("0" + count.ToString ()) : count.ToString ();
	}
}
