using UnityEngine;
using System.Collections;

//quest
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
		LogHelper.Log (stepEntity.GetType () + " is execute stepM state_one");
		
		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide61_title");
		mwp.contentText = TextCenter.GetText("guide61_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
	}
	
	private void ClickOK(object data){
		GameObject first = GameObject.FindWithTag ("city_one");
		NoviceGuideUtil.ForceOneBtnClick (first);
		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,1)});
		UIEventListenerCustom.Get (first).onClick += OnClickItem;
	}
	
	private void OnClickItem(GameObject gm){
				UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
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

public class NoviceGuideStepM_StateTwo:NoviceGuidState{
	
	private static NoviceGuideStepM_StateTwo instance;
	
	public static NoviceGuideStepM_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepM_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepM_StateTwo ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepM state_one");
		
		GameObject first = GameObject.FindWithTag ("city_one");
		NoviceGuideUtil.ForceOneBtnClick (first);
		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,1)});
		UIEventListenerCustom.Get (first).onClick += OnClickItem;
		
	}
	
	private void ClickOK(object data){

	}
	
	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
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
