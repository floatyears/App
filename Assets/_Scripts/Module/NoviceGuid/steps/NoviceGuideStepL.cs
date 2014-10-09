using UnityEngine;
using System.Collections;

//Evolve battle
public class NoviceGuideStepL_1:NoviceGuidStep
{
	public override void Enter()
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide14_title"),TextCenter.GetText ("guide14_content"),TextCenter.GetText ("NEXT"),ClickOK,null,GuidePicPath.GoldBox);
		
		
		
		NoviceGuideStepManager.CurrentNoviceGuideStage = NoviceGuideStage.ANIMATION;
	}
	
	private void ClickOK(object data)
	{

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide15_title"),TextCenter.GetText ("guide15_content"),TextCenter.GetText ("NEXT"),ClickOk2);
		
		
		GameObject sp1 = GameObject.FindWithTag ("battle_sp1");
		NoviceGuideUtil.ShowArrow (new GameObject[]{sp1}, new Vector3[]{new Vector3 (0, 0, 1)});
		
		
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
			NoviceGuideUtil.ShowArrow(new GameObject[]{ item.gameObject},new Vector3[]{new Vector3(0,0,1)});	
		}
	}

	
	
	public override void Exit()
	{
		NoviceGuideUtil.RemoveAllArrows ();
		//CommandEnum.BattleStart;
	}
}