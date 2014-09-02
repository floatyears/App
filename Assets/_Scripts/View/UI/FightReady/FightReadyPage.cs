using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class FightReadyPage : UIComponentUnity {

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

	public override void Init (UIInsConfig config, IUICallback origin) {
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
			partyView.Add(i, puv);
		}

		helper = transform.FindChild("Helper").GetComponent<HelperUnitItem>();
	}

	private void RefreshParty(TUnitParty party){
		List<TUserUnit> partyMemberList = party.GetUserUnit();
		for (int i = 0; i < partyMemberList.Count; i++) {
			partyView[ i ].Init(partyMemberList [ i ]);	
		}
		
		ShowPartyInfo(party);
	}

	private void ShowPartyInfo(TUnitParty party){
		if(FightReadyView.pickedHelperInfo == null) return;
//		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		partyNoLabel.text = DataCenter.Instance.PartyInfo.CurrentPartyId + 1 + "/5";
		UpdateOwnLeaderSkillInfo(party);
		UpdateHelperLeaderSkillInfo();
		UpdatePartyAtkInfo(party);
	}

	private void UpdateOwnLeaderSkillInfo(TUnitParty curParty){
		SkillBase skill = curParty.GetLeaderSkillInfo();
		UpdateLeaderSkillView(skill, ownSkillNameLabel, ownSkillDscpLabel);
	}

	private void UpdateHelperLeaderSkillInfo(){
		if(FightReadyView.pickedHelperInfo == null){
			return;
		}
		
		TUnitInfo unitInfo = FightReadyView.pickedHelperInfo.UserUnit.UnitInfo;
		int skillId = unitInfo.LeaderSkill;
		if(skillId == 0){
			UpdateLeaderSkillView(null, helperSkillNameLabel, helperSkillDcspLabel);
		} else {
			string userUnitKey = FightReadyView.pickedHelperInfo.UserUnit.MakeUserUnitKey();
			SkillBaseInfo baseInfo = DataCenter.Instance.GetSkill(userUnitKey, skillId, SkillType.NormalSkill);
			SkillBase leaderSkill = baseInfo.GetSkillInfo();	
			UpdateLeaderSkillView(leaderSkill, helperSkillNameLabel, helperSkillDcspLabel);
		}
	}

	private void UpdatePartyAtkInfo(TUnitParty curParty){
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
