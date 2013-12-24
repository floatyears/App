﻿using UnityEngine;
using System.Collections;

public class StartView : UIBase
{
	public static PlayerInfoBar playerInfoBar;
	public static MenuBtns menuBtns;
	public static MainBg mainBg;

	private SceneInfoBar sceneInfoBar;
	private UIImageButton backBtn;
	private UILabel sceneInfoLab;

	public StartView(string uiName):base(uiName)
	{

	}

	public override void CreatUI ()
	{
		//add Background
		mainBg = ViewManager.Instance.GetViewObject("MenuBg") as MainBg;
		mainBg.transform.parent = viewManager.CenterPanel.transform;
		mainBg.transform.localPosition = Vector3.zero;
		
		//add top Bar
		playerInfoBar = ViewManager.Instance.GetViewObject("PlayerInfoBar") as PlayerInfoBar;
		playerInfoBar.transform.parent = viewManager.TopPanel.transform;
		playerInfoBar.transform.localPosition = Vector3.zero;

		//add Bottom Btns
		menuBtns = ViewManager.Instance.GetViewObject("MenuBottom") as MenuBtns;
		menuBtns.transform.parent = viewManager.BottomPanel.transform.parent;
		menuBtns.transform.localPosition = Vector3.zero;

		//add scene info bar
		sceneInfoBar = ViewManager.Instance.GetViewObject("SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		backBtn = sceneInfoBar.transform.Find("ImgBtn_Arrow").GetComponent<UIImageButton>();
		sceneInfoLab = sceneInfoBar.transform.Find("Lab_UI_Name").GetComponent<UILabel>();
	}
	
	public static void SetActive(bool b)
	{
		playerInfoBar.gameObject.SetActive(b);
		menuBtns.gameObject.SetActive(b);
		mainBg.gameObject.SetActive(b);
	}
	
	public override void ShowUI ()
	{
		SetActive(true);
		backBtn.isEnabled = false;
		sceneInfoLab.text = uiName;
	}
	
	public override void HideUI ()
	{
		sceneInfoBar.gameObject.SetActive(false);
	}
	
	public override void DestoryUI ()
	{	

	}

}
