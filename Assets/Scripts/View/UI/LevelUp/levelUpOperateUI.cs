using UnityEngine;
using System.Collections;

public class levelUpOperateUI : ConcreteComponent {
	public levelUpOperateUI(string uiName) : base(uiName) {

	}

	public override void CallbackView (object data) {
		base.CallbackView (data);
	}
}
