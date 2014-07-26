using UnityEngine;
using System.Collections;

//party
public class NoviceGuideStepF_StateOne:NoviceGuidState{

	private static NoviceGuideStepF_StateOne instance;
	
	public static NoviceGuideStepF_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepF_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepF_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepF state_one");
		
		GuideWindowParams mwp = new GuideWindowParams ();
		mwp.btnParam = new BtnParam();
		mwp.titleText = TextCenter.GetText ("guide10_title");
		mwp.contentText = TextCenter.GetText("guide10_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = SureCall;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);		
	}
	
	private void SureCall(object data)
	{

		//NoviceGuideUtil.RemoveAllArrows ();

		GameObject empty = GameObject.FindWithTag ("party_unit3");
		//NoviceGuideUtil.ForceOneBtnClick(empty);
		NoviceGuideUtil.ForceOneBtnClick (empty);
		NoviceGuideUtil.ShowArrow (new GameObject[]{empty},new Vector3[]{new Vector3(0,0,3)});


		UIEventListenerCustom.Get (empty).onClick += EmptyClick;


	}
		
	private void EmptyClick(GameObject btn)
	{
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= EmptyClick;

		GameObject unit = GameObject.Find ("PartyWindow(Clone)").GetComponent<PartyView> ().GetUnitItem (2);
		NoviceGuideUtil.ShowArrow (new GameObject[]{unit}, new Vector3[]{new Vector3(0,0,2)});
		NoviceGuideUtil.ForceOneBtnClick (unit);
		UIEventListenerCustom.Get (unit).onClick += OnUnitClick;
	}

	private void OnUnitClick(GameObject btn)
	{
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= OnUnitClick;


		GameObject unit2 = GameObject.FindWithTag ("party_unit2");
		NoviceGuideUtil.ForceOneBtnClick (unit2);
		NoviceGuideUtil.ShowArrow (new GameObject[]{unit2},new Vector3[]{new Vector3(0,0,3)});
		
		UIEventListenerCustom.Get (unit2).onClick += OnUnit2Click;
		NoviceGuideUtil.showTipText ("choose a unit to change the position",new Vector2(50.0f,100.0f));

		//JumpToNextState = true;
	}

	private void OnUnit2Click(GameObject btn){

		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= OnUnit2Click;
		
		GameObject unit1 = GameObject.FindWithTag ("party_unit1");
		NoviceGuideUtil.ForceOneBtnClick (unit1);
		NoviceGuideUtil.ShowArrow (new GameObject[]{unit1},new Vector3[]{new Vector3(0,0,3)});
		
		
		UIEventListenerCustom.Get (unit1).onClick += OnUnit1Click;
		NoviceGuideUtil.showTipText ("choose the position to change to ",new Vector2(100.0f,100.0f));
	}

	private void OnUnit1Click(GameObject btn){
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= OnUnit1Click;

		NoviceGuideUtil.HideTipText ();

		JumpToNextState = true;
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepF_StateTwo.Instance());
		}
		else{
			
		}
	}

}

//stationary guide(to the detail panel)
public class NoviceGuideStepF_StateTwo:NoviceGuidState{

	private UIEventListenerCustom.VoidDelegate click;

	private static NoviceGuideStepF_StateTwo instance;
	
	public static NoviceGuideStepF_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepF_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepF_StateTwo ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{

		GameObject unit = GameObject.Find ("PartyWindow(Clone)").GetComponent<PartyView> ().GetUnitItem (3);
		NoviceGuideUtil.ShowArrow (new GameObject[]{unit}, new Vector3[]{new Vector3(0,0,2)});
		click = UIEventListenerCustom.Get(unit).onClick;
		UIEventListenerCustom.Get (unit).onClick = null;
		NoviceGuideUtil.ForceOneBtnPress (unit);
		UIEventListenerCustom.Get (unit).LongPress += OnUnitPress;
		NoviceGuideUtil.showTipText ("press the unit item to view the detail.",new Vector2(0,-200));
	}
	
	private void OnUnitPress(GameObject btn)
	{
		NoviceGuideUtil.HideTipText ();
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).LongPress -= OnUnitPress;
		UIEventListenerCustom.Get (btn).onClick = click;
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepF_StateFour.Instance());
		}
		else{
			
		}
	}
}

public class NoviceGuideStepF_StateThree:NoviceGuidState{
	
	private static NoviceGuideStepF_StateThree instance;
	
