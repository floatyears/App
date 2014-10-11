using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NoviceGuideStepManager {

	private NoviceGuideStage currentNoviceGuideStage = NoviceGuideStage.NONE;
	
	private Type currentState;
	
	private Type prevState;
	
	private Dictionary<System.Type,NoviceGuidStep> stepInsDic = new Dictionary<System.Type, NoviceGuidStep>();
	
	private static NoviceGuideStepManager instance;
	
	private NoviceGuideStepManager(){
		
	}
	
	public static NoviceGuideStepManager Instance
	{
		get{
			if (instance == null)
				instance = new NoviceGuideStepManager ();
			return instance;
		}
	}
	
	public void AddStep(NoviceGuidStep stepState)
	{
		
		if (!stepInsDic.ContainsKey (stepState.GetType())) {
			stepInsDic.Add(stepState.GetType(),stepState);
		} else {

		}
	}
	
	public void ChangeState(Type nextState)
	{
		prevState = currentState;

		if (prevState != null && stepInsDic.ContainsKey(prevState)) {
			stepInsDic[prevState].Exit();
		}

		if (nextState == null) {
			Debug.LogWarning("Novice Guide's step is null.The State machine will stop.");
			currentState = null;
			return;
		}
		currentState = nextState;

		if (!stepInsDic.ContainsKey(currentState)) {
			Activator.CreateInstance(currentState);
		}
		stepInsDic[currentState].Enter();
	}

	void ServerCallback(object data){
		Debug.LogWarning ("Novice ServerCallback..");
	}

	public NoviceGuideStage CurrentNoviceGuideStage
	{
		get{
			return currentNoviceGuideStage;
			Debug.Log("current novice guide stage(get): " + currentNoviceGuideStage);
		}
		set{
//			Umeng.GA.FinishLevel();
			Umeng.GA.StartLevel(currentNoviceGuideStage.ToString());
			currentNoviceGuideStage = value;
			currentState = Type.GetType(currentNoviceGuideStage.ToString());
			Debug.Log("current novice guide stage(set): " + currentNoviceGuideStage);
			UserController.Instance.FinishUserGuide(ServerCallback, (int)currentNoviceGuideStage);

		}
	}
	public void InitGuideStage(int stage){
		currentNoviceGuideStage = (NoviceGuideStage)stage;
		currentState = Type.GetType (currentNoviceGuideStage.ToString ());

		if(NoviceGuideStepManager.Instance.isInNoviceGuide())
			Umeng.GA.StartLevel("Novice" +(int)currentNoviceGuideStage);
		Debug.Log("current novice guide stage(start): " + currentNoviceGuideStage);
	}

	public bool isInNoviceGuide()
	{
		return currentNoviceGuideStage != NoviceGuideStage.NONE;
	}
//	
	public void FinishCurrentStep(){
		UserController.Instance.FinishUserGuide(ServerCallback, (int)currentNoviceGuideStage);
		//currentNoviceGuideStage++;
	}

	public void StartStep(NoviceGuideStartType startType)
	{
		if( currentState == null ) {
			return;
		}

		Type nextType = currentState;

		if(currentState.Name.IndexOf("NoviceGuideStepA") >= 0){
			switch(startType){
			case NoviceGuideStartType.START_BATTLE:
			case NoviceGuideStartType.FIGHT:
				if(stepInsDic.ContainsKey (nextType)) {
					nextType = stepInsDic[nextType].NextState;
				}
				ChangeState (nextType);
				break;
			}
		}else if(currentState.Name.IndexOf("NoviceGuideStepB") >= 0){

		}

		//get the last step.  
		 

	}

}

//the big steps...then one big step contains several small steps.
public enum NoviceGuideStepEntityID{

	Loading = 1,
	SElECT_ROLE = 2,
	SCRATCH = 3,
	RARE_SCRATCH = 4,
	QUEST = 5,
	PARTY = 6,
	UNITS = 7,
	FIGHT = 8,
	FIGHT_BOSS,
	LEVEL_UP,
	EVOLVE,
	INPUT_NAME,
	EVOLVE_QUEST,
}

public enum NoviceGuideStartType{
	DEFAULT,
	START_BATTLE,
//	GOLD_BOX,
	FIGHT,
//	GET_KEY,
	UNITS,
//	SCRATCH,
//	INPUT_NAME,
//	QUEST_SELECT,
//	PARTY
	OTHERS,
	QUEST,
	SCRATCH
}

public enum NoviceGuideStage{
	BLANK = -2,
	NONE = -1,
//	Preface,
//	SELECT_ROLE,
	NoviceGuideStepA_1 = 0,
	NoviceGuideStepA_2,
	NoviceGuideStepA_3,
	NoviceGuideStepA_4,
	NoviceGuideStepA_5,
	NoviceGuideStepA_6,
	NoviceGuideStepB_1,
	NoviceGuideStepB_2,
	NoviceGuideStepC_1,
	NoviceGuideStepC_2,
	NoviceGuideStepC_3,
	NoviceGuideStepC_4,
	NoviceGuideStepD_1,
	NoviceGuideStepD_2,
	NoviceGuideStepD_3,
	NoviceGuideStepD_4,
	NoviceGuideStepE_1,
	NoviceGuideStepE_2,
	NoviceGuideStepE_3,
	NoviceGuideStepE_4,
	NoviceGuideStepF_1,
	NoviceGuideStepF_2,
	NoviceGuideStepF_3,
	NoviceGuideStepF_4,
	NoviceGuideStepF_5,
	NoviceGuideStepF_6,
	NoviceGuideStepG_1,
	NoviceGuideStepG_2,
	NoviceGuideStepG_3,
	NoviceGuideStepH_1,
	NoviceGuideStepI_1,
	NoviceGuideStepI_2,
	NoviceGuideStepI_3,
	NoviceGuideStepJ_1,
	NoviceGuideStepJ_2,
	NoviceGuideStepJ_3,
}
