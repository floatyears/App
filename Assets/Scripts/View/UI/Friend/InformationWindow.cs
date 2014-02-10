using UnityEngine;
using System.Collections;

public class InformationWindow : UIComponentUnity {
	
	public override void Init(UIInsConfig config, IUIOrigin origin)
	{
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI()
	{
		base.ShowUI();
		ShowTween();
	}

	public override void HideUI()
	{
		base.HideUI();
	}

	public override void DestoryUI()
	{
		base.DestoryUI();
	}
	
	void InitUI(){


	}

	void InitLNoteCount(){
//		UILabel noteCountLabel = FindChild< UILabel >("Label_NoteCount_Vaule");
////		noteCountLabel.text = 
//		IUICallback notes = origin as IUICallback;
//		notes.Callback( origin is IUICallback );
	}

	void ShowTween(){
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)	return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)	continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

}
