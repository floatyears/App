using UnityEngine;
using System.Collections;

//city select
public class NoviceGuideStepM_StateOne:NoviceGuidState{
		
	private static NoviceGuideStepM_StateOne instance;
		
	public static NoviceGuideStepM_StateOne Instance()
	{
		if (instance == null)
		instance = new NoviceGuideStepM_StateOne ();
		return instance;
	}
		
	private NoviceGuideStepM_StateOne ():base()	{}
		
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
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepM_StateTwo.Instance());
		}
		else{
			
		}
	}
	
}

//stage select
public class NoviceGuideStepM_StateTwo:NoviceGuidState{
	
	private static NoviceGuideStepM_StateTwo instance;
	
	public static NoviceGuideStepM_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepM_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepM_StateTwo ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepM state_two");
		
		GameObject first = GameObject.Find ("StageSelectWindow(Clone)").GetComponent<StageSelectView>().GetStageEvolveItem();
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
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepM_StateThree.Instance());
		}
		else{
			
		}
	}
	
}

//quest select
public class NoviceGuideStepM_StateThree:NoviceGuidState{
	
	private static NoviceGuideStepM_StateThree instance;
	
	public static NoviceGuideStepM_StateThree Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepM_StateThree ();
		return instance;
	}
	
	private NoviceGuideStepM_StateThree ():base()	{}
	
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
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepM_StateFive.Instance());
		}
		else{
			
		}
	}
	
}


//quest select
public class NoviceGuideStepM_StateFour:NoviceGuidState{
	
	private static NoviceGuideStepM_StateFour instance;
	
	public static NoviceGuideStepM_StateFour Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepM_StateFour ();
		return instance;
	}
	
	private NoviceGuideStepM_StateFour ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepM state_three");
		
		GameObject first = GameObject.Find ("FriendSelectWindow(Clone)").GetComponent<FriendSelectView>().GetFriendItem(0);
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
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepM_StateFive.Instance());
		}
		else{
			
		}
	}
	
}

//fight ready
public class NoviceGuideStepM_StateFive:NoviceGuidState{
	
	private static NoviceGuideStepM_StateFive instance;
	
	public static NoviceGuideStepM_StateFive Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepM_StateFive ();
		return instance;
	}
	
	private NoviceGuideStepM_StateFive ():base()	{}
	
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

		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.NONE;
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