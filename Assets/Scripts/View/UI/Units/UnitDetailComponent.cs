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
		AddMsgCmd();
        }
	   
	public override void HideUI () {
		base.HideUI ();
		RmvMsgCmd();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void Callback(object data) {

	}

	string skill1Name;
	string skill1Description;

	string skill2Name;
	string skill2Description;

	string leaderSkillName;
	string leaderSkillDescription;

	string activeSkillName;
	string activeSkillDescription;

	Dictionary<int, object> skillTextDic = new Dictionary< int, object>();
	//use to store source path
	List<string> sourcePathList = new List<string>();


	void AddMsgCmd () {
		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, PackSkillText);
        }

	void RmvMsgCmd () {
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitDetail, PackSkillText);
        }
        
	void GetSkill(object msg) {
		UserUnit userUnit = msg as UserUnit;
		TUnitInfo unitInfo = GlobalData.unitInfo[ userUnit.unitId ];

		GetNormalSkill1( unitInfo.GetSkill1() );

		GetNormalSkill2( unitInfo.GetSkill2() );

		GetLeaderSkill( unitInfo.GetLeaderSkill() );

		GetActiveSkill( unitInfo.GetActiveSkill() );
	}

	void GetLeaderSkill(int id) {
		LogHelper.Log("UnitDetailComponent.GetLeaderSkill()");
               	SkillBase skill = GlobalData.skill[ id ].GetSkillInfo();
		skillTextDic.Add( 1, skill.name );
		skillTextDic.Add( 2, skill.description );
        }

	void GetNormalSkill1(int id){
		LogHelper.Log("UnitDetailComponent.GetNormalSkill1()");
		SkillBaseInfo sbi = GlobalData.skill[ id ];
		SkillBase skill =sbi.GetSkillInfo();
		skillTextDic.Add( 5, skill.name );
		skillTextDic.Add( 6, skill.description );
//		NormalSkill normalSkill = sbi as NormalSkill;

//		 //Card Trigger
//		List<int> unitTypeList = normalSkill.activeBlocks;
//		foreach (var item in unitTypeList){
//			sourcePathList.Add( item.ToString() );
//		}
	
	}

	void GetSkillTrigger( int id ) {
		SkillBaseInfo sbi = GlobalData.skill[ id ];
	//	NormalSkill ns = sbi as NormalSkill;
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

	void GetProfile() {
		string profileText = " ";
		skillTextDic.Add( 9, profileText );
	}

	void PackSkillText ( object info) {
		LogHelper.Log("UnitDetailComponent : Call UnitDetailPanel ");

		IUICallback caller = viewComponent as IUICallback;
		GetSkill( info );
		caller.Callback( skillTextDic );
		LogHelper.Log( "UnitDetailComponent.Callback() : SkillTextDic's Count is : " + skillTextDic.Count );
        }
 
}
