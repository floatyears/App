using UnityEngine;
using System.Collections;

//level up
public class NoviceGuideStepE_1:NoviceGuidStep
{

	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepE_2);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_16"));
		
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("unit_btn"), new Vector3 (0, 0, 1), true, true, o1 => {
			NoviceGuideUtil.RemoveAllArrows ();
			GoToNextState();
		});
	}

}

//level up
public class NoviceGuideStepE_2:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepE_3);
//		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_17"));

		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("unit_list"), new Vector3 (0, 0, 1), true, true, o1 => {
			NoviceGuideUtil.RemoveAllArrows ();
			GoToNextState();
		});
	}
	
}

//select leader
public class NoviceGuideStepE_3:NoviceGuidStep
{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepE_4);
		MsgCenter.Instance.Invoke(CommandEnum.SortByRule, SortRule.ID);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_17"));
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("unit_leader_levelup"), new Vector3(0,0,3),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
			GoToNextState();
		});
	}
}

//click auto
public class NoviceGuideStepE_4:NoviceGuidStep
{
	public override void Enter ()
	{
		nextState = null;
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_18"));
		
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("auto_select"), new Vector3(0,0,1),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
			NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("level_up_btn"), new Vector3(0,0,1),true,true,o1=>{
				NoviceGuideUtil.RemoveAllArrows();
				NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.BLANK;
			});
		});
	}
}

