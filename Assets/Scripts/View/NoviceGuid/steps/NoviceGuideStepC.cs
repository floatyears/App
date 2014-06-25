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
		mwp.titleText = TextCenter.GetText ("guide5_title");
		mwp.contentText = TextCenter.GetText ("guide5_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.GetText ("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);

	}

	private void ClickOk(object data){

		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParam = new BtnParam();
		mwp.titleText = TextCenter.GetText ("guide6_title");
		mwp.contentText = TextCenter.GetText ("guide6_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = forceClick;
		sure.text = TextCenter.GetText ("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
	}

	private void forceClick(object data)
	{
		
		GameObject btn = GameObject.FindWithTag("rare_scratch");
		NoviceGuideUtil.ForceOneBtnClick (btn);
		
		UIEventListener.Get (btn).onClick += TapRareCard;
		NoviceGuideUtil.ShowArrow (new GameObject[]{btn}, new Vector3[]{new Vector3(0,0,1)});
	}
	
	private void TapRareCard(GameObject btn)
	{
		UIEventListener.Get (btn).onClick -= TapRareCard;
		NoviceGuideUtil.RemoveArrow (btn);

		JumpToNextState = true;
	}

	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if(JumpToNextState)
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepC_StateTwo.Instance());
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
	
	private NoviceGuideStepC_StateTwo ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepC novistate_one");
		
		MsgWindowView mwv = GameObject.Find ("CommonNoteWindow(Clone)").GetComponent<MsgWindowView> ();
		
		UIButton cwLBtn = mwv.BtnLeft;
		UIEventListenerCustom.Get (cwLBtn.gameObject).onClick += clickLeftBtn;
		
		NoviceGuideUtil.ShowArrow (new GameObject[]{cwLBtn.gameObject},new Vector3[]{new Vector3(0,0,3)});
		
		mwv.BtnRight.isEnabled = false;
	}
	
	private void clickLeftBtn(GameObject btn)
	{
		MsgWindowView mwv = GameObject.Find ("CommonNoteWindow(Clone)").GetComponent<MsgWindowView> ();
		
		UIButton cwLBtn = mwv.BtnLeft;
		UIEventListenerCustom.Get (cwLBtn.gameObject).onClick -= clickLeftBtn;
		
		NoviceGuideUtil.RemoveArrow (btn);
		
		mwv.BtnRight.isEnabled = true;
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if(JumpToNextState)
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepC_StateThree.Instance());
		else{
			
		}
	}
	
}

public class NoviceGuideStepC_StateThree:NoviceGuidState
{
	private static NoviceGuideStepC_StateThree instance;
	
	public static NoviceGuideStepC_StateThree Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepC_StateThree ();
		return instance;
	}
	
	private NoviceGuideStepC_StateThree ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepC state_three");
		
		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide8_title");
		mwp.contentText = TextCenter.GetText("guide8_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.GetText("OK");
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