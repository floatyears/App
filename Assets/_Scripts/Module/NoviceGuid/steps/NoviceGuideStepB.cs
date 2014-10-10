using UnityEngine;
using System.Collections;

public class NoviceGuideStepB_1:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepB_2);

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide2_title"),TextCenter.GetText ("guide2_content"),TextCenter.GetText ("OK"));
	}

}

public class NoviceGuideStepB_2:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = null;

		TipsManager.Instance.ShowGuideMsgWindow( TextCenter.GetText ("guide3_title"),TextCenter.GetText ("guide3_content"), TextCenter.GetText ("OK"),InputName);
	}

	private void InputName(object data)
	{
		uint unitID = DataCenter.Instance.UnitData.GetUnitInfo(9).id;
		ModuleManager.SendMessage (ModuleEnum.LoadingModule, "func", "FirstLogin", "data", unitID);
		NoviceGuideStepManager.Instance.FinishCurrentStep ();
	}

}