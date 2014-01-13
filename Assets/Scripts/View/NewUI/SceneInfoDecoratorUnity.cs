using UnityEngine;
using System.Collections;

public class SceneInfoDecoratorUnity : UIComponentUnity ,IUICallback, IUISetBool{

	private IUICallback iuiCallback; 
	private UILabel label;
	private UIImageButton btn;
	private bool temp = false;


	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);
		InitUI();

		temp = origin is IUICallback;
	}
	
	public override void ShowUI () {
		base.ShowUI ();

		// Animation Here
		//iTween.MoveTo ( this.gameObject, iTween.Hash("y", .1, "easeType", "easeInOutExpo", "delay", .1) );
	}
	
	public override void HideUI () {
		base.HideUI ();

	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {

		label = FindChild( "Lab_UI_Name" ).GetComponent< UILabel >();
		btn =  FindChild( "ImgBtn_Arrow" ).GetComponent< UIImageButton >();

		UIEventListener.Get( btn.gameObject ).onClick = OnClickCallback;
	}
	
	public void Callback (object data)
	{
		string info = string.Empty;
		try {
			info = (string)data;
		} 
		catch (System.Exception ex) {
		}
		if(!string.IsNullOrEmpty(info)){
			label.text = info;
		}
	}

	public void SetEnable (bool b)
	{
		btn.isEnabled = b;
	}

	void OnClickCallback (GameObject go)
	{
		if(temp) {
			IUICallback call = origin as IUICallback;
			call.Callback(go);
		}
	}
}
