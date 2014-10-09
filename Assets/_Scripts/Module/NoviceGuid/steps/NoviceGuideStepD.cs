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
	
	private NoviceGuideStepD_StateOne ():base(){}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide16_title"),TextCenter.GetText ("guide16_content"),TextCenter.GetText ("NEXT"),ClickOk2,null,GuidePicPath.ColorStarMove);
	}
	
	private void ClickOk2(object data){
		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide17_title"),TextCenter.GetText ("guide17_content"),TextCenter.GetText ("NEXT"),StartAnimation);
	}

	private void StartAnimation(object data){
		MsgCenter.Instance.Invoke (CommandEnum.UserGuideAnim);

		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, ClickOk3);
	}

	private void ClickOk3(object data){

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide18_title"),TextCenter.GetText ("guide18_content"),TextCenter.GetText ("OK"),TextCenter.GetText("guide18_once_again"),StartAnimation,OnceAgain);
	}

	
	private void ClickOk4(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, ClickOk3);
		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.FIRST_ATTACK_ONE;
		JumpToNextState = true;
	}
	
	private void OnceAgain(object data){
		MsgCenter.Instance.Invoke (CommandEnum.UserGuideAnim,true);
	}

	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepD_StateTwo.Instance());
		}
		else{
			
		}
	}
}


//attack once
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

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide19_title"),TextCenter.GetText ("guide19_content"),TextCenter.GetText ("NEXT"),ClickOk3);
	}
	
	private void ClickOk3(object data){

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide20_title"),TextCenter.GetText ("guide20_content"),TextCenter.GetText ("NEXT"),ClickOk2,null,GuidePicPath.FullBlock);
	}
	
	private void ClickOk2(object data){
		JumpToNextState = true;
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepD_StateThree.Instance());
		}
		else{
			
		}
	}
}
public class NoviceGuideStepD_StateThree:NoviceGuidState
{
	private static NoviceGuideStepD_StateThree instance;
	
	public static NoviceGuideStepD_StateThree Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepD_StateThree ();
		return instance;
	}
	
	private NoviceGuideStepD_StateThree ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepH state_Seven");
		
		BattleManipulationView bca = GameObject.FindWithTag ("battle_card").GetComponent<BattleManipulationView> ();
		
//		foreach (BattleCardAreaItem b in  bca.battleCardAreaItem) {
//			NoviceGuideUtil.ShowArrow(new GameObject[]{b.gameObject},new Vector3[]{new Vector3(0,0,1)},false);
//		}
		//
		NoviceGuideUtil.showTipText (TextCenter.GetText("guide_tips_1"));
		
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, OnEnemyAttackEnd);
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepD_StateFour.Instance());
		}
		else{
			
		}
	}
	
	public override void Exit (NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log("hide tip text");
		NoviceGuideUtil.HideTipText ();
		NoviceGuideUtil.RemoveAllArrows ();
		
	}
	
	private void OnEnemyAttackEnd(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, OnEnemyAttackEnd);
		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.FIRST_ATTACK_TWO;
		LogHelper.Log ("attack the enemy end!");
		JumpToNextState = true;
	}
}


//attack twice
public class NoviceGuideStepD_StateFour:NoviceGuidState
{
	private static NoviceGuideStepD_StateFour instance;
	
	public static NoviceGuideStepD_StateFour Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepD_StateFour ();
		return instance;
	}
	
	private NoviceGuideStepD_StateFour ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide21_title"),TextCenter.GetText ("guide21_content"),TextCenter.GetText ("NEXT"),ClickOk3);
		
		GameObject leader = GameObject.FindWithTag ("battle_leader");
		NoviceGuideUtil.ShowArrow (new GameObject[]{leader}, new Vector3[]{new Vector3(0,80,1)},false);
