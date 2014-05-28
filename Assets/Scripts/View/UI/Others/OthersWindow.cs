using UnityEngine;
using System.Collections.Generic;

public class OthersWindow : UIComponentUnity {

	GameObject scrollerItem;
	DragPanel othersScroller;
	UILabel titleLabel;

	GameObject nickNamePanel;
	UIButton okButton;
	UIInput nickNameInput;
	
	GameObject musicPanel;
	UIButton bgmOnBtn;
	UIButton bgmOffBtn;
	UISprite maskOn;
	UISprite maskOff;

    GameObject resetOption;

	Dictionary< GameObject, GameObject > options = new Dictionary< GameObject, GameObject>();

	public override void Init ( UIInsConfig config, IUICallback origin ){
		FindUIElement();
		SetOption();
		base.Init (config, origin);
	}
	
	public override void ShowUI(){
		base.ShowUI ();
		SetUIElement();
	}
	
	public override void HideUI(){
		base.HideUI ();
		ResetUIElement();
	}
	
	public override void DestoryUI(){
		base.DestoryUI ();
	}
	
	void SetOption() {
		string itemPath = "Prefabs/UI/Others/OtherOptions";
		GameObject item = Resources.Load( itemPath ) as GameObject;
		
		othersScroller = new DragPanel ( "OthersScroller", scrollerItem );
		othersScroller.CreatUI ();

		GameObject musicOption = othersScroller.AddScrollerItem( item );
		musicOption.name = "MusicOption";
		musicOption.GetComponentInChildren<UILabel>().text = "Music";
		options.Add( musicOption, musicPanel );

		GameObject nickNameOption = othersScroller.AddScrollerItem( item );
		nickNameOption.name = "NickNameOption";
		nickNameOption.GetComponentInChildren<UILabel>().text = "NickName";
		options.Add( nickNameOption, nickNamePanel );

        resetOption = othersScroller.AddScrollerItem( item );
        resetOption.name = "ResetOption";
        resetOption.GetComponentInChildren<UILabel>().text = "Reset Data";
        options.Add( resetOption, nickNamePanel );

		Transform parentTrans = FindChild("OptionItems").transform;
		othersScroller.DragPanelView.SetScrollView(ConfigDragPanel.OthersDragPanelArgs, parentTrans);
		
		for(int i = 0; i < othersScroller.ScrollItem.Count; i++)
			UIEventListener.Get( othersScroller.ScrollItem[ i ].gameObject ).onClick = ClickOption;
	}


	void ClickOption( GameObject go) {
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		SwicthOption( go );
	}

	void SwicthOption( GameObject target ){
		foreach (var item in options)
			item.Value.SetActive( false );
		options[ target ].SetActive( true );

        ///
//        LogHelper.Log("SwicthOption(), target {0} resetOption {1}", target, resetOption);

        if (target == resetOption){
            LogHelper.Log("SwicthOption(), target {0}", resetOption);
            ClickToResetData();
        }
	}
	
    void ClickToResetData(){
        GameDataStore.Instance.StoreData(GameDataStore.UUID, "");
        GameDataStore.Instance.StoreData(GameDataStore.USER_ID, 0);
        UIManager.Instance.ChangeScene(SceneEnum.Loading);
    }

	void FindUIElement() {
		FindMusicPanel();
		FindNickNamePanel();
	}

	void FindMusicPanel(){
		string rootPath;
		rootPath =  "InfoPanel/";
		musicPanel = FindChild( rootPath + "MusicPanel" );

		titleLabel = FindChild< UILabel >( rootPath + "Label_Title");

		rootPath = "InfoPanel/MusicPanel/";
		bgmOnBtn = FindChild< UIButton >(rootPath + "BGM/On" );
        bgmOffBtn = FindChild< UIButton >(rootPath + "BGM/Off" );

		rootPath = "InfoPanel/MusicPanel/BGM/";
		maskOn = FindChild< UISprite >( rootPath + "On/Mask");
		maskOff = FindChild< UISprite >( rootPath + "Off/Mask");

	}

	void FindNickNamePanel(){
		string rootPath;
		rootPath =  "InfoPanel/";
		nickNamePanel = FindChild( rootPath + "NickNamePanel" );

		rootPath = "InfoPanel/NickNamePanel/";
		okButton = FindChild< UIButton >( rootPath + "OKButton" );
		nickNameInput = FindChild< UIInput >( rootPath + "NickNameInput" );
        }


	void SetUIElement(){
		SetMusicPanel();
		SetNickNamePanel();
		SwicthOption( othersScroller.ScrollItem[ 1 ] );
		ShowTween();
	}

	void SetMusicPanel() {
        maskOn.enabled = false;
        maskOff.enabled = true;
		UIEventListener.Get( bgmOnBtn.gameObject ).onClick = ClickBgmBtn;
		UIEventListener.Get( bgmOffBtn.gameObject ).onClick = ClickBgmBtn;
    }

	void SetNickNamePanel(){
		UIEventListener.Get( okButton.gameObject ).onClick = ClickOkButton;
	}
        
	void ClickOkButton( GameObject go ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        LogHelper.Log("ClickOkButton to rename");
		MsgCenter.Instance.Invoke( CommandEnum.ReqRenameNick, nickNameInput.value );
	}

    void ResetUIElement(){
        nickNameInput.label.text = string.Empty;
    }
        
    void SetUIActive(bool active) {

		othersScroller.DragPanelView.gameObject.SetActive( active );
		musicPanel.SetActive( active );
		nickNamePanel.SetActive( active );
    }

	void ShowTween() {
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if( list == null )
			return;
		foreach( var tweenPos in list) {		
			if( tweenPos == null )
                                continue;
                        tweenPos.Reset();
                        tweenPos.PlayForward();
                }
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

	void ClickNameChangeButton( GameObject go ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		Debug.Log( "OthersWindow ClickNameChangeButton() : Start");
		RequestNameChange( GetInputText() );
		Debug.Log( "OthersWindow ClickNameChangeButton() : End");
	}

	string GetInputText(){
		return nickNameInput.label.text;
	}

	void RequestNameChange(string name){
		Debug.Log( "OthersWindow RequestNameChange() : Start");
		Debug.Log( "OthersWindow RequestNameChange() : End");
	}

}
