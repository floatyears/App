using UnityEngine;
using System.Collections;

//untis evolve
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
		LogHelper.Log (stepEntity.GetType () + " is execute stepJ state_one");
		
		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide51_title");
		mwp.contentText = TextCenter.GetText("guide51_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.GetText("NEXT");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
	}
	
	private void ClickOK(object data){
		GameObject first = GameObject.Find ("UnitDisplay(Clone)").GetComponent<UnitDisplayUnity>().GetUnitItem(0);
		NoviceGuideUtil.ForceOneBtnClick (first);
		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,2)});
		UIEventListenerCustom.Get (first).onClick += OnClickItem;
	}

	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();

		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide52_title");
		mwp.contentText = TextCenter.GetText("guide52_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = OnClickItem1;
		sure.text = TextCenter.GetText("NEXT");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
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

	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepJ_StateTwo.Instance());
		}
		else{
			
		}
	}
	
}

public class NoviceGuideStepJ_StateTwo:NoviceGuidState{
	
	private static NoviceGuideStepJ_StateTwo instance;
	
	public static NoviceGuideStepJ_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepJ_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepJ_StateTwo ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepJ state_two");
		
		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide53_title");
		mwp.contentText = TextCenter.GetText("guide53_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.GetText("NEXT");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
	}
	
	private void ClickOK(object data){
		FriendWindows fw = GameObject.Find ("FriendWindows(Clone)").GetComponent<FriendWindows> ();
		GameObject gm = fw.GetHelperUnitItem(0).gameObject;
		NoviceGuideUtil.ForceOneBtnClick (gm);
		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,3)});
		//UIEventListenerCustom.Get (gm).onClick += OnClickFriend;
		//MsgCenter.Instance.AddListener (CommandEnum.OnPickHelper, OnClickFriend);
		fw.selectFriend += OnClickFriend;
	}
	
	private void OnClickFriend(TFriendInfo data){
		//UIEventListenerCustom.Get (gm).onClick -= OnClickFriend;
		LogHelper.Log("pick a friend to evolve");
		GameObject.Find ("FriendWindows(Clone)").GetComponent<FriendWindows> ().selectFriend -= OnClickFriend;
		NoviceGuideUtil.RemoveAllArrows ();
		
		JumpToNextState = true;
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{	
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepJ_StateThree.Instance());
		}
		else{
			
		}
	}
	
}

public class NoviceGuideStepJ_StateThree:NoviceGuidState{
	
	private static NoviceGuideStepJ_StateThree instance;
	
	public static NoviceGuideStepJ_StateThree Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepJ_StateThree ();
		return instance;
	}
	
	private NoviceGuideStepJ_StateThree ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepJ state_three");
		
		GameObject gm = GameObject.FindWithTag ("evolve_btn");

		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,4)});
		UIEventListenerCustom.Get (gm).onClick += OnClickLevelUp;
		NoviceGuideUtil.ForceOneBtnClick (gm);

		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.EVOVLE_QUEST;
	}
	
	
	private void OnClickLevelUp(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickLevelUp;
		NoviceGuideUtil.RemoveAllArrows ();
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
