using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsComponent : ConcreteComponent, IUIParty {

	TUnitParty unitPartyInfo;
	Dictionary<int,TUserUnit> userUnit = new Dictionary<int, TUserUnit> ();

	public UnitsComponent(string uiName):base(uiName) {}

	public override void CreatUI () {
		base.CreatUI ();
//		Debug.Log( "Units Scene: CreateUI");
	}
	
	public override void ShowUI () {
		base.ShowUI ();
//		Debug.Log( "Units Scene: ShowUI");

	}
	
	public override void HideUI () {
		base.HideUI ();
//		Debug.Log( "Units Scene: HideUI");
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void Callback (object data)
	{
		try {
			SceneEnum se = (SceneEnum)data;
			AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
			UIManager.Instance.ChangeScene(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}
	
	public void PartyPaging (object data){
		int partyID = 0;
		try {
			partyID = ( int )data;
		} 
		catch (System.Exception ex) {
			Debug.LogError(ex.Message);
			return;	
		}
		IUIParty partyInterface = viewComponent as IUIParty;

		if( partyInterface == null ) {
			return;
		}
		if( partyID == 1 )
		{
//			unitPartyInfo = ModelManager.Instance.GetData( ModelEnum.UnitPartyInfo, errMsg ) as TUnitParty;
//			Dictionary< int, uint > temp = unitPartyInfo.GetPartyItem();
//			Dictionary< string, object > viewInfo = new Dictionary<string, object>();
//			Dictionary< int, UnitBaseInfo > avatarInfoDic = new Dictionary<int, UnitBaseInfo >();
//
//			foreach (var item in temp) {
//				TUserUnit userUnitInfo = GlobalData.userUnitList.GetMyUnit(item.Value);
//				if( !userUnit.ContainsKey( item.Key )) {
//					userUnit.Add( item.Key, userUnitInfo );
//				}
////				UnitBaseInfo unitBaseInfo = GlobalData.unitBaseInfo[ userUnitInfo.unitBaseInfo ];
////				avatarInfoDic.Add( item.Key, unitBaseInfo );
//			}
//
//			int totalHP = unitPartyInfo.GetBlood() ;
//
//			viewInfo.Add( "avatar", avatarInfoDic);
//			viewInfo.Add("hp", totalHP);
//
//			partyInterface.PartyPaging( viewInfo );
		}
		else {
			partyInterface.PartyPaging( null );
		}
	}



}
