using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDetailComponent : ConcreteComponent,IUICallback {

	//int maxExp, curLevel, curExp, gotExp, expRiseStep;
	Dictionary<int, object> unitDetailInfoDic = new Dictionary< int, object>();

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
		GetProfile( unitInfo.GetProfile() );
	}

	void GetLeaderSkill(int id) {
		LogHelper.Log("UnitDetailComponent.GetLeaderSkill()");
               	SkillBase skill = GlobalData.skill[ id ].GetSkillInfo();
		unitDetailInfoDic.Add( 1, skill.name );
		unitDetailInfoDic.Add( 2, skill.description );
        }

	void GetNormalSkill1(int id){
		LogHelper.Log("UnitDetailComponent.GetNormalSkill1()");
		SkillBaseInfo sbi = GlobalData.skill[ id ];
		SkillBase skill =sbi.GetSkillInfo();
		unitDetailInfoDic.Add( 5, skill.name );
		unitDetailInfoDic.Add( 6, skill.description );

		TNormalSkill ns = sbi as TNormalSkill;
		List<uint> ab = ns.GetObject().activeBlocks;
		LogHelper.Log( "Skill1 activeBlock Count is " + ab.Count );

		unitDetailInfoDic.Add( 10, ab);
	}

	void GetNormalSkill2(int id) {
		LogHelper.Log("UnitDetailComponent.GetNormalSkill2()");

//             SkillBase skill = GlobalData.skill[ id ].GetSkillInfo();
//		unitDetailInfoDic.Add( 7, skill.name );
//		unitDetailInfoDic.Add( 8, skill.description );

		SkillBaseInfo sbi = GlobalData.skill[ id ];
		SkillBase skill =sbi.GetSkillInfo();
		unitDetailInfoDic.Add( 7, skill.name );
		unitDetailInfoDic.Add( 8, skill.description );
		
		TNormalSkill ns = sbi as TNormalSkill;
		List<uint> ab = ns.GetObject().activeBlocks;
                LogHelper.Log( "Skill2 activeBlock Count is " + ab.Count );
                
                unitDetailInfoDic.Add( 11, ab);
        }
	
	void GetActiveSkill(int id) {
		LogHelper.Log("UnitDetailComponent.GetActiveSkill()");
		SkillBase skill = GlobalData.skill[ id ].GetSkillInfo();
		unitDetailInfoDic.Add( 3, skill.name );
		unitDetailInfoDic.Add( 4, skill.description );
        }

	void GetProfile( string text) {
		unitDetailInfoDic.Add( 9, text );
	}

	void PackSkillText ( object info) {
		LogHelper.Log("UnitDetailComponent : Call UnitDetailPanel ");
		IUICallback caller = viewComponent as IUICallback;
		GetSkill( info );
		caller.Callback( unitDetailInfoDic );
		LogHelper.Log( "UnitDetailComponent.Callback() : SkillTextDic's Count is : " + unitDetailInfoDic.Count );
        }
 
}
