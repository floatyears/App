using UnityEngine;
using System.Collections;

public class NoviceGuideTipsView : ViewBase {

	private UILabel label;

	private UISprite avatar;

	private UISprite label_bg;

	private Vector3 uiPos;

	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		avatar = FindChild<UISprite> ("Tips/Avatar");
		label = FindChild<UILabel> ("Tips/Label");
		label_bg = FindChild<UISprite> ("Tips/Label_Bg");
		uiPos = Vector3.zero;
	}

	public override void CallbackView (params object[] args)
	{
		if (args [0].ToString () == "ui_click") {
			ModuleManager.Instance.HideModule(ModuleEnum.NoviceGuideTipsModule);
		}
	}

	public override void ShowUI ()
	{
		if (viewData != null) {
			if(viewData.ContainsKey("tips")){
				label.text = (string)viewData["tips"];
				label_bg.width = (int)label.localSize.x + 40;
				label_bg.height = (int)label.localSize.y + 60;
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
			transform.localPosition = new Vector3(uiPos.x, uiPos.y, 0);
		}else{
			transform.localPosition = new Vector3(-1000, uiPos.y, 0);	
			gameObject.SetActive(false);
		}
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}
}
