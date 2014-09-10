using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCounterView : ViewBase{
	UILabel maxLabel;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null) {
		base.Init(config, data);
		MsgCenter.Instance.AddListener(CommandEnum.RefreshItemCount, UpdateView);
		MsgCenter.Instance.AddListener (CommandEnum.HideItemCount, HideItemCount);
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		ShowUIAnimation();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void DestoryUI () {
		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshItemCount, UpdateView);
		MsgCenter.Instance.RemoveListener (CommandEnum.HideItemCount, HideItemCount);
		base.DestoryUI ();
	}
	
	void InitUIElement(){
		maxLabel = FindChild<UILabel>("Label_Max");
	}

	void HideItemCount(object data) {

	}

	public void UpdateView(object msg){
		Dictionary<string, object> viewInfo = msg as Dictionary<string, object>;
		int current = (int)viewInfo["current"];
		int max = (int)viewInfo["max"];
		Vector3 pos = this.gameObject.transform.localPosition;
		if (viewInfo.ContainsKey ("posy")) {
			pos.y = (int)viewInfo["posy"];
			pos.z = 0;
		}
		this.gameObject.transform.localPosition = pos;
		
		if(max == 0){
			maxLabel.text = viewInfo["title"] + TextCenter.GetText("CounterCurrent" , current) + string.Empty;
		} else {
			if(current > max){
				maxLabel.text = viewInfo["title"] + "[FF0000]" + TextCenter.GetText("CounterCurrent" , current) + "[-]" + TextCenter.GetText("CounterMax" , max);
			} else{
				maxLabel.text = viewInfo["title"] + TextCenter.GetText("CounterCurrent" , current) + TextCenter.GetText("CounterMax" , max);
			}
		}
	}

	private void ShowUIAnimation(){
		transform.localPosition = new Vector3(1000, -792, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 190, "time", 0.4f, "islocal", true));
	}
}
