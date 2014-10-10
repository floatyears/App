using UnityEngine;
using System.Collections;

public class NoviceGuideStepE_1:NoviceGuidStep
{
	private LayerMask camLastLayer;

	private int btnLastLayer;

	private bool changeEnd = false;

	public override void Enter()
	{
	
		nextState = null;
//		QuestView qv = GameObject.Find ("HomeWorldMap(Clone)").GetComponent<QuestView>();
//		GameObject cityObj =  qv.GetCityItem (0);
//		NoviceGuideUtil.ForceOneBtnClick (cityObj);
//
//		UIEventListenerCustom.Get (cityObj).onClick += TapCityItem;
		MsgCenter.Instance.AddListener (CommandEnum.ChangeSceneComplete,OnChange);

		ModuleManager.Instance.ShowModule (ModuleEnum.OthersModule);
	}

	void OnChange(object data){
//		if (UIManager.Instance.baseScene.CurrentScene == ModuleEnum.Others) {
//			if (!changeEnd) {
//				ModuleManger.Instance.ShowModule (ModuleEnum.NickName);
//			}else{
//				NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.UNIT_EVOLVE;
//				GameObject.Find("NickNameWindow(Clone)/CancelButton").GetComponent<UIButton>().isEnabled = true;
//				MsgCenter.Instance.RemoveListener (CommandEnum.ChangeSceneComplete,OnChange);
//
//				ModuleManger.Instance.ShowModule(ModuleEnum.Home);
//			}
//
//		} else if(UIManager.Instance.baseScene.CurrentScene == ModuleEnum.NickName) {
//			GameObject.Find("NickNameWindow(Clone)/CancelButton").GetComponent<UIButton>().isEnabled = false;
//			changeEnd = true;
//		}

	}

}
