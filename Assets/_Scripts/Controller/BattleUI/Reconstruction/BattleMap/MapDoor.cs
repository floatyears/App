using UnityEngine;
using System.Collections;

public class MapDoor : UIBaseUnity {
	public BattleMap battleMap;
	private UISprite TapToBattle;
	private TweenAlpha tweenA;

	private bool _doorOpen = false;
	[HideInInspector]
	public bool doorOpen {
		get {return _doorOpen; }
		set { _doorOpen = value;  ShowTapToBattle(); }
	}
	[HideInInspector]
	public bool canEnterDoor = false;
	private bool checkOut = false;

	public override void Init (string name) {
		base.Init (name);
		TapToBattle = FindChild<UISprite>("Sprite");
		tweenA = FindChild<TweenAlpha>("Sprite");
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
		TapToBattle.spriteName = QuestFullScreenTips.BossBattle;
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void SetPosition(Vector3 pos) {
		transform.localPosition = pos;
	}

	void OpenDoor (object data) {
		doorOpen = true;
//		ShowTapToBattle ();
	}

	void QuestEnd(object data) {
		canEnterDoor = (bool)data;
	}

	public void ShowTapToBattle () {
		TapToBattle.enabled = doorOpen;	
		tweenA.enabled = doorOpen;
	}
	
	void ClickDoor(GameObject go) {
		if (!TapToBattle.enabled) {
			return;	
		}

		if (TapToBattle.spriteName == QuestFullScreenTips.BossBattle && TapToBattle.enabled) {
			if(!canEnterDoor) {
				return;
			}
			battleMap.bQuest.ClickDoor();
			TapToBattle.enabled = false;	
			tweenA.enabled = false;
			return;
		}

		if (TapToBattle.spriteName == QuestFullScreenTips.CheckOut && checkOut) {
			checkOut = false;
			TapToBattle.enabled = checkOut;	
			tweenA.enabled = checkOut;
			battleMap.bQuest.CheckOut();
		}
	}

	public void ShowTapToCheckOut () {
		TapToBattle.enabled = true;
		tweenA.enabled = true;
		TapToBattle.spriteName = QuestFullScreenTips.CheckOut;
		checkOut = true;
	}
}
