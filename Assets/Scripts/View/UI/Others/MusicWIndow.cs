using UnityEngine;
using System.Collections;
using System;

public class MusicWIndow : UIComponentUnity {
	
//	UIButton bgmOnBtn;
//	UIButton bgmOffBtn;
//	UISprite maskOn;
//	UISprite maskOff;

	UISlider soundSlider;
	UISlider bgmSlider;
	GameObject okBtn;

	public override void Init ( UIInsConfig config, IUICallback origin ){
		FindUIElement();
		//		SetOption();
		base.Init (config, origin);

		SetMusicPanel ();
	}
	
	public override void ShowUI(){
		base.ShowUI ();
		//		SetUIElement();
		okBtn.SetActive(true);

		MsgCenter.Instance.Invoke (CommandEnum.SetBlocker,new BlockerMaskParams(BlockerReason.MessageWindow,true));
	}
	
	public override void HideUI(){
		base.HideUI ();
		//		ResetUIElement();
		okBtn.SetActive(false);
		MsgCenter.Instance.Invoke (CommandEnum.SetBlocker,new BlockerMaskParams(BlockerReason.MessageWindow,false));

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
//		bgmOnBtn = FindChild< UIButton >("BGM/On" );
//		bgmOffBtn = FindChild< UIButton >("BGM/Off" );
//
//		maskOn = FindChild< UISprite >("BGM/On/Mask");
//		maskOff = FindChild< UISprite >("BGM/Off/Mask");

		soundSlider = FindChild<UISlider> ("Sound/Slider");
		bgmSlider = FindChild<UISlider> ("BGM/Slider");

		okBtn = FindChild("OkBtn");
		
	}

	void SetMusicPanel() {
//		maskOn.enabled = false;
//		maskOff.enabled = true;
		UIEventListenerCustom.Get (okBtn).onClick += ClickOk;

		UIEventListenerCustom.Get( soundSlider.gameObject ).onPress += PressSoundBtn;
		UIEventListenerCustom.Get( bgmSlider.gameObject ).onPress += PressBgmBtn;
		soundSlider.value = GameDataStore.Instance.GetIntDataNoEncypt ("sound");
		bgmSlider.value = GameDataStore.Instance.GetIntDataNoEncypt ("bgm");
	}
//
	void ClickOk(GameObject obj){
		UIManager.Instance.ChangeScene (SceneEnum.Others);
	}

	void PressBgmBtn( GameObject btn,bool state ){
//		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		if (state == false) {
			if(bgmSlider.value < 0.5){
				StartCoroutine(SetValue(bgmSlider,0));
			}else{
				StartCoroutine(SetValue(bgmSlider,1));
			}
		}
//		if( btn.name == "On") {
//			//			AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_home);
//			AudioManager.Instance.CloseBackground(false);
//			maskOn.enabled = false;
//			maskOff.enabled = true;
//		}else if(btn.name == "Off"){
//			//			AudioManager.Instance.PauseAudio(AudioEnum.music_home);
//			AudioManager.Instance.CloseBackground(true);
//			maskOn.enabled = true;
//			maskOff.enabled = false;
//		}
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
		if (slider == soundSlider) {
			AudioManager.Instance.CloseSound (value == 0 ? true : false);
			GameDataStore.Instance.StoreIntDatNoEncypt("sound",(int)value);
		}
			
		else{
			AudioManager.Instance.CloseBackground (value == 0 ? true : false);
			GameDataStore.Instance.StoreIntDatNoEncypt("bgm",(int)value);
		}
			
	}

//	public void OnSoundValueChange(){
//		if (soundSlider.value > 0.5) {
//			soundSlider.value = 1;
//		}else{
//			soundSlider.value = 0;
//		}
//	}
//
//	public void OnBGMValueChange(){
//		if (bgmSlider.value < 0.5) {
//			bgmSlider.value = 1;
//		}else{
//			bgmSlider.value = 0;
//		}
//	}

}
