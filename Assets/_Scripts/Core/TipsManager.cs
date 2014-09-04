using UnityEngine;
using System.Collections;

public class TipsManager {

	private static TipsManager instance;

	public static TipsManager Instance{
		get{
			if(instance == null){
				instance = new TipsManager();
			}
			return instance;
		}
	}

	private TipsManager(){
		tipsLabelUI = ViewManager.Instance.CenterPanel.transform.Find ("Panel/LabelPanel/Label").GetComponent<TipsLabelUI> ();
	}


	private TipsLabelUI tipsLabelUI;
	
	public void ShowTipsLabel (string content) {
		tipsLabelUI.ShowInfo (content);
	}
	
	public void ShowTipsLabel (string content, params object[] data) {
		string info = string.Format (content, data);
		tipsLabelUI.ShowInfo (info);
	}
	
	public void ShowTipsLabel(string content, GameObject target) {
		tipsLabelUI.ShowInfo (content, target);
	}

	public void ShowMsgWindow(string title, string content, string leftBtn, string rightBtn, DataListener leftCallback = null,DataListener rightCallback = null, object leftData = null, object rightData = null){
		MsgWindowParams param = new MsgWindowParams ();

		param.titleText = title;
		param.contentText = content;
		
		BtnParam left = new BtnParam ();
		left.callback = leftCallback;
		left.text = leftBtn;
		left.args = leftData;

		BtnParam right = new BtnParam ();
		right.callback = rightCallback;
		right.text = rightBtn;
		right.args = rightData;

		param.btnParams = new BtnParam[2]{left,right};

		ModuleManger.SendMessage (ModuleEnum.MsgWindowModule,"show", param);
	}

	public void ShowMsgWindow(string title, string content, string centerBtn, DataListener centerCallback = null, object centerData = null){
		MsgWindowParams param = new MsgWindowParams ();

		param.titleText = title;
		param.contentText = content;
		
		BtnParam center = new BtnParam ();
		center.callback = centerCallback;
		center.text = centerBtn;
		center.args = centerData;
		
		param.btnParam = center;

		ModuleManger.SendMessage (ModuleEnum.MsgWindowModule,"hide", param);
	}
}
