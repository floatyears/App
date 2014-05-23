﻿using UnityEngine;
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
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide26_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide26_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
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
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide27_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide27_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
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
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide28_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide28_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
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
		NoviceGuideUtil.showTipText ("drag the red heart togather");
		
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnd);
	}
	
	private void AttackEnd(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnd);
		NoviceGuideUtil.HideTipText ();
		
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide29_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide29_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk1;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
	}
	
	private void ClickOk1(object data){
		JumpToNextState = true;
	}
	
	
}


//skill
public class NoviceGuideStepK_StateFour:NoviceGuidState
{
	private GameObject nmw;
	
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
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide30_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide30_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
		GameObject leader = GameObject.FindWithTag ("battle_leader");
		NoviceGuideUtil.ShowArrow (new GameObject[]{leader}, new Vector3[]{new Vector3 (0, 140, 1)});
		
		BattleBottom bbs = GameObject.Find ("BattleBottom(Clone)").GetComponent<BattleBottom>();
		bbs.SetLeaderToNoviceGuide (true);
		bbs.IsUseLeaderSkill = true;
		MsgCenter.Instance.AddListener (CommandEnum.UseLeaderSkill, OnUseLeaderSkill);
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
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide31_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide31_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk1;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
		
		nmw = GameObject.Find ("NoviceGuideWindow(Clone)");
		UIPanel up = nmw.AddComponent<UIPanel>();
		up.depth = 4;
		
		MsgCenter.Instance.RemoveListener (CommandEnum.UseLeaderSkill, OnUseLeaderSkill);
		BattleBottom bbs = GameObject.Find ("BattleBottom(Clone)").GetComponent<BattleBottom>();
		bbs.SetLeaderToNoviceGuide (false);
		bbs.IsUseLeaderSkill = false;
		//UIEventListener.Get (leader).onClick += ClickLeader;
	}
	
	private void ClickOk1(object btn){
		GameObject.Destroy (nmw.GetComponent<UIPanel> ());
		nmw = null;
		
		GameObject bs = GameObject.FindWithTag ("boost_skill");
		
		NoviceGuideUtil.ForceOneBtnClick (bs);
		NoviceGuideUtil.ShowArrow (new GameObject[]{bs}, new Vector3[]{new Vector3(0,0,4)});
		UIEventListener.Get (bs).onClick += ClickSkill;
		
		MsgCenter.Instance.AddListener (CommandEnum.ActiveSkillAttack, SkillAttack);
	}
	
	
	private void ClickSkill(GameObject btn){
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListener.Get (btn).onClick -= ClickSkill;
		
	}
	
	private void SkillAttack(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveSkillAttack, SkillAttack);
		
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide32_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide32_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk2;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}
	
	private void ClickOk2(object data){
		JumpToNextState = true;
	}
	
}

//
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
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide33_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide33_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
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
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide34_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide34_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
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