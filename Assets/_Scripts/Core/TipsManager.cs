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

	/// <summary>
	/// Shows the message window with left and right button. And content is one string.
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="content">Content.</param>
	/// <param name="leftBtn">Left button.</param>
	/// <param name="rightBtn">Right button.</param>
	/// <param name="leftCallback">Left callback.</param>
	/// <param name="rightCallback">Right callback.</param>
	/// <param name="leftData">Left data.</param>
	/// <param name="rightData">Right data.</param>
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

		ModuleManager.Instance.ShowModule (ModuleEnum.MsgWindowModule,"data", param);
	}

	/// <summary>
	/// Shows the message window with left and right button. And content is string array
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="content">Content.</param>
	/// <param name="leftBtn">Left button.</param>
	/// <param name="rightBtn">Right button.</param>
	/// <param name="leftCallback">Left callback.</param>
	/// <param name="rightCallback">Right callback.</param>
	/// <param name="leftData">Left data.</param>
	/// <param name="rightData">Right data.</param>
	public void ShowMsgWindow(string title, string[] content, string leftBtn, string rightBtn, DataListener leftCallback = null,DataListener rightCallback = null, object leftData = null, object rightData = null){
		MsgWindowParams param = new MsgWindowParams ();
		
		param.titleText = title;
		param.contentTexts = content;
		
		BtnParam left = new BtnParam ();
		left.callback = leftCallback;
		left.text = leftBtn;
		left.args = leftData;
		
		BtnParam right = new BtnParam ();
		right.callback = rightCallback;
		right.text = rightBtn;
		right.args = rightData;
		
		param.btnParams = new BtnParam[2]{left,right};
		
		ModuleManager.Instance.ShowModule (ModuleEnum.MsgWindowModule,"data", param);
	}

	/// <summary>
	/// Shows the message window only with center btn
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="content">Content.</param>
	/// <param name="centerBtn">Center button.</param>
	/// <param name="centerCallback">Center callback.</param>
	/// <param name="centerData">Center data.</param>
	public void ShowMsgWindow(string title, string content, string centerBtn, DataListener centerCallback = null, object centerData = null){
//		Debug.Log ("Show Msg Window: " + title);

		MsgWindowParams param = new MsgWindowParams ();

		param.titleText = title;
		param.contentText = content;
		
		BtnParam center = new BtnParam ();
		center.callback = centerCallback;
		center.text = centerBtn;
		center.args = centerData;
		
		param.btnParam = center;

		ModuleManager.Instance.ShowModule (ModuleEnum.MsgWindowModule,"data", param);
	}

	/// <summary>
	/// Shows the message window only with center button. And content string is string array.
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="content">Content.</param>
	/// <param name="centerBtn">Center button.</param>
	/// <param name="centerCallback">Center callback.</param>
	/// <param name="centerData">Center data.</param>
	public void ShowMsgWindow(string title, string[] content, string centerBtn, DataListener centerCallback = null, object centerData = null){
		MsgWindowParams param = new MsgWindowParams ();
		
		param.titleText = title;
		param.contentTexts = content;
		
		BtnParam center = new BtnParam ();
		center.callback = centerCallback;
		center.text = centerBtn;
		center.args = centerData;
		
		param.btnParam = center;
		
		ModuleManager.Instance.ShowModule (ModuleEnum.MsgWindowModule,"data", param);
	}

	/// <summary>
	/// Shows the guide message window.
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="content">Content.</param>
	/// <param name="leftBtn">Left button.</param>
	/// <param name="rightBtn">Right button.</param>
	/// <param name="leftCallback">Left callback.</param>
	/// <param name="rightCallback">Right callback.</param>
	/// <param name="leftData">Left data.</param>
	/// <param name="rightData">Right data.</param>
	public void ShowGuideMsgWindow(string title, string content, string leftBtn, string rightBtn, DataListener leftCallback = null, DataListener rightCallback = null, object leftData = null, object rightData = null, GuidePicPath picPath = GuidePicPath.None){
		GuideWindowParams param = new GuideWindowParams ();
		
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
		param.guidePic = picPath;

		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceMsgWindowModule,"data", param);
	}

	/// <summary>
	/// Shows the guide message window.
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="content">Content.</param>
	/// <param name="rightBtn">Right button.</param>
	/// <param name="rightCallback">Right callback.</param>
	/// <param name="rightData">Right data.</param>
	public void ShowGuideMsgWindow(string title, string content, string rightBtn, DataListener rightCallback = null, object rightData = null, GuidePicPath picPath = GuidePicPath.None){


		GuideWindowParams param = new GuideWindowParams ();
		
		param.titleText = title;
		param.contentText = content;
		
		BtnParam center = new BtnParam ();
		center.callback = rightCallback;
		center.text = rightBtn;
		center.args = rightData;
		
		param.btnParam = center;

		param.guidePic = picPath;
		
		ModuleManager.Instance.ShowModule (ModuleEnum.NoviceMsgWindowModule,"data", param);
	}
}
