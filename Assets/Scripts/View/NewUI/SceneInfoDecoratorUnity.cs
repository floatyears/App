using UnityEngine;
using System.Collections;

public class SceneInfoDecoratorUnity : UIComponentUnity ,IUICallback, IUISetBool{
	
	private UILabel labelSceneName;
	private UIImageButton btnBackScene;

	private IUICallback iuiCallback; 
	private bool temp = false;
	
	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);
		InitUI();

		temp = origin is IUICallback;
	}
	
	public override void ShowUI () {

		base.ShowUI ();

		Vector3 tar = CaculateReallyPoint(new Vector3(0f,100f,0f),transform.parent.parent.localPosition);
		iTween.MoveFrom ( this.gameObject, tar, 1f );

	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		labelSceneName = FindChild< UILabel >( "Label_Scene_Name" );
		btnBackScene =  FindChild< UIImageButton >( "ImgBtn_Back_Scene" );

		UIEventListener.Get( btnBackScene.gameObject ).onClick = BackPreScene;
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
			labelSceneName.text = info;
		}
	}

	public void SetEnable (bool b)
	{
		btnBackScene.isEnabled = b;
	}

	void BackPreScene (GameObject go)
	{
		if(temp) {
			IUICallback call = origin as IUICallback;
			call.Callback(go);
		}
	}
}
