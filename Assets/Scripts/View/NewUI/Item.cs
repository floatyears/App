using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
	float timer = 0;
	bool isPressed = false ;

	void Start()
	{
		UIEventListener.Get(this.gameObject).onPress = TurnScene;
	}

	void TurnScene(GameObject go, bool isPressed )
	{
		LogHelper.Log("Turn to Scene of Unit Detail!");
		Debug.LogError("isPressed: "+ isPressed );
		this.isPressed = isPressed;

	}

	void Update()
	{
		if( isPressed )
		{
			timer += Time.deltaTime;
		}
		if( timer > 0.5f)
		{

			UIManager.Instance.ChangeScene( SceneEnum.UnitDetail );
			isPressed = false;

			timer = 0f;
		}
	}
}
