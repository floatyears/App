using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApplyLogic : ConcreteComponent {

	List<UnitItemViewInfo> friendUnitItemViewList = new List<UnitItemViewInfo>();
	List<string> friendNickNameList = new List<string>();


	public ApplyLogic( string uiName ) : base( uiName ) {}
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

	void AddCommandListener(){
	}
	
	void RemoveCommandListener(){
	}
	


}
