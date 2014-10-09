using UnityEngine;
using System.Collections;

//level up
public class NoviceGuideStepI_StateOne:NoviceGuidState{
	
	private static NoviceGuideStepI_StateOne instance;
	
	public static NoviceGuideStepI_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepI_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepI_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepI state_one");

		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText("guide42_title"),TextCenter.GetText("guide42_content"),TextCenter.GetText("NEXT"),ClickOK);
		
	}

	private void ClickOK(object data){
		uint leaderId = DataCenter.Instance.UnitData.PartyInfo.CurrentParty.GetPartyItem(0).UnitInfo.id;
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

//		GameObject gm = GameObject.Find ("LevelUpUI(Clone)").GetComponent<LevelUpView>().GetPartyUnitItem(leaderId).gameObject;
//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,2)});
//
//		UIEventListenerCustom.Get (gm).onClick += OnClickItem;
//		//Debug.LogError ("gm : " + gm);
//		NoviceGuideUtil.ForceOneBtnClick (gm);
	}

	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();

		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText("guide43_title"),TextCenter.GetText("guide43_content"),TextCenter.GetText("NEXT"),ClickOK1);
	}

	private void ClickOK1(object data){

//		GameObject.Find ("LevelUpUI(Clone)").GetComponent<LevelUpView>().SetItemVisible(67);
////		GameTimer.GetInstance ().AddCountDown (1f, Callback);
//		GameObject gm = GameObject.Find ("LevelUpUI(Clone)").GetComponent<LevelUpView> ().GetPartyUnitItem (67).gameObject;
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3 (0, 0, 2)});
//		UIEventListenerCustom.Get (gm).onClick += OnClickItem1;
//		NoviceGuideUtil.ForceOneBtnClick (gm);
		
	}

	private void Callback(){
//		GameObject gm = GameObject.Find ("LevelUpUI(Clone)").GetComponent<LevelUpView> ().GetPartyUnitItem (67).gameObject;
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3 (0, 0, 2)});
//		UIEventListenerCustom.Get (gm).onClick += OnClickItem1;
//		NoviceGuideUtil.ForceOneBtnClick (gm);
	}
	
//	private IEnumerator LateExe(){

//		GameObject gm = GameObject.Find ("LevelUpUI(Clone)").GetComponent<LevelUpOperateUnity>().GetPartyUnitItem(99).gameObject;
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,2)});
//		UIEventListenerCustom.Get (gm).onClick += OnClickItem1;
//		NoviceGuideUtil.ForceOneBtnClick (gm);
//	}

	private void OnClickItem1(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem1;
		NoviceGuideUtil.RemoveAllArrows ();


		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText("guide44_title"),TextCenter.GetText("guide44_content"),TextCenter.GetText("NEXT"),ClickOK2);
	}

	private void ClickOK2(object data){
		GameObject gm = GameObject.FindWithTag ("level_up_friend");

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
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepI_StateTwo.Instance());
		}
		else{
			
		}
	}
	
}

public class NoviceGuideStepI_StateTwo:NoviceGuidState{
	
	private static NoviceGuideStepI_StateTwo instance;
	
	public static NoviceGuideStepI_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepI_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepI_StateTwo ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText("guide45_title"),TextCenter.GetText("guide45_content"),TextCenter.GetText("NEXT"),ClickOK);
	}

	private void ClickOK(object data){
//		GameObject gm = GameObject.Find ("FriendWindows(Clone)").GetComponent<FriendSelectLevelUpView> ().GetHelperUnitItem(0).gameObject;
//		NoviceGuideUtil.ForceOneBtnClick (gm);
//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,3)});
//		UIEventListenerCustom.Get (gm).onClick += OnClickFriend;
	}

	private void OnClickFriend(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickFriend;
		NoviceGuideUtil.RemoveAllArrows ();

		JumpToNextState = true;
	}

	public override void Execute(NoviceGuideStepEntity stepEntity)
	{	
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepI_StateThree.Instance());
		}
		else{
			
		}
	}

}

public class NoviceGuideStepI_StateThree:NoviceGuidState{
	
	private static NoviceGuideStepI_StateThree instance;
	
	public static NoviceGuideStepI_StateThree Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepI_StateThree ();
		return instance;
	}
	
	private NoviceGuideStepI_StateThree ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepI state_three");
		
		GameObject gm = GameObject.FindWithTag ("level_up_btn");
		UIEventListenerCustom.Get (gm).onClick += OnClickLevelUp;
		NoviceGuideUtil.ForceOneBtnClick (gm);
		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,4)});

	}

	
	private void OnClickLevelUp(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickLevelUp;
		NoviceGuideUtil.RemoveAllArrows ();

		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.SCRATCH;
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepI_StateFour.Instance());
		}
		else{
			
		}
	}
	
}

public class NoviceGuideStepI_StateFour:NoviceGuidState{
	
	private static NoviceGuideStepI_StateFour instance;
	
	public static NoviceGuideStepI_StateFour Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepI_StateFour ();
		return instance;
	}
	
	private NoviceGuideStepI_StateFour ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepI state_four");

//		ModuleManger.Instance.forbidChangeScene = true;

		MsgCenter.Instance.AddListener (CommandEnum.levelDone, OnLevelDone);

	}
	
	private void OnLevelDone(object data){

//		UIManager.Instance.forbidChangeScene = false;

		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText("guide46_title"),TextCenter.GetText("guide46_content"),TextCenter.GetText("NEXT"),ClickOK);
	}

	private void ClickOK(object data){
//		GameObject gm = GameObject.FindWithTag ("scene_back_btn");
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,3)});
//		UIEventListenerCustom.Get (gm).onClick += OnClickBack;
//		NoviceGuideUtil.ForceOneBtnClick (gm);

		ModuleManager.Instance.ShowModule (ModuleEnum.ScratchModule);
	}

	private void OnClickBack(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickBack;
		UIEventListenerCustom.Get (gm).onClick += OnClickBack1;
		NoviceGuideUtil.ForceOneBtnClick (gm);


	}

	private void OnClickBack1(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickBack1;
		NoviceGuideUtil.RemoveAllArrows ();
		JumpToNextState = true;

//		NoviceGuideStepEntityManager.CurrentNoviceGuideStage++;
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

//public class NoviceGuideStepI_StateFive:NoviceGuidState{
//	
//	private static NoviceGuideStepI_StateFive instance;
//	
//	public static NoviceGuideStepI_StateFive Instance()
//	{
//		if (instance == null)
//			instance = new NoviceGuideStepI_StateFive ();
//		return instance;
//	}
//	
//	private NoviceGuideStepI_StateFive ():base()	{}
//	
//	public override void Enter(NoviceGuideStepEntity stepEntity)
//	{
//		LogHelper.Log (stepEntity.GetType () + " is execute stepI state_five");
//		
//		GameObject gm = GameObject.FindWithTag ("scene_back_btn");
//		
//		NoviceGuideUtil.ShowArrow (new GameObject[]{gm}, new Vector3[]{new Vector3(0,0,3)});
//		UIEventListenerCustom.Get (gm).onClick += OnClickBack;
//		NoviceGuideUtil.ForceOneBtnClick (gm);
//	}
//	
//	private void OnClickBack(GameObject gm){
//		UIEventListenerCustom.Get (gm).onClick -= OnClickBack;
//		NoviceGuideUtil.RemoveAllArrows();
//
//		NoviceGuideStepEntityManager.CurrentNoviceGuideStage++;
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
//}

