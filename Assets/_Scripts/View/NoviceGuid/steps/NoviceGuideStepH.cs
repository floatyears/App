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
			NoviceGuideUtil.ShowArrow(new GameObject[]{ item.gameObject},new Vector3[]{new Vector3(0,0,1)},false);	
		}

		GuideWindowParams mwp = new GuideWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide12_title");
		mwp.contentText = TextCenter.GetText("guide12_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.args = item.gameObject;
		sure.text = TextCenter.GetText("NEXT");
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
		mwp.titleText = TextCenter.GetText("guide13_title");
		mwp.contentText = TextCenter.GetText("guide13_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk1;
		sure.text = TextCenter.GetText("NEXT");
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
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, OnBattleEnd);
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
		mwp.titleText = TextCenter.GetText("guide14_title");
		mwp.contentText = TextCenter.GetText("guide14_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.text = TextCenter.GetText("NEXT");
		mwp.btnParam = sure;

		mwp.guidePic = GuidePicPath.GoldBox;

		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);


		NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.ANIMATION;
	}

	private void ClickOK(object data)
	{
		GuideWindowParams mwp = new GuideWindowParams();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide15_title");
		mwp.contentText = TextCenter.GetText("guide15_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOk2;
		sure.text = TextCenter.GetText("NEXT");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);

		GameObject sp1 = GameObject.FindWithTag ("battle_sp1");
		NoviceGuideUtil.ShowArrow (new GameObject[]{sp1}, new Vector3[]{new Vector3 (0, 0, 1)},false);
	}

	private void ClickOk2(object data){
		NoviceGuideUtil.RemoveAllArrows ();

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
			NoviceGuideUtil.ShowArrow(new GameObject[]{ item.gameObject},new Vector3[]{new Vector3(0,0,1)},false);	
		}
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (JumpToNextState) {
			stepEntity.GetStateMachine ().ChangeState (null);
		}
		else{
			
		}
	}

	
	public override void Exit(NoviceGuideStepEntity stepEntity)
	{
		NoviceGuideUtil.RemoveAllArrows ();
		//CommandEnum.BattleStart;
	}
}