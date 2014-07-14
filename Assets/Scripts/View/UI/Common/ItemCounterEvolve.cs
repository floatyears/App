using UnityEngine;
using System.Collections.Generic;

public class ItemCounterEvolve : MonoBehaviour {

	UILabel titleLabel;
	UILabel curLabel;
	UILabel maxLabel;
	
	public void Init() {
		InitUIElement();
	}
	
	public void ShowUI(){ }
	
	public void HideUI(){ }
	
	public void DestoryUI () { }
	
	void InitUIElement(){
		titleLabel = transform.Find("Label_Title").GetComponent<UILabel>(); //FindChild<UILabel>("Label_Title");
		curLabel = transform.Find("Label_Current").GetComponent<UILabel>(); //FindChild<UILabel>("Label_Current");
		maxLabel = transform.Find("Label_Max").GetComponent<UILabel>(); //FindChild<UILabel>("Label_Max");
	}
	
	public void UpdateView(object msg){
		Dictionary<string, object> viewInfo = msg as Dictionary<string, object>;

		titleLabel.text = viewInfo["title"] as string;

		int current = (int)viewInfo["current"];
		int max = (int)viewInfo["max"];
		curLabel.text = current.ToString ();

		Vector3 pos = this.gameObject.transform.localPosition;

		if (viewInfo.ContainsKey ("posy")) {
			pos.y = (int)viewInfo["posy"];
			this.gameObject.transform.localPosition = pos;
		}
	
		if(max == 0){
			maxLabel.text = string.Empty;
		} else{
			maxLabel.text = TextCenter.GetText("CounterMax" , max);
			if(current > max){
				curLabel.color = Color.red;
			}
			else{
				curLabel.color = Color.white;
			}
		}
	}
	
//	private void ShowUIAnimation(){
//		transform.localPosition = new Vector3(1000, -792, 0);
//		iTween.MoveTo(gameObject, iTween.Hash("x", 213, "time", 0.4f, "islocal", true));
//	}
	
	//	void End() {
	//		Debug.LogError("ItemCounterView show ui animation end");
	//	}
}
