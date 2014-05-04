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
		LogHelper.Log (stepEntity.GetType () + " get into stepD state_one");

		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParam = new BtnParam();
		mwp.titleText = TextCenter.Instace.GetCurrentText ("guide5_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText ("guide5_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = delegate {
			JumpToNextState = true;
				};
		sure.text = TextCenter.Instace.GetCurrentText ("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);

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
	private bool isCardTapped = false;

	private LayerMask camLastLayer;

	private int btnLastLayer;

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
		LogHelper.Log (stepEntity.GetType () + " get into stepC state_two");

		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParam = new BtnParam();
		mwp.titleText = TextCenter.Instace.GetCurrentText ("guide6_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText ("guide6_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = forceClick;
		sure.text = TextCenter.Instace.GetCurrentText ("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState){
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepC_StateThree.Instance());
		}
	}

	private void forceClick(object data)
	{
		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
		camLastLayer = mainCam.eventReceiverMask;
		UIButton btn = GameObject.Find ("ScratchWindow(Clone)").GetComponent<ScratchView>().BtnRareGacha;
		UIEventListener.Get (btn.gameObject).onClick += TapRareCard;

		btnLastLayer = btn.gameObject.layer;
		LayerMask mask =  1 << LayerMask.NameToLayer ("NoviceGuide");
		mainCam.eventReceiverMask = mask;
		btn.transform.gameObject.layer = LayerMask.NameToLayer ("NoviceGuide");
		

		LogHelper.Log ("main camera:" + mainCam.eventReceiverMask.value);
	}

	private void TapRareCard(GameObject btn)
	{
		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
		mainCam.eventReceiverMask = camLastLayer;

		UIButton rareBtn = GameObject.Find ("ScratchWindow(Clone)").GetComponent<ScratchView>().BtnRareGacha;
		rareBtn.gameObject.layer = btnLastLayer;
		UIEventListener.Get (rareBtn.gameObject).onClick -= TapRareCard;

		isCardTapped = true;
	}
	
	public override void Exit(NoviceGuideStepEntity stepEntity)
	{
		
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
//		
//		MsgWindowParams mwp = new MsgWindowParams ();
//		mwp.btnParam = new BtnParam();
//		mwp.titleText = TextCenter.Instace.GetCurrentText ("guide7_title");
//		mwp.contentText = TextCenter.Instace.GetCurrentText ("guide7_content");
//		
//		BtnParam sure = new BtnParam ();
//		sure.callback = RareScrath;
//		sure.text = TextCenter.Instace.GetCurrentText ("OK");
//		mwp.btnParam = sure;
//		
//		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
		RareScrath();
	}

	private void RareScrath()
	{
		UIManager.Instance.ChangeScene (SceneEnum.RareScratch);
		JumpToNextState = true;
	}

	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if(JumpToNextState)
			stepEntity.GetStateMachine ().ChangeState (null);
		else{
			
		}
	}
}