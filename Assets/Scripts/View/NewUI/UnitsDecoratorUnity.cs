using UnityEngine;
using System.Collections.Generic;

public class UnitsDecoratorUnity : UIComponentUnity {
	IUICallback iuiCallback;
	bool temp = false;
	
	private Dictionary<GameObject,SceneEnum> buttonInfo = new Dictionary<GameObject, SceneEnum> ();
	
	public override void Init (UIInsConfig config, IUIOrigin origin) {
		base.Init (config, origin);
		InitButton ();
		
		temp = origin is IUICallback;
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
	
	void InitButton() {
		GameObject go;

		go = FindChild ("Bottom/Catalog");
		buttonInfo.Add (go, SceneEnum.UnitCatalog);
		
		go = FindChild ("Bottom/Evolve");
		buttonInfo.Add (go, SceneEnum.Evolve);
		
		go = FindChild ("Bottom/LevelUp");
		buttonInfo.Add (go, SceneEnum.LevelUp);
		
		go = FindChild ("Bottom/Party");
		buttonInfo.Add (go, SceneEnum.Party);
		
		go = FindChild ("Bottom/Sell");
		buttonInfo.Add (go, SceneEnum.Sell);
		
		go = FindChild ("Bottom/UnitList");
		buttonInfo.Add (go, SceneEnum.UnitList);
		
		foreach (var item in buttonInfo.Keys) {
			UIEventListener.Get(item).onClick = OnClickCallback;
		}
	}
	
	void OnClickCallback( GameObject caller ) {
		if (!temp) {
			return;
		}
		
		SceneEnum se = buttonInfo [caller];
		
		if (iuiCallback == null) {
			iuiCallback = origin as IUICallback;
		} 

		iuiCallback.Callback(se);
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
