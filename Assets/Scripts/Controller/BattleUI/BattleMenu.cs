using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleMenu : UIBaseUnity {
	public override void Init (string name) {
		base.Init (name);
		InitUIComponent ();
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void ShowUI () {
		gameObject.SetActive (true);
		BattleBottom.notClick = true;
		Main.Instance.GInput.IsCheckInput = false;
	}

	public override void HideUI () {
		CancelMenu (null);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public BattleQuest battleQuest;

	private GameObject menuWindow;
	private UISlider bgmSlider;
	private UISlider soundSlider;
	private UIButton okButton;
	private UIButton returnButton;

	private GameObject confirmWindow;
	private UIButton confirmButton;
	private UIButton cancelButton;
	private UILabel confirmTitleLabel;
	private UILabel	confirmContentLabel;
	private UILabel confirmButtonLabel;
	private UILabel cancelButtonLabel;

	private UILabel menuTitleLabel;
	private UILabel bgmLabel;
	private UILabel soundLabel;
	private UILabel okButtonLabel;
	private UILabel returnButtonLabel;

	void InitUIComponent() {
		menuWindow = FindChild<Transform> ("BattleMenuWindow").gameObject;

		bgmSlider = FindChild<UISlider> ("BattleMenuWindow/BGM/Slider");
		bgmLabel = FindChild<UILabel> ("BattleMenuWindow/BGM/Label");
		soundSlider = FindChild<UISlider> ("BattleMenuWindow/Sound/Slider");
		soundLabel = FindChild<UILabel> ("BattleMenuWindow/Sound/Label");
		okButton = FindChild<UIButton> ("BattleMenuWindow/OkBtn");
		okButtonLabel = FindChild<UILabel> ("BattleMenuWindow/OkBtn/Label");
		returnButton = FindChild<UIButton> ("BattleMenuWindow/CancleButton");
		returnButtonLabel = FindChild<UILabel> ("BattleMenuWindow/OkBtn/Label");
		menuTitleLabel = FindChild <UILabel>("BattleMenuWindow/Title");
		SetEvent ();

		confirmWindow = FindChild<Transform> ("RetireWindow").gameObject;

		confirmButton = FindChild<UIButton> ("RetireWindow/Button_Left");
		cancelButton = FindChild<UIButton> ("RetireWindow/Button_Right");
		confirmButtonLabel = FindChild<UILabel> ("RetireWindow/Button_Left/Label");
		cancelButtonLabel = FindChild<UILabel> ("RetireWindow/Button_Right/Label");
//		confirmTitleLabel = FindChild<UILabel> ("BattleMenuWindow/RetireWindow/Label_Title");
		confirmContentLabel = FindChild<UILabel> ("RetireWindow/Label_Msg_Center");

		UIEventListener.Get (confirmButton.gameObject).onClick = CancelFight;
		UIEventListener.Get (cancelButton.gameObject).onClick = CancelMenu;

		confirmButtonLabel.text = TextCenter.GetText ("OK");
		cancelButtonLabel.text = TextCenter.GetText ("CANCEL");
		returnButtonLabel.text = TextCenter.GetText ("Resume");
		okButtonLabel.text = TextCenter.GetText("Exit");
		confirmContentLabel.text = TextCenter.GetText ("ExitBattleConfirm");
		menuTitleLabel.text = TextCenter.GetText("BattleMenu");
		confirmWindow.SetActive (false);
	}

	void SetEvent() {
		UIEventListener.Get (bgmSlider.gameObject).onPress = BGMSliderPress;
		UIEventListener.Get (soundSlider.gameObject).onPress = SoundSliderPress;
		UIEventListener.Get (okButton.gameObject).onClick = OKButtonClick;
		UIEventListener.Get (returnButton.gameObject).onClick = ReturnButtonClick;
	}

	void BGMSliderPress(GameObject go, bool b) {
		if (!b) {
			StartCoroutine(SetValue(bgmSlider, bgmSlider.value > 0.5f ? 1 : 0));
		}
	}

	void SoundSliderPress(GameObject go, bool b) {
		if (!b) {
			StartCoroutine(SetValue(soundSlider, soundSlider.value > 0.5f ? 1 : 0));
		}
	}

	void ReturnButtonClick(GameObject go) {
		CancelMenu (null);
	}

	void OKButtonClick(GameObject go) {
		confirmWindow.SetActive (true);
		menuWindow.SetActive (false);
	}

	void CancelFight (GameObject data) {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		CancelMenu (null);
		Battle.isShow = false;
		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd);
		battleQuest.Retire (false);
	}

	void CancelMenu(GameObject data) {
		gameObject.SetActive (false);
		confirmWindow.SetActive (false);
		menuWindow.SetActive (true);
		Main.Instance.GInput.IsCheckInput = true;
		BattleBottom.notClick = false;
	}

	IEnumerator SetValue(UISlider slider, float value ){
		yield return 0; 
		slider.value = value;
		if (slider == soundSlider) {
			AudioManager.Instance.CloseSound (value == 1 ? true : false);
			GameDataStore.Instance.StoreIntDatNoEncypt("sound",(int)value);
		} else{
			AudioManager.Instance.CloseBackground (value == 0 ? true : false);
			GameDataStore.Instance.StoreIntDatNoEncypt("bgm",(int)value);
		}
	}
}
