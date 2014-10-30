using UnityEngine;
using System.Collections;

//battle
public class NoviceGuideStepF_1:NoviceGuidStep{
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepF_2);

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_19"));
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "guide", BattleGuideType.MULTI_DRAG);

		GoToNextState();
	}
}

//sp
public class NoviceGuideStepF_2:NoviceGuidStep{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepF_3);

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_22"));

		GoToNextState();
//		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText ("guide7_title"), TextCenter.GetText ("guide7_content"), TextCenter.GetText ("NEXT"),o=>{
//			GoToNextState();
//		});
	}
	
}


//boss - heal hp
public class NoviceGuideStepF_3:NoviceGuidStep{

	private bool stepNext = false;

	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepF_4);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, OnAttackEnd);
	}

	private void OnAttackEnd(object data){
		GoToNextState();
		if (!stepNext) {
			stepNext = true;
			ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_21"));
			ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "guide", BattleGuideType.HP_DRAG);
		}
		else{
			MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, OnAttackEnd);
			ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_20"));
			ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "guide", BattleGuideType.BOOST_DRAG);
		}

	}
}
//boss - shadow
public class NoviceGuideStepF_4:NoviceGuidStep{
	
	public override void Enter()
	{
		nextState = nextState = null;
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("guide7_title"),TextCenter.GetText ("guide7_content"),TextCenter.GetText ("OK"));
		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "clear_quest_highlight");
		NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.BLANK;
	}
}





