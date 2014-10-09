using UnityEngine;
using System.Collections;


//untis party
public class NoviceGuideStepG_1:NoviceGuidStep{

	
	public override void Enter()
	{

		GameObject party = GameObject.FindWithTag ("party");


		//LogHelper.Log (party.name);
		NoviceGuideUtil.ForceOneBtnClick (party);
		
		NoviceGuideUtil.ShowArrow (new GameObject[]{party}, new Vector3[]{new Vector3(0,0,1)});
		
		UIEventListenerCustom.Get (party).onClick += TapParty;

		NoviceGuideStepManager.CurrentNoviceGuideStage = NoviceGuideStage.PARTY;
	}
	
	private void TapParty(GameObject btn)
	{

		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= TapParty;
	}

	
}

//untis level_up
public class NoviceGuideStepG_2:NoviceGuidStep{
	

	
	public override void Enter()
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide41_title"),TextCenter.GetText ("guide41_content"),TextCenter.GetText ("NEXT"),ClickOK);
		
	}

	private void ClickOK(object data){
		GameObject party = GameObject.FindWithTag ("level_up");
		
		//LogHelper.Log (party.name);
		NoviceGuideUtil.ForceOneBtnClick (party,true);
		UIEventListenerCustom.Get (party).onClick += TapParty;


		NoviceGuideUtil.ShowArrow (new GameObject[]{party}, new Vector3[]{new Vector3(0,0,1)});
		


		NoviceGuideStepManager.CurrentNoviceGuideStage = NoviceGuideStage.LEVEL_UP;
	}
	
	private void TapParty(GameObject btn)
	{
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= TapParty;
	}

}

//untis evolve
public class NoviceGuideStepG_3:NoviceGuidStep{
	

	
	public override void Enter()
	{

		GameObject party = GameObject.FindWithTag ("evolve");
		
		//LogHelper.Log (party.name);
		NoviceGuideUtil.ForceOneBtnClick (party);
		
		NoviceGuideUtil.ShowArrow (new GameObject[]{party}, new Vector3[]{new Vector3(0,0,1)});
		
		UIEventListenerCustom.Get (party).onClick += TapParty;

		NoviceGuideStepManager.CurrentNoviceGuideStage = NoviceGuideStage.EVOLVE;
		
	}
	
	private void TapParty(GameObject btn)
	{
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= TapParty;
	}
	
	
}




