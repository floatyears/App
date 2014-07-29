using UnityEngine;
using System.Collections;

public class NoviceGuideStepB_StateOne:NoviceGuidState
{
	private static NoviceGuideStepB_StateOne instance;
	
	public static NoviceGuideStepB_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepB_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepB_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepB state_one");
		
		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParam = new BtnParam();
		mwp.titleText = TextCenter.GetText ("guide2_title");
		mwp.contentText = TextCenter.GetText ("guide2_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.GetText ("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if(JumpToNextState)
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepB_StateTwo.Instance());
		else{
			
		}
	}
}

public class NoviceGuideStepB_StateTwo:NoviceGuidState
{
	
	private static NoviceGuideStepB_StateTwo instance;
	
	public static NoviceGuideStepB_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepB_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepB_StateTwo ():base(){}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepB state_two");

		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParam = new BtnParam();
		mwp.titleText = TextCenter.GetText ("guide3_title");
		mwp.contentText = TextCenter.GetText ("guide3_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = InputName;
		sure.text = TextCenter.GetText ("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if(JumpToNextState)
			stepEntity.GetStateMachine ().ChangeState (null);
		else{
			
		}
	}

	private void InputName(object data)
	{
		uint unitID = DataCenter.Instance.GetUnitInfo(9).ID;
		MsgCenter.Instance.Invoke(CommandEnum.StartFirstLogin, unitID);
		NoviceGuideStepEntityManager.FinishCurrentStep ();
	}

	public override void Exit(NoviceGuideStepEntity stepEntity)
	{
		
	}
}