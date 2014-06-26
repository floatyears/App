using UnityEngine;
using System.Collections;

public class MusicWIndow : UIComponentUnity {
	
	UIButton bgmOnBtn;
	UIButton bgmOffBtn;
	UISprite maskOn;
	UISprite maskOff;

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
		UIEventListener.Get( bgmOnBtn.gameObject ).onClick = null;
		UIEventListener.Get( bgmOffBtn.gameObject ).onClick = null;

		base.DestoryUI ();
	}

	void FindUIElement(){

		FindChild< UILabel > ("BGM/Label").text = TextCenter.GetText ("Text_BGM");
		FindChild< UILabel > ("Sound/Label").text = TextCenter.GetText ("Text_Sound");

		bgmOnBtn = FindChild< UIButton >("BGM/On" );
		bgmOffBtn = FindChild< UIButton >("BGM/Off" );

		maskOn = FindChild< UISprite >("BGM/On/Mask");
		maskOff = FindChild< UISprite >("BGM/Off/Mask");
		
	}

	void SetMusicPanel() {
		maskOn.enabled = false;
		maskOff.enabled = true;
		UIEventListener.Get( bgmOnBtn.gameObject ).onClick = ClickBgmBtn;
		UIEventListener.Get( bgmOffBtn.gameObject ).onClick = ClickBgmBtn;
	}

	void ClickBgmBtn( GameObject btn ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		
		if( btn.name == "On") {
			//			AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_home);
			AudioManager.Instance.CloseBackground(false);
			maskOn.enabled = false;
			maskOff.enabled = true;
		}else if(btn.name == "Off"){
			//			AudioManager.Instance.PauseAudio(AudioEnum.music_home);
			AudioManager.Instance.CloseBackground(true);
			maskOn.enabled = true;
			maskOff.enabled = false;
		}
	}

}
