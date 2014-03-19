using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class EvolveFriendUnity : LevelUpFriendWindow {
	private Dictionary<GameObject,TFriendInfo> evolveFridn = new Dictionary<GameObject, TFriendInfo> ();
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);

		levelLabel = transform.Find ("Info_Panel/VauleLabel/" + 0).GetComponent<UILabel>();
		raceLabel = transform.Find ("Info_Panel/VauleLabel/" + 1).GetComponent<UILabel>();
		typeLabel = transform.Find ("Info_Panel/VauleLabel/" + 2).GetComponent<UILabel>();
	}

	public override void ShowUI () {
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.EvolveFriend, EvolveFriend);
		MsgCenter.Instance.AddListener (CommandEnum.UnitDisplayState, UnitDisplayState);
	}

	public override void HideUI () {
		base.HideUI ();
		ClearData ();
		MsgCenter.Instance.RemoveListener (CommandEnum.EvolveFriend, EvolveFriend);
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitDisplayState, UnitDisplayState);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void Callback (object data) {
		base.Callback (data);
	}

	private UILabel levelLabel;
	private UILabel raceLabel;
	private UILabel typeLabel;

	void EvolveFriend (object data) {

		if (!gameObject.activeSelf) {
			gameObject.SetActive (true);	
		}

		TUserUnit tuu = data as TUserUnit;
		ShieldSelect (tuu);

	}

	void ClearData () {
		evolveFridn.Clear ();
		foreach (var item in evolveFridn) {
			Shield(item.Key, false);
		}
	}

	
	void UnitDisplayState (object data) {
		int es = (int)data;
		if (es != 5) {
			ClearData ();
			if (gameObject.activeSelf) {
				gameObject.SetActive (false);	
			}
		}
	}

	public void ShieldSelect (TUserUnit baseItem) {
		if (baseItem == null) {
			return;	
		}
		HelperRequire hr = baseItem.UnitInfo.evolveInfo.helperRequire;
		int level = hr.level;
		EUnitRace race = hr.race;
		EUnitType type = hr.type;
		if (level == 0) {
			levelLabel.text = "-";	
		} else {
			levelLabel.text = level.ToString();
		}

		if (race == EUnitRace.ALL) {
			raceLabel.text = "-";
		} else {
			raceLabel.text = race.ToString();
		}

		if (type == EUnitType.UALL) {
			raceLabel.text = "-";
		} else {
			raceLabel.text = type.ToString();
		}

		foreach (var item in friendUnitInfoDic) {
			TUserUnit tuu = item.Value.UserUnit;
			bool b1 = level <= tuu.Level;
			bool b2 = race == EUnitRace.ALL || race == tuu.UnitInfo.Race;
			bool b3 = type == EUnitType.UALL || type == tuu.UnitInfo.Type;
			if(b1 && b2 && b3) {
				evolveFridn.Add(item.Key,item.Value);
				Shield(item.Key,false);
			}
			else{
				Shield(item.Key,true);
			}
		}
	}

	void Shield (GameObject go, bool b) {
		UISprite sprite = go.transform.Find("Mask").GetComponent<UISprite>();
		sprite.enabled = b;
	}

	protected override void ClickFriendItem (GameObject item) {
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		TFriendInfo tfi;
		if (evolveFridn.TryGetValue (item, out tfi)) {
			MsgCenter.Instance.Invoke(CommandEnum.PickFriendUnitInfo, tfi.UserUnit);
		}

	}
}
