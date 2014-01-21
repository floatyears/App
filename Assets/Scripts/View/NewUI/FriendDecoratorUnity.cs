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
		ShowTweenPostion(0.2f);

	}
	
	public override void HideUI () {
		ShowTweenPostion();
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



	private void ShowTweenPostion( float mDelay = 0f, UITweener.Method mMethod = UITweener.Method.Linear ) 
	{
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		
		if( list == null )
			return;
		
		foreach( var tweenPos in list)
		{		
			if( tweenPos == null )
				continue;
			
			Vector3 temp;
			temp = tweenPos.to;
			tweenPos.to = tweenPos.from;
			tweenPos.from = temp;
			
			tweenPos.delay = mDelay;
			tweenPos.method = mMethod;
			
			tweenPos.Reset();
			tweenPos.PlayForward();
			
		}
	}

}
