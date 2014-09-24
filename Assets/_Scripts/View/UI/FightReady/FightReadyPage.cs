using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class FightReadyPage : ViewBase {
	private UILabel partyNoLabel;

	private UILabel totalHPLabel;
	private UILabel totalAtkLabel;
	private UILabel lightAtkLabel;
	private UILabel darkAtkLabel;
	private UILabel fireAtkLabel;
	private UILabel waterAtkLabel;
	private UILabel windAtkLabel;
	private UILabel wuAtkLabel;
	
	private UILabel helperSkillNameLabel;
	private UILabel helperSkillDcspLabel;
	private UILabel ownSkillNameLabel;
	private UILabel ownSkillDscpLabel;

	private HelperUnitItem helper;
	private Dictionary<int, PageUnitItem> partyView = new Dictionary<int, PageUnitItem>();
	public List<PageUnitItem> partyViewList = new List<PageUnitItem> ();

	void Awake() {
		Init (null, null);
	}

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data) {
		base.Init (uiconfig, data);

		partyNoLabel = FindChild<UILabel> ("Label_Party_No");
		
		totalHPLabel = transform.FindChild("Label_Total_HP").GetComponent<UILabel>();
		totalAtkLabel = transform.FindChild("Label_Total_ATK").GetComponent<UILabel>();
		
		fireAtkLabel = transform.FindChild("Label_ATK_Fire").GetComponent<UILabel>();
		waterAtkLabel = transform.FindChild("Label_ATK_Water").GetComponent<UILabel>();
		lightAtkLabel = transform.FindChild("Label_ATK_Light").GetComponent<UILabel>();
		darkAtkLabel = transform.FindChild("Label_ATK_Dark").GetComponent<UILabel>();
		windAtkLabel = transform.FindChild("Label_ATK_Wind").GetComponent<UILabel>();
		wuAtkLabel = transform.FindChild("Label_ATK_Wu").GetComponent<UILabel>();
		
		helperSkillNameLabel = transform.FindChild("Label_Helper_Leader_Skill_Name").GetComponent<UILabel>();
		helperSkillDcspLabel = transform.FindChild("Label_Helper_Skill_Dscp").GetComponent<UILabel>();
		ownSkillNameLabel = transform.FindChild("Label_Own_Leader_Skill_Name").GetComponent<UILabel>();
		ownSkillDscpLabel = transform.FindChild("Label_Own_Skill_Dscp").GetComponent<UILabel>();
		
		for (int i = 0; i < 4; i++){
			PageUnitItem puv = FindChild<PageUnitItem>(i.ToString());
			partyViewList.Add(puv);
			partyView.Add(i, puv);
		}
		
		helper = transform.FindChild("Helper").GetComponent<HelperUnitItem>();
	}

	public void RefreshParty(UnitParty party){
		RefreshView (party.GetUserUnit ());
		
		ShowPartyInfo(party);
	}

	private void RefreshView(List<UserUnit> partyData) {
		for (int i = 0; i < partyData.Count; i++) {
			partyView[i].UserUnit = partyData[i];
		}
	}

	private void ShowPartyInfo(UnitParty party){
//		Debug.LogError ("FightReadyView.pickedHelperInfo == null : " + (FightReadyView.pickedHelperInfo == null));
		if(FightReadyView.pickedHelperInfo == null) return;
		int partyIDIndex = party.id + 1;
		string suffix = partyIDIndex > 5 ? partyIDIndex.ToString() : "5";
		partyNoLabel.text = partyIDIndex.ToString() + "/" + suffix;
		UpdateOwnLeaderSkillInfo(party);
		UpdateHelperLeaderSkillInfo();
		UpdatePartyAtkInfo(party);

		if (FightReadyView.pickedHelperInfo != null) {
			ShowHelper(FightReadyView.pickedHelperInfo);
		}
	}

	void ShowHelper(FriendInfo friendInfo) {
		helper.Init(friendInfo);
	} 
	

	private void UpdateOwnLeaderSkillInfo(UnitParty curParty){
		SkillBase skill = curParty.GetLeaderSkillInfo();
		UpdateLeaderSkillView(skill, ownSkillNameLabel, ownSkillDscpLabel);
	}

	private void UpdateHelperLeaderSkillInfo(){
		if(FightReadyView.pickedHelperInfo == null){
			return;
		}
		
		UnitInfo unitInfo = FightReadyView.pickedHelperInfo.UserUnit.UnitInfo;
		int skillId = unitInfo.leaderSkill;
		if(skillId == 0){
			UpdateLeaderSkillView(null, helperSkillNameLabel, helperSkillDcspLabel);
		} else {
			string userUnitKey = FightReadyView.pickedHelperInfo.UserUnit.MakeUserUnitKey();
			SkillBase baseInfo = DataCenter.Instance.BattleData.GetSkill(userUnitKey, skillId, SkillType.NormalSkill);
			SkillBase leaderSkill = baseInfo;	
			UpdateLeaderSkillView(leaderSkill, helperSkillNameLabel, helperSkillDcspLabel);
		}
	}

	private void UpdatePartyAtkInfo(UnitParty curParty){
		int totalHp = curParty.TotalHp + FightReadyView.pickedHelperInfo.UserUnit.Hp;
		totalHPLabel.text = totalHp.ToString();
		
		int totalAtk = curParty.GetTotalAtk() + FightReadyView.pickedHelperInfo.UserUnit.Attack;
		totalAtkLabel.text = totalAtk.ToString();
		
		int value = 0;
		curParty.TypeAttack.TryGetValue (EUnitType.UFIRE, out value);
		fireAtkLabel.text = value.ToString();
		
		curParty.TypeAttack.TryGetValue (EUnitType.UWATER, out value);
		waterAtkLabel.text = value.ToString();
		
		curParty.TypeAttack.TryGetValue (EUnitType.UWIND, out value);
		windAtkLabel.text = value.ToString();
		
		curParty.TypeAttack.TryGetValue (EUnitType.UNONE, out value);
		wuAtkLabel.text = value.ToString();
		
		curParty.TypeAttack.TryGetValue (EUnitType.ULIGHT, out value);
		lightAtkLabel.text = value.ToString();
		
		curParty.TypeAttack.TryGetValue (EUnitType.UDARK, out value);
		darkAtkLabel.text = value.ToString();
	}

	private void UpdateLeaderSkillView(SkillBase skill, UILabel name, UILabel dscp){
		if(skill == null){
			name.text = TextCenter.GetText("LeaderSkillText") +  TextCenter.GetText("Text_None");
			dscp.text = "";
		}
		else{
			name.text = TextCenter.GetText("LeaderSkillText") + TextCenter.GetText("SkillName_" + skill.id);//skill.name;
			dscp.text = TextCenter.GetText("SkillDesc_" + skill.id);//skill.description;
		}
	}
}
