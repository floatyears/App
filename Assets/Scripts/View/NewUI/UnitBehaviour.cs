using UnityEngine;
using System.Collections;

public class UnitBehaviour : MonoBehaviour
{
	private void OnEnable()
	{
		UIEventListener.Get( this.gameObject ).onPress = ViewUnitDetail;
	}

	private void ViewUnitDetail( GameObject go, bool isPressed )
	{
		if( isPressed )
		{
			GameTimer.GetInstance().AddCountDown( UIConfig.longPressedTimeCount, TimeCountCallback );
		}
		else
		{
			GameTimer.GetInstance().ExitCountDonw( TimeCountCallback );

			//Tell the UnitPool of LevelUp To Pick This Unit 

		}
	}

	private void TimeCountCallback()
	{
		UIManager.Instance.ChangeScene( SceneEnum.UnitDetail );
	}

	//private void  

}
