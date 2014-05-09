using UnityEngine;
using System.Collections;

public class NoviceGuideStepH_StateOne:NoviceGuidState
{
	private static NoviceGuideStepH_StateOne instance;
	
	public static NoviceGuideStepH_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepH_StateOne ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepH state_one");

		BattleMap bm = GameObject.Find ("Map").GetComponent<BattleMap> ();
		MapItem item = null;
		for(int i = 0; i < MapConfig.MapWidth; i++){
			for(int j = 0; j < MapConfig.MapHeight; j++){
				item = bm.GetMapItem(new Coordinate(i,j));
				if(item.IsKey()){
					LogHelper.Log ("map coordinate: i:" + i + "j: " + j);
					goto outLoop;
				}
			}
		}

	outLoop:
		if (item != null) {
			NoviceGuideUtil.ShowArrow(new GameObject[]{ item.gameObject},new Vector3[]{new Vector3(0,0,1)});	
		}

		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide12_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide12_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.args = item.gameObject;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;

		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}
	
	private void ClickOK(object data)
	{
		LogHelper.Log("--------////////step h:"+data);
		NoviceGuideUtil.RemoveArrow ((GameObject)data);

		GuideWindowParams mwp = new GuideWindowParams ();
		mwp.guidePic = GuidePicPath.StarMove;
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide13_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide13_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk1;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;

		MsgCenter.Instance.Invoke (CommandEnum.OpenGuideMsgWindow, mwp);

		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, OnBattleEnd);
	}

	private void ClickOk1(object data)
	{
		BattleMap bm = GameObject.Find ("Map").GetComponent<BattleMap> ();
		GameObject[] objs = new GameObject[] {
						bm.GetMapItem (new Coordinate (1, 0)).gameObject,
						bm.GetMapItem (new Coordinate (2, 1)).gameObject,
						bm.GetMapItem (new Coordinate (3, 0)).gameObject
				};
		NoviceGuideUtil.ShowArrow(objs,new Vector3[]{new Vector3(0,0,1),new Vector3(0,0,1),new Vector3(0,0,1)});

		NoviceGuideUtil.ForceBtnsClick(objs,delegate(GameObject btn){
			NoviceGuideUtil.RemoveAllArrows();
			//JumpToNextState = true;
		});
	}

	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateTwo.Instance());
		}
		else{
			
		}
	}

	private void OnBattleEnd(object data){
		JumpToNextState = true;
	}

}

public class NoviceGuideStepH_StateTwo:NoviceGuidState
{
	private static NoviceGuideStepH_StateTwo instance;
	
	public static NoviceGuideStepH_StateTwo Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateTwo ();
		return instance;
	}
	
	private NoviceGuideStepH_StateTwo ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide14_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide14_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;

		mwp.guidePic = GuidePicPath.GoldBox;

		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);

	}

	private void ClickOK(object data)
	{
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide15_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide15_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk2;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);

		GameObject sp1 = GameObject.FindWithTag ("battle_sp1");
		NoviceGuideUtil.ShowArrow (new GameObject[]{sp1}, new Vector3[]{new Vector3 (0, 0, 1)});


	}

	private void ClickOk2(object data){
		NoviceGuideUtil.RemoveAllArrows ();

		JumpToNextState = true;
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateThree.Instance());
		}
		else{
			
		}
	}
}

public class NoviceGuideStepH_StateThree:NoviceGuidState
{
	private static NoviceGuideStepH_StateThree instance;
	
	public static NoviceGuideStepH_StateThree Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateThree ();
		return instance;
	}
	
	private NoviceGuideStepH_StateThree ():base()	{}

	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepH state_Three");

		BattleMap bm = GameObject.Find ("Map").GetComponent<BattleMap> ();
		MapItem item = null;
		for(int i = 0; i < MapConfig.MapWidth; i++){
			for(int j = 0; j < MapConfig.MapHeight; j++){
				item = bm.GetMapItem(new Coordinate(i,j));
				if(item.IsKey()){
					LogHelper.Log ("map coordinate: i:" + i + "j: " + j);
					goto outLoop;
				}
			}
		}
		
	outLoop:
		if (item != null) {
			NoviceGuideUtil.ShowArrow(new GameObject[]{ item.gameObject},new Vector3[]{new Vector3(0,0,1)});	
		}

		MsgCenter.Instance.AddListener (CommandEnum.BattleStart, OnBattleStart);
	}
	
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateFour.Instance());
		}
		else{
			
		}
	}

	public override void Exit(NoviceGuideStepEntity stepEntity)
	{
		NoviceGuideUtil.RemoveAllArrows ();
	}

	private void OnBattleStart(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleStart, OnBattleStart);
		JumpToNextState = true;
	}
}

public class NoviceGuideStepH_StateFour:NoviceGuidState
{
	private static NoviceGuideStepH_StateFour instance;
	
