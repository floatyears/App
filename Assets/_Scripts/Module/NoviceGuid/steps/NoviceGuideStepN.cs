using UnityEngine;
using System.Collections;

//city select
public class NoviceGuideStepN_StateOne:NoviceGuidState{
	
	private static NoviceGuideStepN_StateOne instance;
	
	public static NoviceGuideStepN_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepN_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepN_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		Debug.Log (stepEntity.GetType () + " is execute stepM state_one");
		
		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide61_title");
		mwp.contentText = TextCenter.GetText("guide61_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.GetText("NEXT");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
	}
	
	private void ClickOK(object data){
		GameObject first = GameObject.FindWithTag ("city_one");
		if (first == null)
			return;
		NoviceGuideUtil.ForceOneBtnClick (first);
		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,1)});
		UIEventListenerCustom.Get (first).onClick += OnClickItem;
	}
	
	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();
		
	}
	
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepN_StateTwo.Instance());
		}
		else{
			
		}
	}
	
}

//stage select
public class NoviceGuideStepN_StateTwo:NoviceGuidState{
	
	private static NoviceGuideStepN_StateTwo instance;
	
	public static NoviceGuideStepN_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepN_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepN_StateTwo ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepM state_two");
		
		GameObject first = GameObject.Find ("StageSelectWindow(Clone)").GetComponent<StageSelectView>().GetStageNewItem();
		//		if(first == null)
		//			stepEntity.GetStateMachine ().ChangeState (null);
		NoviceGuideUtil.ForceOneBtnClick (first);
		//		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,1)});
		UIEventListenerCustom.Get (first).onClick += OnClickItem;
	}
	
	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		//		NoviceGuideUtil.RemoveAllArrows ();
		
	}
	
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepN_StateThree.Instance());
		}
		else{
			
		}
	}
	
}

//quest select
public class NoviceGuideStepN_StateThree:NoviceGuidState{
	
	private static NoviceGuideStepN_StateThree instance;
	
	public static NoviceGuideStepN_StateThree Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepN_StateThree ();
		return instance;
	}
	
	private NoviceGuideStepN_StateThree ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepM state_three");
		
		GameObject first = GameObject.Find ("QuestSelectWindow(Clone)").GetComponent<QuestSelectView>().GetDragItem(0);
		NoviceGuideUtil.ForceOneBtnClick (first);
		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,3)});
		UIEventListenerCustom.Get (first).onClick += OnClickItem;
	}
	
	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();
		
	}
	
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepN_StateFour.Instance());
		}
		else{
			
		}
	}
	
}


//quest select
public class NoviceGuideStepN_StateFour:NoviceGuidState{
	
	private static NoviceGuideStepN_StateFour instance;
	
	public static NoviceGuideStepN_StateFour Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepN_StateFour ();
		return instance;
	}
	
	private NoviceGuideStepN_StateFour ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepM state_three");
		
		GameObject first = GameObject.Find ("FriendSelectWindow(Clone)").GetComponent<FriendHelperView>().GetFriendItem(0);
		NoviceGuideUtil.ForceOneBtnClick (first);
		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,3)});
		UIEventListenerCustom.Get (first).onClick += OnClickItem;
	}
	
	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();
		
	}
	
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepN_StateFive.Instance());
		}
		else{
			
		}
	}
	
}

//fight ready
public class NoviceGuideStepN_StateFive:NoviceGuidState{
	
	private static NoviceGuideStepN_StateFive instance;
	
	public static NoviceGuideStepN_StateFive Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepN_StateFive ();
		return instance;
	}
	
	private NoviceGuideStepN_StateFive ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepM state_five");
		
		GameObject first = GameObject.FindWithTag ("fight_btn");
		NoviceGuideUtil.ForceOneBtnClick (first,true);
		NoviceGuideUtil.ShowArrow (new GameObject[]{first}, new Vector3[]{new Vector3(0,0,1)});
		UIEventListenerCustom.Get (first).onClick += OnClickItem;
	}
	
	private void OnClickItem(GameObject gm){
		UIEventListenerCustom.Get (gm).onClick -= OnClickItem;
		NoviceGuideUtil.RemoveAllArrows ();
		
		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.INPUT_NAME;
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