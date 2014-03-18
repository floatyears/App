using UnityEngine;
using System.Collections.Generic;

public class BattleSkill : UIBaseUnity {
	public override void Init (string name) {
		base.Init (name);
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

	private string[] SKill = new string[5]{ "LeaderSkill","NormalSkill1","NormalSkill2","ActiveSkill","PassiveSKill"};
	private const string path = "/Label";
	private const string pathName = "/SkillName";
	private const string pathDescribe = "/DescribeLabel";
	
	private Dictionary<string, SkillItem> skillDic = new Dictionary<string, SkillItem> ();

	void InitUI () {
		SkillItem si = null;
		string info = string.Empty;
		for (int i = 0; i < SKill.Length; i++) {
		 	si = new SkillItem();
			info = SKill[i];
			si.skillTypeLabel = FindChild<UILabel> (info + path);
			si.skillName = FindChild<UILabel>(info + pathName);
			si.skillDescribeLabel = FindChild<UILabel>(info + pathDescribe);

			if(i == 1 || i == 2) {
				List<UISprite> temp = new List<UISprite> ();
				for (int j = 1; j < 6; i++) {
					temp.Add(FindChild<UISprite> (info + info + "/" + j));
				}
				si.skillSprite = temp;
			}
			skillDic.Add(SKill[i], si);
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

	public void ShowSkillInfo (SkillBaseInfo sbi) {
		if (sbi == null) {
			Clear();
			return;
		}
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