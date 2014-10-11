using UnityEngine;
using System.Collections;



public class NoviceGuideStepD_1:NoviceGuidStep
{
	
	public override void Enter()
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
		GoToNextState();
	}
	
	private void OnceAgain(object data){
		MsgCenter.Instance.Invoke (CommandEnum.UserGuideAnim,true);
	}
	
}


//attack once
public class NoviceGuideStepD_2:NoviceGuidStep
{
	
	public override void Enter()
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide19_title"),TextCenter.GetText ("guide19_content"),TextCenter.GetText ("NEXT"),ClickOk3);
	}
	
	private void ClickOk3(object data){

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide20_title"),TextCenter.GetText ("guide20_content"),TextCenter.GetText ("NEXT"),ClickOk2,null,GuidePicPath.FullBlock);
	}
	
	private void ClickOk2(object data){
		GoToNextState();
	}

}
public class NoviceGuideStepD_3:NoviceGuidStep
{
	
	public override void Enter()
	{
		
		BattleManipulationView bca = GameObject.FindWithTag ("battle_card").GetComponent<BattleManipulationView> ();
		
//		foreach (BattleCardAreaItem b in  bca.battleCardAreaItem) {
//			NoviceGuideUtil.ShowArrow(new GameObject[]{b.gameObject},new Vector3[]{new Vector3(0,0,1)},false);
//		}
		//
		NoviceGuideUtil.showTipText (TextCenter.GetText("guide_tips_1"));
		
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, OnEnemyAttackEnd);
	}
	
	public override void Exit ()
	{
		LogHelper.Log("hide tip text");
		NoviceGuideUtil.HideTipText ();
		NoviceGuideUtil.RemoveAllArrows ();
		
	}
	
	private void OnEnemyAttackEnd(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, OnEnemyAttackEnd);
		LogHelper.Log ("attack the enemy end!");
		GoToNextState();
	}
}


//attack twice
public class NoviceGuideStepD_4:NoviceGuidStep
{
	
	public override void Enter()
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
		
		MsgCenter.Instance.AddListener (CommandEnum.FightEnd, OnBattleEnd);
	}

	
	public override void Exit ()
	{
		NoviceGuideUtil.HideTipText ();
	}
	
	private void OnBattleEnd(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.FightEnd, OnBattleEnd);

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide23_title"),TextCenter.GetText ("guide23_content"),TextCenter.GetText ("NEXT"),ClickOk4);
		GoToNextState();
	}

	private void ClickOk4(object data){
		GoToNextState();
	}
}


//key
public class NoviceGuideStepD_6:NoviceGuidStep
{
	
	public override void Enter()
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

	
	public override void Exit ()
	{
		NoviceGuideUtil.RemoveAllArrows ();
	}
	
	private void FindKey(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.OpenDoor, FindKey);
		GoToNextState();
	}
	
}


public class NoviceGuideStepD_7:NoviceGuidStep
{
	
	public override void Enter()
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide24_title"),TextCenter.GetText ("guide24_content"),TextCenter.GetText ("NEXT"),ClickOk3,null, GuidePicPath.FindKey);
		
		GameObject door = GameObject.FindWithTag ("map_door");
		NoviceGuideUtil.ShowArrow (new GameObject[]{door}, new Vector3[]{new Vector3 (-50, 0, 2)},false);
		
		MsgCenter.Instance.AddListener (CommandEnum.QuestEnd, OnQuestEnd);

	}
	
	
	private void ClickOk3(object data){
		
	}
	
	public override void Exit ()
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

