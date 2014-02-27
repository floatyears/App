using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDetailComponent : ConcreteComponent,IUICallback {

	//int maxExp, curLevel, curExp, gotExp, expRiseStep;
	Dictionary<string, object> unitDetailInfoDic = new Dictionary< string, object>();
	//use to store source path
	List<string> sourcePathList = new List<string>();

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

	void GetStatus( object msg ){
		Debug.Log( "GetStatus");
		UserUnit userUnit = msg as UserUnit;
		TUnitInfo unitInfo = GlobalData.unitInfo[ userUnit.unitId ];

		unitDetailInfoDic.Add( "no", unitInfo.GetID.ToString() );
		unitDetailInfoDic.Add( "lv", userUnit.level.ToString() );
		unitDetailInfoDic.Add( "maxLv", unitInfo.GetMaxLevel().ToString() );

		int hpType = unitInfo.GetHPType();
		int hp = GlobalData.Instance.GetUnitValue( hpType, userUnit.level );
		unitDetailInfoDic.Add( "hp", hp.ToString() );


		unitDetailInfoDic.Add( "name", unitInfo.GetName() );
		unitDetailInfoDic.Add( "cost", unitInfo.GetCost().ToString() );
		unitDetailInfoDic.Add( "Rare", unitInfo.GetRare().ToString() );
	}

	void GetLeaderSkill(int id) {
		LogHelper.Log("UnitDetailComponent.GetLeaderSkill()");
               	SkillBase skill = GlobalData.skill[ id ].GetSkillInfo();
		unitDetailInfoDic.Add( "ls_n", skill.name );
		unitDetailInfoDic.Add( "ls_dscp", skill.description );
        }
	
	void GetNormalSkill1(int id){
		LogHelper.Log("UnitDetailComponent.GetNormalSkill1()");
		SkillBaseInfo sbi = GlobalData.skill[ id ];
		SkillBase skill =sbi.GetSkillInfo();
		unitDetailInfoDic.Add( "ns1_n", skill.name );
		unitDetailInfoDic.Add( "ns1_dscp", skill.description );

		TNormalSkill ns = sbi as TNormalSkill;
		List<uint> ab = ns.GetObject().activeBlocks;
		LogHelper.Log( "Skill1 activeBlock Count is " + ab.Count );

		unitDetailInfoDic.Add( "bls1", ab);
	}

	void GetNormalSkill2(int id) {
		LogHelper.Log("UnitDetailComponent.GetNormalSkill2()");

		SkillBaseInfo sbi = GlobalData.skill[ id ];
		SkillBase skill =sbi.GetSkillInfo();
		unitDetailInfoDic.Add( "ns2_n", skill.name );
		unitDetailInfoDic.Add( "ns2_dscp", skill.description );
		
		TNormalSkill ns = sbi as TNormalSkill;
		List<uint> ab = ns.GetObject().activeBlocks;
                LogHelper.Log( "Skill2 activeBlock Count is " + ab.Count );
                
                unitDetailInfoDic.Add( "bls2", ab);
        }
	
	void GetActiveSkill(int id) {
		LogHelper.Log("UnitDetailComponent.GetActiveSkill()");
		SkillBase skill = GlobalData.skill[ id ].GetSkillInfo();
		unitDetailInfoDic.Add( "as_n", skill.name );
		unitDetailInfoDic.Add( "as_dscp", skill.description );
        }

	void GetProfile( string text) {
		unitDetailInfoDic.Add( "pf", text );
	}

	void PackSkillText ( object info) {
		LogHelper.Log("UnitDetailComponent : Call UnitDetailPanel ");
		IUICallback caller = viewComponent as IUICallback;
		GetStatus( info );
		GetSkill( info );
		caller.Callback( unitDetailInfoDic );
		LogHelper.Log( "UnitDetailComponent.Callback() : SkillTextDic's Count is : " + unitDetailInfoDic.Count );
        }
 
}