	public static NoviceGuideStepH_StateFour Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateFour ();
		return instance;
	}
	
	private NoviceGuideStepH_StateFour ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide16_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide16_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk2;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;

		mwp.guidePic = GuidePicPath.ColorStarMove;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}

	private void ClickOk2(object data){
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide17_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide17_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk3;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}

	private void ClickOk3(object data){
		JumpToNextState = true;
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateFive.Instance());
		}
		else{
			
		}
	}
}

public class NoviceGuideStepH_StateFive:NoviceGuidState
{
	private static NoviceGuideStepH_StateFive instance;
	
	public static NoviceGuideStepH_StateFive Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateFive ();
		return instance;
	}
	
	private NoviceGuideStepH_StateFive ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		//TODO:the attack guide animation.

		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParams = new BtnParam[2];
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide18_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide18_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk3;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParams[0] = sure;

		sure = new BtnParam ();
		sure.text = "once again";
		sure.callback = OnceAgain;
		mwp.btnParams [1] = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}

	private void ClickOk3(object data){
		JumpToNextState = true;
	}

	private void OnceAgain(object data){
		
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateSix.Instance());
		}
		else{
			
		}
	}
}

public class NoviceGuideStepH_StateSix:NoviceGuidState
{
	private static NoviceGuideStepH_StateSix instance;
	
	public static NoviceGuideStepH_StateSix Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateSix ();
		return instance;
	}
	
	private NoviceGuideStepH_StateSix ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide19_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide19_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk3;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}
	
	private void ClickOk3(object data){

		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide20_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide20_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk2;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;

		mwp.guidePic = GuidePicPath.FullBlock;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}

	private void ClickOk2(object data){
		JumpToNextState = true;
	}

	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateSeven.Instance());
		}
		else{
			
		}
	}
}
public class NoviceGuideStepH_StateSeven:NoviceGuidState
{
	private static NoviceGuideStepH_StateSeven instance;
	
	public static NoviceGuideStepH_StateSeven Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateSeven ();
		return instance;
	}
	
	private NoviceGuideStepH_StateSeven ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " get into stepH state_Seven");

		BattleCardArea bca = GameObject.FindWithTag ("battle_card").GetComponent<BattleCardArea> ();

		foreach (BattleCardAreaItem b in  bca.battleCardAreaItem) {
			NoviceGuideUtil.ShowArrow(new GameObject[]{b.gameObject},new Vector3[]{new Vector3(0,0,1)});
		}
		//
		NoviceGuideUtil.showTipText ("try to put the blue item in one block");

		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, OnEnemyAttackEnd);
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateEight.Instance());
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
		LogHelper.Log ("attack the enemy end!");
		JumpToNextState = true;
	}
}

public class NoviceGuideStepH_StateEight:NoviceGuidState
{
	private static NoviceGuideStepH_StateEight instance;
	
	public static NoviceGuideStepH_StateEight Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateEight ();
		return instance;
	}
	
	private NoviceGuideStepH_StateEight ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide21_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide21_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk3;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);

		GameObject leader = GameObject.FindWithTag ("battle_leader");
		NoviceGuideUtil.ShowArrow (new GameObject[]{leader}, new Vector3[]{new Vector3(0,140,1)});
	}
	
	private void ClickOk3(object data){
		NoviceGuideUtil.RemoveAllArrows ();

		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide22_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide22_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk2;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}

	private void ClickOk2(object data){
		NoviceGuideUtil.showTipText ("try to drag the block to take down the enemy");

		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, OnBattleEnd);
	}

	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateNine.Instance());
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
		JumpToNextState = true;
	}
}

public class NoviceGuideStepH_StateNine:NoviceGuidState
{
	private static NoviceGuideStepH_StateNine instance;
	
	public static NoviceGuideStepH_StateNine Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateNine ();
		return instance;
	}
	
	private NoviceGuideStepH_StateNine ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide23_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide23_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk3;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);


	}
	
	private void ClickOk3(object data){
		BattleMap bm = GameObject.Find ("Map").GetComponent<BattleMap> ();
		MapItem item = null;
		for(int i = 0; i < MapConfig.MapWidth; i++){
			for(int j = 0; j < MapConfig.MapHeight; j++){
				item = bm.GetMapItem(new Coordinate(i,j));
				if(item.IsKey()){
					LogHelper.Log ("map coordinate: i:" + i + "j: " + j);
					goto outLoop;
				}
			}
		}
		
	outLoop:
		if (item != null) {
			NoviceGuideUtil.ShowArrow(new GameObject[]{ item.gameObject},new Vector3[]{new Vector3(0,0,1)});	
		}

		MsgCenter.Instance.AddListener (CommandEnum.OpenDoor, FindKey);

	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateTen.Instance());
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

public class NoviceGuideStepH_StateTen:NoviceGuidState
{
	private static NoviceGuideStepH_StateTen instance;
	
