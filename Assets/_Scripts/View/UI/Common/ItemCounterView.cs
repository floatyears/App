using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCounterView : UIComponentUnity{
//	UILabel titleLabel;
//	UILabel curLabel;
	UILabel maxLabel;

	public override void Init(UIInsConfig config,IUICallback origin) {
		base.Init(config,origin);
		MsgCenter.Instance.AddListener(CommandEnum.RefreshItemCount, UpdateView);
		InitUIElement();
	}

	public override void ShowUI(){
//		Debug.LogError ("ItemCounterView showui 1");
		base.ShowUI();
		ShowUIAnimation();
//		Debug.LogError ("ItemCounterView showui 2");
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void DestoryUI () {
		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshItemCount, UpdateView);
		base.DestoryUI ();
	}
	
	void InitUIElement(){
//		titleLabel = FindChild<UILabel>("Label_Title");
//		curLabel = FindChild<UILabel>("Label_Current");
		maxLabel = FindChild<UILabel>("Label_Max");
	}

	public void UpdateView(object msg){
		Dictionary<string, object> viewInfo = msg as Dictionary<string, object>;
//		Debug.LogError("UpdateView 1");
//		titleLabel.text = viewInfo["title"] as string;
		int current = (int)viewInfo["current"];
//		Debug.LogError("UpdateView 2");
		int max = (int)viewInfo["max"];
//		Debug.LogError("UpdateView 3");
//		curLabel.text = TextCenter.GetText("CounterCurrent" , current);

		Vector3 pos = this.gameObject.transform.localPosition;
//		Debug.LogError("UpdateView 4");
		if (viewInfo.ContainsKey ("posy")) {
			pos.y = (int)viewInfo["posy"];
			pos.z = 0;
		}
//		pos.y = (int)viewInfo["posy"];
		this.gameObject.transform.localPosition = pos;
		
		if(max == 0){
			maxLabel.text = viewInfo["title"] + TextCenter.GetText("CounterCurrent" , current) + string.Empty;
		}
		else{

			if(current > max){
				maxLabel.text = viewInfo["title"] + "[FF0000]" + TextCenter.GetText("CounterCurrent" , current) + "[-]" + TextCenter.GetText("CounterMax" , max);
//				curLabel.color = Color.red;
			} else{
				maxLabel.text = viewInfo["title"] + TextCenter.GetText("CounterCurrent" , current) + TextCenter.GetText("CounterMax" , max);
//				curLabel.color = Color.white;
			}
		}
//		Debug.LogError("UpdateView 5");
	}

	private void ShowUIAnimation(){
//		Debug.LogError ("ShowUIAnimation 1");
		transform.localPosition = new Vector3(1000, -792, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 190, "time", 0.4f, "islocal", true));
		                                      //"oncomplete", "MoveEnd", "oncompletetarget", gameObject));
//		Debug.LogError("ShowUIAnimation transform.localPosition : " + transform.localPosition);
	}

//	void MoveEnd() {
//		Debug.LogError("MoveEnd : " + transform.localPosition);
//	}
}
