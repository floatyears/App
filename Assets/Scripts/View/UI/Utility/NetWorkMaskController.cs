using UnityEngine;
using System.Collections;

public class NetWorkMaskController : MaskController {

	public NetWorkMaskController(string name) : base(name){}

	public override void ShowUI(){
		base.ShowUI();
		AddCommandListener();
	}
	
	public override void HideUI(){
		base.HideUI();
		RemoveCommandListener();
	} 
	
	public override void Callback(object data){
		base.Callback(data);
	}

	private void AddCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.StartRequest, ShowMask);
	}
	
	private void RemoveCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.StartRequest, ShowMask);
	}

	void ShowMask(object msg){
		bool canShow = (bool)msg;
		if(canShow){
			ShowUI();
		}
		else{
			HideUI();
		}
	}

}
