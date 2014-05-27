using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class PartyInfoView : UIComponentUnity {
	
	private UILabel totalHpLabel;
	private UILabel curCostLabel;
	private UILabel maxCostLabel;
	private UILabel fireAtkLabel;
	private UILabel waterAtkLabel;
	private UILabel windAtkLabel;
	private UILabel lightAtkLabel;
	private UILabel darkAtkLabel;
	private UILabel noneAtkLabel;
	private UILabel leaderSkillNameLabel;
	private UILabel leaderSkillDscpLabel;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
		UpdateView(DataCenter.Instance.PartyInfo.CurrentParty);
		ShowUIAnimation();
	}

	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
	}
	
	private void InitUI(){
		UILabel label;
		totalHpLabel = transform.FindChild("Label_Total_HP").GetComponent<UILabel>();
		curCostLabel = transform.FindChild("Label_Cost_Cur").GetComponent<UILabel>();
		maxCostLabel = transform.FindChild("Label_Cost_Max").GetComponent<UILabel>();

		fireAtkLabel = transform.FindChild("Label_Atk_Fire").GetComponent<UILabel>();
		waterAtkLabel = transform.FindChild("Label_Atk_Water").GetComponent<UILabel>();
		windAtkLabel = transform.FindChild("Label_Atk_Wind").GetComponent<UILabel>();
		lightAtkLabel = transform.FindChild("Label_Atk_Light").GetComponent<UILabel>();
		darkAtkLabel = transform.FindChild("Label_Atk_Dark").GetComponent<UILabel>();
		noneAtkLabel = transform.FindChild("Label_Atk_None").GetComponent<UILabel>();

		leaderSkillNameLabel = transform.FindChild("Label_Leader_Skill_Name").GetComponent<UILabel>();
		leaderSkillDscpLabel = transform.FindChild("Label_Leader_Skill_Dscp").GetComponent<UILabel>();
	}
	
	private void UpdateView(object data){
		TUnitParty unitParty = data as TUnitParty;
		if(unitParty == null){
			Debug.LogError("PartyInfoView.UpdateView(), TUnitParty is NULL!");
			return;
		}

		SkillBase skillBase = unitParty.GetLeaderSkillInfo ();
		if (skillBase == null) {
			leaderSkillNameLabel.text = string.Empty;
			leaderSkillDscpLabel.text = string.Empty;
		}
		else{
			leaderSkillNameLabel.text = skillBase.name;
			leaderSkillDscpLabel.text = skillBase.description;
		}

		totalHpLabel.text = unitParty.TotalHp.ToString();	
		curCostLabel.text = unitParty.TotalCost.ToString();
		maxCostLabel.text = DataCenter.Instance.UserInfo.CostMax.ToString();

		int value = 0;
		unitParty.TypeAttack.TryGetValue (EUnitType.UFIRE, out value);
		fireAtkLabel.text = value.ToString();
		
		unitParty.TypeAttack.TryGetValue (EUnitType.UWATER, out value);	
		waterAtkLabel.text =  value.ToString();
		
		unitParty.TypeAttack.TryGetValue (EUnitType.UWIND, out value);
		windAtkLabel.text =  value.ToString();
		
		unitParty.TypeAttack.TryGetValue (EUnitType.UNONE, out value);
		noneAtkLabel.text =  value.ToString();
		
		unitParty.TypeAttack.TryGetValue (EUnitType.ULIGHT, out value);
		lightAtkLabel.text =  value.ToString();
		
		unitParty.TypeAttack.TryGetValue (EUnitType.UDARK, out value);
		darkAtkLabel.text =  value.ToString();


	}
	
	private void ShowUIAnimation(){
		transform.localPosition = 1000 * Vector3.up;
		float offsetY = 0;
		if(UIManager.Instance.baseScene.CurrentScene == SceneEnum.Units){
			offsetY = -410;
		}
		else if(UIManager.Instance.baseScene.CurrentScene == SceneEnum.Party){
			offsetY = -386;
		}
		iTween.MoveTo(gameObject, iTween.Hash("y", offsetY, "time", 0.4f, "islocal", true));  
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPartyPanelInfo, UpdateView);
	}
	
	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshPartyPanelInfo, UpdateView);
	}

}
