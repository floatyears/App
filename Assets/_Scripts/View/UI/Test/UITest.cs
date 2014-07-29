using UnityEngine;
using System.Collections;

public class UITest : MonoBehaviour {
	public UICamera uiCamera;
	public UIButton button;
	// Use this for initialization
	void Start () {
		UIEventListener.Get(button.gameObject).onClick = ClickButton;
		UIEventListener.Get(button.gameObject).onPress = PressButton;
//		UIEventListener.Get(button.gameObject)
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ClickButton(GameObject go){
		Debug.LogError(go.name);
	}

	void PressButton(GameObject go, bool isPress){
		Debug.LogError("Name : " + go.name + "  isPress : " + isPress );
	}
}
