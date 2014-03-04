using UnityEngine;
using System.Collections;

public class UserIDWindow : UIComponentUnity, IUICallback {


	public override void Init(UIInsConfig config, IUICallback origin)
	{
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI()
	{
		base.ShowUI();

		GetUserID();
		ShowTween();
	}

	public override void HideUI()
	{
		base.HideUI();
	}

	public override void DestoryUI()
	{
		base.DestoryUI();
	}

	private void InitUI(){
		UIButton buttonOK = FindChild< UIButton >("Button_OK");
		UIEventListener.Get( buttonOK.gameObject ).onClick = ClickButton;
	}

	void ClickButton(GameObject go){
		UIManager.Instance.ChangeScene( SceneEnum.Friends );
	}

	void GetUserID() {
		IUICallback call = origin as IUICallback;
		if (call == null)	return;
		//Debug.Log("origin is IUICallback is" + origin is IUICallback);
		call.Callback( origin is IUICallback );
	}
	
	public void Callback(object data){
		string id = (string) data;
		UILabel idLabel = FindChild< UILabel >("Label_ID_Vaule");
		idLabel.text = id;
	}

	private void ShowTween(){
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)	return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)	continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

}
