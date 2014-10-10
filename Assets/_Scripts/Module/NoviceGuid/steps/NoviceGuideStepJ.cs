using UnityEngine;
using System.Collections;
using bbproto;

//untis evolve
public class NoviceGuideStepJ_1:NoviceGuidStep{

	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepJ_2);
		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide51_title"),TextCenter.GetText ("guide51_content"),TextCenter.GetText ("NEXT"),ClickOK);
		
		
	}
	
	private void ClickOK(object data){
		EvolveView evoView = GameObject.Find ("UnitDisplay(Clone)").GetComponent<EvolveView>();

//		uint id = evoView.GetMaxLvUnitID();
//		evoView.SetItemVisible (id);

//		GameObject first = evoView.GetMaxLvUnitItem();
//		NoviceGuideUtil.ForceOneBtnClick (first);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,2)});
//		UIEventListenerCustom.Get (first).onClick += OnClickItem;
	}

	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide52_title"),TextCenter.GetText ("guide52_content"),TextCenter.GetText ("NEXT"),OnClickItem1);
		
	}

//	private void ClickOK1(object data){
//		GameObject gm = GameObject.Find ("UnitDisplay(Clone)").GetComponent<UnitDisplayUnity>().GetUnitItem(-1);
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,2)});
//		UIEventListenerCustom.Get (gm).onClick += OnClickItem1;
//		NoviceGuideUtil.ForceOneBtnClick (gm);
//	}

	private void OnClickItem1(object data){

		//UIEventListenerCustom.Get (item).onClick -= OnClickItem1;
		//NoviceGuideUtil.RemoveAllArrows ();

		GameObject gm = GameObject.FindWithTag ("evolve_friend_btn");
		
		UIEventListenerCustom.Get (gm).onClick += OnClickFriend;
		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,3)});
		NoviceGuideUtil.ForceOneBtnClick (gm);
		
	}
	
	private void OnClickFriend(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickFriend;
		NoviceGuideUtil.RemoveAllArrows ();

	}

	
}

public class NoviceGuideStepJ_2:NoviceGuidStep{

	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepJ_3);
		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide53_title"),TextCenter.GetText ("guide53_content"),TextCenter.GetText ("NEXT"),ClickOK);
		
	}
	
	private void ClickOK(object data){
//		FriendSelectLevelUpView fw = GameObject.Find ("FriendWindows(Clone)").GetComponent<FriendSelectLevelUpView> ();
//		GameObject gm = fw.GetHelperUnitItem(0).gameObject;
//		NoviceGuideUtil.ForceOneBtnClick (gm);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,3)});
//		//UIEventListenerCustom.Get (gm).onClick += OnClickFriend;
//		//MsgCenter.Instance.AddListener (CommandEnum.OnPickHelper, OnClickFriend);
//		fw.selectFriend += OnClickFriend;
	}
	
	private void OnClickFriend(FriendInfo data){
		//UIEventListenerCustom.Get (gm).onClick -= OnClickFriend;
		LogHelper.Log("pick a friend to evolve");
//		GameObject.Find ("FriendWindows(Clone)").GetComponent<FriendSelectLevelUpView> ().selectFriend -= OnClickFriend;
		NoviceGuideUtil.RemoveAllArrows ();
		
		GoToNextState();
	}

	
}

public class NoviceGuideStepJ_3:NoviceGuidStep{

	
	public override void Enter()
	{
		nextState = null;
		
		GameObject gm = GameObject.FindWithTag ("evolve_btn");

		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,4)});
		UIEventListenerCustom.Get (gm).onClick += OnClickLevelUp;
		NoviceGuideUtil.ForceOneBtnClick (gm);

	}
	
	
	private void OnClickLevelUp(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickLevelUp;
		NoviceGuideUtil.RemoveAllArrows ();
	}

	
}

//public class NoviceGuideStepJ_StateFour:NoviceGuidState{
//	
//	private static NoviceGuideStepJ_StateFour instance;
//	
//	public static NoviceGuideStepJ_StateFour Instance()
//	{
//		if (instance == null)
//			instance = new NoviceGuideStepJ_StateFour ();
//		return instance;
//	}
//	
//	private NoviceGuideStepJ_StateFour ():base()	{}
//	
//	public override void Enter(NoviceGuideStepEntity stepEntity)
//	{
//		LogHelper.Log (stepEntity.GetType () + " is execute stepJ state_four");
//		
//		UIManager.Instance.forbidChangeScene = true;
//		
//		MsgCenter.Instance.AddListener (CommandEnum.levelDone, OnLevelDone);
//		
//	}
//	
//	private void OnLevelDone(object data){
//		UIManager.Instance.forbidChangeScene = false;
//		
//		GuideWindowParams mwp = new GuideWindowParams ();
//		//mwp.btnParams = new BtnParam[1];
//		mwp.btnParam = new BtnParam ();
//		mwp.titleText = TextCenter.GetText("guide46_title");
//		mwp.contentText = TextCenter.GetText("guide46_content");
//		
//		BtnParam sure = new BtnParam ();
//		sure.callback = ClickOK;
//		sure.text = TextCenter.GetText("NEXT");
//		mwp.btnParam = sure;
//		
//		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
//	}
//	
//	private void ClickOK(object data){
//		GameObject gm = GameObject.FindWithTag ("scene_back_btn");
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,4)});
//		UIEventListenerCustom.Get (gm).onClick += OnClickLevelUp;
//		NoviceGuideUtil.ForceOneBtnClick (gm);
//	}
//	
//	private void OnClickLevelUp(GameObject gm){
//		UIEventListenerCustom.Get (gm).onClick -= OnClickLevelUp;
//		NoviceGuideUtil.ForceOneBtnClick (gm);
//		
//	}
//	
//	public override void Execute(NoviceGuideStepEntity stepEntity)
//	{
//		
//		if (JumpToNextState) {
//			stepEntity.GetStateMachine ().ChangeState (null);
//		}
//		else{
//			
//		}
//	}
//	
//	public override void Exit (NoviceGuideStepEntity stepEntity)
//	{
//		NoviceGuideUtil.RemoveAllArrows ();
//	}
//	
//}
