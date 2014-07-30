using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoviceGuideStepEntityManager {

	private Dictionary<NoviceGuideStepEntityID,NoviceGuideStepEntity> stepEntityDic;

	private static NoviceGuideStepEntityManager instance;

	private NoviceGuideStepEntity currentStep;

	private static NoviceGuideStage currentNoviceGuideStage = NoviceGuideStage.NONE;

	private NoviceGuideStepEntityManager()
	{
		stepEntityDic = new Dictionary<NoviceGuideStepEntityID, NoviceGuideStepEntity> ();
		GameInput.OnUpdate += Update;

		//MsgCenter.Instance.AddListener(CommandEnum.ChangeScene,OnChangeScene);
	}


	public static NoviceGuideStepEntityManager Instance()
	{
		if (instance == null) {

			instance = new NoviceGuideStepEntityManager ();

		}
		return instance;
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
			if(currentNoviceGuideStage == NoviceGuideStage.EVOLVE || currentNoviceGuideStage == NoviceGuideStage.PARTY || currentNoviceGuideStage == NoviceGuideStage.LEVEL_UP)
				return;
			FinishUserGuide.SendRequest(ServerCallback, (int)currentNoviceGuideStage);

		}
	}
	public static void InitGuideStage(int stage){
		currentNoviceGuideStage = (NoviceGuideStage)stage;
		if(NoviceGuideStepEntityManager.isInNoviceGuide())
			Umeng.GA.StartLevel("Novice" +(int)currentNoviceGuideStage);
		Debug.Log("current novice guide stage(start): " + currentNoviceGuideStage);
	}

	public static bool isInNoviceGuide()
	{
		return currentNoviceGuideStage != NoviceGuideStage.NONE && currentNoviceGuideStage!= NoviceGuideStage.UNIT_EVOLVE;
	}
//	
	public static void FinishCurrentStep(){
		FinishUserGuide.SendRequest (ServerCallback, (int)currentNoviceGuideStage);
		//currentNoviceGuideStage++;
	}