	public static NoviceGuideStepH_StateTen Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateTen ();
		return instance;
	}
	
	private NoviceGuideStepH_StateTen ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide24_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide24_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk3;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;

		mwp.guidePic = GuidePicPath.FindKey;

		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);

		GameObject door = GameObject.FindWithTag ("map_door");
		NoviceGuideUtil.ShowArrow (new GameObject[]{door}, new Vector3[]{new Vector3 (-50, 0, 2)});

		MsgCenter.Instance.AddListener (CommandEnum.QuestEnd, OnQuestEnd);
	}


	private void ClickOk3(object data){

	}
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateEleven.Instance());
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

			GuideWindowParams mwp = new GuideWindowParams();
			mwp.btnParam = new BtnParam ();
			mwp.titleText = TextCenter.Instace.GetCurrentText("guide25_title");
			mwp.contentText = TextCenter.Instace.GetCurrentText("guide25_content");
			
			BtnParam sure = new BtnParam ();
			sure.callback = ClickOk3;
			sure.text = TextCenter.Instace.GetCurrentText("OK");
			mwp.btnParam = sure;
			
			MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);

			NoviceGuideUtil.RemoveAllArrows();

			MsgCenter.Instance.AddListener(CommandEnum.BattleStart,OnBattleStart);
		}
	}

	private void OnBattleStart(object data){
		MsgCenter.Instance.RemoveListener(CommandEnum.BattleStart,OnBattleStart);
		JumpToNextState = true;
	}
}

public class NoviceGuideStepH_StateEleven:NoviceGuidState
{
	private static NoviceGuideStepH_StateEleven instance;
	
	public static NoviceGuideStepH_StateEleven Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateEleven ();
		return instance;
	}
	
	private NoviceGuideStepH_StateEleven ():base()	{}
	
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
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateTwelve.Instance());
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

public class NoviceGuideStepH_StateTwelve:NoviceGuidState
{
	private static NoviceGuideStepH_StateTwelve instance;
	
	public static NoviceGuideStepH_StateTwelve Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateTwelve ();
		return instance;
	}
	
	private NoviceGuideStepH_StateTwelve ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide27_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide27_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		mwp.guidePic = GuidePicPath.HealBlock;
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateThirteen.Instance());
		}
		else{
			
		}
	}
}

public class NoviceGuideStepH_StateThirteen:NoviceGuidState
{
	private static NoviceGuideStepH_StateThirteen instance;
	
	public static NoviceGuideStepH_StateThirteen Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateThirteen ();
		return instance;
	}
	
	private NoviceGuideStepH_StateThirteen ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide29_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide29_content");
		
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
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateForteen.Instance());
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
		JumpToNextState = true;
	}
}

public class NoviceGuideStepH_StateForteen:NoviceGuidState
{
	private static NoviceGuideStepH_StateForteen instance;
	
	public static NoviceGuideStepH_StateForteen Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateForteen ();
		return instance;
	}
	
	private NoviceGuideStepH_StateForteen ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide30_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide30_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;

		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateFifteen.Instance());
		}
		else{
			
		}
	}
	
	private void ClickOk(object data){

		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide31_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide31_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);

		GameObject leader = GameObject.FindWithTag ("battle_leader");
		NoviceGuideUtil.ShowArrow (new GameObject[]{leader}, new Vector3[]{new Vector3 (0, 140, 1)});
		NoviceGuideUtil.ForceOneBtnClick (leader);
		UIEventListener.Get (leader).onClick += ClickLeader;
	}

	private void ClickLeader(GameObject btn){
		UIEventListener.Get (btn).onClick = ClickLeader;

		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide32_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide32_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk1;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}

	private void ClickOk1(object btn){
		GameObject bs = GameObject.FindWithTag ("boost_skill");

		NoviceGuideUtil.ForceOneBtnClick (bs);
		NoviceGuideUtil.ShowArrow (new GameObject[]{bs}, new Vector3[]{new Vector3(0,0,3)});
		UIEventListener.Get (bs).onClick += ClickSkill;
	}

	private void ClickSkill(GameObject btn){
		NoviceGuideUtil.RemoveArrow (btn);
		UIEventListener.Get (btn).onClick -= ClickSkill;

		MsgCenter.Instance.AddListener (CommandEnum.ActiveSkillAttack, SkillAttack);
	}

	private void SkillAttack(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveSkillAttack, SkillAttack);

		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide33_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide33_content");
		
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

public class NoviceGuideStepH_StateFifteen:NoviceGuidState
{
	private static NoviceGuideStepH_StateFifteen instance;
	
	public static NoviceGuideStepH_StateFifteen Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_StateFifteen ();
		return instance;
	}
	
	private NoviceGuideStepH_StateFifteen ():base()	{}
	
	public override void Enter(NoviceGuideStepEntity stepEntity)
	{
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide34_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide34_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}

	private void ClickOk(object data){

	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (NoviceGuideStepH_StateTen.Instance());
		}
		else{
			
		}
	}


}