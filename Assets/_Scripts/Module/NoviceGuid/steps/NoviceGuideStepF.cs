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

//sp
public class NoviceGuideStepF_2:NoviceGuidStep{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepF_3);
		
		NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.BLANK;
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_22"));
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "guide", 5); //sp
		
//		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText ("guide7_title"), TextCenter.GetText ("guide7_content"), TextCenter.GetText ("NEXT"),o=>{
//			GoToNextState();
//		});
	}
	
}


//boss - heal hp
public class NoviceGuideStepF_3:NoviceGuidStep{

	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepF_3);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, OnAttackEnd);

	}

	private void OnAttackEnd(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, OnAttackEnd);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_21"));
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "guide", 2);
	}
}
//boss - boost
public class NoviceGuideStepF_4:NoviceGuidStep{
	
	public override void Enter()
	{
		nextState = null;
		NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.BLANK;
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, OnAttackEnd);
		
	}
	
	private void OnAttackEnd(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, OnAttackEnd);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_20"));
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "guide", 2);

		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, OnAttackEnemeyEnd);
	}

	private void OnAttackEnemeyEnd(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, OnAttackEnemeyEnd);
		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText ("guide6_title"), TextCenter.GetText ("guide6_content"), TextCenter.GetText ("NEXT"),o=>{
			GoToNextState();
		});
	}
}





