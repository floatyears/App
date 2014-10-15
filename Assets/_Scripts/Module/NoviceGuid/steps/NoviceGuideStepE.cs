using UnityEngine;
using System.Collections;

public class NoviceGuideStepE_1:NoviceGuidStep
{

	public override void Enter()
	{
	
		nextState = typeof(NoviceGuideStepE_2);

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_15"));

		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("unit_btn"), new Vector3(0,0,1),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
			ModuleManager.Instance.HideModule(ModuleEnum.NoviceGuideTipsModule);
		});
	}

}

public class NoviceGuideStepE_2:NoviceGuidStep
{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepE_3);
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("level_up"), new Vector3(0,0,1),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
		});
	}
}

public class NoviceGuideStepE_3:NoviceGuidStep
{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepE_4);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_16"));
		
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("unit_leader"), new Vector3(0,0,1),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
			ModuleManager.Instance.HideModule(ModuleEnum.NoviceGuideTipsModule);
		});
	}
}

//select material
public class NoviceGuideStepE_4:NoviceGuidStep
{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepE_5);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_17"));
		
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("unit_leader"), new Vector3(0,0,1),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
			ModuleManager.Instance.HideModule(ModuleEnum.NoviceGuideTipsModule);
		});
	}
}

//select friend
public class NoviceGuideStepE_5:NoviceGuidStep
{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepE_6);
	}
}

public class NoviceGuideStepE_6:NoviceGuidStep
{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepE_5);
	}
}
