using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDetailComponent : ConcreteComponent,IUICallback {

	//int maxExp, curLevel, curExp, gotExp, expRiseStep;

	public UnitDetailComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
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

	public void Callback(object data) {
		IUICallback caller = viewComponent as IUICallback;

		LogHelper.Log( "UnitDetailComponent.Callback() : SkillTextDic's Count is : " + skillTextDic.Count );
		caller.Callback( skillTextDic );
	}

	string skill1Name;
	string skill1Description;

	string skill2Name;
	string skill2Description;

	string leaderSkillName;
	string leaderSkillDescription;

	string activeSkillName;
	string activeSkillDescription;

	Dictionary<int, string> skillTextDic = new Dictionary< string, string>();

	void GetSkill(object msg) {
		UnitInfo unitInfo = msg as UnitInfo;

		int id;

		if ( id == -1 ){
			LogHelper.LogError("Find Skill Error!");
			return;
		}

		id = unitInfo.skill1;
		GetNormalSkill1(id);

		id = unitInfo.skill2;
		GetNormalSkill2(id);

		id = unitInfo.leaderSkill;
		GetLeaderSkill(id);

		id = unitInfo.activeSkill;
		GetActiveSkill(id);
	}

	void GetLeaderSkill(int id) {
		LogHelper.Log("UnitDetailComponent.GetLeaderSkill()");
               	SkillBase skill = GlobalData.skill[ id ].GetSkillInfo();
		skillTextDic.Add( 1, skill.name );
		skillTextDic.Add( 2, skill.description );
        }

	void GetNormalSkill1(int id){
		LogHelper.Log("UnitDetailComponent.GetNormalSkill1()");
		SkillBase skill = GlobalData.skill[ id ].GetSkillInfo();
		skillTextDic.Add( 5, skill.name );
		skillTextDic.Add( 6, skill.description );
	}

	void GetNormalSkill2(int id) {
		LogHelper.Log("UnitDetailComponent.GetNormalSkill2()");
                SkillBase skill = GlobalData.skill[ id ].GetSkillInfo();
		skillTextDic.Add( 7, skill.name );
		skillTextDic.Add( 8, skill.description );
        }
	

	void GetActiveSkill(int id) {
		LogHelper.Log("UnitDetailComponent.GetActiveSkill()");
		SkillBase skill = GlobalData.skill[ id ].GetSkillInfo();
		skillTextDic.Add( 3, skill.name );
		skillTextDic.Add( 4, skill.description );
        }


//
//	void PackSkillText () {
//
//	}



}
