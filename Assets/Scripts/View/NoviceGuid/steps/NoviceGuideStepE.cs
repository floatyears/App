using UnityEngine;
using System.Collections;

public class NoviceGuideStepE_StateOne:NoviceGuidState
{
	private LayerMask camLastLayer;

	private int btnLastLayer;

	private bool isCityTapped;

	private static NoviceGuideStepE_StateOne instance;
	
	public static NoviceGuideStepE_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepE_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepE_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepE state_one");
	

//		QuestView qv = GameObject.Find ("HomeWorldMap(Clone)").GetComponent<QuestView>();
//		GameObject cityObj =  qv.GetCityItem (0);
//		NoviceGuideUtil.ForceOneBtnClick (cityObj);
//
//		UIEventListener.Get (cityObj).onClick += TapCityItem;

	}
	
	private void TapCityItem(GameObject btn)
	{
		UIEventListener.Get (btn).onClick -= TapCityItem;
		
		isCityTapped = true;
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
