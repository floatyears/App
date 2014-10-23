using UnityEngine;
using System.Collections;

//
public class NoviceGuideStepA_1:NoviceGuidStep
{
	public override void Enter(){
		nextState = typeof(NoviceGuideStepA_2);
		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide1_title"),TextCenter.GetText ("guide1_content"),TextCenter.GetText ("NEXT"),o=>{
			GoToNextState();
		});
	}
}

//step in the first cell
public class NoviceGuideStepA_2:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepA_3);

		NoviceGuideUtil.ShowArrow(ModuleManager.Instance.GetModule<BattleMapModule> (ModuleEnum.BattleMapModule).GetMapItem(2,1),new Vector3(0,0,1),true,true,ClickOK);	

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_1"));
	}
	
	private void ClickOK(GameObject data)
	{
		NoviceGuideUtil.RemoveArrow ((GameObject)data);
		
	}

}

//fight
public class NoviceGuideStepA_3:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepA_4);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_2"));
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule,"guide", 1);
//		UIEventListenerCustom.
		MsgCenter.Instance.AddListener (CommandEnum.FightEnd, OnFightEnd);
	}
	
	private void OnFightEnd(object data)
	{
		MsgCenter.Instance.RemoveListener (CommandEnum.FightEnd, OnFightEnd);
		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText ("guide2_title"), TextCenter.GetText ("guide2_content"), TextCenter.GetText ("NEXT"),o=>{
			GoToNextState();
		});

	}
}

//show_key
public class NoviceGuideStepA_4:NoviceGuidStep
{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepA_5);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_3"));
		GameTimer.GetInstance ().AddCountDown (2f, () => {
			ModuleManager.Instance.HideModule (ModuleEnum.NoviceGuideTipsModule);
		});
		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "guide",true, 1);
	}
}

//gold
public class NoviceGuideStepA_5:NoviceGuidStep{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepA_6);
		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText ("guide3_title"), TextCenter.GetText ("guide3_content"), TextCenter.GetText ("NEXT"));
	}
}

//show_door
public class NoviceGuideStepA_6:NoviceGuidStep{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepB_1);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_4"));

		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "guide",true, 2);
	}
}