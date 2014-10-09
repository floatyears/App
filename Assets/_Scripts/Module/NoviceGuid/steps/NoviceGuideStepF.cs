using UnityEngine;
using System.Collections;

//party
public class NoviceGuideStepF_1:NoviceGuidStep{


	public override void Enter()
	{
		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide10_title"),TextCenter.GetText ("guide10_content"),TextCenter.GetText ("NEXT"),SureCall);
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

//		uint leaderId = DataCenter.Instance.UnitData.PartyInfo.CurrentParty.GetPartyItem(0).UnitInfo.ID;
//		uint cardId = 0;
//		switch (leaderId) {
//		case 1:
//			cardId = 77;
//			break;
//		case 5:
//			cardId = 73;
//			break;
//		case 9:
//			cardId = 75;
//			break;
//		default:
//			break;
//		}
		
//		GameObject gm = GameObject.Find ("LevelUpUI(Clone)").GetComponent<LevelUpOperateUnity>().GetPartyUnitItem(cardId).gameObject;

		GameObject unit = GameObject.Find ("PartyWindow(Clone)").GetComponent<PartyView> ().GetUnitItem (86);
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
		NoviceGuideUtil.showTipText (TextCenter.GetText("guide_tips_3"),new Vector2(50.0f,100.0f));

		//JumpToNextState = true;
	}

	private void OnUnit2Click(GameObject btn){

		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= OnUnit2Click;
		
		GameObject unit1 = GameObject.FindWithTag ("party_unit1");
		NoviceGuideUtil.ForceOneBtnClick (unit1);
		NoviceGuideUtil.ShowArrow (new GameObject[]{unit1},new Vector3[]{new Vector3(0,0,3)});
		
		
		UIEventListenerCustom.Get (unit1).onClick += OnUnit1Click;
		NoviceGuideUtil.showTipText (TextCenter.GetText("guide_tips_4"),new Vector2(100.0f,100.0f));
	}

	private void OnUnit1Click(GameObject btn){
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= OnUnit1Click;

		NoviceGuideUtil.HideTipText ();

		GoToNextState();
	}

}

//stationary guide(to the detail panel)
public class NoviceGuideStepF_2:NoviceGuidStep{

	private UIEventListenerCustom.VoidDelegate click;
	

	public override void Enter()
	{

		GameObject unit = GameObject.Find ("PartyWindow(Clone)").GetComponent<PartyView> ().GetUnitItem (86);
		NoviceGuideUtil.ShowArrow (new GameObject[]{unit}, new Vector3[]{new Vector3(0,0,2)});
		click = UIEventListenerCustom.Get(unit).onClick;
		UIEventListenerCustom.Get (unit).onClick = OnItemClick;
		NoviceGuideUtil.ForceOneBtnPress (unit);
		UIEventListenerCustom.Get (unit).LongPress += OnUnitPress;
		NoviceGuideUtil.showTipText (TextCenter.GetText("guide_tips_5"),new Vector2(0,-200));
	}

	private void OnItemClick(GameObject item){
		NoviceGuideUtil.showTipTextAnimation ();
	}

	private void OnUnitPress(GameObject btn)
	{
		NoviceGuideUtil.HideTipText ();
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).LongPress -= OnUnitPress;
		UIEventListenerCustom.Get (btn).onClick = click;
	}

}

public class NoviceGuideStepF_3:NoviceGuidStep{
	

	
	public override void Enter()
	{
		GameObject sbb = GameObject.FindWithTag ("scene_back_btn");
		if (sbb == null) {
			Debug.LogError("NoviceGuideStepF_StateThree scene btn is null");

			return;
		}
		NoviceGuideUtil.ForceOneBtnClick (sbb);

		NoviceGuideUtil.ShowArrow (new GameObject[]{sbb}, new Vector3[]{new Vector3(0,0,3)});

		UIEventListenerCustom.Get (sbb).onClick += TapBackBtn;
	}
	
	private void TapBackBtn(GameObject btn)
	{
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= TapBackBtn;
		GoToNextState();


	}
}

