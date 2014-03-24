using UnityEngine;
using System.Collections;

public class TipsBarBehavior : UIComponentUnity {

	private UILabel labelTips;

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
        AddListener();
	}
	
	public override void HideUI () {
		base.HideUI ();
        RemoveListener();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

    private void AddListener(){
        MsgCenter.Instance.AddListener(CommandEnum.EnableMenuBtns, EnableDisplay);
    }
    
    private void RemoveListener(){
        MsgCenter.Instance.RemoveListener(CommandEnum.EnableMenuBtns, EnableDisplay);
    }

	private void InitUI() 
	{
		labelTips = FindChild< UILabel >("Scroll/Label_Tips");
	}

    private void EnableDisplay(object args){
        this.gameObject.SetActive((bool)args);
    }
	

}
