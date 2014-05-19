using UnityEngine;
using System.Collections;

//untis
public class NoviceGuideStepJ_StateOne:NoviceGuidState{
	
	private static NoviceGuideStepJ_StateOne instance;
	
	public static NoviceGuideStepJ_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepJ_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepJ_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepI state_one");
		
		MsgWindowParams mwp = new MsgWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide41_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide41_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
	}
	
	private void ClickOK(object data){
		
	}

	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (null);
		}
		else{
			
		}
	}
	
}
