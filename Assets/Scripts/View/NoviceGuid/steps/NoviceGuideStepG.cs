﻿using UnityEngine;
using System.Collections;


//untis
public class NoviceGuideStepG_StateOne:NoviceGuidState{
	
	private static NoviceGuideStepG_StateOne instance;
	
	public static NoviceGuideStepG_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepG_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepG_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepG state_one");
		
		
		GameObject party = GameObject.FindWithTag ("party");

		//LogHelper.Log (party.name);
		NoviceGuideUtil.ForceOneBtnClick (party);

		NoviceGuideUtil.ShowArrow (new GameObject[]{party}, new Vector3[]{new Vector3(0,0,1)});

		UIEventListener.Get (party).onClick += TapParty;
		
	}
	
	private void TapParty(GameObject btn)
	{
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListener.Get (btn).onClick -= TapParty;
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (null);
		}
		else{
			
		}
	}
	
}



