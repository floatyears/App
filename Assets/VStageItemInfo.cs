using UnityEngine;
using System.Collections.Generic;

public class VStageItemInfo{
	private GameObject viewItem;

	public GameObject ViewItem
	{
		get
		{
			return viewItem;
		}
		set
		{
			viewItem = value;
		}
	}

	TStageInfo stageInfo;

	public TStageInfo StageInfo
	{
		get
		{
			return stageInfo;
		}
		set
		{
			stageInfo = value;
		}
	}

	private UILabel clearInfoLabel;

	public UILabel ClearInfoLabel
	{
		get
		{
			return clearInfoLabel;
		}
		set
		{
			clearInfoLabel = value;
		}
	}

	private UILabel nameLabel;

	public UILabel NameLabel
	{
		get
		{
			return nameLabel;
		}
		set
		{
			nameLabel = value;
		}
	}

	private UITexture texture;

	public UITexture Texture
	{
		get
		{
			return texture;
		}
		set
		{
			texture = value;
		}
	}

	public void Refresh(GameObject viewItem, TStageInfo stageInfo){
		this.viewItem = viewItem;
		this.stageInfo = stageInfo;

		clearInfoLabel = viewItem.transform.Find("Label_Top").GetComponent<UILabel>();
		nameLabel = viewItem.transform.Find("Label_Bottom").GetComponent<UILabel>();
		texture = viewItem.transform.Find("Texture").GetComponent<UITexture>();

		//TODO clearInfo
//		clearInfoLabel = stageInfo

		nameLabel.text = stageInfo.StageName;
		texture.mainTexture = Resources.Load("Stage/" + stageInfo.StageId) as Texture2D;
	}

}

