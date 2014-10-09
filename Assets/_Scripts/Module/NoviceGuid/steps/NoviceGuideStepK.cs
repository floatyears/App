using UnityEngine;
using System.Collections;

//boss attack
public class NoviceGuideStepK_StateOne:NoviceGuidState
{
	private static NoviceGuideStepK_StateOne instance;
	
	public static NoviceGuideStepK_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepK_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepK_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide26_title"),TextCenter.GetText ("guide26_content"),TextCenter.GetText ("NEXT"),ClickOk,null,GuidePicPath.ChangeBlockOrder);
		
		
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
	}
	
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepK_StateTwo.Instance());
		}
		else{
			
		}
	}
	
	private void ClickOk(object data){
		NoviceGuideUtil.showTipText (TextCenter.GetText("guide_tips_7"));
	}
	
	private void EnemyAttackEnd(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttackEnd);
		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.BOSS_ATTACK_HEAL;
		JumpToNextState = true;
	}
	
	public override void Exit (NoviceGuideStepEntity stepEntity)
	{
		NoviceGuideUtil.HideTipText ();
	}
}

public class NoviceGuideStepK_StateTwo:NoviceGuidState
{
	private static NoviceGuideStepK_StateTwo instance;
	
	public static NoviceGuideStepK_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepK_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepK_StateTwo ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide27_title"),TextCenter.GetText ("guide27_content"),TextCenter.GetText ("NEXT"),ClickOk,null,GuidePicPath.HealBlock);
		
	}
	
	
	private void ClickOk(object data){
		JumpToNextState = true;
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepK_StateThree.Instance());
		}
		else{
			
		}
	}
}


//heal
public class NoviceGuideStepK_StateThree:NoviceGuidState
{
	private static NoviceGuideStepK_StateThree instance;
	
	public static NoviceGuideStepK_StateThree Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepK_StateThree ();
		return instance;
	}
	
	private NoviceGuideStepK_StateThree ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide28_title"),TextCenter.GetText ("guide28_content"),TextCenter.GetText ("NEXT"),ClickOk,null,GuidePicPath.HealSkill);
		
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepK_StateFour.Instance());
		}
		else{
			
		}
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
		

		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.BOSS_ATTACK_SKILL;
		
	}
	
	private void ClickOk1(object data){
		JumpToNextState = true;
	}
	
	
}


//skill
public class NoviceGuideStepK_StateFour:NoviceGuidState
{
//	private GameObject nmw;
	
	private static NoviceGuideStepK_StateFour instance;
	
	public static NoviceGuideStepK_StateFour Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepK_StateFour ();
		return instance;
	}
	
	private NoviceGuideStepK_StateFour ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
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
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepK_StateFive.Instance());
		}
		else{
			
		}
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
		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.BOSS_ATTACK_BOOST;
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveSkillAttack, SkillAttack);

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide32_title"),TextCenter.GetText ("guide32_content"),TextCenter.GetText ("NEXT"),ClickOk2);
		
	}
	
	private void ClickOk2(object data){
		JumpToNextState = true;
	}
	
}

//boost
public class NoviceGuideStepK_StateFive:NoviceGuidState
{
	private static NoviceGuideStepK_StateFive instance;
	
	public static NoviceGuideStepK_StateFive Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepK_StateFive ();
		return instance;
	}
	
	private NoviceGuideStepK_StateFive ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide33_title"),TextCenter.GetText ("guide33_content"),TextCenter.GetText ("NEXT"),ClickOk,null,GuidePicPath.Boost);
		
		
		
		//GameObject.Find ();
//		NoviceGuideUtil.ShowArrow (new GameObject[]{BattleCardAreaItem.boostObject}, new Vector3[]{new Vector3(0,0,4)},false);
	}
	
	private void ClickOk(object data){
		
		NoviceGuideUtil.RemoveAllArrows ();
		
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, OnBattleEnd);
	}
	
	private void OnBattleEnd (object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, OnBattleEnd);

		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.UNIT_PARTY;

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide34_title"),TextCenter.GetText ("guide34_content"),TextCenter.GetText ("NEXT"));
		

	}

	private void Again(object data){
//		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.GOLD_BOX;
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
