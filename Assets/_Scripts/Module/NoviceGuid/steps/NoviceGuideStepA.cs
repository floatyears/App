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
		GameObject mi = ModuleManager.Instance.GetModule<BattleMapModule> (ModuleEnum.BattleMapModule).GetMapItem(2,1);//GameObject.FindWithTag ("map_item");

		NoviceGuideUtil.ShowArrow(new GameObject[]{mi},new Vector3[]{new Vector3(0,0,1)},true);	
		NoviceGuideUtil.ForceOneBtnClick (mi);
		UIEventListenerCustom.Get (mi).onClick += ClickOK;

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_1"));
	}
	
	private void ClickOK(GameObject data)
	{
		NoviceGuideUtil.RemoveArrow ((GameObject)data);
		ModuleManager.Instance.HideModule (ModuleEnum.NoviceGuideTipsModule);
		UIEventListenerCustom.Get (data).onClick -= ClickOK;
		
	}

}

//fight
public class NoviceGuideStepA_3:NoviceGuidStep
{
	
	public override void Enter()
	{
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_2"));
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule,"guide", 1);
//		UIEventListenerCustom.
		MsgCenter.Instance.AddListener (CommandEnum.FightEnd, OnFightEnd);
	}
	
	private void OnFightEnd(object data)
	{
		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText ("guide2_title"), TextCenter.GetText ("guide2_content"), TextCenter.GetText ("NEXT"),o=>{
			GoToNextState();
		});
	}
}

//key
public class NoviceGuideStepA_4:NoviceGuidStep
{
	public override void Enter ()
	{
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_3"));
		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "guide", 1);
	}
}

public class NoviceGuideStepA_5:NoviceGuidStep{
	public override void Enter ()
	{

	}
}