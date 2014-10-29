using UnityEngine;
using System.Collections;

//scratch,party
public class NoviceGuideStepD_1:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepD_2);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_10"));
	
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("scratch_btn"), new Vector3 (0, 0, 1), true, true, o1 => {
			NoviceGuideUtil.RemoveAllArrows ();
			GoToNextState();
		});
	}
}


//rare scratch
public class NoviceGuideStepD_2:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepD_3);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_11"));

		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("rare_scratch"), new Vector3 (0, 0, 3), true, true, o1 => {
			NoviceGuideUtil.RemoveAllArrows ();
			GoToNextState();
		});
	}

}
//click units
public class NoviceGuideStepD_3:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepD_4);
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_12"));

		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("unit_btn"), new Vector3 (0, 0, 1), true, true, o1 => {
			NoviceGuideUtil.RemoveAllArrows ();
			GoToNextState();
		});
	}

}

//click party
public class NoviceGuideStepD_4:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepD_5);
		
		NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("party"), new Vector3 (0, 0, 1), true, true, o1 => {
			NoviceGuideUtil.RemoveAllArrows ();
			GoToNextState();
		});
	}
	
}


//add party
public class NoviceGuideStepD_5:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = null;
		NoviceGuideStepManager.Instance.CurrentGuideStep = NoviceGuideStage.BLANK;
		GameObject obj = GameObject.FindWithTag ("party_unit3");
		if (obj.GetComponent<PageUnitItem> ().UserUnit != null) {
			ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_14"));
			NoviceGuideUtil.ShowArrow (obj, new Vector3 (0, 0, 3), true, true, o1 => {
				NoviceGuideUtil.RemoveAllArrows ();

				NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("unitId_86"), new Vector3 (0, 0, 2), true, true, o2 => {
					GoToNextState();
					NoviceGuideUtil.RemoveAllArrows ();
					ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_15"));
				});
			});
		}else{
			ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_13"));
			NoviceGuideUtil.ShowArrow (GameObject.FindWithTag ("unitId_86"), new Vector3 (0, 0, 2), true, true, o1 => {
				GoToNextState();
				NoviceGuideUtil.RemoveAllArrows ();
				ModuleManager.Instance.ShowModule (ModuleEnum.NoviceGuideTipsModule, "tips", TextCenter.GetText ("guide_string_15"));
			});
		}


	}
}

