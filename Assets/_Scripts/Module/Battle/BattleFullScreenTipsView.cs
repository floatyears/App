using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleFullScreenTipsView : ViewBase {


	private UILabel[] label = new UILabel[3];// 0=top, 1=bottom, 2=center
	private TweenAlpha[] tweenAlpha = new TweenAlpha[3];

	private Vector3 initLocalPosition = Vector3.zero;
	private Vector3 initLocalScale = Vector3.zero;
	private Callback callBack;

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);

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
	}

	public override void CallbackView (params object[] args)
	{
		switch (args[0].ToString()) {
		case "boss":
			ShowTexture(BossAppears,MeetBoss);
			break;
		case "gate":
			ShowTexture(OpenGate,OpenGateFunc);
			break;
		case "first":
			ShowTexture(FirstAttack,AttackBackFunc);
			break;
		case "back":
			ShowTexture(BackAttack,AttackBack);
			break;
		case "readymove":
			ShowTexture(standReady,ReadyMoveFunc,(float)args[1]);
			break;
		case "clear":
			ShowTexture(QuestClear,(Callback)args[1]);
			break;
		default:
			break;
		}
	}


	void AttackBackFunc() {
		BattleUseData.Instance.ac.FirstAttack ();
	}
	
	void AttackEnd () {
		//		battle.ShieldInput(true);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
	}

	void AttackBack(){
		BattleUseData.Instance.ac.AttackPlayer ();
	}

	void MeetBoss () {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_boss_battle);
		//		AudioManager.Instance.StopBackgroundMusic (true);
		//		battle.ShieldInput ( true );
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
//		BattleMapView.waitMove = false;
		//		ShowBattle();
		List<TEnemyInfo> temp = new List<TEnemyInfo> ();
		for (int i = 0; i < ConfigBattleUseData.Instance.questDungeonData.Boss.Count; i++) {
			TEnemyInfo tei = ConfigBattleUseData.Instance.questDungeonData.Boss [i];
			tei.EnemySymbol = (uint)i;
			temp.Add (tei);
		}
		TDropUnit bossDrop = ConfigBattleUseData.Instance.questDungeonData.DropUnit.Find (a => a.DropId == 0);
		BattleUseData.Instance.InitBoss (ConfigBattleUseData.Instance.questDungeonData.Boss, bossDrop);
		
		ConfigBattleUseData.Instance.storeBattleData.isBattle = 2; // 2 == battle boss. 
		//		battle.ShowEnemy (temp, true);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);

		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
	}

	void HideUI(bool b) {
		if (b) {
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

		string[] splitName = name.Split('|');
		Color32[] colors = new Color32[2];
	
		switch (name) {
		case FirstAttack:
			colors = firstGroupColor;
			break;
		case SPLimit:
		case BackAttack:
		case GameOver:
		case BossAppears:
			colors = thirdGroupColor;
			break;
		default:
			colors = secondGroupColor;
			break;
		}

		if (splitName.Length == 2) {
			label [0].text = splitName [0];
			label [1].text = splitName [1];
			SetLabelGradient(label[0], colors);
			SetLabelGradient(label[1], colors);
		} else if (splitName.Length == 1) {
			label[2].text = name;	
			SetLabelGradient(label[2], colors);
		}

		callBack = cb;
		PlayAnimation (name);
	}

	void ReadyMoveFunc() {
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
		NoviceGuideStepEntityManager.Instance ().StartStep ( NoviceGuideStartType.BATTLE );
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
		ActiveTweenAlpha ();
		AudioManager.Instance.PlayAudio (AudioEnum.sound_quest_ready);
		iTween.ScaleFrom (gameObject, iTween.Hash ("scale", new Vector3 (3f, 3f, 3f), "time", tempTime, "easetype", iTween.EaseType.easeOutCubic, "oncomplete", "PlayEnd", "oncompletetarget", gameObject));
	}

	void PlayAll () {
		ActiveTweenAlpha ();
		iTween.ScaleFrom (gameObject, iTween.Hash ("scale", new Vector3(3f,3f,3f), "time", tempTime == 0f ? 0.4f : tempTime, "easetype", iTween.EaseType.easeOutCubic, "oncomplete", "PlayEnd", "oncompletetarget", gameObject));
	}

	void OpenGateFunc() {
		//		battle.ShieldInput (true);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
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

	//---------------------------------------------color-----------------------------------------------------
	public static Color32[] firstGroupColor = new Color32[2] { new Color32(141, 227, 246, 255), new Color32(68, 149, 208, 255)};
	public static Color32[] secondGroupColor = new Color32[2] { new Color32(238, 232, 0, 255), new Color32(251, 149, 0, 255)};
	public static Color32[] thirdGroupColor = new Color32[2] { new Color32(247, 8, 8, 255), new Color32(210, 43, 43, 255)};

	public static void SetLabelGradient(UILabel label, Color32[] colors) {
		label.gradientTop = colors [0];
		label.gradientBottom = colors [1];
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
	public const string standReady = "STAND READY"; //"stand-ready";
}
