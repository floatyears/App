using UnityEngine;
using System.Collections;

public class NoviceGuideStepC_1:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepC_2);

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_6"));

		GameObject obj = GameObject.FindWithTag ("quest_new");
		NoviceGuideUtil.ShowArrow (new GameObject[]{obj}, new Vector3[]{new Vector3 (0, 0, 3)});
		NoviceGuideUtil.ForceOneBtnClick (obj, o=>{
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

		GameObject obj = GameObject.FindWithTag ("friend_one");
		NoviceGuideUtil.ShowArrow (new GameObject[]{obj}, new Vector3[]{new Vector3 (0, 0, 3)});
		NoviceGuideUtil.ForceOneBtnClick (obj, o=>{
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
		GameObject obj = GameObject.FindWithTag ("fight_btn");
		NoviceGuideUtil.ShowArrow (new GameObject[]{obj}, new Vector3[]{new Vector3 (0, 0, 1)});
		NoviceGuideUtil.ForceOneBtnClick (obj, o=>{
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
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_8"));

		GameObject obj = GameObject.FindWithTag ("battle_leader");
		NoviceGuideUtil.ShowArrow (new GameObject[]{obj}, new Vector3[]{new Vector3 (0, 0, 1)});
		NoviceGuideUtil.ForceOneBtnClick (obj, o=>{
			ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_9"));
			NoviceGuideUtil.RemoveAllArrows();

			MsgCenter.Instance.AddListener(CommandEnum.AttackEnemyEnd,OnSkillRelease);

			GameObject obj1 = GameObject.FindWithTag ("battle_leader");
			NoviceGuideUtil.ShowArrow (new GameObject[]{obj1}, new Vector3[]{new Vector3 (0, 0, 3)});
			NoviceGuideUtil.ForceOneBtnClick(obj1,o1=>{
				ModuleManager.Instance.HideModule(ModuleEnum.NoviceGuideTipsModule);
				NoviceGuideUtil.RemoveAllArrows();
			});

		});
	}

	void OnSkillRelease(object data){
		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText ("guide5_title"), TextCenter.GetText ("guide5_content"), TextCenter.GetText ("NEXT"));
	}
}