	public static NoviceGuideStepF_StateThree Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepF_StateThree ();
		return instance;
	}
	
	private NoviceGuideStepF_StateThree ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		GameObject sbb = GameObject.FindWithTag ("scene_back_btn");
		NoviceGuideUtil.ForceOneBtnClick (sbb);

		NoviceGuideUtil.ShowArrow (new GameObject[]{sbb}, new Vector3[]{new Vector3(0,0,3)});

		UIEventListener.Get (sbb).onClick += TapBackBtn;
	}
	
	private void TapBackBtn(GameObject btn)
	{
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListener.Get (btn).onClick -= TapBackBtn;
		JumpToNextState = true;


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

//units detail
public class NoviceGuideStepF_StateFour:NoviceGuidState{
	
	private static NoviceGuideStepF_StateFour instance;
	
	public static NoviceGuideStepF_StateFour Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepF_StateFour ();
		return instance;
	}
	
	private NoviceGuideStepF_StateFour ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{

		GameObject btn0 = GameObject.FindWithTag ("unit_detail_btn2");
		//NoviceGuideUtil.ForceOneBtnClick(empty);
		NoviceGuideUtil.ForceOneBtnClick (btn0);
		NoviceGuideUtil.ShowArrow (new GameObject[]{btn0},new Vector3[]{new Vector3(0,0,1)});

		UIEventListener.Get (btn0).onClick += Btn0Click;


	}
	
	private void Btn0Click(GameObject btn)
	{

		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListener.Get (btn).onClick -= Btn0Click;

		NoviceGuideUtil.showTipText ("touch screen to return", new Vector2 (0, 0));

//		GameObject sbb = GameObject.FindWithTag ("scene_back_btn");
//		NoviceGuideUtil.ForceOneBtnClick (sbb);
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{sbb}, new Vector3[]{new Vector3(-282,-135,3)});
//		
//		UIEventListener.Get (sbb).onClick += TapBackBtn;

//		GameObject btn1 = GameObject.FindWithTag ("unit_detail_btn2");
//		//NoviceGuideUtil.ForceOneBtnClick(empty);
//		NoviceGuideUtil.ForceOneBtnClick (btn1);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{btn1},new Vector3[]{new Vector3(0,0,1)});
//		
//		UIEventListener.Get (btn1).onClick += TapBackBtn;
	
	}

//	private void Btn1Click(GameObject btn)
//	{
//		NoviceGuideUtil.RemoveArrow (btn);
//		UIEventListener.Get (btn).onClick -= Btn1Click;
//		
//		GameObject btn2 = GameObject.FindWithTag ("unit_detail_btn2");
//		//NoviceGuideUtil.ForceOneBtnClick(empty);
//		NoviceGuideUtil.ForceOneBtnClick (btn2);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{btn2},new Vector3[]{new Vector3(0,0,1)});
//		
//		UIEventListener.Get (btn2).onClick += TapBackBtn;
//		
//	}
//
//	private void Btn2Click(GameObject btn)
//	{
//		NoviceGuideUtil.RemoveArrow (btn);
//		UIEventListener.Get (btn).onClick -= Btn2Click;
//		
//		GameObject btn3 = GameObject.FindWithTag ("unit_detail_btn3");
//		//NoviceGuideUtil.ForceOneBtnClick(empty);
//		NoviceGuideUtil.ForceOneBtnClick (btn3);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{btn3},new Vector3[]{new Vector3(0,0,1)});
//		
//		UIEventListener.Get (btn3).onClick += Btn3Click;
//
//
//	}
//
//	private void Btn3Click(GameObject btn)
//	{
//		NoviceGuideUtil.RemoveArrow (btn);
//		UIEventListener.Get (btn).onClick -= Btn3Click;
//
//		GameObject sbb = GameObject.FindWithTag ("scene_back_btn");
//		NoviceGuideUtil.ForceOneBtnClick (sbb);
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{sbb}, new Vector3[]{new Vector3(0,0,3)});
//		
//		UIEventListener.Get (sbb).onClick += TapBackBtn;
//
//
//	}
//	
//	private void TapBackBtn(GameObject btn)
//	{
//		NoviceGuideUtil.RemoveArrow (btn);
//		UIEventListener.Get (btn).onClick -= TapBackBtn;
//		JumpToNextState = true;
//
//	}

	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepF_StateThree.Instance());
		}
		else{
			
		}
	}

	public override void Exit (NoviceGuideStepEntity stepEntity)
	{
		NoviceGuideUtil.HideTipText ();

		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.UNIT_LEVEL_UP;
	}
}


