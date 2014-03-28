using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class VStageItemInfo{
	private GameObject viewItem;
	public GameObject ViewItem{
		get{
			return viewItem;
		}
		set{
			viewItem = value;
		}
	}

	private TStageInfo stageInfo;
	public TStageInfo StageInfo{
		get{
			return stageInfo;
		}
		set{
			stageInfo = value;
		}
	}

	private UILabel clearInfoLabel;
	public UILabel ClearInfoLabel{
		get{
			return clearInfoLabel;
		}
		set{
			clearInfoLabel = value;
		}
	}

	private UILabel nameLabel;
	public UILabel NameLabel{
		get{
			return nameLabel;
		}
		set{
			nameLabel = value;
		}
	}

	private UITexture texture;
	public UITexture Texture{
		get{
			return texture;
		}
		set{
			texture = value;
		}
	}

	public void Refresh(GameObject viewItem, TStageInfo stageInfo){
		this.viewItem = viewItem;
		this.stageInfo = stageInfo;

		clearInfoLabel = viewItem.transform.Find("Label_Top").GetComponent<UILabel>();
		nameLabel = viewItem.transform.Find("Label_Bottom").GetComponent<UILabel>();
		texture = viewItem.transform.Find("Texture").GetComponent<UITexture>();

		clearInfoLabel.text = GetStageStateText(stageInfo.State);
		clearInfoLabel.color = GetStageStateTextColor(stageInfo.State);
		nameLabel.text = stageInfo.StageName;
		texture.mainTexture = Resources.Load("Stage/" + stageInfo.StageId) as Texture2D;
	}

	private string GetStageStateText(EQuestState state){
		switch (state){
			case EQuestState.QS_NEW :
				return TextCenter.Instace.GetCurrentText("StageStateNew");
			case EQuestState.QS_CLEARED : 
				return TextCenter.Instace.GetCurrentText("StageStateClear");
			default:
				return string.Empty;
		}
	}

	private Color GetStageStateTextColor(EQuestState state){
		switch (state){
			case EQuestState.QS_NEW :
				return Color.green;
			case EQuestState.QS_CLEARED : 
				return Color.yellow;
			default:
				return Color.white;
		}
	}

}

