using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class BattleSkillView : ViewBase {
	private string[] SKill = new string[5]{ "LeaderSkill","NormalSKill1","NormalSkill2","ActiveSkill","PassiveSKill"};
	private const string path = "/Label";
	private const string pathName = "/SkillName";
	private const string pathDescribe = "/DescribeLabel";
	private UILabel roundLabel;
	private UIButton boostButton;

	private UserUnit userUnit;
	private bool isShow = false;

	private Dictionary<string, SkillItem> skillDic = new Dictionary<string, SkillItem> ();
	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);
		SkillItem si = null;
		string info = string.Empty;
		
		info = SKill[0];
		si = new SkillItem();
		si.skillTypeLabel = FindChild<UILabel> (info + path);
		si.skillName = FindChild<UILabel>(info + pathName);
		si.skillDescribeLabel = FindChild<UILabel>(info + pathDescribe);
		skillDic.Add(info, si);
		
		info = SKill[4];
		si = new SkillItem();
		si.skillTypeLabel = FindChild<UILabel> ("PassiveSkill/Label");
		si.skillName = FindChild<UILabel>("PassiveSkill/SkillName");
		si.skillDescribeLabel = FindChild<UILabel>("PassiveSkill/DescribeLabel");
		skillDic.Add(info, si);
		
		info = SKill[1];
		si = new SkillItem();
		si.skillTypeLabel = FindChild<UILabel> ("NormalSKill1/Label");
		si.skillName = FindChild<UILabel>("NormalSKill1/SkillName");
		si.skillDescribeLabel = FindChild<UILabel>("NormalSKill1/DescribeLabel");
		List<UISprite> temp = new List<UISprite> ();
		for (int j = 1; j < 6; j++) {
			temp.Add(FindChild<UISprite> ("NormalSKill1/" + j));
		}
		si.skillSprite = temp;
		skillDic.Add(info, si);
		
		info = SKill[2];
		si = new SkillItem();
		si.skillTypeLabel = FindChild<UILabel> ("NormalSKill2/Label");
		si.skillName = FindChild<UILabel>("NormalSKill2/SkillName");
		si.skillDescribeLabel = FindChild<UILabel>("NormalSKill2/DescribeLabel");
		temp = new List<UISprite> ();
		for (int j = 1; j < 6; j++) {
			temp.Add(FindChild<UISprite> ("NormalSKill2/" + j));
		}
		si.skillSprite = temp;
		skillDic.Add(info, si);
		info = SKill[3];
		si = new SkillItem();
		si.skillTypeLabel = FindChild<UILabel> ("ActiveSkill/Label");
		si.skillName = FindChild<UILabel>("ActiveSkill/SkillName");
		si.skillDescribeLabel = FindChild<UILabel>("ActiveSkill/DescribeLabel");
		skillDic.Add(info, si);
		roundLabel = FindChild<UILabel>("RoundLabel");
		boostButton = FindChild<UIButton>("BoostButton");
		FindChild<UILabel> ("BoostButton/Label").text = TextCenter.GetText ("Btn_TriggerSkill");
		UIEventListenerCustom.Get (boostButton.gameObject).onClick = Boost;
		Transform trans = FindChild<Transform>("Title/Button_Close");
		UIEventListenerCustom.Get (trans.gameObject).onClick = Close;
	}

	public override void ShowUI ()
	{
		if (!isShow) {
			base.ShowUI ();
			if (viewData != null && viewData.ContainsKey("show_skill_window")) {
				userUnit = viewData ["show_skill_window"] as UserUnit;
				Refresh(userUnit);
			}	
			isShow = true;
		}else{
			ModuleManager.Instance.HideModule(ModuleEnum.BattleSkillModule);
		}

	}

	public override void HideUI ()
	{
		base.HideUI ();
		isShow = false;
		ModuleManager.SendMessage (ModuleEnum.BattleBottomModule, "close_skill_window");
	}

	void Boost(GameObject go) {
		Close (null);
		BattleAttackManager.Instance.ExcuteActiveSkill (userUnit);
	}

	void Close(GameObject go) {
		ModuleManager.Instance.HideModule (ModuleEnum.BattleSkillModule);
	}

	bool boost = false;
	bool isRecoveSP = false;
	bool isBattle = false ;
 	void Refresh(UserUnit userUnitInfo) {
		UnitInfo tui = userUnitInfo.UnitInfo;

		UpdateSkillInfo (0, DataCenter.Instance.BattleData.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.leaderSkill, SkillType.LeaderSkill));

		UpdateSkillInfo (1, DataCenter.Instance.BattleData.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.skill1, SkillType.NormalSkill));

		UpdateSkillInfo (2, DataCenter.Instance.BattleData.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.skill2, SkillType.NormalSkill));

		SkillBase sbi = DataCenter.Instance.BattleData.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.activeSkill, SkillType.ActiveSkill);
		UpdateSkillInfo (3, sbi);

		bool notNull = sbi != null;
		bool isCooling = notNull && (sbi.CoolingDone);
		isRecoveSP = notNull && sbi.GetType () == typeof(SkillRecoverSP);
		isBattle = ModuleManager.Instance.IsModuleShow(ModuleEnum.BattleEnemyModule);
		if ( isCooling ) {
			if(!isRecoveSP && !isBattle) {
				boostButton.isEnabled = false;
			} else{
				boostButton.isEnabled = true;
			}
		} else {
			boostButton.isEnabled = false;
		}
		if (sbi == null) {
			roundLabel.text = "";
		}  else {
			roundLabel.text = "CD: "+ sbi.skillCooling;
		}

		UpdateSkillInfo (4, DataCenter.Instance.BattleData.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.passiveSkill, SkillType.PassiveSkill));
	}

	void UpdateSkillInfo(int index, SkillBase sbi) {
		if (index == 4 ) {
			if(sbi == null)
				skillDic [SKill [index]].ShowSkillInfo (sbi, true);
			else
				skillDic [SKill [ 2 ]].ShowSkillInfo (null, true); 	//2 == normalskill2.
		} else {
			skillDic [SKill [index]].ShowSkillInfo (sbi);
		}
	}
}

