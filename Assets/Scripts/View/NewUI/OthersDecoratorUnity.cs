using UnityEngine;
using System.Collections.Generic;

public class OthersDecoratorUnity : UIComponentUnity {

	private GameObject scrollerItem;
	private DragPanel othersScroller;

	private Dictionary< string, GameObject > options = new Dictionary<string, GameObject>();
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
	}

	private void SetUIActive(bool b) {
		othersScroller.RootObject.gameObject.SetActive(b);
	}

	private void CreateScroller() {
		options.Add("option1", null );
		options.Add("option2", null );
		options.Add("options", null );

		string itemPath = "Prefabs/UI/Others/OtherOptions";
		GameObject item = Resources.Load( itemPath ) as GameObject;
		
		othersScroller = new DragPanel ( "OthersScroller", scrollerItem );
		othersScroller.CreatUI ();
		InitOtherScrollArgs();
		
//		foreach (string name in options.Keys) {
//			options[ name ] = othersScroller.AddScrollerItem( item );
//			UILabel label = options[ name].GetComponentInChildren< UILabel >();
//			label.text = name;
//		}
		GameObject temp;

		temp = othersScroller.AddScrollerItem(item);
		temp.GetComponentInChildren<UILabel>().text = "option1";

		temp = othersScroller.AddScrollerItem(item);
		temp.GetComponentInChildren<UILabel>().text = "option2";

		temp = othersScroller.AddScrollerItem(item);
		temp.GetComponentInChildren<UILabel>().text = "option3";

		temp = othersScroller.AddScrollerItem(item);
		temp.GetComponentInChildren<UILabel>().text = "option4";

		othersScroller.RootObject.SetScrollView( otherScrollerArgsDic );
		
		for(int i = 0; i < othersScroller.ScrollItem.Count; i++)
			UIEventListener.Get( othersScroller.ScrollItem[ i ].gameObject ).onClick = GetOptions;
	}

	private void ShowTween() {
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

	private void GetOptions( GameObject go) {
		//Debug.LogError( " Show Options..." );
	}

	private void InitOtherScrollArgs() {
		Transform parentTrans = FindChild("scroller").transform;
		otherScrollerArgsDic.Add( "parentTrans", 			parentTrans			      				);
		otherScrollerArgsDic.Add( "scrollerScale", 			Vector3.one								);
		otherScrollerArgsDic.Add( "scrollerLocalPos" ,	-190*Vector3.up							);
		otherScrollerArgsDic.Add( "position", 				Vector3.zero 								);
		otherScrollerArgsDic.Add( "clipRange", 				new Vector4( 0, 0, 640, 200)			);
		otherScrollerArgsDic.Add( "gridArrange", 			UIGrid.Arrangement.Horizontal 	);
		otherScrollerArgsDic.Add( "maxPerLine", 			0 												);
		otherScrollerArgsDic.Add( "scrollBarPosition", 	new Vector3(-320,-120,0)			);
		otherScrollerArgsDic.Add( "cellWidth", 				150 											);
		otherScrollerArgsDic.Add( "cellHeight",				130 											);
	}
}
