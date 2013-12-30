using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class PartyInfo : BaseModel {

	private bool isChange = false;

	public PartyInfo (object instance) : base(instance) {

	}

	protected override void Init (object instance) {
		base.Init (instance);
		

	}

	protected override void ReceiveNetData (WWW www) {
		base.ReceiveNetData (www);
	}

	public override bool NetRequest () {
		return false;
	}
}


