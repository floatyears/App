using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelUpReadyPanelUI : UIComponentUnity {
	
	GameObject basePanel;
	GameObject materialPanel;
	GameObject friendPanel;

	Dictionary<UIButton, GameObject > buttonDic = new Dictionary< UIButton, GameObject >();

	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	void InitUI(){
		UIButton button;

		button = FindChild<UIButton>("Button_Base");
		//buttonDic.Add();
		
	}

}
