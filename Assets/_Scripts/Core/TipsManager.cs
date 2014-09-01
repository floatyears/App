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
}
