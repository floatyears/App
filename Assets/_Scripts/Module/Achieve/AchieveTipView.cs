﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class AchieveTipView : ViewBase {

	private TaskConf data;

	private UILabel name;
	private UISprite bigIcon;

	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		FindChild<UILabel> ("Label_Congratulation").text = TextCenter.GetText ("Label_Congratulations");
		FindChild<UILabel> ("Label_Achieved").text = TextCenter.GetText ("Achieve_Complete");
		bigIcon = FindChild<UISprite> ("Item/Img");
		name = FindChild<UILabel> ("Name");
	}

	public override void ShowUI ()
	{
		base.ShowUI ();

		if (viewData != null && viewData.ContainsKey ("data")) {
			data = viewData["data"] as TaskConf;
			name.text = data.taskDesc;

			if(data.giftItem.Count > 0){
				bigIcon.spriteName = "icon_" + data.giftItem[0].content;
			}

		}

	}

	public override void HideUI ()
	{
		base.HideUI ();
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}

	protected override void ToggleAnimation (bool isShow)
	{
		iTween.Stop(gameObject);
		if (isShow) {
			gameObject.SetActive(true);
			transform.localPosition = new Vector3(config.localPosition.x, -900, 0);

			iTween.MoveTo(gameObject,iTween.Hash("y", config.localPosition.y,"time", 1f,"oncomplete","OnAnimateEnd","oncompletetarget",gameObject,"islocal",true));
			
		}else{
			iTween.MoveTo(gameObject,iTween.Hash("x",700,"time", 1f,"islocal",true));
//			transform.localPosition = new Vector3(config.localPosition.x, -300, 0);	
			gameObject.SetActive(false);
		}
	}

	public void OnAnimateEnd(){
		ModuleManager.Instance.HideModule (ModuleEnum.AchieveTipModule);
	}
}