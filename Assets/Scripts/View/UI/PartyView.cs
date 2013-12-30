using UnityEngine;
using System.Collections;

public class PartyView : UIBase
{
	private PartyUnity partyWindow;
	private SceneInfoBar sceneInfoBar;

	public PartyView(string uiName) : base(uiName){}
	
	public override void CreatUI ()
	{
		sceneInfoBar = ViewManager.Instance.GetViewObject( UIConfig.sharePath + "SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		partyWindow = ViewManager.Instance.GetViewObject( UIConfig.unitPath +  "PartyWindow" ) as PartyUnity;
		partyWindow.Init ("PartyWindow");
		currentUIDic.Add( partyWindow.UIName, partyWindow );
	}
	
	public override void ShowUI ()
	{
		SetUIActive(true);
		sceneInfoBar.BackBtn.isEnabled = true;
		sceneInfoBar.UITitleLab.text = UIName;
		UIEventListener.Get(sceneInfoBar.BackBtn.gameObject).onClick += BackUI;
	}
	
	public override void HideUI ()
	{
		SetUIActive(false);
		UIEventListener.Get(sceneInfoBar.BackBtn.gameObject).onClick -= BackUI;
	}
	
	private void SetUIActive(bool b)
	{
		partyWindow.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}
	
	private void BackUI(GameObject btn)
	{
		controllerManger.BackToPrevScene();
	}
}

