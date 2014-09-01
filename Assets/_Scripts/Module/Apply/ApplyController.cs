using UnityEngine;
using System.Collections;

public class ApplyController : ModuleBase{
	public ApplyController( string uiName ) : base( uiName ){}

	public override void HideUI () {
		base.HideUI ();

		base.DestoryUI ();
	}
}
