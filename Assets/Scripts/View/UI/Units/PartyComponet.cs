using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyComponent : ConcreteComponent, IUIParty {

	TUnitParty unitPartyInfo;
	Dictionary<int,TUserUnit> userUnit = new Dictionary<int, TUserUnit> ();

	public PartyComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void Callback (object data)
	{
		try {
			SceneEnum scene = (SceneEnum)data;
			UIManager.Instance.ChangeScene( scene );

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

		unitPartyInfo = ModelManager.Instance.GetData( ModelEnum.UnitPartyInfo, errMsg ) as TUnitParty;

		Dictionary< int, uint > temp = unitPartyInfo.GetPartyItem();
		Dictionary< string, object > viewInfo = new Dictionary<string, object>();
		Dictionary< int, UnitBaseInfo > avatarInfoDic = new Dictionary<int, UnitBaseInfo >();
		int totalHP = 0;

		if( partyID == 1 )
		{
			//deal avatar
			foreach (var item in temp) {
				TUserUnit userUnitInfo = GlobalData.userUnitInfo[ item.Value ];
				if( !userUnit.ContainsKey( item.Key )) {
					userUnit.Add( item.Key, userUnitInfo );
				}
				UnitBaseInfo unitBaseInfo = GlobalData.unitBaseInfo[ userUnitInfo.unitBaseInfo ];
				avatarInfoDic.Add( item.Key, unitBaseInfo );
			}

			//deal hpCount
			totalHP = unitPartyInfo.GetBlood() ;

			//add data list
			viewInfo.Add( "avatar", avatarInfoDic);
			viewInfo.Add("hp", totalHP);

			//call back to behaviour
			partyInterface.PartyPaging( viewInfo );
		}
		else {
			partyInterface.PartyPaging( null );
		}
	}

}
