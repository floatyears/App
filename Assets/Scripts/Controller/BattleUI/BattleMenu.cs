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
		base.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public BattleQuest battleQuest;

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
		bgmSlider = FindChild<UISlider> ("BattleMenuWindow/BGM/Slider");
		bgmLabel = FindChild<UILabel> ("BattleMenuWindow/BGM/Label");
		soundSlider = FindChild<UISlider> ("BattleMenuWindow/Sound/Slider");
		soundLabel = FindChild<UILabel> ("BattleMenuWindow/Sound/Label");
		okButton = FindChild<UIButton> ("BattleMenuWindow/OkBtn");
		okButtonLabel = FindChild<UILabel> ("BattleMenuWindow/OkBtn/Label");
		returnButton = FindChild<UIButton> ("BattleMenuWindow/CancleButton");
		returnButtonLabel = FindChild<UILabel> ("BattleMenuWindow/OkBtn/Label");

		Transform confirmWindowTrans = FindChild<Transform> ("BattleMenuWindow/RetireWindow");
		confirmWindow = confirmWindowTrans.gameObject;

		confirmButton = FindChild<UIButton> ("BattleMenuWindow/RetireWindow/Button_Left");
		cancelButton = FindChild<UIButton> ("BattleMenuWindow/RetireWindow/Button_Right");
		confirmButtonLabel = FindChild<UILabel> ("BattleMenuWindow/RetireWindow/Button_Left/Label");
		cancelButtonLabel = FindChild<UILabel> ("BattleMenuWindow/RetireWindow/Button_Right/Label");
		confirmTitleLabel = FindChild<UILabel> ("BattleMenuWindow/RetireWindow/Label_Title");
		confirmContentLabel = FindChild<UILabel> ("BattleMenuWindow/RetireWindow/Label_Msg_Center");

		confirmWindow.gameObject.SetActive (false);
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
		
	}

	void OKButtonClick(GameObject go) {

	}

	void CancelFight (object data) {

	}

	void CancelMenu(object data) {

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
