using UnityEngine;
using System.Collections;

//Evolve battle
public class NoviceGuideStepL_StateOne:NoviceGuidState
{
	private static NoviceGuideStepL_StateOne instance;
	
	public static NoviceGuideStepL_StateOne Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepL_StateOne ();
		return instance;
	}
	
	private NoviceGuideStepL_StateOne ():base()	{}
	
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
		NoviceGuideUtil.ShowArrow (new GameObject[]{sp1}, new Vector3[]{new Vector3 (0, 0, 1)});
		
		
	}
	
	private void ClickOk2(object data){
		NoviceGuideUtil.RemoveAllArrows ();
		
		BattleMapView bm = GameObject.Find ("Map").GetComponent<BattleMapView> ();
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