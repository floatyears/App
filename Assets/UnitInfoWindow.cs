using UnityEngine;
using System.Collections;

public class UnitInfoWindow : UIComponentUnity {

	protected UIButton selectButton;
	protected UIButton cancelButton;
	protected UIButton viewDeailButton;
	protected UITexture avatarTex;
	protected UILabel levelLabel;
	protected UILabel sLevelLabel;
	protected UILabel hpLabel;
	protected UILabel atkLabel;
	protected UILabel nameLabel;
	protected UILabel raceLabel;

	public override void Init(UIInsConfig config, IUICallback origin){
		FindUIElement();
		base.Init(config, origin);
	}

	public override void ShowUI(){
		base.ShowUI();
		SetUIElement();
	}

	public override void HideUI(){
		base.HideUI();
		ResetUIElement();
	}

	public override void DestoryUI(){
		base.DestoryUI();
	}

	void FindUIElement(){
		Debug.Log("UnitInfoWindow FindUIElement() : ");
		selectButton = FindChild<UIButton>("Button_Select");
		cancelButton = FindChild<UIButton>("Button_Cancel");
		viewDeailButton = FindChild<UIButton>("Button_ViewDetail");
	}

	void SetUIElement(){
		Debug.Log("UnitInfoWindow SetUIElement() : ");
		UIEventListener.Get(selectButton.gameObject).onClick = ClickSelectButton;
		UIEventListener.Get(cancelButton.gameObject).onClick = ClickCancelButton;
		UIEventListener.Get(viewDeailButton.gameObject).onClick = ClickViewDetailButton;
	}

	void ClickSelectButton( GameObject go ){
		Debug.Log("UnitInfoWindow ClickSelectButton(), Button Name : " + go.name);
//		MsgCenter.Instance.Invoke(CommandEnum.PickBaseUnitInfo, );
	}

	void ClickCancelButton( GameObject go ){
		Debug.Log("UnitInfoWindow ClickCancelButton(), Button Name : " + go.name);
		this.gameObject.SetActive(false);
	}

	void ClickViewDetailButton( GameObject go ){
		Debug.Log("UnitInfoWindow ClickViewDetailButton(), Button Name : " + go.name);
	}

	void ResetUIElement(){
		Debug.Log("UnitInfoWindow ResetUIElement() : ");
	}
}
