using UnityEngine;
using System.Collections;

public class UnitDetailDecoratorUnity : UIComponentUnity 
{
	private UISprite detaiSprite;
	private UIWidget detailWidget;

	public override void Init ( UIInsConfig config, IUIOrigin origin )
	{
		base.Init (config, origin);

		InitUI();
	}
	
	public override void ShowUI ()
	{
		base.ShowUI ();

		ShowUnitDetailInfo();
		ShowUnitScale();
		UIManager.Instance.HideBaseScene();
	}
	
	public override void HideUI () 
	{
		base.HideUI ();
		UIManager.Instance.ShowBaseScene();
	}
	
	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}

	private void InitUI()
	{
		detaiSprite = FindChild< UISprite >("detailSprite");
		detailWidget = FindChild< UIWidget >("detailSprite");

		UIEventListener.Get( detaiSprite.gameObject ).onClick = BackPreScene;

	}

	private void ShowUnitDetailInfo()
	{
		if( Messager.toViewUnitName == string.Empty ) {
			return;
		}

		detaiSprite.spriteName = Messager.toViewUnitName;
	}

	private void BackPreScene( GameObject go )
	{
		SceneEnum preScene = UIManager.Instance.baseScene.PrevScene;

		UIManager.Instance.ChangeScene( preScene );
	}

	private void ShowUnitScale()
	{
		TweenScale unitScale = gameObject.GetComponentInChildren< TweenScale >();
		TweenAlpha unitAlpha = gameObject.GetComponentInChildren< TweenAlpha >();

		if( unitScale == null || unitAlpha == null )
			return;

		unitScale.Reset();
		unitScale.PlayForward();

		unitAlpha.Reset();
		unitAlpha.PlayForward();
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
