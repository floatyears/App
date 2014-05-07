using UnityEngine;
using System.Collections;

public class NoviceGuideStepD_StateOne:NoviceGuidState
{
	private static NoviceGuideStepD_StateOne instance;
	
	public static NoviceGuideStepD_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepD_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepD_StateOne ():base()	{}

	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepD novistate_one");

		MsgWindowView mwv = GameObject.Find ("CommonNoteWindow(Clone)").GetComponent<MsgWindowView> ();

		UIButton cwLBtn = mwv.BtnLeft;
		UIEventListener.Get (cwLBtn.gameObject).onClick += clickLeftBtn;

		NoviceGuideUtil.ShowArrow (new GameObject[]{cwLBtn.gameObject},new Vector3[]{new Vector3(0,0,3)});

		mwv.BtnRight.isEnabled = false;
	}
	
	private void clickLeftBtn(GameObject btn)
	{
		MsgWindowView mwv = GameObject.Find ("CommonNoteWindow(Clone)").GetComponent<MsgWindowView> ();

		UIButton cwLBtn = mwv.BtnLeft;
		UIEventListener.Get (cwLBtn.gameObject).onClick -= clickLeftBtn;

		NoviceGuideUtil.RemoveArrow (btn);

		mwv.BtnRight.isEnabled = true;
	}

	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if(JumpToNextState)
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepD_StateTwo.Instance());
		else{
			
		}
	}
	
}

public class NoviceGuideStepD_StateTwo:NoviceGuidState
{
	private static NoviceGuideStepD_StateTwo instance;
	
	public static NoviceGuideStepD_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepD_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepD_StateTwo ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepD state_two");

		MsgWindowParams mwp = new MsgWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide8_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide8_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}
	

	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if(JumpToNextState)
			stepEntity.GetStateMachine ().ChangeState (null);
		else{
			
		}
	}
	
}