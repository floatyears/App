using UnityEngine;
using System.Collections.Generic;

public class MenuBottomUnity : UIComponentUnity {
	IUICallback iuiCallback;
	bool temp = false;

	private Dictionary<GameObject,SceneEnum> buttonInfo = new Dictionary<GameObject, SceneEnum> ();

	public override void Init (UIInsConfig config, IUIOrigin origin) {
		base.Init (config, origin);
		InitButton ();

		temp = origin is IUICallback;
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

	void InitButton() {
		GameObject go = FindChild ("ImgBtn_Friends");
		buttonInfo.Add (go, SceneEnum.Friends);

		go = FindChild ("ImgBtn_Quest");
		buttonInfo.Add (go, SceneEnum.Quest);

		go = FindChild ("ImgBtn_Scratch");
		buttonInfo.Add (go, SceneEnum.Scratch);

		go = FindChild ("ImgBtn_Shop");
		buttonInfo.Add (go, SceneEnum.Shop);

		go = FindChild ("ImgBtn_Others");
		buttonInfo.Add (go, SceneEnum.Others);

		go = FindChild ("ImgBtn_Units");
		buttonInfo.Add (go, SceneEnum.Units);

		foreach (var item in buttonInfo.Keys) {
			UIEventListener.Get(item).onClick = OnClickCallback;
		}
	}

	void OnClickCallback( GameObject caller ) {

//		Debug.LogError ("onclickcallback : " + temp + "```" + caller);

		if (!temp) {
			return;
		}

		SceneEnum se = buttonInfo [caller];

		if (iuiCallback == null) {
			iuiCallback = origin as IUICallback;
		} 
		else {
			iuiCallback.Callback(se);
			LogHelper.Log("Click Btn: " + se.ToString() );

			//-->Quest Scene
			UIManager.Instance.ChangeScene( se );
		}
	}
}
