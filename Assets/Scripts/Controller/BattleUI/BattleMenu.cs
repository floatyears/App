using UnityEngine;
using System.Collections;

public class BattleMenu : UIBaseUnity {
	private UIButton CloseButton;

	//==================questinfo component=======
	private UILabel areaNameLabel;
	private UILabel questNameLabel;
	private UILabel floorLabel;
	//============================================

	//==================options component=======
	private UIButton bgmOnButton;
	private UIButton bgmOffButton;
	private UIButton seOnButton;
	private UIButton seOffButton;
	private UIButton guideOnButton;
	private UIButton guideOffButton;
	//============================================

	//==================options component=======
	private UIButton exitButton;
	private UIButton cancelButton;
	//============================================

	private UIButton closeButton;
	private UIToggle defaultToggle;

	private BattleQuest _battleQuest;
	public BattleQuest battleQuest {
		get { return _battleQuest; }
		set { _battleQuest = value; }
	}

	private AudioManager audioManager;

	public override void Init (string name) {
		base.Init (name);
		audioManager = AudioManager.Instance;
	
		closeButton = FindChild<UIButton> ("Title/Button_Close");
		UIEventListener.Get (closeButton.gameObject).onClick = CancelButton;

		string Path = "Tabs/Content_QuestInfo";
		areaNameLabel = FindChild<UILabel> (Path + "/AreaNameLabel");
		questNameLabel = FindChild<UILabel>(Path + "/QuestNameLabel");
		floorLabel = FindChild<UILabel>(Path + "/FloorLabel");
		defaultToggle = FindChild<UIToggle> (Path);
		Path = "Tabs/Content_Options/";

		bgmOnButton = FindChild<UIButton> (Path + "Button_BGM_ON");
		UIEventListener.Get (bgmOnButton.gameObject).onClick = BGMOn;

		bgmOffButton = FindChild<UIButton> (Path + "Button_BGM_OFF");
		UIEventListener.Get (bgmOffButton.gameObject).onClick = BGMOff;

		seOnButton = FindChild<UIButton> (Path + "Button_SE_ON");
		UIEventListener.Get (seOnButton.gameObject).onClick = SeOnButton;

		seOffButton = FindChild<UIButton> (Path + "Button_SE_OFF");
		UIEventListener.Get (seOffButton.gameObject).onClick = SeOffButton;

		guideOnButton = FindChild<UIButton> (Path + "Button_GUIDE_ON");
		UIEventListener.Get (guideOnButton.gameObject).onClick = GUIDEOnButton;

		guideOffButton = FindChild<UIButton> (Path + "Button_GUIDE_OFF");
		UIEventListener.Get (guideOffButton.gameObject).onClick = GUIDEOffButton;

		Path = "Tabs/Content_Retire/";
		exitButton = FindChild<UIButton> (Path + "Button_OK");
		UIEventListener.Get (exitButton.gameObject).onClick = ExitButton;

		cancelButton = FindChild<UIButton> (Path + "Button_Cancel");
		UIEventListener.Get (cancelButton.gameObject).onClick = CancelButton;
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		defaultToggle.value = true;
	}

	public override void HideUI () {
		base.HideUI ();
		gameObject.SetActive (false);
	}

	void BGMOn(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		audioManager.CloseBackground (false);
	}

	void BGMOff(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		audioManager.CloseBackground (true);
	}

	void SeOnButton (GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		audioManager.CloseSound (false);
	}

	void SeOffButton(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		audioManager.CloseSound (true);
	}

	void GUIDEOnButton(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
	}

	void GUIDEOffButton(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
	}

	void ExitButton(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		_battleQuest.QuestEnd ();
	}

	void CancelButton(GameObject go) {
		audioManager.PlayAudio (AudioEnum.sound_click);
		HideUI ();
	}
}
