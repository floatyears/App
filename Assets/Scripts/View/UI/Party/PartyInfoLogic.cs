using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyInfoLogic : ConcreteComponent {
	public PartyInfoLogic(string uiName):base(uiName) {}
}

public class UnitInfoLogic : ConcreteComponent {
	public UnitInfoLogic(string uiName):base(uiName) {}
}