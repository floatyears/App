using UnityEngine;
using System.Collections;
using bbproto;

public class PartyInfo : BaseModel {

	private bool isChange = false;

	public PartyInfo (object instance) : base(instance) {

	}

	protected override void Init (object instance) {
		base.Init (instance);
		PartyItem pi = new PartyItem ();

	}


}