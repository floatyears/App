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
		Debug.Log("//////////current scene: " + UIManager.Instance.current.CurrentDecoratorScene);
		switch (UIManager.Instance.current.CurrentDecoratorScene) {
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
		}
	}

	public void NextState()
	{
		if(currentStep != null)
			currentStep.GetStateMachine ().CurrentState.JumpToNextState = true;
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

	public NoviceGuideStepEntity CreateStepEntityByID(NoviceGuideStepEntityID id)
	{

		if (stepEntityDic.ContainsKey(id)) {
			LogHelper.Log("there is already an stepEntity. ID: " + id.ToString() + ".");
			return stepEntityDic[id];
		}
		else {

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
				default:
					LogHelper.LogError("-----------=========------------there is no such step:" + id.ToString());
					break;
			}
			currentStep = stepEn;
			return stepEn;

		}
		return null;
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
	RARE_SCRATCH = 4
}
