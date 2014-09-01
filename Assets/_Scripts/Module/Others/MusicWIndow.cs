using UnityEngine;
using System.Collections;
using System;

public class MusicWIndow : ViewBase {
	
//	UIButton bgmOnBtn;
//	UIButton bgmOffBtn;
//	UISprite maskOn;
//	UISprite maskOff;

	UISlider soundSlider;
	UISlider bgmSlider;
	GameObject okBtn;

	public override void Init ( UIInsConfig config ){
		FindUIElement();
		//		SetOption();
		base.Init (config);

		SetMusicPanel ();
	}
	
	public override void ShowUI(){
		base.ShowUI ();
		okBtn.SetActive(true);
//		MsgCenter.Instance.Invoke (CommandEnum.SetBlocker,new BlockerMaskParams(BlockerReason.MessageWindow,true));
	}
	
	public override void HideUI(){
		base.HideUI ();
		okBtn.SetActive(false);
//		MsgCenter.Instance.Invoke (CommandEnum.SetBlocker,new BlockerMaskParams(BlockerReason.MessageWindow,false));
	}
	
	public override void DestoryUI(){
		UIEventListenerCustom.Get( soundSlider.gameObject ).onPress -= PressSoundBtn;
		UIEventListenerCustom.Get( bgmSlider.gameObject ).onPress -= PressBgmBtn;
		UIEventListenerCustom.Get (okBtn).onClick -= ClickOk;

		base.DestoryUI ();
	}

	void FindUIElement(){
		FindChild< UILabel > ("BGM/Label").text = TextCenter.GetText ("Text_BGM");
		FindChild< UILabel > ("Sound/Label").text = TextCenter.GetText ("Text_Sound");

		FindChild< UILabel > ("Title").text = TextCenter.GetText ("Game_Setting_Option_Music");
		FindChild< UILabel > ("OkBtn/Label").text = TextCenter.GetText ("OK");

		soundSlider = FindChild<UISlider> ("Sound/Slider");
		bgmSlider = FindChild<UISlider> ("BGM/Slider");

		okBtn = FindChild("OkBtn");
	}

	void SetMusicPanel() {
		UIEventListenerCustom.Get (okBtn).onClick += ClickOk;

		UIEventListenerCustom.Get( soundSlider.gameObject ).onPress += PressSoundBtn;
		UIEventListenerCustom.Get( bgmSlider.gameObject ).onPress += PressBgmBtn;
		soundSlider.value = GameDataStore.Instance.GetIntDataNoEncypt ("sound");
		bgmSlider.value = GameDataStore.Instance.GetIntDataNoEncypt ("bgm");
	}

	void ClickOk(GameObject obj){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );

		UIManager.Instance.ChangeScene (ModuleEnum.Others);
//		HideUI ();
	}

	void PressBgmBtn( GameObject btn,bool state ) {
		if (state == false) {
			if(bgmSlider.value < 0.5){
				StartCoroutine(SetValue(bgmSlider,0));
			}else{
				StartCoroutine(SetValue(bgmSlider,1));
			}
		}
	}

	void PressSoundBtn(GameObject btn,bool state){
		if (state == false) {
			if(soundSlider.value < 0.5){
				StartCoroutine(SetValue(soundSlider,0));
			}else{
				StartCoroutine(SetValue(soundSlider,1));
			}
		}
	}

	IEnumerator SetValue(UISlider slider, float value ){
		yield return 0; 
		slider.value = value;
		if (slider.Equals(soundSlider)) {
			AudioManager.Instance.CloseSound (value == 1 ? true : false);
			GameDataStore.Instance.StoreIntDatNoEncypt("sound",(int)value);
		} else{
			AudioManager.Instance.StopBackgroundMusic (value == 1 ? true : false);
			GameDataStore.Instance.StoreIntDatNoEncypt("bgm",(int)value);
			AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_home);
		}
	}
}
