using UnityEngine;
using System.Collections.Generic;

public class OthersDecoratorUnity : UIComponentUnity {

	private GameObject scrollerItem;
	private DragPanel othersScroller;

	private UILabel titleLabel;

	private GameObject musicOption;
	private UIButton bgmOnBtn;
	private UIButton bgmOffBtn;

	private Dictionary< int, GameObject > options = new Dictionary<int, GameObject>();
	private Dictionary< string, object > otherScrollerArgsDic = new Dictionary< string, object >();

	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		InitUI();
		base.Init (config, origin);
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowTween();
		SetUIActive(true);
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	void InitUI()
	{
		CreateScroller();
		InitMusic();
	}

	private void SetUIActive(bool b) {
		othersScroller.RootObject.gameObject.SetActive(b);
	}

	private void InitMusic() {

		titleLabel = FindChild< UILabel >( "InfoPanel/Label_Title");
		titleLabel.text = "Music";

		string rootPath =  "InfoPanel/Music/";
		musicOption = FindChild( rootPath );
		musicOption.SetActive(false);

		bgmOnBtn = FindChild< UIButton >(rootPath + "BGM/On" );
		bgmOffBtn = FindChild< UIButton >(rootPath + "BGM/Off" );

		UIEventListener.Get( bgmOnBtn.gameObject ).onClick = ClickBgmBtn;
		UIEventListener.Get( bgmOffBtn.gameObject ).onClick = ClickBgmBtn;

		string path = "InfoPanel/Music/BGM/";
		UISprite maskOn = FindChild< UISprite >( path + "On/Mask");
		UISprite maskOff = FindChild< UISprite >( path + "Off/Mask");
		//AudioManager.Instance.OnMusic();
		maskOn.enabled = false;
		maskOff.enabled = true;
	}

	private void ClickBgmBtn( GameObject btn ) {
		string path = "InfoPanel/Music/BGM/";
		UISprite maskOn = FindChild< UISprite >( path + "On/Mask");
		UISprite maskOff = FindChild< UISprite >( path + "Off/Mask");

		if( btn.name == "On") {
			AudioManager.Instance.PlayAudio(AudioEnum.music_home);
			maskOn.enabled = false;
			maskOff.enabled = true;
		}else if(btn.name == "Off"){
			AudioManager.Instance.PauseAudio(AudioEnum.music_home);
			maskOn.enabled = true;
			maskOff.enabled = false;
		}
	}


	private void CreateScroller() {

		string itemPath = "Prefabs/UI/Others/OtherOptions";
		GameObject item = Resources.Load( itemPath ) as GameObject;
		
		othersScroller = new DragPanel ( "OthersScroller", scrollerItem );
		othersScroller.CreatUI ();
		InitOtherScrollArgs();

		GameObject musicOption = othersScroller.AddScrollerItem(item);
		musicOption.name = UIConfig.otherMusicSettingIndex.ToString();
		musicOption.GetComponentInChildren<UILabel>().text = UIConfig.otherMusicSettingName;
		options.Add( UIConfig.otherMusicSettingIndex, musicOption );

		othersScroller.RootObject.SetScrollView( otherScrollerArgsDic );
		
		for(int i = 0; i < othersScroller.ScrollItem.Count; i++)
			UIEventListener.Get( othersScroller.ScrollItem[ i ].gameObject ).onClick = GetOptions;
	}

	private void ShowTween() {
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if( list == null )
			return;
		foreach( var tweenPos in list) {		
			if( tweenPos == null )
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

	private void GetOptions( GameObject go) {
		if( go.name == "1")
		{
			musicOption.SetActive( true );
		}
	}

	private void InitOtherScrollArgs() {
		Transform parentTrans = FindChild("OptionItems").transform;
		otherScrollerArgsDic.Add( "parentTrans", 			parentTrans					);
		otherScrollerArgsDic.Add( "scrollerScale", 			Vector3.one					);
		otherScrollerArgsDic.Add( "scrollerLocalPos" ,		-190*Vector3.up					);
		otherScrollerArgsDic.Add( "position", 				Vector3.zero 					);
		otherScrollerArgsDic.Add( "clipRange", 				new Vector4( 0, 0, 640, 200)			);
		otherScrollerArgsDic.Add( "gridArrange", 			UIGrid.Arrangement.Horizontal 		);
		otherScrollerArgsDic.Add( "maxPerLine", 			0 							);
		otherScrollerArgsDic.Add( "scrollBarPosition", 		new Vector3(-320,-120,0)			);
		otherScrollerArgsDic.Add( "cellWidth", 				150 							);
		otherScrollerArgsDic.Add( "cellHeight",				130 							);
	}
}
