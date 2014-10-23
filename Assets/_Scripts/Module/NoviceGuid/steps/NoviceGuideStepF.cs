using UnityEngine;
using System.Collections;

//battle
public class NoviceGuideStepF_1:NoviceGuidStep{
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepF_2);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_19"));
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "guide", 3);
	}

}

//boss
public class NoviceGuideStepF_2:NoviceGuidStep{

	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepF_3);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_20"));
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "guide", 2);
	}
}

public class NoviceGuideStepF_3:NoviceGuidStep{

	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepF_4);
		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText ("guide6_title"), TextCenter.GetText ("guide6_content"), TextCenter.GetText ("NEXT"),o=>{
			GoToNextState();
		});

	}
}

//boss
public class NoviceGuideStepF_4:NoviceGuidStep{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepF_5);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_20"));
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "guide", 4);
	}

}

//sp
public class NoviceGuideStepF_5:NoviceGuidStep{
	
	public override void Enter()
	{
		nextState = null;

		NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.BLANK;
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_20"));
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "guide", 5); //sp

		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText ("guide6_title"), TextCenter.GetText ("guide6_content"), TextCenter.GetText ("NEXT"),o=>{
			GoToNextState();
		});
	}
	
}




