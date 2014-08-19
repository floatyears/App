using UnityEngine;
using System.Collections;

public class MapDoor : UIBaseUnity {
	public BattleMap battleMap;

	private UILabel topLabel;
	private UILabel bottomLabel;

	private TweenAlpha topAlpha;
	private TweenAlpha bottomAlpha;

	private bool _doorOpen = false;

	[HideInInspector]
	public bool doorOpen {
		get {return _doorOpen; }
		set { _doorOpen = value;  ShowTapToBattle(); }
	}

	[HideInInspector]
	public bool canEnterDoor = false;
	private bool checkOut = false;

	private string currentShowInfo;

	public override void Init (string name) {
		base.Init (name);
		topLabel = FindChild<UILabel> ("Top");
		bottomLabel = FindChild<UILabel> ("Bottom");
		topAlpha = topLabel.GetComponent<TweenAlpha>();
		bottomAlpha = bottomLabel.GetComponent<TweenAlpha>();

		UIEventListener.Get (gameObject).onClick = ClickDoor;
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
		doorOpen = ConfigBattleUseData.Instance.storeBattleData.HitKey;
		MsgCenter.Instance.AddListener (CommandEnum.OpenDoor, OpenDoor);
		MsgCenter.Instance.AddListener (CommandEnum.QuestEnd, QuestEnd);
	}

	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.OpenDoor, OpenDoor);
		MsgCenter.Instance.RemoveListener (CommandEnum.QuestEnd, QuestEnd);
		doorOpen = false;
		canEnterDoor = false;
		checkOut = false;
	
//		SetName (QuestFullScreenTips.BossBattle);
	}

	void SetName(string name) {
		currentShowInfo = name;

		string[] info = currentShowInfo.Split('|');
		topLabel.text = info [0];
		bottomLabel.text = info [1];
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void SetPosition(Vector3 pos) {
		transform.localPosition = pos;
	}

	void OpenDoor (object data) {
		doorOpen = true;
	}

	void QuestEnd(object data) {
		Debug.LogError ("QuestEnd : " + data);
		canEnterDoor = (bool)data;
	}

	public void ShowTapToBattle () {
		topLabel.enabled = doorOpen;	
		bottomLabel.enabled = doorOpen;
		topAlpha.enabled = doorOpen;
		bottomAlpha.enabled = doorOpen;
		SetName (QuestFullScreenTips.BossBattle);
	}
	
	void ClickDoor(GameObject go) {
		Debug.LogError ("topLabel.enabled : " + topLabel.enabled + " content equal : " + (currentShowInfo == QuestFullScreenTips.BossBattle) + " canEnterDoor : " + canEnterDoor);
		if (!topLabel.enabled) {
			return;	
		}

		if (currentShowInfo == QuestFullScreenTips.BossBattle && topLabel.enabled) {
			if(!canEnterDoor) {
				return;
			}
			battleMap.bQuest.ClickDoor();
			topLabel.enabled = false;
			bottomLabel.enabled = false;
			topAlpha.enabled = false;
			bottomAlpha.enabled = false;
			return;
		}

		if (currentShowInfo == QuestFullScreenTips.CheckOut && checkOut) {
			checkOut = false;
			topLabel.enabled = checkOut;	
			topAlpha.enabled = checkOut;
			topLabel.enabled = checkOut;	
			topAlpha.enabled = checkOut;
			battleMap.bQuest.CheckOut();
		}
	}

	public void ShowTapToCheckOut () {
		topLabel.enabled = true;
		topAlpha.enabled = true;
		bottomLabel.enabled = true;
		bottomAlpha.enabled = true;

		SetName (QuestFullScreenTips.CheckOut);
		checkOut = true;
	}


}
