using UnityEngine;
using System.Collections;

public class SortController : ModuleBase {
	public SortController(string uiName) : base(uiName){}

	public override void CreatUI () {
//		Debug.LogError("SortController creat ui 1");
		base.CreatUI ();
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
