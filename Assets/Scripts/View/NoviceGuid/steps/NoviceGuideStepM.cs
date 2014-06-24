using UnityEngine;
using System.Collections;

	public class NoviceGuideStepM_StateOne:NoviceGuidState{
		
	private static NoviceGuideStepM_StateOne instance;
		
	public static NoviceGuideStepM_StateOne Instance()
	{
		if (instance == null)
		instance = new NoviceGuideStepM_StateOne ();
		return instance;
	}
		
	private NoviceGuideStepM_StateOne ():base()	{}
		
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepJ state_one");
		
		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide51_title");
		mwp.contentText = TextCenter.GetText("guide51_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
	}
	
	private void ClickOK(object data){
		GameObject first = GameObject.Find ("UnitDisplay(Clone)").GetComponent<UnitDisplayUnity>().GetUnitItem(0);
		NoviceGuideUtil.ForceOneBtnClick (first);
		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,2)});
		UIEventListenerCustom.Get (first).onClick += OnClickItem;
	}
	
	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();
		
		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide52_title");
		mwp.contentText = TextCenter.GetText("guide52_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = OnClickItem1;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}
	
	//	private void ClickOK1(object data){
	//		GameObject gm = GameObject.Find ("UnitDisplay(Clone)").GetComponent<UnitDisplayUnity>().GetUnitItem(-1);
	//		
	//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,2)});
	//		UIEventListenerCustom.Get (gm).onClick += OnClickItem1;
	//		NoviceGuideUtil.ForceOneBtnClick (gm);
	//	}
	
	private void OnClickItem1(object data){
		
		//UIEventListenerCustom.Get (item).onClick -= OnClickItem1;
		//NoviceGuideUtil.RemoveAllArrows ();
		
		GameObject gm = GameObject.FindWithTag ("evolve_friend_btn");
		
		UIEventListenerCustom.Get (gm).onClick += OnClickFriend;
		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,3)});
		NoviceGuideUtil.ForceOneBtnClick (gm);
		
	}
	
	private void OnClickFriend(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickFriend;
		NoviceGuideUtil.RemoveAllArrows ();
		
	}
	
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepJ_StateTwo.Instance());
		}
		else{
			
		}
	}
	
}
