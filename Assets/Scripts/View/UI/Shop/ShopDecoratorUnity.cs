using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopDecoratorUnity : UIComponentUnity {

	private Dictionary<string,UIButton> buttonDic = new Dictionary<string, UIButton>();

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowTweenPostion(0.2f);
	}
	
	public override void HideUI () {
		base.HideUI ();
		ShowTweenPostion();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		UIButton[] buttons = FindChild("btns").GetComponentsInChildren< UIButton >();
		for (int i = 0; i < buttons.Length; i++){
			buttonDic.Add( string.Format("Chip{0}", i), buttons[ i ] );
			UIEventListener.Get( buttons[ i ].gameObject ).onClick = ClickButton;
		}
	}

	private void ClickButton( GameObject button) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
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
