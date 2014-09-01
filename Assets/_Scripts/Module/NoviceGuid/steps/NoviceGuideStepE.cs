using UnityEngine;
using System.Collections;

public class NoviceGuideStepE_StateOne:NoviceGuidState
{
	private LayerMask camLastLayer;

	private int btnLastLayer;

	private bool changeEnd = false;

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
		MsgCenter.Instance.AddListener (CommandEnum.ChangeSceneComplete,OnChange);

		UIManager.Instance.ChangeScene (ModuleEnum.Others);
	}

	void OnChange(object data){
//		if (UIManager.Instance.baseScene.CurrentScene == ModuleEnum.Others) {
//			if (!changeEnd) {
//				UIManager.Instance.ChangeScene (ModuleEnum.NickName);
//			}else{
//				NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.UNIT_EVOLVE;
//				GameObject.Find("NickNameWindow(Clone)/CancelButton").GetComponent<UIButton>().isEnabled = true;
//				MsgCenter.Instance.RemoveListener (CommandEnum.ChangeSceneComplete,OnChange);
//
//				UIManager.Instance.ChangeScene(ModuleEnum.Home);
//			}
//
//		} else if(UIManager.Instance.baseScene.CurrentScene == ModuleEnum.NickName) {
//			GameObject.Find("NickNameWindow(Clone)/CancelButton").GetComponent<UIButton>().isEnabled = false;
//			changeEnd = true;
//		}

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
