using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class BattleDataModel : ProtobufDataBase {

	public  ActiveSkillData activeSkill;


	private Dictionary<int, SkillBase> skill = new Dictionary<int, SkillBase> ();
	public Dictionary<int, SkillBase> Skill {
		get { 
			return skill; 
		}
		set { skill = value; } 
	}

	private Dictionary<string, SkillBase> allSkill = new Dictionary<string, SkillBase>();
	public Dictionary<string, SkillBase> AllSkill {
		get { 
			return allSkill; 
		}
		set { allSkill = value; } 
	}
	
	public SkillBase GetSkill(string userUnitID, int skillID, SkillType skillType) {
		if (skillID == 0) {
			
			return null;
		}
		SkillBase skill = null;
		string skillUserID = GetSkillID (userUnitID, skillID);
		
		if (!allSkill.TryGetValue (skillUserID, out skill)) {
			skill = DGTools.LoadSkill(skillID, skillType);
			if(skill == null) {
				Debug.LogError("load skill faile. not have this skill config ! " + userUnitID + " skillID : " + skillID) ;
				return null;
			}
//			Debug.Log("log: " + typeof(SkillBase).GetProperty("baseInfo",typeof(SkillBase)));
//			SkillBase sl = typeof(SkillBase).GetProperty("baseInfo",typeof(SkillBase)).GetValue(skill,null) as SkillBase;
			SkillBase sl = skill.GetType().GetProperty("baseInfo",typeof(SkillBase)).GetValue(skill,null) as SkillBase;
			skill.id = sl.id;
			skill.name = sl.name;
			skill.skillCooling = sl.skillCooling;
			skill.description = sl.description;
			allSkill.Add(skillUserID, skill);
		}
		
		return skill;
	}
	
	
	public string GetSkillID (string userUnitID, int skillID) {
		return userUnitID + "_" + skillID;
	}
	
	private Dictionary<uint, EnemyInfo> enemyInfo;
	public Dictionary<uint, EnemyInfo> EnemyInfo {
		get { 
			return enemyInfo; 
		}
		set { 
			enemyInfo = value; 
		} 
	}
	
	
	//-------new add
	
	
	//-------end new add

	private Dictionary<uint, TrapBase> trapInfo = new Dictionary<uint, TrapBase> ();
	public Dictionary<uint, TrapBase> TrapInfo {
		get { 
			return trapInfo; 
		}
		set { 
			trapInfo = value;
		} 
	}

}