public class SkillItem {
	/// <summary>
	/// The skill type label. don't change this label content;
	/// </summary>
	public UILabel skillTypeLabel;
	public UILabel skillName;
	public UILabel skillDescribeLabel;
	public List<UISprite> skillSprite;

	public void ShowSkillInfo (SkillBase sbi, bool isPassiveSkill = false) {
		if (sbi == null) {
			if(isPassiveSkill) {
				ClearPassiveSkill();
			} else{
				Clear();
			}
			return;
		}
		skillTypeLabel.enabled = true;
		string id = sbi.ToString ();

		skillName.text = TextCenter.GetText (SkillBase.SkillNamePrefix + id);
		skillDescribeLabel.text = TextCenter.GetText (SkillBase.SkillDescribeFix + id);

		NormalSkill tns = sbi as NormalSkill;
		if (tns != null) {
			ShowSprite (tns.Blocks);
		} else {
			ShowSprite(null);
		}
	}

	void Clear() {
		skillName.text = TextCenter.GetText ("Text_None");
		skillDescribeLabel.text = "";
		ShowSprite (null);
	}

	void ClearPassiveSkill () {
		skillTypeLabel.enabled = false;
		skillName.text = "";
		skillDescribeLabel.text = "";
		ShowSprite (null);
	}

	void ShowSprite (List<uint> blocks) {
		if (skillSprite == null) {
			return;	
		}

		if (blocks == null) {
			for (int i = 0; i < skillSprite.Count; i++) {
				skillSprite[i].spriteName = string.Empty;	
			}
			return;
		}

		for (int i = 0; i < blocks.Count; i++) {
			skillSprite[i].spriteName = blocks[i].ToString();
		}

		for (int i = blocks.Count; i < skillSprite.Count; i++) {
			skillSprite[i].spriteName = string.Empty;	
		}
	}

}