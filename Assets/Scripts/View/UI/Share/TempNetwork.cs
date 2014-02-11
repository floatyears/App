using UnityEngine;
using System.Collections;

public class TempNetwork
{
	public static SearchInfoUI infoUI;

	public static void VerifySearchedID( string strID) {
		if( strID == string.Empty ) {

		}
		//other verify logic here
		else {
			if( infoUI == null ){
				Debug.LogError("InfoUI is Null");
				return;
			}
			infoUI.ShowSelf();
			Debug.Log("Net ");
		}
			
	}


}
