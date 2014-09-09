using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InformationView : ViewBase {
	
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init(config, data);
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
			tweenPos.ResetToBeginning();
			tweenPos.PlayForward();
		}
	}

}
