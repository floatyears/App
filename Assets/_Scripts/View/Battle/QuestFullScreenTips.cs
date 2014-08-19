using UnityEngine;
using System.Collections;

public class QuestFullScreenTips : UIBaseUnity {
	public override void Init (string name) {
		base.Init (name);
		initLocalPosition = transform.localPosition;
		initLocalScale = transform.localScale;

		UILabel uilabel = FindChild<UILabel>("TopLabel");
		TweenAlpha ta = uilabel.GetComponent<TweenAlpha>();
		label [0] = uilabel;
		tweenAlpha [0] = ta;

		uilabel = FindChild<UILabel>("BottomLabel");
		ta = uilabel.GetComponent<TweenAlpha>();
		label [1] = uilabel;
		tweenAlpha [1] = ta;

		uilabel = FindChild<UILabel>("CenterLabel");
		ta = uilabel.GetComponent<TweenAlpha>();
		label [2] = uilabel;
		tweenAlpha [2] = ta;

		HideUI ();
	}
	
	public override void ShowUI (){
		base.ShowUI ();
		HideUI (false);
	}

	public override void HideUI () {
		base.HideUI ();
		HideUI (true);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
//	private UISprite sprite;
//	private TweenAlpha tweenAlpha;

	private UILabel[] label = new UILabel[3];// 0=top, 1=bottom, 2=center
	private TweenAlpha[] tweenAlpha = new TweenAlpha[3];

//	private UILabel topLabel;
//	private UILabel bottomLabel;
//	private UILabel centerLabel;

	private Vector3 initLocalPosition = Vector3.zero;
	private Vector3 initLocalScale = Vector3.zero;
	private Callback callBack;

	void HideUI(bool b) {
		if (b) {
//			sprite.spriteName = string.Empty;
			foreach (var item in label) {
				item.text = "";
			}

			transform.localPosition = initLocalPosition;
			transform.localScale = initLocalScale;
		} 
	}
	float tempTime = 0f;
	public void ShowTexture(string name,Callback cb,float time = 0f) {
		ShowUI ();
		tempTime = time;
//		sprite.spriteName = name;

		string[] splitName = name.Split('|');

		if (splitName.Length == 2) {
			label [0].text = splitName [0];
			label [1].text = splitName [1];
		} else if (splitName.Length == 1) {
			label[2].text = name;	
		}

		callBack = cb;
		PlayAnimation (name);
	}

	void PlayAnimation (string name) {
		if (name == BossAppears) {
				PlayAppear ();
		} else if (name == ReadyMove) {
			PlayReadyMove ();
		} else {
			if(name == BackAttack || name == FirstAttack) {
				tempTime = 0.2f;
				transform.localPosition += new Vector3(0f, 100f, 0f);
			}
			else{
				transform.localPosition = initLocalPosition;
			}
			PlayAll ();
		}
	}

	void PlayReadyMove() {
		transform.localPosition = initLocalPosition;
//		tweenAlpha.enabled = true;
//		tweenAlpha.ResetToBeginning ();
		ActiveTweenAlpha ();
		AudioManager.Instance.PlayAudio (AudioEnum.sound_quest_ready);

		iTween.ScaleFrom (gameObject, iTween.Hash ("scale", new Vector3 (3f, 3f, 3f), "time", tempTime, "easetype", iTween.EaseType.easeOutCubic, "oncomplete", "PlayEnd", "oncompletetarget", gameObject));
	}

	void PlayAll () {
//		tweenAlpha.enabled = true;
//		tweenAlpha.ResetToBeginning ();

		ActiveTweenAlpha ();
		iTween.ScaleFrom (gameObject, iTween.Hash ("scale", new Vector3(3f,3f,3f), "time", tempTime == 0f ? 0.4f : tempTime, "easetype", iTween.EaseType.easeOutCubic, "oncomplete", "PlayEnd", "oncompletetarget", gameObject));

	}

	void ActiveTweenAlpha() {
		foreach (var item in tweenAlpha) {
			item.enabled = true;
			item.ResetToBeginning();
		}
	}

	void PlayEnd () {
		GameTimer.GetInstance ().AddCountDown (0.8f, End);
	}

	void End() {
		HideUI ();
		if (callBack != null) {
			callBack();
		}
	}

	//---------------------------------------------appear-----------------------------------------------------
	private Vector3 position = Vector3.zero;
	private Vector3 startPosition = Vector3.zero;

	void PlayAppear () {
		float xOffset = -Screen.width;
		startPosition = new Vector3 (initLocalPosition.x + xOffset, initLocalPosition.y, initLocalPosition.z);
		transform.localPosition = startPosition;
		transform.localScale = new Vector3 (1f, 0.1f, 1f);

		StartMoveUI ("BossAppearAnim");
	}

	void StartMoveUI (string func) {
		iTween.MoveTo(gameObject,iTween.Hash("position",initLocalPosition,"time",0.2f,"islocal",true,"easetype",iTween.EaseType.easeInCubic,"oncomplete", func,"oncompletetarget", gameObject));
	}

	void BossAppearAnim() {
		iTween.ScaleTo (gameObject, iTween.Hash ("y", 1f, "time", 0.3f, "easetype", iTween.EaseType.easeInCubic, "oncomplete", "PlayEnd", "oncompletetarget", gameObject));
	}

	//---------------------------------------------appear-----------------------------------------------------
	public const string GameOver = "Game Over"; //"GAME-OVER-";
	public const string BossAppears = "BOSS|APPEARS"; //"boss-APPEARS";
	public const string OpenGate = "GO TO THE|OPEN GATE !"; //"go-to-the-OPENED-GATE";
	public const string BossBattle = "TAP TO|BOSS BATTLE !"; //"tap-to-boss-battle!";
	public const string CheckOut = "TAP TO|CHECK OUT !"; //"tap-to-Check-Out-!";
	public const string SPLimit = "SP LIMIT OVER !"; //"SP-LIMIT-OVER!-";
	public const string RankUp = "RANK UP"; //"rank-up";
	public const string ReadyMove = "READY TO|MOVE ON !!"; //"Ready-to-move-on";
	public const string QuestClear = "QUEST CLEAR !"; //"Quest--Clear!";
	public const string FirstAttack = "FIRST ATTACK"; //"FIRST-ATTACK-";
	public const string BackAttack = "BACK ATTACK"; //"BACK-ATTACK-";
	public const string standReady = "STADN READY"; //"stand-ready";
}
