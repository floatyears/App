using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NoviceGuideStepManager {

	private int currentStep;

	private static NoviceGuideStage currentNoviceGuideStage = NoviceGuideStage.NONE;
	
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
	
	public bool AddStep(NoviceGuidStep stepState)
	{
		
		if (stepInsDic.ContainsKey (stepState.GetType())) {
			LogHelper.LogError("there is already an " + stepState.GetType().ToString() + " in the dictionary,");
			return false;
		} else {
			stepInsDic.Add(stepState.GetType(),stepState);
			return true;
		}
		
		return false;
	}
	
	public void ChangeState(Type nextState)
	{
		prevState = currentState;

		if (stepInsDic.ContainsKey(prevState)) {
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

	static void ServerCallback(object data){
		Debug.LogWarning ("Novice ServerCallback..");
	}

	public static NoviceGuideStage CurrentNoviceGuideStage
	{
		get{return currentNoviceGuideStage;Debug.Log("current novice guide stage(get): " + currentNoviceGuideStage);}
		set{
//			Umeng.GA.FinishLevel();
			Umeng.GA.StartLevel("Novice" +(int)currentNoviceGuideStage);
			currentNoviceGuideStage = value;
			Debug.Log("current novice guide stage(set): " + currentNoviceGuideStage);
			// the following three stage don't send to server
			if(currentNoviceGuideStage == NoviceGuideStage.EVOLVE || currentNoviceGuideStage == NoviceGuideStage.PARTY || currentNoviceGuideStage == NoviceGuideStage.LEVEL_UP || currentNoviceGuideStage == NoviceGuideStage.EVOVLE_QUEST)
				return;
			UserController.Instance.FinishUserGuide(ServerCallback, (int)currentNoviceGuideStage);

		}
	}
	public static void InitGuideStage(int stage){
		currentNoviceGuideStage = (NoviceGuideStage)stage;

		if (currentNoviceGuideStage == NoviceGuideStage.NONE) {
			MsgCenter.Instance.Invoke(CommandEnum.DestoryGuideMsgWindow);	
		}

		if(NoviceGuideStepManager.isInNoviceGuide())
			Umeng.GA.StartLevel("Novice" +(int)currentNoviceGuideStage);
		Debug.Log("current novice guide stage(start): " + currentNoviceGuideStage);
	}

	public static bool isInNoviceGuide()
	{
		return currentNoviceGuideStage != NoviceGuideStage.NONE && currentNoviceGuideStage!= NoviceGuideStage.UNIT_EVOLVE;
	}
//	
	public static void FinishCurrentStep(){
		UserController.Instance.FinishUserGuide(ServerCallback, (int)currentNoviceGuideStage);
		//currentNoviceGuideStage++;
	}

	public void StartStep(NoviceGuideStartType startType)
	{
		NoviceGuideStage ngs = (NoviceGuideStage) currentNoviceGuideStage;
		if (ngs == NoviceGuideStage.NONE) {
			return;		
		}
		Type nextType;
		if (startType == NoviceGuideStartType.BATTLE) {
			switch (ngs) {
			case NoviceGuideStage.GOLD_BOX://goldBox
				nextType = typeof(NoviceGuideStepH_1);
					break;
			case NoviceGuideStage.GET_KEY://GetKey
				nextType = typeof(NoviceGuideStepD_6);
					break;
			case NoviceGuideStage.EVOVLE_BATTLE:
				nextType = typeof(NoviceGuideStepL_1);
					break;

			}
		} else if (startType == NoviceGuideStartType.FIGHT) {
			switch (ngs) {
			//Fist Enemy
			case NoviceGuideStage.ANIMATION://Animation
				nextType = typeof(NoviceGuideStepD_1);
					break;
			case NoviceGuideStage.FIRST_ATTACK_ONE://AttackOnce
				nextType = typeof(NoviceGuideStepD_2);
					break;
			case NoviceGuideStage.FIRST_ATTACK_TWO://AttackTwice
				nextType = typeof(NoviceGuideStepD_4);
					break;
			//Boss Enemy
			case NoviceGuideStage.BOSS_ATTACK_ONE://AttackOnce
				nextType = typeof(NoviceGuideStepK_1);
					break;
			case NoviceGuideStage.BOSS_ATTACK_HEAL://Heal
				nextType = typeof(NoviceGuideStepK_3);
					break;
			case NoviceGuideStage.BOSS_ATTACK_SKILL://Skill
				nextType = typeof(NoviceGuideStepK_4);
					break;
			case NoviceGuideStage.BOSS_ATTACK_BOOST://Booost
				nextType = typeof(NoviceGuideStepK_5);
					break;
			}
		} else if (startType == NoviceGuideStartType.UNITS) {
			switch (ngs) {
			case NoviceGuideStage.UNIT_PARTY:
				nextType = typeof(NoviceGuideStepG_1);
					break;
			case NoviceGuideStage.UNIT_LEVEL_UP:
				nextType = typeof(NoviceGuideStepG_2);
					break;
			case NoviceGuideStage.UNIT_EVOLVE_EXE:
				nextType = typeof(NoviceGuideStepG_3);
					break;
			case NoviceGuideStage.PARTY://Party
				nextType = typeof(NoviceGuideStepF_1);
					break;
			case NoviceGuideStage.LEVEL_UP://LevelUp
				nextType = typeof(NoviceGuideStepI_1);
					break;
			case NoviceGuideStage.EVOLVE://Evolve
				nextType = typeof(NoviceGuideStepJ_1);
					break;
			}
		} else if (startType == NoviceGuideStartType.QUEST) {
		
			switch (ngs) {
				case NoviceGuideStage.EVOVLE_QUEST:
				nextType = typeof(NoviceGuideStepM_2);
					break;
				case NoviceGuideStage.FRIEND_SELECT:
				nextType = typeof(NoviceGuideStepN_1);
					break;
			}
		} else if (startType == NoviceGuideStartType.SCRATCH && ngs == NoviceGuideStage.SCRATCH) {
			nextType = typeof(NoviceGuideStepC_1);
		}else if (startType == NoviceGuideStartType.OTHERS) {
			switch (ngs) {
				case NoviceGuideStage.INPUT_NAME://InputName
				nextType = typeof(NoviceGuideStepE_1);
						break;
				default:
						break;
			}
		}
	}

	private bool RemoveStepEntity<T>() where T : NoviceGuidStep
	{
		if(stepInsDic.ContainsKey(typeof(T)))
		{
			stepInsDic.Remove(typeof(T));
			return true;
		}else{
			return false;
		}
		return false;
	}

	private NoviceGuidStep GetStepEntityById<T>() where T : NoviceGuidStep
	{
		if (stepInsDic.ContainsKey (typeof(T)))
			return stepInsDic [typeof(T)];
		else {
			return null;
		}
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
	BATTLE,
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
	NONE = -1,
//	Preface,
//	SELECT_ROLE,
	GOLD_BOX = 0,
	ANIMATION,
	FIRST_ATTACK_ONE,
	FIRST_ATTACK_TWO,
	GET_KEY,
	BOSS_ATTACK_ONE,
	BOSS_ATTACK_HEAL,
	BOSS_ATTACK_SKILL,
	BOSS_ATTACK_BOOST,
	UNIT_PARTY,
	UNIT_LEVEL_UP = 10,
	UNIT_EVOLVE,
	UNIT_EVOLVE_EXE,
	PARTY,
	LEVEL_UP,
	SCRATCH = 15,
	INPUT_NAME,
	FRIEND_SELECT = 17,
//	QUEST_SELECT,
	EVOLVE,
	EVOVLE_BATTLE,
	EVOVLE_QUEST = 20
}