//		NoviceGuideUtil.ForceOneBtnClick (leader);
	}
	
	private void ClickOk3(object data){
		NoviceGuideUtil.RemoveAllArrows ();


		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide22_title"),TextCenter.GetText ("guide22_content"),TextCenter.GetText ("NEXT"),ClickOk2);
	}
	
	private void ClickOk2(object data){
		NoviceGuideUtil.showTipText (TextCenter.GetText("guide_tips_2"));
		
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, OnBattleEnd);
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepD_StateSix.Instance());
		}
		else{
			
		}
	}
	
	public override void Exit (NoviceGuideStepEntity stepEntity)
	{
		NoviceGuideUtil.HideTipText ();
	}
	
	private void OnBattleEnd(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, OnBattleEnd);
		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.GET_KEY;

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide23_title"),TextCenter.GetText ("guide23_content"),TextCenter.GetText ("NEXT"),ClickOk4);
		JumpToNextState = true;
	}

	private void ClickOk4(object data){
		JumpToNextState = true;
	}
}


//key
public class NoviceGuideStepD_StateSix:NoviceGuidState
{
	private static NoviceGuideStepD_StateSix instance;
	
	public static NoviceGuideStepD_StateSix Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepD_StateSix ();
		return instance;
	}
	
	private NoviceGuideStepD_StateSix ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
//		BattleMapView bm = GameObject.Find ("Map").GetComponent<BattleMapView> ();
//		MapItem item = null;
//		for(int i = 0; i < MapConfig.MapWidth; i++){
//			for(int j = 0; j < MapConfig.MapHeight; j++){
//				item = bm.GetMapItem(new Coordinate(i,j));
//				if(item.IsKey()){
//					LogHelper.Log ("map coordinate: i:" + i + "j: " + j);
//					goto outLoop;
//				}
//			}
//		}
		
	outLoop:
//		if (item != null) {
//			NoviceGuideUtil.ShowArrow(new GameObject[]{ item.gameObject},new Vector3[]{new Vector3(0,0,1)},false);	
//		}
		
		MsgCenter.Instance.AddListener (CommandEnum.OpenDoor, FindKey);
		
		
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepD_StateSeven.Instance());
		}
		else{
			
		}
	}
	
	public override void Exit (NoviceGuideStepEntity stepEntity)
	{
		NoviceGuideUtil.RemoveAllArrows ();
	}
	
	private void FindKey(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.OpenDoor, FindKey);
		JumpToNextState = true;
	}
	
}


public class NoviceGuideStepD_StateSeven:NoviceGuidState
{
	private static NoviceGuideStepD_StateSeven instance;
	
	public static NoviceGuideStepD_StateSeven Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepD_StateSeven ();
		return instance;
	}
	
	private NoviceGuideStepD_StateSeven ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide24_title"),TextCenter.GetText ("guide24_content"),TextCenter.GetText ("NEXT"),ClickOk3,null, GuidePicPath.FindKey);
		
		GameObject door = GameObject.FindWithTag ("map_door");
		NoviceGuideUtil.ShowArrow (new GameObject[]{door}, new Vector3[]{new Vector3 (-50, 0, 2)},false);
		
		MsgCenter.Instance.AddListener (CommandEnum.QuestEnd, OnQuestEnd);

		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.BOSS_ATTACK_ONE;
	}
	
	
	private void ClickOk3(object data){
		
	}
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (null);
		}
		else{
			
		}
	}
	
	public override void Exit (NoviceGuideStepEntity stepEntity)
	{
		NoviceGuideUtil.RemoveAllArrows ();
	}
	
	private void OnQuestEnd(object data){
		if ((bool)data) {
			MsgCenter.Instance.RemoveListener (CommandEnum.QuestEnd, OnQuestEnd);

			TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide25_title"),TextCenter.GetText ("guide25_content"),TextCenter.GetText ("NEXT"),ClickOk3,null, GuidePicPath.FindKey);
			
			NoviceGuideUtil.RemoveAllArrows();

		}
	}

}

