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

	void GetSkill(object msg) {
		UnitInfo unitInfo = msg as UnitInfo;

		int skillID;
		skillID = unitInfo.skill1;
		GetNormalSkill1(skillID);

	}

	void GetNormalSkill1(int id){
		Debug.Log("UnitDetailComponent.GetNormalSkill1()");

	}

	void GetNormalSkill2() {

	}

	void GetLeaderSkill() {

	}

	void GetActiveSkill() {

	}







}
