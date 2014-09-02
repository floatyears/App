using UnityEngine;
using System.Collections;

public class ApplyController : ModuleBase{
	public ApplyController(UIConfigItem config) : base(   config ){}

	public override void HideUI () {
		base.HideUI ();

		base.DestoryUI ();
	}
}
