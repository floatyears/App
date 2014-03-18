using UnityEngine;
using System.Collections.Generic;

public class SellView : UIComponentUnity
{
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
	}
	
	public override void ShowUI(){
		base.ShowUI();

		gameObject.transform.localPosition = new Vector3(-1000, 267, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "easetype", iTween.EaseType.linear));                
	}
	
	public override void HideUI()
	{
		base.HideUI();
	}
	
	public override void DestoryUI()
	{
		base.DestoryUI();
	}

}