//units detail
public class NoviceGuideStepF_4:NoviceGuidStep{
	
	public override void Enter()
	{
		nextState = null;
		GameObject btn0 = GameObject.FindWithTag ("unit_detail_btn2");
		//NoviceGuideUtil.ForceOneBtnClick(empty);
		NoviceGuideUtil.ForceOneBtnClick (btn0);
		NoviceGuideUtil.ShowArrow (new GameObject[]{btn0},new Vector3[]{new Vector3(0,0,1)});

		UIEventListenerCustom.Get (btn0).onClick += Btn0Click;


	}
	
	private void Btn0Click(GameObject btn)
	{

		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= Btn0Click;

		NoviceGuideUtil.showTipText (TextCenter.GetText("guide_tips_6"), new Vector2 (0, 0));

		NoviceGuideStepManager.CurrentNoviceGuideStage = NoviceGuideStage.UNIT_LEVEL_UP;

		MsgCenter.Instance.AddListener (CommandEnum.ChangeSceneComplete, onChangeScene);
//		GameObject sbb = GameObject.FindWithTag ("scene_back_btn");
//		NoviceGuideUtil.ForceOneBtnClick (sbb);
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{sbb}, new Vector3[]{new Vector3(-282,-135,3)});
//		
//		UIEventListenerCustom.Get (sbb).onClick += TapBackBtn;

//		GameObject btn1 = GameObject.FindWithTag ("unit_detail_btn2");
//		//NoviceGuideUtil.ForceOneBtnClick(empty);
//		NoviceGuideUtil.ForceOneBtnClick (btn1);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{btn1},new Vector3[]{new Vector3(0,0,1)});
//		
//		UIEventListenerCustom.Get (btn1).onClick += TapBackBtn;
	
	}

	void onChangeScene(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.ChangeSceneComplete, onChangeScene);
		GoToNextState();
	}

//	private void Btn1Click(GameObject btn)
//	{
//		NoviceGuideUtil.RemoveArrow (btn);
//		UIEventListenerCustom.Get (btn).onClick -= Btn1Click;
//		
//		GameObject btn2 = GameObject.FindWithTag ("unit_detail_btn2");
//		//NoviceGuideUtil.ForceOneBtnClick(empty);
//		NoviceGuideUtil.ForceOneBtnClick (btn2);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{btn2},new Vector3[]{new Vector3(0,0,1)});
//		
//		UIEventListenerCustom.Get (btn2).onClick += TapBackBtn;
//		
//	}
//
//	private void Btn2Click(GameObject btn)
//	{
//		NoviceGuideUtil.RemoveArrow (btn);
//		UIEventListenerCustom.Get (btn).onClick -= Btn2Click;
//		
//		GameObject btn3 = GameObject.FindWithTag ("unit_detail_btn3");
//		//NoviceGuideUtil.ForceOneBtnClick(empty);
//		NoviceGuideUtil.ForceOneBtnClick (btn3);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{btn3},new Vector3[]{new Vector3(0,0,1)});
//		
//		UIEventListenerCustom.Get (btn3).onClick += Btn3Click;
//
//
//	}
//
//	private void Btn3Click(GameObject btn)
//	{
//		NoviceGuideUtil.RemoveArrow (btn);
//		UIEventListenerCustom.Get (btn).onClick -= Btn3Click;
//
//		GameObject sbb = GameObject.FindWithTag ("scene_back_btn");
//		NoviceGuideUtil.ForceOneBtnClick (sbb);
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{sbb}, new Vector3[]{new Vector3(0,0,3)});
//		
//		UIEventListenerCustom.Get (sbb).onClick += TapBackBtn;
//
//
//	}
//	
//	private void TapBackBtn(GameObject btn)
//	{
//		NoviceGuideUtil.RemoveArrow (btn);
//		UIEventListenerCustom.Get (btn).onClick -= TapBackBtn;
//		JumpToNextState = true;
//
//	}
	

	public override void Exit ()
	{
		NoviceGuideUtil.HideTipText ();

	}
}


