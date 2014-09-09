using UnityEngine;
using System.Collections;

public class SortController : ModuleBase {
	public SortController(UIConfigItem config) : base(  config){}

	public override void InitUI () {
//		Debug.LogError("SortController creat ui 1");
		base.InitUI ();
//		Debug.LogError("SortController creat ui 2");
	}

	public override void ShowUI ()
	{
//		Debug.LogError("SortController show ui 1");
		base.ShowUI ();
//		Debug.LogError("SortController show ui 2");
	}

	public override void HideUI () {
		base.HideUI ();
//		base.DestoryUI ();
	}
}
