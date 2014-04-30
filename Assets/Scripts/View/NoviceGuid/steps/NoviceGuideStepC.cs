using UnityEngine;
using System.Collections;

public class NoviceGuideStepC_StateOne:NoviceGuidState
{
	private static NoviceGuideStepC_StateOne instance;
	
	public static NoviceGuideStepC_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepC_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepC_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepC state_one");
		
		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParam = new BtnParam();
		mwp.titleText = TextCenter.Instace.GetCurrentText ("guide2_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText ("guide2_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.Instace.GetCurrentText ("OK");
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

public class NoviceGuideStepC_StateTwo:NoviceGuidState
{
	
	private static NoviceGuideStepC_StateTwo instance;
	
	public static NoviceGuideStepC_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepC_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepC_StateTwo ():base(){}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParam = new BtnParam();
		mwp.titleText = TextCenter.Instace.GetCurrentText ("guide4_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText ("guide4_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.Instace.GetCurrentText ("OK");
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
	
	public override void Exit(NoviceGuideStepEntity stepEntity)
	{
		
	}
}