using UnityEngine;
using System.Collections;

//city
public class NoviceGuideStepB_1:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepB_2);

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_5"));
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("city_one"), new Vector3(0,0,1),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
			ModuleManager.Instance.HideModule(ModuleEnum.NoviceGuideTipsModule);
		});
	}

}

//stage
public class NoviceGuideStepB_2:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepB_3);
		NoviceGuideUtil.ForceOneBtnClick (GameObject.FindWithTag("stage_new"),null);
	}

}

//quest_select
public class NoviceGuideStepB_3:NoviceGuidStep
{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepB_4);
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("quest_new"), new Vector3 (0, 0, 3),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
		});
	}
}

//fight ready
public class NoviceGuideStepB_4:NoviceGuidStep{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepB_5);
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("fight_btn"), new Vector3 (0, 0, 1),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
		});
	}
}

//key
public class NoviceGuideStepB_5:NoviceGuidStep
{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepC_1);

		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText ("guide4_title"), TextCenter.GetText ("guide4_content"), TextCenter.GetText ("NEXT"));
		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "guide",true, 1);
	}
}