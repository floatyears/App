using UnityEngine;
using System.Collections;

public class MapDoor : UIBaseUnity {
	public BattleMap battleMap;
	private UISprite TapToBattle;
	private TweenAlpha tween;
	[HideInInspector]
	public bool doorOpen = false;
	[HideInInspector]
	public bool canEnterDoor = false;

	private bool isClick = false;

	public override void Init (string name) {
		base.Init (name);
		TapToBattle = FindChild<UISprite>("Sprite");
		tween = FindChild<TweenAlpha>("Sprite");
		UIEventListener.Get (gameObject).onClick = ClickDoor;
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.OpenDoor, OpenDoor);
		MsgCenter.Instance.AddListener (CommandEnum.QuestEnd, QuestEnd);
	}

	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.OpenDoor, OpenDoor);
		MsgCenter.Instance.RemoveListener (CommandEnum.QuestEnd, QuestEnd);
		doorOpen = false;
		canEnterDoor = false;
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
		bool b = (bool)data;
		canEnterDoor = b;
		ShowTapToBattle ();
	}

	public void ShowTapToBattle () {
		bool b = canEnterDoor && doorOpen;
		TapToBattle.enabled = b;	
		tween.enabled = b;
	}
	
	void ClickDoor(GameObject go) {
		if (TapToBattle.enabled && !isClick) {
			battleMap.bQuest.ClickDoor();
			Destroy(GetComponent<UIEventListener>());
			TapToBattle.enabled = isClick;	
			tween.enabled = isClick;
			isClick = true;
		}
	}
}
