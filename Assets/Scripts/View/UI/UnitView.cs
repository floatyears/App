using UnityEngine;
using System.Collections;

public class UnitView : UIBase
{
	private UnitUnity window;
	private SceneInfoBar sceneInfoBar;

	public UnitView(string uiName) : base(uiName){}
	
	public override void CreatUI ()
	{
		sceneInfoBar = ViewManager.Instance.GetViewObject(UIConfig.sharePath + "SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		window = ViewManager.Instance.GetViewObject( UIConfig.unitPath + "UnitWindow") as UnitUnity;
		window.Init ("UnitWindow");
		currentUIDic.Add(window.UIName, window);
		window.gameObject.transform.localPosition = 315*Vector3.up;
	}

	public override void ShowUI ()
	{
		SetUIActive(true);
		sceneInfoBar.BackBtn.isEnabled = false;
		sceneInfoBar.UITitleLab.text = UIName;

		UIEventListener.Get(window.PartyBtn.gameObject).onClick += window.JumpToParty;
		UIEventListener.Get(window.LevelUpBtn.gameObject).onClick += window.JumpToLevelUp;
		UIEventListener.Get(window.ListBtn.gameObject).onClick += window.JumpToList;
		UIEventListener.Get(window.EvolveBtn.gameObject).onClick += window.JumpToEvolve;
		UIEventListener.Get(window.SellBtn.gameObject).onClick += window.JumpToSell;
		UIEventListener.Get(window.CatalogBtn.gameObject).onClick += window.JumpToCatalog;
	}
	
	public override void HideUI ()
	{
		SetUIActive(false);

		UIEventListener.Get(window.PartyBtn.gameObject).onClick -= window.JumpToParty;
		UIEventListener.Get(window.LevelUpBtn.gameObject).onClick -= window.JumpToList;
		UIEventListener.Get(window.ListBtn.gameObject).onClick -= window.JumpToList;
		UIEventListener.Get(window.EvolveBtn.gameObject).onClick -= window.JumpToEvolve;
		UIEventListener.Get(window.SellBtn.gameObject).onClick -= window.JumpToSell;
		UIEventListener.Get(window.CatalogBtn.gameObject).onClick -= window.JumpToCatalog;
	}
	
	private void SetUIActive(bool b)
	{
		window.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}
}

