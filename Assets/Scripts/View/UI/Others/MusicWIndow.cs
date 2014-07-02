using UnityEngine;
using System.Collections;

public class MusicWIndow : UIComponentUnity {
	
//	UIButton bgmOnBtn;
//	UIButton bgmOffBtn;
//	UISprite maskOn;
//	UISprite maskOff;

	UISlider soundSlider;
	UISlider bgmSlider;

	public override void Init ( UIInsConfig config, IUICallback origin ){
		FindUIElement();
		//		SetOption();
		base.Init (config, origin);

		SetMusicPanel ();
	}
	
	public override void ShowUI(){
		base.ShowUI ();
		//		SetUIElement();
	}
	
	public override void HideUI(){
		base.HideUI ();
		//		ResetUIElement();
	}
	
	public override void DestoryUI(){
		UIEventListenerCustom.Get( soundSlider.gameObject ).onPress -= PressSoundBtn;
		UIEventListenerCustom.Get( bgmSlider.gameObject ).onPress -= PressBgmBtn;

		base.DestoryUI ();
	}

	void FindUIElement(){

		FindChild< UILabel > ("BGM/Label").text = TextCenter.GetText ("Text_BGM");
		FindChild< UILabel > ("Sound/Label").text = TextCenter.GetText ("Text_Sound");

//		bgmOnBtn = FindChild< UIButton >("BGM/On" );
//		bgmOffBtn = FindChild< UIButton >("BGM/Off" );
//
//		maskOn = FindChild< UISprite >("BGM/On/Mask");
//		maskOff = FindChild< UISprite >("BGM/Off/Mask");

		soundSlider = FindChild<UISlider> ("Sound/Slider");
		bgmSlider = FindChild<UISlider> ("BGM/Slider");
		
	}

	void SetMusicPanel() {
//		maskOn.enabled = false;
//		maskOff.enabled = true;
		UIEventListenerCustom.Get( soundSlider.gameObject ).onPress += PressSoundBtn;
		UIEventListenerCustom.Get( bgmSlider.gameObject ).onPress += PressBgmBtn;
	}
//
	void PressBgmBtn( GameObject btn,bool state ){
//		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		if (state == false) {
			if(bgmSlider.value < 0.5){
				bgmSlider.value = 0;
			}else{
				bgmSlider.value = 1;
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
				soundSlider.value = 0;
			}else{
				soundSlider.value = 1;
			}
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
