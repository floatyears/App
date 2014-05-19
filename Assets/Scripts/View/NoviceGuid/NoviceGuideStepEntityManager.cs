using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoviceGuideStepEntityManager {

	private Dictionary<NoviceGuideStepEntityID,NoviceGuideStepEntity> stepEntityDic;

	private static NoviceGuideStepEntityManager instance;

	private NoviceGuideStepEntity currentStep;

	private static int currentNoviceGuideStage = 2;

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

	public static int CurrentNoviceGuideStage
	{
		get{return currentNoviceGuideStage;}
		set{currentNoviceGuideStage = value;}
	}

	public static bool isInNoviceGuide()
	{
		return currentNoviceGuideStage > 0;
	}
//	
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
	
	public void StartStep()
	{
		//return;
		Debug.Log("//////////current scene: " + UIManager.Instance.baseScene.CurrentScene);
		switch (UIManager.Instance.baseScene.CurrentScene) {
			case SceneEnum.Loading:
				CreateStepEntityByID(NoviceGuideStepEntityID.Loading,NoviceGuideStepA_StateOne.Instance());
				break;
			case SceneEnum.SelectRole:
				CreateStepEntityByID(NoviceGuideStepEntityID.SElECT_ROLE,NoviceGuideStepB_StateOne.Instance());
				break;
			case SceneEnum.Scratch:
				CreateStepEntityByID(NoviceGuideStepEntityID.SCRATCH,NoviceGuideStepC_StateOne.Instance());
				break;
			case SceneEnum.RareScratch:
				CreateStepEntityByID(NoviceGuideStepEntityID.RARE_SCRATCH,NoviceGuideStepD_StateOne.Instance());
				break;
			case SceneEnum.Quest:
				CreateStepEntityByID(NoviceGuideStepEntityID.QUEST,NoviceGuideStepE_StateOne.Instance());
				break;
			case SceneEnum.Party:
				CreateStepEntityByID(NoviceGuideStepEntityID.PARTY,NoviceGuideStepF_StateOne.Instance());
				break;
			case SceneEnum.Units:
				if(currentNoviceGuideStage == 3){
					CreateStepEntityByID(NoviceGuideStepEntityID.UNITS,NoviceGuideStepG_StateThree.Instance());
				}else if(currentNoviceGuideStage == 2){
					CreateStepEntityByID(NoviceGuideStepEntityID.UNITS,NoviceGuideStepG_StateTwo.Instance());
				}else{
					CreateStepEntityByID(NoviceGuideStepEntityID.UNITS,NoviceGuideStepG_StateOne.Instance());	
				}
				break;
			case SceneEnum.Fight:
				CreateStepEntityByID(NoviceGuideStepEntityID.FIGHT,NoviceGuideStepH_StateOne.Instance());
				break;
			case SceneEnum.LevelUp:
				CreateStepEntityByID(NoviceGuideStepEntityID.LEVEL_UP,NoviceGuideStepI_StateOne.Instance());
				break;
			case SceneEnum.Evolve:
				CreateStepEntityByID(NoviceGuideStepEntityID.EVOLVE,NoviceGuideStepJ_StateOne.Instance());
				break;
		}
	}

	public void NextState()
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
				LogHelper.Log("there is already an stepEntity. ID:"+id.ToString()+"but the start state isn't the same, start: " + stepEntityDic[id].StartState+" next is: " + state);
				stepEntityDic[id].GetStateMachine().CurrentState = state;
				stepEntityDic[id].GetStateMachine().IsRunning = true;
			} //GetStateMachine().CurrentState = state;
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

public enum NoviceGuideStepEntityID{

	Loading = 1,
	SElECT_ROLE = 2,
	SCRATCH = 3,
	RARE_SCRATCH = 4,
	QUEST = 5,
	PARTY = 6,
	UNITS = 7,
	FIGHT = 8,
	LEVEL_UP,
	EVOLVE
}
