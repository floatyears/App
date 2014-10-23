using UnityEngine;
using System.Collections;

public class NoviceGuideTipsView : ViewBase {

	private UILabel label;

	private UISprite avatar;

	private UISprite label_bg;

	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		avatar = FindChild<UISprite> ("Tips/Avatar");
		label = FindChild<UILabel> ("Tips/Label");
		label_bg = FindChild<UISprite> ("Tips/Label_Bg");
	}

	public override void CallbackView (params object[] args)
	{
		if (args [0].ToString () == "ui_click") {
			ModuleManager.Instance.HideModule(ModuleEnum.NoviceGuideTipsModule);
		}
	}

	public override void ShowUI ()
	{
		base.ShowUI ();

		if (viewData != null) {
			if(viewData.ContainsKey("tips")){
				label.text = (string)viewData["tips"];
				label_bg.width = label.width + 10;
				label_bg.height = label.height + 10;
			}	
			if(viewData.ContainsKey("coor")){

			}
		}
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}
}
