using UnityEngine;
using System.Collections;

public class ApplyController : ConcreteComponent{
	public ApplyController( string uiName ) : base( uiName ){}

	public override void HideUI () {
		base.HideUI ();

		base.DestoryUI ();
	}
}
