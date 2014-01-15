using UnityEngine;
using System.Collections.Generic;

public class FriendDecoratorUnity : UIComponentUnity {

	private Dictionary< GameObject, SceneEnum > btns = new Dictionary< GameObject, SceneEnum >();
	
	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);

		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowTweeners();

	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		GameObject go;

		go = FindChild( "BtnList_Window/ImgBtn_FriendList" );
		btns.Add( go, SceneEnum.FriendList );

		go = FindChild( "BtnList_Window/ImgBtn_Information" );
		btns.Add( go, SceneEnum.Information );

		go = FindChild( "BtnList_Window/ImgBtn_SearchFriend" );
		btns.Add( go, SceneEnum.SearchFriend );

		go = FindChild( "BtnList_Window/ImgBtn_Apply" );
		btns.Add( go, SceneEnum.Apply);

		go = FindChild( "BtnList_Window/ImgBtn_Reception" );
		btns.Add( go, SceneEnum.Reception );

		go = FindChild( "BtnList_Window/ImgBtn_YourID" );
		btns.Add( go, SceneEnum.YourID );

		foreach( var btn in btns.Keys ) {
			UIEventListener.Get( btn ).onClick = ClickBtn;
		}


	}

	private void ClickBtn( GameObject btn ) {
		SceneEnum scene = btns[ btn ];
		UIManager.Instance.ChangeScene( scene );
	}





}
