using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendListPanel : UIComponentUnity, IUIFriendList {

	private DragPanel scroller;
	private List< FriendListViewData > viewDataList = new List< FriendListViewData >();

	public override void Init(UIInsConfig config, IUIOrigin origin) {
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI() {
		base.ShowUI();
		ShowTween();
	}

	private void InitUI() {
		InitExtraFunction();
	}

	private void InitExtraFunction(){
		IUIFriendList call = origin as IUIFriendList;
		if(call == null )
			return;
		//Debug.Log("FriendListPanel Request");
		call.CustomExtraFunction( true );
	}

	private void InitScroller() {

		string itemSourcePath = "Prefabs/UI/Friend/FriendScrollerItem";
		GameObject scrollItem = Resources.Load( itemSourcePath ) as GameObject;

		scroller = new DragPanel( "Scroller", scrollItem );
		scroller.CreatUI();

		scroller.AddItem( viewDataList.Count );
		for (int i = 0; i < scroller.ScrollItem.Count; i++) {
			GameObject targetItem = scroller.ScrollItem[ i ];

			UITexture avatarTex = targetItem.GetComponentInChildren< UITexture >();
			string texPath = "";
			avatarTex.mainTexture = Resources.Load( texPath ) as Texture2D;

			UILabel[ ] labels = targetItem.GetComponentsInChildren< UILabel >();
			foreach (var labelItem in labels) {
				switch ( labelItem.gameObject.name ) {
					case "Label_Name" :
						labelItem.text = viewDataList[ i ].friendUnitName;
						break;
					case "Label_Level" :
						labelItem.text = viewDataList[ i ].friendUnitLevel.ToString();
						break;
					default:
						labelItem.text = string.Empty;
						break;
				}
			}

		}

		Dictionary<string, object> scrollerArgs = InitScrollerArgs();
		scroller.RootObject.SetScrollView( scrollerArgs );

	}

	public void CustomExtraFunction(object message){
		string extraFunctionName = (string)message;
		//Debug.Log(extraFunctionName);
		string path = "Button_Refuse_All";
		UIButton customButton = FindChild< UIButton >( path);
		UILabel customLabel = customButton.gameObject.GetComponentInChildren<UILabel>();

		switch (extraFunctionName) {
			case "":
				customButton.gameObject.SetActive( false );
				break;
			case "Update" :
				customLabel.text = extraFunctionName;
				UIEventListener.Get( customButton.gameObject ).onClick = ClickCustomButton;
				break;

			case "Refuse All":
				customLabel.text = extraFunctionName;
				UIEventListener.Get( customButton.gameObject ).onClick = ClickCustomButton;
				break;
			default:
				break;
		}
	}

	private void ShowTween(){
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)	return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)	continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}
	
	private Dictionary<string, object> InitScrollerArgs(){
		Dictionary<string, object> argsDic = new Dictionary<string, object>();
		
		argsDic.Add("parentTrans", 			this.transform				);
		argsDic.Add("scrollerScale", 		Vector3.one				);
		argsDic.Add("scrollerLocalPos",		 -270 * Vector3.up			);
		argsDic.Add("position", 			Vector3.zero				);
		argsDic.Add("clipRange", 			new Vector4(0, 0, 640, 200)		);
		argsDic.Add("gridArrange", 			UIGrid.Arrangement.Vertical	);
		argsDic.Add("maxPerLine", 			4						);
		argsDic.Add("scrollBarPosition", 		new Vector3(-320, -118, 0)		);
		argsDic.Add("cellWidth", 			110						);
		argsDic.Add("cellHeight", 			110						);
		
		return argsDic;
	}

	public void Callback(object data){}

	private void ClickCustomButton( GameObject go ){}
}


public class FriendListViewData {
	public  string friendUnitName = string.Empty;
	public  int friendUnitLevel = 0;
	public  Texture avatarTexture;

}

