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
					break;
				}
			}
		}
		if (item != null) {
			NoviceGuideUtil.ShowArrow(new GameObject[]{ item.gameObject},new Vector3[]{new Vector3(0,0,1)});	
		}

		MsgWindowParams mwp = new MsgWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.Instace.GetCurrentText("guide12_title");
		mwp.contentText = TextCenter.Instace.GetCurrentText("guide12_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = ClickOK;
		sure.args = item;
		sure.text = TextCenter.Instace.GetCurrentText("OK");
		mwp.btnParam = sure;



		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}
	
	private void ClickOK(object data)
	{
		NoviceGuideUtil.RemoveArrow ((GameObject)data);
	}
	
	public override void Execute(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + " is execute stepA state_one");
		if (JumpToNextState) {
			
			stepEntity.GetStateMachine ().ChangeState (null);
		}
		else{
			
		}
	}
	
	public override void Exit(NoviceGuideStepEntity stepEntity)
	{
		LogHelper.Log (stepEntity.GetType () + "exit stepA state_one");
	}

}