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
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide26_title");
		mwp.contentText = TextCenter.GetText("guide26_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		mwp.guidePic = GuidePicPath.ChangeBlockOrder;
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
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
		NoviceGuideUtil.showTipText ("attack by sort the colors.");
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
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide27_title");
		mwp.contentText = TextCenter.GetText("guide27_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		mwp.guidePic = GuidePicPath.HealBlock;
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
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
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide28_title");
		mwp.contentText = TextCenter.GetText("guide28_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		mwp.guidePic = GuidePicPath.HealSkill;
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
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

		NoviceGuideUtil.showTipText ("drag the red heart togather");
		
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnd);
	}
	
	private void AttackEnd(object data){

		MsgCenter.Instance.Invoke (CommandEnum.UserGuideCard, -1);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnd);
		NoviceGuideUtil.HideTipText ();
		
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide29_title");
		mwp.contentText = TextCenter.GetText("guide29_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk1;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);

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
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide30_title");
		mwp.contentText = TextCenter.GetText("guide30_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
		GameObject leader = GameObject.FindWithTag ("battle_leader");
		NoviceGuideUtil.ShowArrow (new GameObject[]{leader}, new Vector3[]{new Vector3 (0, 140, 1)});
		
		BattleBottom bbs = GameObject.Find ("BattleBottom").GetComponent<BattleBottom>();
		bbs.SetLeaderToNoviceGuide (true);
		bbs.IsUseLeaderSkill = true;
		MsgCenter.Instance.AddListener (CommandEnum.UseLeaderSkill, OnUseLeaderSkill);
		BattleBottom.SetClickItem (0);
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
		NoviceGuideUtil.RemoveAllArrows ();
		
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide31_title");
		mwp.contentText = TextCenter.GetText("guide31_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk1;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);

		BattleBottom.SetClickItem (-1);
//		nmw = GameObject.Find ("NoviceGuideWindow(Clone)");
//		UIPanel up = nmw.AddComponent<UIPanel>();
//		up.depth = 4;
		
		MsgCenter.Instance.RemoveListener (CommandEnum.UseLeaderSkill, OnUseLeaderSkill);
		BattleBottom bbs = GameObject.Find ("BattleBottom").GetComponent<BattleBottom>();
		bbs.SetLeaderToNoviceGuide (false);
		bbs.IsUseLeaderSkill = false;
		//UIEventListener.Get (leader).onClick += ClickLeader;
	}
	
	private void ClickOk1(object btn){
//		GameObject.Destroy (nmw.GetComponent<UIPanel> ());
//		nmw = null;

		BattleBottom.notClick = true;

		GameObject bs = GameObject.FindWithTag ("boost_skill");
		
		NoviceGuideUtil.ForceOneBtnClick (bs);
		NoviceGuideUtil.ShowArrow (new GameObject[]{bs}, new Vector3[]{new Vector3(0,0,4)});
		UIEventListener.Get (bs).onClick += ClickSkill;
		
		MsgCenter.Instance.AddListener (CommandEnum.ActiveSkillAttack, SkillAttack);
	}
	
	
	private void ClickSkill(GameObject btn){
		BattleBottom.notClick = false;

		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListener.Get (btn).onClick -= ClickSkill;
		
	}
	
	private void SkillAttack(object data){
		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.BOSS_ATTACK_BOOST;
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveSkillAttack, SkillAttack);
		
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide32_title");
		mwp.contentText = TextCenter.GetText("guide32_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk2;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
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
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide33_title");
		mwp.contentText = TextCenter.GetText("guide33_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		mwp.guidePic = GuidePicPath.Boost;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
		//GameObject.Find ();
		NoviceGuideUtil.ShowArrow (new GameObject[]{BattleCardAreaItem.boostObject}, new Vector3[]{new Vector3(0,0,4)});
	}
	
	private void ClickOk(object data){
		
		NoviceGuideUtil.RemoveAllArrows ();
		
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, OnBattleEnd);
	}
	
	private void OnBattleEnd (object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, OnBattleEnd);
		
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam();
		mwp.titleText = TextCenter.GetText("guide34_title");
		mwp.contentText = TextCenter.GetText("guide34_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;

//		sure = new BtnParam ();
//		sure.callback = Again;
//		sure.text = TextCenter.GetText("Again");
//		mwp.btnParams[1] = sure;

		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.UNIT_PARTY;
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);

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
