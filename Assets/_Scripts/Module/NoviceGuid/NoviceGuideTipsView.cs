using UnityEngine;
using System.Collections;

public class NoviceGuideTipsView : ViewBase {

	private UILabel label;

	private UISprite avatar;

	private UISprite label_bg;

	private Vector3 uiPos;

	private Quaternion rotateAngle = new Quaternion (0, 180, 0, 0);
	private UICallback callback;

	private BoxCollider ban_mask;

	private Transform tipsRoot;

	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		avatar = FindChild<UISprite> ("Tips/Avatar");
		label = FindChild<UILabel> ("Tips/Label");
		label_bg = FindChild<UISprite> ("Tips/Label_Bg");
		ban_mask = transform.GetComponent<BoxCollider> ();
		tipsRoot = transform.FindChild("Tips");
		uiPos = Vector3.zero;

		UIEventListenerCustom.Get (gameObject).onClick = OnClickAny;
	}

	public override void CallbackView (params object[] args)
	{
		if (args [0].ToString () == "ui_click") {
			ModuleManager.Instance.HideModule(ModuleEnum.NoviceGuideTipsModule);
			if(callback != null){
				callback(null);
				callback = null;
			}

		}
	}

	public override void ShowUI ()
	{

		if (viewData != null) {
			if(viewData.ContainsKey("ban_click") && (bool)viewData["ban_click"]){
				ban_mask.enabled = true;
			}else{
				ban_mask.enabled = false;
			}
			if(viewData.ContainsKey("callback")){
				callback = (UICallback)viewData["callback"];
			}

			if(viewData.ContainsKey("tips")){
				label.text = (string)viewData["tips"];
				label_bg.width = (int)label.localSize.x + 40;
				label_bg.height = (int)label.localSize.y + 60;

			}	
			if(viewData.ContainsKey("rotate") && (bool)viewData["rotate"]){
				label_bg.transform.rotation = rotateAngle;
				avatar.transform.rotation = rotateAngle;
				avatar.transform.localPosition = new Vector3(-label_bg.width/2+100,-label_bg.height/2+30,0);
			}else{
				label_bg.transform.rotation = Quaternion.identity;
				avatar.transform.rotation = Quaternion.identity;
				avatar.transform.localPosition = new Vector3(label_bg.width/2-100,-label_bg.height/2+30,0);
			}
			if(viewData.ContainsKey("coor")){
				uiPos = (Vector3)viewData["coor"];
			}
		}
		base.ShowUI ();
	}

	protected override void ToggleAnimation (bool isShow)
	{
		if (isShow) {
			gameObject.SetActive(true);
			transform.localPosition = Vector3.zero;
			tipsRoot.localPosition = new Vector3(uiPos.x, uiPos.y, 0);
		}else{
			tipsRoot.localPosition = new Vector3(-1000, uiPos.y, 0);	
			gameObject.SetActive(false);
		}
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}

	void OnClickAny(GameObject obj){

	}
}
