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


	}

	string skill1Name;
	string skill1Description;
	string skill2Name;
	string skill2Description;
	string leaderSkillName;
	string leaderSkillDescription;
	string activeSkillName;


	void GetSkill(object msg) {
		UnitInfo unitInfo = msg as UnitInfo;

		int id;
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
                
        }

	void GetNormalSkill1(int id){
		LogHelper.Log("UnitDetailComponent.GetNormalSkill1()");

	}

	void GetNormalSkill2() {
		LogHelper.Log("UnitDetailComponent.GetNormalSkill2()");
                
        }

	void GetLeaderSkill() {
		LogHelper.Log("UnitDetailComponent.GetLeaderSkill()");
                
        }

	void GetActiveSkill() {
		LogHelper.Log("UnitDetailComponent.GetActiveSkill()");
                
        }







}
