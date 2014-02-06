using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyComponent : ConcreteComponent, IUIParty {

	UnitPartyInfo unitPartyInfo;
	Dictionary<int,UserUnitInfo> userUnit = new Dictionary<int, UserUnitInfo> ();

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

	//Logic Interface : Party Page Turn 
	public void PartyPaging (object data)
	{
		int partyID = 0;
		try {
			partyID = ( int )data;
		} 
		catch (System.Exception ex) {
			Debug.LogError(ex.Message);
			return;	
		}
		//view is UnitsDecoratorUnity -- Behaviour Interface 
		IUIParty partyInterface = viewComponent as IUIParty;
		if( partyInterface == null ) {
			return;
		}
		
		if( partyID == 1 )
		{
			unitPartyInfo = ModelManager.Instance.GetData( ModelEnum.UnitPartyInfo, errMsg ) as UnitPartyInfo;
			Dictionary< int, int > temp = unitPartyInfo.GetPartyItem();
			Dictionary< int, UnitBaseInfo > viewInfo = new Dictionary<int, UnitBaseInfo >();
			foreach (var item in temp) {
				UserUnitInfo userUnitInfo = GlobalData.tempUserUnitInfo[ item.Value ];
				if( !userUnit.ContainsKey( item.Key )) {
					userUnit.Add( item.Key, userUnitInfo );
				}
				UnitBaseInfo unitBaseInfo = GlobalData.tempUnitBaseInfo[ userUnitInfo.unitBaseInfo ];
				viewInfo.Add( item.Key, unitBaseInfo );
			}
			
			partyInterface.PartyPaging( viewInfo );
		}
		else {
			partyInterface.PartyPaging( null );
		}
	}

}
