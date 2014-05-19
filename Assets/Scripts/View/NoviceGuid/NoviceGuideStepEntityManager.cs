using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoviceGuideStepEntityManager {

	private Dictionary<NoviceGuideStepEntityID,NoviceGuideStepEntity> stepEntityDic;

	private static NoviceGuideStepEntityManager instance;

	private NoviceGuideStepEntity currentStep;

	private static int currentNoviceGuideStage = 1;

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
				CreateStepEntityByID(NoviceGuideStepEntityID.Loading);
				break;
			case SceneEnum.SelectRole:
				CreateStepEntityByID(NoviceGuideStepEntityID.SElECT_ROLE);
				break;
			case SceneEnum.Scratch:
				CreateStepEntityByID(NoviceGuideStepEntityID.SCRATCH);
				break;
			case SceneEnum.RareScratch:
				CreateStepEntityByID(NoviceGuideStepEntityID.RARE_SCRATCH);
				break;
			case SceneEnum.Quest:
				CreateStepEntityByID(NoviceGuideStepEntityID.QUEST);
				break;
			case SceneEnum.Party:
				CreateStepEntityByID(NoviceGuideStepEntityID.PARTY);
				break;
			case SceneEnum.Units:
				if(currentNoviceGuideStage == 3){
					CreateStepEntityByID(NoviceGuideStepEntityID.UNITS);
				}else if(currentNoviceGuideStage == 2){
					CreateStepEntityByID(NoviceGuideStepEntityID.UNITS,1);
				}else{
				CreateStepEntityByID(NoviceGuideStepEntityID.UNITS,2);	
				}
				break;
			case SceneEnum.Fight:
				CreateStepEntityByID(NoviceGuideStepEntityID.FIGHT);
				break;
			case SceneEnum.LevelUp:
				CreateStepEntityByID(NoviceGuideStepEntityID.LEVEL_UP);
				break;
			case SceneEnum.Evolve:
				CreateStepEntityByID(NoviceGuideStepEntityID.EVOLVE);
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

	public void CreateStepEntityByID(NoviceGuideStepEntityID id,int step = 0)
	{

		if (stepEntityDic.ContainsKey(id)) {
			LogHelper.Log("there is already an stepEntity. ID: " + id.ToString() + ".");
			//if already in state, then goto the next state.
			NextState();
			//return stepEntityDic[id];
		}
		else {

			//bengin a new step, stop the all before steps
			foreach(NoviceGuideStepEntityID stepId in stepEntityDic.Keys)
			{
				stepEntityDic[stepId].IsRunning = false;
			}

			NoviceGuideStepEntity stepEn = null;
			switch(id){			
				case NoviceGuideStepEntityID.Loading:
					stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepA_StateOne.Instance());
					break;
				case NoviceGuideStepEntityID.SElECT_ROLE:
					stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepB_StateOne.Instance());
					break;
				case NoviceGuideStepEntityID.SCRATCH:
					stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepC_StateOne.Instance());
					break;
				case NoviceGuideStepEntityID.RARE_SCRATCH:
					stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepD_StateOne.Instance());
					break;
				case NoviceGuideStepEntityID.QUEST:
					stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepE_StateOne.Instance());
					break;
				case NoviceGuideStepEntityID.PARTY:
					stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepF_StateOne.Instance());
					break;
				case NoviceGuideStepEntityID.UNITS:
					if(step == 0){
						stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepG_StateOne.Instance());
					}else if(step == 1){
						stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepG_StateTwo.Instance());
					}else if(step == 2){
					stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepG_StateThree.Instance());
					}
					break;
				case NoviceGuideStepEntityID.FIGHT:
					stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepH_StateOne.Instance());
					break;
				case NoviceGuideStepEntityID.LEVEL_UP:
					stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepI_StateOne.Instance());
					break;
				case NoviceGuideStepEntityID.EVOLVE:
					stepEn = new NoviceGuideStepEntity(id,NoviceGuideStepJ_StateOne.Instance());
					break;
				default:
					LogHelper.LogError("-----------=========------------there is no such step:" + id.ToString());
					break;
			}
			currentStep = stepEn;
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
