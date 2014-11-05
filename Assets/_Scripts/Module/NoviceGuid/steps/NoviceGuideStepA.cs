using UnityEngine;
using System.Collections;

//
public class NoviceGuideStepA_1:NoviceGuidStep
{
	public override void Enter(){
		nextState = typeof(NoviceGuideStepA_2);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide1_content"),
		                                   "ban_click",true,"rotate",true,   "coor", new Vector3(-100,0,1),
		                                   "callback",  (UICallback)(o=>{
			GoToNextState(true);
		}));
	}
}

//step in the first cell
public class NoviceGuideStepA_2:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepA_3);

		NoviceGuideUtil.ShowArrow(ModuleManager.Instance.GetModule<BattleMapModule> (ModuleEnum.BattleMapModule).GetMapItem(2,1),new Vector3(0,-30,1),true,true,ClickOK);	

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_1"), "coor", new Vector3(-10, 80));
	}
	
	private void ClickOK(GameObject data)
	{
		NoviceGuideUtil.RemoveArrow ((GameObject)data);
		GoToNextState();
	}

}

//fight
public class NoviceGuideStepA_3:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepA_4);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_2"), "coor", new Vector3(-40, 130));
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule,"guide", BattleGuideType.SING_DRAG);
//		UIEventListenerCustom.
		MsgCenter.Instance.AddListener (CommandEnum.FightEnd, OnFightEnd);
	}
	
	private void OnFightEnd(object data)
	{
		GoToNextState();
		MsgCenter.Instance.RemoveListener (CommandEnum.FightEnd, OnFightEnd);

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide2_content"),"coor", new Vector3(40, 200), "callback",  (UICallback)(o=>{
			GoToNextState(true);
		}));

	}
}

//show_key
public class NoviceGuideStepA_4:NoviceGuidStep
{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepA_5);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_3"), "coor", new Vector3(0,90,0));
		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "guide",true, 1);
//		GoToNextState();
	}
}

//gold
public class NoviceGuideStepA_5:NoviceGuidStep{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepA_6);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText("guide3_content"));
		GoToNextState();
	}
}

//show_door
public class NoviceGuideStepA_6:NoviceGuidStep{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepB_1);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_4"),"coor", new Vector3(0, 100));

		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "guide",true, 2);
		GoToNextState();
	}
}