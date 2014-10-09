using UnityEngine;
using System.Collections;

public class NoviceGuideStepH_1:NoviceGuidStep
{
	
	public override void Enter()
	{
		BattleMapView bm = GameObject.Find ("Map").GetComponent<BattleMapView> ();
		MapItem item = null;
		for(int i = 0; i < MapConfig.MapWidth; i++){
			for(int j = 0; j < MapConfig.MapHeight; j++){
//				item = bm.GetMapItem(new Coordinate(i,j));
//				if(item.IsKey()){
//					LogHelper.Log ("map coordinate: i:" + i + "j: " + j);
//					goto outLoop;
//				}
			}
		}

	outLoop:
		if (item != null) {
			NoviceGuideUtil.ShowArrow(new GameObject[]{ item.gameObject},new Vector3[]{new Vector3(0,0,1)},false);	
		}

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide12_title"),TextCenter.GetText ("guide12_content"),TextCenter.GetText ("NEXT"),ClickOK,item.gameObject);
	}
	
	private void ClickOK(object data)
	{
		LogHelper.Log("--------////////step h:"+data);
		NoviceGuideUtil.RemoveArrow ((GameObject)data);

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide13_title"),TextCenter.GetText ("guide13_content"),TextCenter.GetText ("NEXT"),ClickOk1);

		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, OnBattleEnd);
	}

	private void ClickOk1(object data)
	{
		BattleMapView bm = GameObject.Find ("Map").GetComponent<BattleMapView> ();
//		GameObject[] objs = new GameObject[] {
//						bm.GetMapItem (new Coordinate (1, 0)).gameObject,
//						bm.GetMapItem (new Coordinate (2, 1)).gameObject,
//						bm.GetMapItem (new Coordinate (3, 0)).gameObject
//				};
//		NoviceGuideUtil.ShowArrow(objs,new Vector3[]{new Vector3(0,0,1),new Vector3(0,0,1),new Vector3(0,0,1)});
//
//		NoviceGuideUtil.ForceBtnsClick(objs,delegate(GameObject btn){
//			NoviceGuideUtil.RemoveAllArrows();
//			//JumpToNextState = true;
//		});
	}

	private void OnBattleEnd(object data){
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, OnBattleEnd);
		GoToNextState();
	}

}

public class NoviceGuideStepH_2:NoviceGuidStep
{
	private static NoviceGuideStepH_2 instance;
	
	public static NoviceGuideStepH_2 Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepH_2 ();
		return instance;
	}
	
	private NoviceGuideStepH_2 ():base()	{}
	
	public override void Enter()
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide14_title"),TextCenter.GetText ("guide14_content"),TextCenter.GetText ("NEXT"),ClickOK,GuidePicPath.GoldBox);
		


		NoviceGuideStepManager.CurrentNoviceGuideStage = NoviceGuideStage.ANIMATION;
	}

	private void ClickOK(object data)
	{
		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide15_title"),TextCenter.GetText ("guide15_content"),TextCenter.GetText ("NEXT"),ClickOk2);
		

		GameObject sp1 = GameObject.FindWithTag ("battle_sp1");
		NoviceGuideUtil.ShowArrow (new GameObject[]{sp1}, new Vector3[]{new Vector3 (0, 0, 1)},false);
	}

	private void ClickOk2(object data){
		NoviceGuideUtil.RemoveAllArrows ();

		BattleMapView bm = GameObject.Find ("Map").GetComponent<BattleMapView> ();
		MapItem item = null;
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
		if (item != null) {
			NoviceGuideUtil.ShowArrow(new GameObject[]{ item.gameObject},new Vector3[]{new Vector3(0,0,1)},false);	
		}
	}

	
	public override void Exit()
	{
		NoviceGuideUtil.RemoveAllArrows ();
		//CommandEnum.BattleStart;
	}
}