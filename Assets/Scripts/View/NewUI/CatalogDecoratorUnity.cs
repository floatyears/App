using UnityEngine;
using System.Collections;

public class CatalogDecoratorUnity : UIComponentUnity {
	
	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowTweenPostion( 0.2f );
	}
	
	public override void HideUI () {
		base.HideUI ();
		ShowTweenPostion();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
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
