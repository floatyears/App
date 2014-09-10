using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelUpMaterialUI : ModuleBase{

	private List< UserUnit > ownUnits;

	public LevelUpMaterialUI(UIConfigItem config):base(  config) {
		GetUnitsData();
	}

	public override void InitUI(){
		base.InitUI();
	}
	
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
	}
	
	public override void DestoryUI(){
		base.DestoryUI();
	}

	private void GetUnitsData(){
		ownUnits = DataCenter.Instance.ConfigViewData;
		if( ownUnits.Count < 1 ){
			LogHelper.LogError( "LevelUp MaterialUI : Not got Material Data" );
		}
		LogHelper.Log( "LevelUp MaterialUI : Material Item Count : " + ownUnits.Count );
	}
	
	private void ListItem(){
//		Dictionary< string, object > itemInfo = new Dictionary<string, object>();
//		if( ownUnits == null )	{
//			LogHelper.LogError("Not have Units Data");
//			return;
//		}



	}
	
	public void OnReceiveMessage(object data){
//		IUICallback call = viewComponent as IUICallback;
//		call.Callback( data );
	}
}

