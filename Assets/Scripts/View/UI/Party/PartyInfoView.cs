using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyInfoView : UIComponentUnity {
	GameObject tabStatus;
	GameObject tabSkill;
	UIToggle focus;
	Dictionary<string, UILabel> viewLabel = new Dictionary<string, UILabel>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		FindUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		SetUIElement();
		ShowUIAnimation();
	}

	public override void HideUI(){
		base.HideUI();
	}

    public override void ResetUIState() {
        ResetUIElement();
        SetUIElement();
    }

	void FindUIElement(){
		UILabel label;
		label = FindChild<UILabel>("content_status/VauleLabel/Label_HP_Value");
		viewLabel.Add("hp", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_CurCost_Value");
		viewLabel.Add("curCost", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_MaxCost_Value");
		viewLabel.Add("maxCost", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Fire_Value");
		viewLabel.Add("fire", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Water_Value");
		viewLabel.Add("water", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Wind_Value");
		viewLabel.Add("wind", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Light_Value");
		viewLabel.Add("light", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Dark_Value");
		viewLabel.Add("dark", label);

		label = FindChild<UILabel>("content_status/VauleLabel/Label_Wu_Value");
		viewLabel.Add("wu",label);

		label = FindChild<UILabel>("content_leader_skill/VauleLabel/Label_LeaderSkillName");
		viewLabel.Add("skillName", label);

		label = FindChild<UILabel>("content_leader_skill/VauleLabel/Label_LeaderSkillDscp");
		viewLabel.Add("skillDscp", label);

		tabStatus = transform.FindChild("tab_status").gameObject;
		tabSkill = transform.FindChild("tab_leader_skill").gameObject;
		UIEventListener.Get(tabStatus).onClick = ClickTab;
		UIEventListener.Get(tabSkill).onClick = ClickTab;
		focus = FindChild<UIToggle>("tab_status");
	}

	void ClickTab(GameObject tab){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
	}

	void SetUIElement(){
		ResetTabFocus();
	}

	void ResetUIElement(){
		ResetTabFocus();
	}

	void ResetTabFocus(){
		focus.value = true;
	}

	void UpdateLabel(Dictionary<string, string> text){
		foreach (var item in text.Keys){
			if( viewLabel.ContainsKey(item)){
				viewLabel[item].text = text[item];
			}
		}
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
		Dictionary<string,string> viewInfoDic = data as Dictionary<string,string>;
		if( viewInfoDic == null ){
			Debug.LogError("PartyInfoPanel.Callback(), ViewInfo is Null!");
			return;
		}	
		UpdateLabel(viewInfoDic);
	}

//	void ShowUIAnimation(){
//		TweenPosition[ ] list = gameObject.c<TweenPosition>();
//		if (list == null) return;
//		foreach (var tweenPos in list){		
//			if (tweenPos == null) continue;
//			tweenPos.Reset();
//			tweenPos.PlayForward();
//		}
//	}
	private void ShowUIAnimation(){
		transform.localPosition = 1000 * Vector3.up;
		iTween.MoveTo(gameObject, iTween.Hash("y", 0, "time", 0.4f));  
	}

}
