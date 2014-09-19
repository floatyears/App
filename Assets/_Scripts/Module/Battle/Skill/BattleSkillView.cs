using UnityEngine;
using System.Collections.Generic;

public class BattleSkillView : ViewBase {
	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);
		InitUI ();
	}

	private string[] SKill = new string[5]{ "LeaderSkill","NormalSKill1","NormalSkill2","ActiveSkill","PassiveSKill"};
	private const string path = "/Label";
	private const string pathName = "/SkillName";
	private const string pathDescribe = "/DescribeLabel";
	private Callback boostAcitveSkill;
	private Callback CloseSkill;
	private UILabel roundLabel;
	private UIButton boostButton;
	
	private Dictionary<string, SkillItem> skillDic = new Dictionary<string, SkillItem> ();

	[HideInInspector]
	public BattleMapModule battleQuest;

	void MeetEnemy(object data) {
		boost = true;
	}

	void BattleEnd(object data) {
		boost = false;
	}

	void InitUI () {
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
		UIEventListener.Get (boostButton.gameObject).onClick = Boost;
		Transform trans = FindChild<Transform>("Title/Button_Close");
		UIEventListener.Get (trans.gameObject).onClick = Close;
	}

	void Boost(GameObject go) {
		if (boostAcitveSkill == null) {
			return;
		}
		if (isBattle || isRecoveSP) {
			boostAcitveSkill ();
		}

//		RemoveSkillEffect (prevID);
		//		Debug.LogError("Boost Active Skill : " + tuu);
		//		MsgCenter.Instance.Invoke(CommandEnum.LaunchActiveSkill, tuu);
		BattleAttackManager.Instance.ExcuteActiveSkill ();

	}

	public void Close(GameObject go) {
		if (CloseSkill != null) {
			CloseSkill();
		}
	}

	bool boost = false;
	bool isRecoveSP = false;
	bool isBattle = false ;
	public void Refresh(TUserUnit userUnitInfo, Callback boostSKill, Callback close) {
		boostAcitveSkill = boostSKill;
		CloseSkill = close;
		TUnitInfo tui = userUnitInfo.UnitInfo;

		SkillBaseInfo sbi = DataCenter.Instance.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.LeaderSkill, SkillType.LeaderSkill);
		Refresh (0, sbi);

		sbi = DataCenter.Instance.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.NormalSkill1, SkillType.NormalSkill);
		Refresh (1, sbi);

		sbi = DataCenter.Instance.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.NormalSkill2, SkillType.NormalSkill);
		Refresh (2, sbi);

		sbi = DataCenter.Instance.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.ActiveSkill, SkillType.ActiveSkill);
		Refresh (3, sbi);

		bool notNull = sbi != null;
		bool isCooling = notNull && (sbi.skillBase.skillCooling == 0);
		isRecoveSP = notNull && sbi.GetType () == typeof(TSkillRecoverSP);
//		isBattle = battleQuest.battle.GetState == UIState.UIShow;
		if (notNull && isCooling) {
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
			roundLabel.text = "CD: "+ sbi.skillBase.skillCooling;
		}

		sbi = DataCenter.Instance.GetSkill (userUnitInfo.MakeUserUnitKey (), tui.PassiveSkill, SkillType.PassiveSkill);	
		Refresh (4, sbi);
	}

	void Refresh(int index, SkillBaseInfo sbi) {
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

	public void ShowSkillInfo (SkillBaseInfo sbi, bool isPassiveSkill = false) {
		if (sbi == null) {
			if(isPassiveSkill) {
				ClearPassiveSkill();
			} else{
				Clear();
			}
			return;
		}
		skillTypeLabel.enabled = true;
		string id = sbi.skillBase.id.ToString ();

		skillName.text = TextCenter.GetText (SkillBaseInfo.SkillNamePrefix + id);
		skillDescribeLabel.text = TextCenter.GetText (SkillBaseInfo.SkillDescribeFix + id);

		TNormalSkill tns = sbi as TNormalSkill;
		if (tns != null) {
			ShowSprite (tns.Blocks);
		} else {
			ShowSprite(null);
		}
	}

	void Clear() {
		skillName.text = "";
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