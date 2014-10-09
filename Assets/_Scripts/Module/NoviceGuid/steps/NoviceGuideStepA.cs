using UnityEngine;
using System.Collections;

public class NoviceGuideStepA_1:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = null;
		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText("guide1_title"),TextCenter.GetText("guide1_content"),TextCenter.GetText("NEXT"),ClickOK);
	}

	private void ClickOK(object data)
	{
		LogHelper.Log("goto the selectrole");
		//selectRole
		//NoviceGuideStepEntityManager.FinishCurrentStep ();
//		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.SELECT_ROLE;
		ModuleManager.Instance.ShowModule (ModuleEnum.SelectRoleModule);
	}

}