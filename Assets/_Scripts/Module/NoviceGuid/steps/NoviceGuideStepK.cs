using UnityEngine;
using System.Collections;

//boss attack
public class NoviceGuideStepK_1:NoviceGuidStep
{
	
	public override void Enter()
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide26_title"),TextCenter.GetText ("guide26_content"),TextCenter.GetText ("NEXT"),ClickOk,null,GuidePicPath.ChangeBlockOrder);
		
		
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
	}

	private void ClickOk(object data){
		NoviceGuideUtil.showTipText (TextCenter.GetText("guide_tips_7"));
	}
	
	private void EnemyAttackEnd(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
		GoToNextState();
	}
	
	public override void Exit ()
	{
		NoviceGuideUtil.HideTipText ();
	}
}

public class NoviceGuideStepK_2:NoviceGuidStep
{
	
	public override void Enter()
	{
		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide27_title"),TextCenter.GetText ("guide27_content"),TextCenter.GetText ("NEXT"),ClickOk,null,GuidePicPath.HealBlock);
		
	}
	
	
	private void ClickOk(object data){
		GoToNextState();
	}
}


//heal
public class NoviceGuideStepK_3:NoviceGuidStep
{
	
	public override void Enter()
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide28_title"),TextCenter.GetText ("guide28_content"),TextCenter.GetText ("NEXT"),ClickOk,null,GuidePicPath.HealSkill);
		
	}
	
	private void ClickOk(object data){
		MsgCenter.Instance.Invoke (CommandEnum.UserGuideCard, 7);

//		MsgCenter.Instance.RemoveListener (CommandEnum.ChangeCardColor, );

		NoviceGuideUtil.showTipText (TextCenter.GetText("guide_tips_8"));
		
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnd);
	}
	
	private void AttackEnd(object data){

		MsgCenter.Instance.Invoke (CommandEnum.UserGuideCard, -1);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnd);
		NoviceGuideUtil.HideTipText ();

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide29_title"),TextCenter.GetText ("guide29_content"),TextCenter.GetText ("NEXT"),ClickOk1);
		

	}
	
	private void ClickOk1(object data){
		GoToNextState();
	}
	
	
}


//skill
public class NoviceGuideStepK_4:NoviceGuidStep
{
	
	public override void Enter()
	{
		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide30_title"),TextCenter.GetText ("guide30_content"),TextCenter.GetText ("NEXT"));
		
		
		GameObject leader = GameObject.FindWithTag ("battle_leader");
		NoviceGuideUtil.ShowArrow (new GameObject[]{leader}, new Vector3[]{new Vector3 (0, 80, 1)});
		
		BattleBottomView bbs = GameObject.Find ("BattleBottom").GetComponent<BattleBottomView>();
		bbs.SetLeaderToNoviceGuide (true);
//		bbs.IsUseLeaderSkill = true;
		MsgCenter.Instance.AddListener (CommandEnum.UseLeaderSkill, OnUseLeaderSkill);

		Debug.Log ("battle leader skill");
		MsgCenter.Instance.Invoke (CommandEnum.ShiledInput, true);
//		BattleBottomView.SetClickItem (0);
//		ExcuteActiveSkill.CoolingDoneLeaderActiveSkill ();
	}
	
	private void OnUseLeaderSkill(object btn){

		MsgCenter.Instance.Invoke (CommandEnum.ShiledInput, false);
		NoviceGuideUtil.RemoveAllArrows ();

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide31_title"),TextCenter.GetText ("guide31_content"),TextCenter.GetText ("NEXT"),ClickOk1);
		

//		BattleBottomView.SetClickItem (-1);
//		nmw = GameObject.Find ("NoviceGuideWindow(Clone)");
//		UIPanel up = nmw.AddComponent<UIPanel>();
//		up.depth = 4;
		
		MsgCenter.Instance.RemoveListener (CommandEnum.UseLeaderSkill, OnUseLeaderSkill);
		BattleBottomView bbs = GameObject.Find ("BattleBottom").GetComponent<BattleBottomView>();
		bbs.SetLeaderToNoviceGuide (false);
//		bbs.IsUseLeaderSkill = false;
		//UIEventListenerCustom.Get (leader).onClick += ClickLeader;
	}
	
	private void ClickOk1(object btn){
//		GameObject.Destroy (nmw.GetComponent<UIPanel> ());
//		nmw = null;

//		BattleBottomView.notClick = true;

		GameObject bs = GameObject.FindWithTag ("boost_skill");
		
		NoviceGuideUtil.ForceOneBtnClick (bs);
		NoviceGuideUtil.ShowArrow (new GameObject[]{bs}, new Vector3[]{new Vector3(0,0,4)});
		UIEventListenerCustom.Get (bs).onClick += ClickSkill;
		
		MsgCenter.Instance.AddListener (CommandEnum.ActiveSkillAttack, SkillAttack);
	}
	
	
	private void ClickSkill(GameObject btn){
//		BattleBottomView.notClick = false;

		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListenerCustom.Get (btn).onClick -= ClickSkill;
		
	}
	
	private void SkillAttack(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveSkillAttack, SkillAttack);

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide32_title"),TextCenter.GetText ("guide32_content"),TextCenter.GetText ("NEXT"),ClickOk2);
		
	}
	
	private void ClickOk2(object data){
		GoToNextState();
	}
	
}

//boost
public class NoviceGuideStepK_5:NoviceGuidStep
{
	
	public override void Enter()
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide33_title"),TextCenter.GetText ("guide33_content"),TextCenter.GetText ("NEXT"),ClickOk,null,GuidePicPath.Boost);
		
	}
	
	private void ClickOk(object data){
		
		NoviceGuideUtil.RemoveAllArrows ();
		
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, OnBattleEnd);
	}
	
	private void OnBattleEnd (object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, OnBattleEnd);


		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide34_title"),TextCenter.GetText ("guide34_content"),TextCenter.GetText ("NEXT"));
		

	}

	private void Again(object data){
//		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.GOLD_BOX;
	}


}
