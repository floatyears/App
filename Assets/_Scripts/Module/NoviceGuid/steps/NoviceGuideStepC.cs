using UnityEngine;
using System.Collections;
using bbproto;

public class NoviceGuideStepC_1:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepC_2);

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_6"));

		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("quest_new"), new Vector3 (0, 0, 3),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
			ModuleManager.Instance.HideModule(ModuleEnum.NoviceGuideTipsModule);
		});

	}

}

//select friend
public class NoviceGuideStepC_2:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepC_3);

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_7"));

		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("friend_one"), new Vector3 (0, 0, 3),true,true,o=>{
			ModuleManager.Instance.HideModule(ModuleEnum.NoviceGuideTipsModule);
			NoviceGuideUtil.RemoveAllArrows();
		});
		
	}

}

//fight ready
public class NoviceGuideStepC_3:NoviceGuidStep{
	public override void Enter ()
	{
		nextState = typeof(NoviceGuideStepC_4);
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("fight_btn"), new Vector3 (0, 0, 1),true,true,o=>{
			NoviceGuideUtil.RemoveAllArrows();
		});
	}
}

//boost skill
public class NoviceGuideStepC_4:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepD_1);

		UserUnit uu = DataCenter.Instance.UnitData.PartyInfo.CurrentParty.UserUnit [0];//.ActiveSkill
		SkillBase sbi = DataCenter.Instance.BattleData.GetSkill (uu.MakeUserUnitKey (), uu.UnitInfo.activeSkill, SkillType.ActiveSkill);
		sbi.ResetCooling();

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_8"));

		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("battle_leader"), new Vector3 (0, 0, 1),true,true,o=>{
				ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_9"));
				NoviceGuideUtil.RemoveAllArrows();
				
				MsgCenter.Instance.AddListener(CommandEnum.AttackEnemyEnd,OnSkillRelease);
				MsgCenter.Instance.AddListener(CommandEnum.BattleSkillPanel,OnBattleSkillShow);
				
				
				
			});
	}

	void OnBattleSkillShow(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleSkillPanel,OnBattleSkillShow);
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("boost_skill"), new Vector3 (0, 0, 3),true,true,o1=>{
			ModuleManager.Instance.HideModule(ModuleEnum.NoviceGuideTipsModule);
			NoviceGuideUtil.RemoveAllArrows();
		});
	}

	void OnSkillRelease(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd,OnSkillRelease);
		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText ("guide5_title"), TextCenter.GetText ("guide5_content"), TextCenter.GetText ("NEXT"));
	}
}
