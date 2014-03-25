using UnityEngine;
using System.Collections.Generic;

public class BattleSkill : UIBaseUnity {
	public override void Init (string name) {
		base.Init (name);
		InitUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private string[] SKill = new string[5]{ "LeaderSkill","NormalSKill1","NormalSkill2","ActiveSkill","PassiveSKill"};
	private const string path = "/Label";
	private const string pathName = "/SkillName";
	private const string pathDescribe = "/DescribeLabel";
	private Callback boostAcitveSkill;
	private UILabel roundLabel;
	
	private Dictionary<string, SkillItem> skillDic = new Dictionary<string, SkillItem> ();

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
		si.skillTypeLabel = FindChild<UILabel> ("NormalSkill2/Label");
		si.skillName = FindChild<UILabel>("NormalSkill2/SkillName");
		si.skillDescribeLabel = FindChild<UILabel>("NormalSkill2/DescribeLabel");
		temp = new List<UISprite> ();
		for (int j = 1; j < 6; j++) {
			temp.Add(FindChild<UISprite> ("NormalSkill2/" + j));
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

		GameObject go = transform.Find ("BoostButton").gameObject;
		UIEventListener.Get (go).onClick = Boost;
	}

	void Boost(GameObject go) {
		if (boostAcitveSkill != null) {
			boostAcitveSkill();	
		}
	}

	public void Refresh(TUserUnit userUnitInfo, Callback boostSKill) {
		boostAcitveSkill = boostSKill;
		TUnitInfo tui = userUnitInfo.UnitInfo;
		SkillBaseInfo sbi = DataCenter.Instance.GetSkill (tui.LeaderSkill);
		Refresh (0, sbi);
//		Debug.LogError ("leaderskill : " + sbi);
		sbi = DataCenter.Instance.GetSkill (tui.NormalSkill1);
		Refresh (1, sbi);
//		Debug.LogError ("NormalSkill1 : " + sbi);
		sbi = DataCenter.Instance.GetSkill (tui.NormalSkill2);
		Refresh (2, sbi);
//		Debug.LogError ("NormalSkill2 : " + sbi);
		sbi = DataCenter.Instance.GetSkill (tui.ActiveSkill);
		Refresh (3, sbi);
		if (sbi == null) {
			roundLabel.text = "";		
		} 
		else {
			roundLabel.text =  sbi.BaseInfo.skillCooling + "  round";
		}
//		Debug.LogError ("ActiveSkill : " + sbi);
		sbi = DataCenter.Instance.GetSkill (tui.PassiveSkill);
		Refresh (4, sbi);

//		Debug.LogError ("PassiveSkill : " + sbi);
	}

	void Refresh(int index, SkillBaseInfo sbi) {
		skillDic [SKill [index]].ShowSkillInfo (sbi);
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

	public void ShowSkillInfo (SkillBaseInfo sbi) {
		if (sbi == null) {
			Clear();
			return;
		}
		skillTypeLabel.enabled = true;
		skillName.text = sbi.SkillName;
		skillDescribeLabel.text = sbi.SkillDescribe;

		TNormalSkill tns = sbi as TNormalSkill;
		if (tns != null) {
			ShowSprite (tns.Blocks);
		} else {
			ShowSprite(null);
		}
	}

	void Clear() {

		skillTypeLabel.enabled = false;
		skillName.text = string.Empty;
		skillDescribeLabel.text = string.Empty;
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
		for (int i = blocks.Count - 1; i < skillSprite.Count; i++) {
			skillSprite[i].spriteName = string.Empty;	
		}
	}

}