using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleCardArea : ViewBase {
	private UISprite backTexture;
	[HideInInspector]
	public BattleCardAreaItem[] battleCardAreaItem;
	private UILabel stateLabel;
	private Vector3 sourcePosition;
	private const int oneWordSize = 42;

	private Battle battle;
	public Battle BQuest {
		set{ battle = value; }
	}
	private Dictionary<int,List<CardItem>> battleAttack = new Dictionary<int, List<CardItem>>();
	private static List<GameObject> battleCardIns = new List<GameObject>();
	private GameObject cardItem;
	public static Vector3 startPosition;
	public static Vector3 middlePosition;
	public static Vector3 endPosition;
	public static Vector3 activeSkillStartPosition;
	public static Vector3 activeSkillEndPosition;

	public override void Init (UIConfigItem config)
	{
		base.Init (config);
		backTexture = FindChild<UISprite>("Back"); 
		backTexture.gameObject.SetActive(false);
		stateLabel = FindChild<UILabel>("StateLabel");
		stateLabel.text = string.Empty;
		if (cardItem == null) {
			ResourceManager.Instance.LoadLocalAsset ("Prefabs/" + Config.battleCardName,o=>{
				GameObject go = o as GameObject;
				cardItem = go.transform.Find("Texture").gameObject;
			});
		}
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		for (int i = 0; i < battleCardAreaItem.Length; i++)  {
			battleCardAreaItem[i].ShowUI();
		}
		MsgCenter.Instance.AddListener (CommandEnum.StateInfo, StateInfo);
		MsgCenter.Instance.AddListener (CommandEnum.ExcuteActiveSkill, RecoverStateInfo);
	}

	public override void HideUI () {
		base.HideUI ();
		for (int i = 0; i < battleCardAreaItem.Length; i++)  {
			battleCardAreaItem[i].HideUI();
		}
		gameObject.SetActive (false);

		MsgCenter.Instance.RemoveListener (CommandEnum.StateInfo, StateInfo);
		MsgCenter.Instance.RemoveListener (CommandEnum.ExcuteActiveSkill, RecoverStateInfo);
	}

	Vector3 HidePosition;
	Vector3 showPosition;

	string prevInfo = "";

	int boostIndex = -1;

	int maxBoostRandom = 0;

	void SetBoost () {
		maxBoostRandom = NoviceGuideStepEntityManager.isInNoviceGuide() ? 5 : 10;

		if (boostIndex > -1 && boostIndex < 5) {
			battleCardAreaItem[boostIndex].isBoost = false;
		}

		boostIndex = Random.Range (0, maxBoostRandom);
		if(boostIndex < 5) 
			battleCardAreaItem[boostIndex].isBoost = true;
	}

	void StateInfo(object data) {
		string info = (string)data;
		if (string.IsNullOrEmpty (info) && !string.IsNullOrEmpty(stateLabel.text)) {
			HideStateLabel(string.Empty);
			return;
		}			

		if (info == DGTools.stateInfo [0]) {
			SetBoost();
		}

		if (stateLabel.text == info) {
			return;	
		}

		if (info == DGTools.stateInfo [4]) {
			prevInfo = stateLabel.text;
		}

		Color32[] colors;

		if (info == DGTools.stateInfo [0] || info == DGTools.stateInfo [1]) {
			colors = QuestFullScreenTips.thirdGroupColor;		
		} else {
			colors = QuestFullScreenTips.secondGroupColor;
		}

		QuestFullScreenTips.SetLabelGradient (stateLabel, colors);

		if (stateLabel.text == string.Empty) {
			stateLabel.transform.localPosition = HidePosition;	
			ShowStateLabel ();
		} else {
			HideStateLabel("ShowStateLabel");
		}

		stateLabel.text = info;
	}

	void RecoverStateInfo(object data) {
		bool b = (bool)data;
		if (!b) {
			StateInfo (prevInfo);
			prevInfo = "";
		}
	}

	void HideStateLabel (string nextFunction) {
		float x = showPosition.x - stateLabel.width;
		iTween.MoveTo(stateLabel.gameObject, iTween.Hash("x", x, "islocal", true,"time",0.15f,"easetype",iTween.EaseType.easeOutCubic,"oncompletetarget",gameObject,"oncomplete","ClearTexture"));
	}

	void ClearTexture () {
		stateLabel.text = string.Empty;
	}

	void ShowStateLabel () {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_text_appear);

		iTween.MoveTo (stateLabel.gameObject, iTween.Hash ("position", showPosition, "islocal", true, "time", 0.15f));
	}	       

	public void CreatArea(Vector3[] position,int height) {
		if (position == null)
				return;
		battleCardAreaItem = new BattleCardAreaItem[position.Length];
		float xOffset = backTexture.width * -0.5f;
		float yOffset = backTexture.height * 1.7f;
		stateLabel.transform.localPosition = position [0] + new Vector3 (xOffset, yOffset, 0f);
		showPosition = stateLabel.transform.localPosition;
		HidePosition = stateLabel.transform.localPosition + Vector3.right * -(stateLabel.mainTexture.width + Screen.width * 0.5f);
		stateLabel.enabled = true;
		int length = position.Length;

		GameObject tempObject = null;

		for (int i = 0; i < length; i++) {
				tempObject = NGUITools.AddChild (gameObject, backTexture.gameObject);
				tempObject.SetActive (true);
				tempObject.layer = GameLayer.BattleCard;
				tempObject.transform.localPosition = new Vector3 (position [i].x + 5f, position [i].y + 3f + height, position [i].z);
				BattleCardAreaItem bca = tempObject.AddComponent<BattleCardAreaItem> ();
//				bca.Init (tempObject.name);
				bca.AreaItemID = i;
				battleCardAreaItem [i] = bca;
		}

		BattleCardAreaItem bcai = battleCardAreaItem [length - 1];
		//normal skill is from right top to left bottom.
		Vector3 pos = bcai.transform.localPosition;		// get last area item position.

		startPosition = new Vector3 (pos.x + height, pos.y - height * 0.5f, pos.z); //normal skill start position.

		middlePosition = battleCardAreaItem [2].transform.localPosition;

		pos = battleCardAreaItem [0].transform.localPosition;	// get first area item position.

		endPosition = new Vector3 (pos.x - height * 0.5f, pos.y - height * 1.5f, pos.z);	//normal skill end position.

		//active skill is from left top to right top. normal skill start position is active skill end position.
		activeSkillStartPosition = new Vector3 (pos.x - height * 0.5f - 640f, pos.y - height * 0.5f, pos.z);	//active skill from position.

		Vector3 actveiPosition = battleCardAreaItem[2].transform.localPosition;

		activeSkillEndPosition = new Vector3 (actveiPosition.x , startPosition.y, startPosition.z);
	}
	
	static int count = 0;
	public static GameObject GetCard() {
		if (count == battleCardIns.Count) {
			count = 0;
		}
		GameObject go = battleCardIns [count];
		count ++;
		return go;
	}
	
	bool showCountDown = false;
	int time = 0;
	public void ShowCountDown (bool isShow,int time) {
		showCountDown = isShow;
		this.time = time;
	}
}
