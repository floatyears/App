using UnityEngine;
using System.Collections;

public class OthersDecoratorUnity : UIComponentUnity {


	private GameObject scrollerItem;
	private DragPanel othersScroller;

	public override void Init ( UIInsConfig config, IUIOrigin origin ) {

		InitUI();
		base.Init (config, origin);
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowTweenPostion(0.2f);
		SetUIActive(true);
	}
	
	public override void HideUI () {
		ShowTweenPostion();
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	void InitUI()
	{
		scrollerItem = Resources.Load("Prefabs/UI/Others/OthersScrollerItem") as GameObject;
		othersScroller = new DragPanel ("OthersScroller", scrollerItem);
		othersScroller.CreatUI ();
		othersScroller.AddItem (15);
		othersScroller.RootObject.SetItemWidth(150);

		othersScroller.RootObject.gameObject.transform.parent = this.gameObject.transform.FindChild( "scroller" );
		othersScroller.RootObject.gameObject.transform.localScale = Vector3.one;

		othersScroller.RootObject.gameObject.transform.localPosition = -190*Vector3.up;
	}

	private void SetUIActive(bool b)
	{
		othersScroller.RootObject.gameObject.SetActive(b);
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