//	private void OnChangeScene(object msg)
//	{
//		
//		SceneEnum se = (SceneEnum)msg;
//		Debug.Log ("change scene to: " + se.ToString ());
//		switch (se) {
//		case SceneEnum.SelectRole:
//			CreateStepEntityByID(NoviceGuideStepEntityID.StepA);
//			break;
//		}
//	}
	
	public void StartStep(NoviceGuideStartType startType)
	{
//		return;
//		if (currentNoviceGuideStage <= 0) {
//			return;	
//		}
//
//		Debug.Log("//////////current scene: " + UIManager.Instance.baseScene.CurrentScene);
//		switch (UIManager.Instance.baseScene.CurrentScene) {
//			case SceneEnum.Loading:
//				CreateStepEntityByID(NoviceGuideStepEntityID.Loading,NoviceGuideStepA_StateOne.Instance());
//				break;
//			case SceneEnum.SelectRole:
//				CreateStepEntityByID(NoviceGuideStepEntityID.SElECT_ROLE,NoviceGuideStepB_StateOne.Instance());
//				break;
//			case SceneEnum.Scratch:
//				CreateStepEntityByID(NoviceGuideStepEntityID.SCRATCH,NoviceGuideStepC_StateOne.Instance());
//				break;
//			case SceneEnum.RareScratch:
//				CreateStepEntityByID(NoviceGuideStepEntityID.RARE_SCRATCH,NoviceGuideStepD_StateOne.Instance());
//				break;
//			case SceneEnum.Quest:
//				CreateStepEntityByID(NoviceGuideStepEntityID.QUEST,NoviceGuideStepE_StateOne.Instance());
//				break;
//			case SceneEnum.Party:
//				CreateStepEntityByID(NoviceGuideStepEntityID.PARTY,NoviceGuideStepF_StateOne.Instance());
//				break;
//			case SceneEnum.Units:
//				if(currentNoviceGuideStage == 16){
//					CreateStepEntityByID(NoviceGuideStepEntityID.UNITS,NoviceGuideStepG_StateThree.Instance());
//				}else if(currentNoviceGuideStage == 15){
//					CreateStepEntityByID(NoviceGuideStepEntityID.UNITS,NoviceGuideStepG_StateTwo.Instance());
//				}else if(currentNoviceGuideStage == 14){
//					CreateStepEntityByID(NoviceGuideStepEntityID.UNITS,NoviceGuideStepG_StateOne.Instance());	
//				}
//				break;
//			case SceneEnum.Fight:
//				
//				CreateStepEntityByID(NoviceGuideStepEntityID.FIGHT,NoviceGuideStepH_StateOne.Instance());
//				break;
//			case SceneEnum.LevelUp:
//				CreateStepEntityByID(NoviceGuideStepEntityID.LEVEL_UP,NoviceGuideStepI_StateOne.Instance());
//				break;
//			case SceneEnum.Evolve:
//				CreateStepEntityByID(NoviceGuideStepEntityID.EVOLVE,NoviceGuideStepJ_StateOne.Instance());
//				break;
//		}
//		Debug.Log ("current stage: " + currentNoviceGuideStage);
		NoviceGuideStage ngs = (NoviceGuideStage) currentNoviceGuideStage;
		if (ngs == NoviceGuideStage.NONE) {
			return;		
		}
		if (startType == NoviceGuideStartType.BATTLE) {
						switch (ngs) {
						case NoviceGuideStage.GOLD_BOX://goldBox
								CreateStepEntityByID (NoviceGuideStepEntityID.FIGHT, NoviceGuideStepH_StateOne.Instance ());
								break;
						case NoviceGuideStage.GET_KEY://GetKey
								CreateStepEntityByID (NoviceGuideStepEntityID.FIGHT, NoviceGuideStepD_StateSix.Instance ());
								break;
						case NoviceGuideStage.EVOVLE_BATTLE:
								CreateStepEntityByID (NoviceGuideStepEntityID.EVOLVE, NoviceGuideStepL_StateOne.Instance ());
								break;

						}
				} else if (startType == NoviceGuideStartType.FIGHT) {
						switch (ngs) {
						//Fist Enemy
						case NoviceGuideStage.ANIMATION://Animation
								CreateStepEntityByID (NoviceGuideStepEntityID.FIGHT, NoviceGuideStepD_StateOne.Instance ());
								break;
						case NoviceGuideStage.FIRST_ATTACK_ONE://AttackOnce
								CreateStepEntityByID (NoviceGuideStepEntityID.FIGHT, NoviceGuideStepD_StateTwo.Instance ());
								break;
						case NoviceGuideStage.FIRST_ATTACK_TWO://AttackTwice
								CreateStepEntityByID (NoviceGuideStepEntityID.FIGHT, NoviceGuideStepD_StateFour.Instance ());
								break;
						//Boss Enemy
						case NoviceGuideStage.BOSS_ATTACK_ONE://AttackOnce
								CreateStepEntityByID (NoviceGuideStepEntityID.FIGHT_BOSS, NoviceGuideStepK_StateOne.Instance ());
								break;
						case NoviceGuideStage.BOSS_ATTACK_HEAL://Heal
								CreateStepEntityByID (NoviceGuideStepEntityID.FIGHT_BOSS, NoviceGuideStepK_StateThree.Instance ());
								break;
						case NoviceGuideStage.BOSS_ATTACK_SKILL://Skill
								CreateStepEntityByID (NoviceGuideStepEntityID.FIGHT_BOSS, NoviceGuideStepK_StateFour.Instance ());
								break;
						case NoviceGuideStage.BOSS_ATTACK_BOOST://Booost
								CreateStepEntityByID (NoviceGuideStepEntityID.FIGHT_BOSS, NoviceGuideStepK_StateFive.Instance ());
								break;
						}
				} else if (startType == NoviceGuideStartType.UNITS) {
						switch (ngs) {
						case NoviceGuideStage.UNIT_PARTY:
								CreateStepEntityByID (NoviceGuideStepEntityID.UNITS, NoviceGuideStepG_StateOne.Instance ());
								break;
						case NoviceGuideStage.UNIT_LEVEL_UP:
								CreateStepEntityByID (NoviceGuideStepEntityID.UNITS, NoviceGuideStepG_StateTwo.Instance ());
								break;
						case NoviceGuideStage.UNIT_EVOLVE:
								CreateStepEntityByID (NoviceGuideStepEntityID.UNITS, NoviceGuideStepG_StateThree.Instance ());
								break;
						case NoviceGuideStage.PARTY://Party
								CreateStepEntityByID (NoviceGuideStepEntityID.PARTY, NoviceGuideStepF_StateOne.Instance ());
								break;
						case NoviceGuideStage.LEVEL_UP://LevelUp
								CreateStepEntityByID (NoviceGuideStepEntityID.LEVEL_UP, NoviceGuideStepI_StateOne.Instance ());
								break;
						case NoviceGuideStage.EVOLVE://Evolve
								CreateStepEntityByID (NoviceGuideStepEntityID.EVOLVE, NoviceGuideStepJ_StateOne.Instance ());
								break;
						}
				} else if (startType == NoviceGuideStartType.QUEST) {
				
						switch (ngs) {
						case NoviceGuideStage.EVOVLE_QUEST:
								CreateStepEntityByID (NoviceGuideStepEntityID.QUEST, NoviceGuideStepM_StateTwo.Instance ());
								break;
						case NoviceGuideStage.FRIEND_SELECT:
							CreateStepEntityByID (NoviceGuideStepEntityID.QUEST, NoviceGuideStepM_StateOne.Instance ());
							break;
						}
				} else if (startType == NoviceGuideStartType.SCRATCH && ngs == NoviceGuideStage.SCRATCH) {
					CreateStepEntityByID (NoviceGuideStepEntityID.SCRATCH, NoviceGuideStepC_StateOne.Instance ());
					
			}else if (startType == NoviceGuideStartType.OTHERS) {
					switch (ngs) {
//		case NoviceGuideStage.Preface://preface
//				CreateStepEntityByID(NoviceGuideStepEntityID.Loading,NoviceGuideStepA_StateOne.Instance());
//				break;
//		case NoviceGuideStage.SELECT_ROLE://selectRole
//			CreateStepEntityByID(NoviceGuideStepEntityID.SElECT_ROLE,NoviceGuideStepB_StateOne.Instance());
//			break;
				case NoviceGuideStage.INPUT_NAME://InputName
						CreateStepEntityByID (NoviceGuideStepEntityID.INPUT_NAME, NoviceGuideStepE_StateOne.Instance ());
						break;
//				case NoviceGuideStage.QUEST_SELECT://QuestSelect
//						CreateStepEntityByID (NoviceGuideStepEntityID.QUEST, NoviceGuideStepE_StateOne.Instance ());
//						break;
//
//				case NoviceGuideStage.EVOVLE_BATTLE://EvovlveBattle
//						break;
//			case novi
//		case 19://selectRole
//			break;
//		case 20://selectRole
//			break;
				default:
						break;
				}
		}
	}

	private void NextState()
	{
		//return;
		if (currentStep != null ) {
			if(currentStep.GetStateMachine ().CurrentState != null){
				currentStep.GetStateMachine ().CurrentState.JumpToNextState = true;
			}else{
				LogHelper.Log("current state is null");
			}
		}
			
	}

	public void ReleaseUpdate()
	{
		GameInput.OnUpdate -= Update;
	}

	public bool AddStepEntity(NoviceGuideStepEntity stepEntity)
	{
		if(stepEntityDic.ContainsKey(stepEntity.ID))
		{
			LogHelper.Log("there is already an stepEntity" + stepEntity.ID.ToString() + ".");
			return false;
		}else{
			stepEntityDic.Add (stepEntity.ID, stepEntity);
			return true;
		}
		return false;
	}

	public bool RemoveStepEntity(NoviceGuideStepEntity stepEntity)
	{
		if(stepEntityDic.ContainsKey(stepEntity.ID))
		{
			stepEntityDic.Remove(stepEntity.ID);
			return true;
		}else{
			LogHelper.LogError("there is no such a stepEntity to remove,ID:"+stepEntity.ID);
			return false;
		}
		return false;
	}

	public NoviceGuideStepEntity GetStepEntityById(NoviceGuideStepEntityID id)
	{
		if (stepEntityDic.ContainsKey (id))
			return stepEntityDic [id];
		else {
			LogHelper.LogError("there is no such a stepEntity,ID:" + id.ToString());
			return null;
		}
	}

	public void CreateStepEntityByID(NoviceGuideStepEntityID id,NoviceGuidState state)
	{

		if (stepEntityDic.ContainsKey(id)) {
			LogHelper.Log("there is already an stepEntity. ID: " + id.ToString() + ".");
			//if already in state, then goto the next state.
			//NextState();
			//return stepEntityDic[id];
			if(stepEntityDic[id].StartState != state){
				Debug.Log("the start state isn't the same, start: " + stepEntityDic[id].StartState+" next is: " + state);
				stepEntityDic[id].GetStateMachine().CurrentState = state;
				stepEntityDic[id].GetStateMachine().IsRunning = true;
			} else{
				Debug.Log("start state is the same:" + state + ", goto the next state");
				NextState();
				stepEntityDic[id].GetStateMachine().IsRunning = true;

			}//GetStateMachine().CurrentState = state;
		}
		else {

			//bengin a new step, stop the all before steps
			foreach(NoviceGuideStepEntityID stepId in stepEntityDic.Keys)
			{
				stepEntityDic[stepId].IsRunning = false;
			}

			currentStep = new NoviceGuideStepEntity(id,state);		
			//return stepEn;

		}
		//return null;
	}

	public void Update()
	{
		foreach (NoviceGuideStepEntity entity in stepEntityDic.Values) {
			if(entity.IsRunning)
				entity.Update();
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
	GOLD_BOX,
	ANIMATION,
	FIRST_ATTACK_ONE,
	FIRST_ATTACK_TWO,
	GET_KEY,
	BOSS_ATTACK_ONE,
	BOSS_ATTACK_HEAL,
	BOSS_ATTACK_SKILL,
	BOSS_ATTACK_BOOST,
	UNIT_PARTY,
	UNIT_LEVEL_UP,
	UNIT_EVOLVE,
	PARTY,
	LEVEL_UP,
	SCRATCH,
	INPUT_NAME,
	FRIEND_SELECT,
//	QUEST_SELECT,
	EVOLVE,
	EVOVLE_BATTLE,
	EVOVLE_QUEST
}
