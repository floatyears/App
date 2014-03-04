using UnityEngine;
using System.Collections;

public class PartyPageUILogic : ConcreteComponent {

	public PartyPageUILogic(string uiName):base(uiName) {}

	public override void Callback(object data){
		base.Callback(data);
//		ExcuteCallback()
	}

}
