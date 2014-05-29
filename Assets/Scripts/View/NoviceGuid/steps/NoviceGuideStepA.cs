using UnityEngine;
using System.Collections;

public class NoviceGuideStepA_StateGlobal:NoviceGuidState
{
	private static NoviceGuideStepA_StateGlobal instance;
	
	public static NoviceGuideStepA_StateGlobal Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepA_StateGlobal ();
		return instance;
	}
	
	private NoviceGuideStepA_StateGlobal ():base(){}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		
	}
	
	override public void Execute(NoviceGuideStepEntity stepEntity)
	{
		
	}
	
	public override void Exit(NoviceGuideStepEntity stepEntity)
	{
		
	}
}

public class NoviceGuideStepA_StateOne:NoviceGuidState
{
	private static NoviceGuideStepA_StateOne instance;
	
	public static NoviceGuideStepA_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepA_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepA_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepA state_one");

		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide1_title");
		mwp.contentText = TextCenter.GetText("guide1_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepA state_one");
		if (JumpToNextState) {

			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepA_StateTwo.Instance ());
		}
		else{

		}
	}
	
	public override void Exit(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + "exit stepA state_one");
	}

	private void ClickOK(object data)
	{
		LogHelper.Log("goto the selectrole");
		//selectRole
		//NoviceGuideStepEntityManager.FinishCurrentStep ();
		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.SELECT_ROLE;
		UIManager.Instance.ChangeScene (SceneEnum.SelectRole);
	}
}

public class NoviceGuideStepA_StateTwo:NoviceGuidState
{
	
	private static NoviceGuideStepA_StateTwo instance;
	
	public static NoviceGuideStepA_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepA_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepA_StateTwo ():base(){}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
//		LogHelper.Log (stepEntity.GetType () + " is execute stepA state_one");
//
//		if(JumpToNextState)
		//this is the last step. It will end when It execute
		stepEntity.GetStateMachine ().ChangeState (null);
//		else{
//			
//		}
	}	
	public override void Exit(NoviceGuideStepEntity stepEntity)
	{

	}
}