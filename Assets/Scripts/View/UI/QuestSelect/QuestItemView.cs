using UnityEngine;
using System.Collections;

public class QuestItemView : MonoBehaviour {
	private UISprite bgSpr;
	private UILabel nameLabel;
	private UILabel staminaLabel;
	private UILabel floorLabel;
	private UILabel expLabel;
	private UILabel coinLabel;

	private static GameObject prefab;
	public static GameObject Prefab{
		get{
			if(prefab == null){
				string sourcePath = "Prefabs/UI/Quest/QuestItemPrefab";
				prefab = Resources.Load(sourcePath) as GameObject;
			}
			return prefab;
		}
	}

	public static QuestItemView Inject(GameObject view){
		QuestItemView stageItemView = view.GetComponent<QuestItemView>();
		if(stageItemView == null) stageItemView = view.AddComponent<QuestItemView>();
		return stageItemView;
	}

	private uint stageID;
	public uint StageID{
		get{
			return stageID;
		}
		set{
			stageID = value;
		}
	}

	private TQuestInfo data;
	public TQuestInfo Data{
		get{
			return data;
		}
		set{
			data = value;
			if(data == null){
				Debug.LogError("QuestItemView, Data is NULL!");
				return;
			}
			FindUIElement();
			ShowQuestInfo();
			AddEventListener();
		}
	}

	public TStageInfo stageInfo;
	
	private void ShowQuestInfo(){
		bgSpr.atlas = DataCenter.Instance.GetAvatarAtlas(data.BossID[ 0 ]);
		bgSpr.spriteName = data.BossID[ 0 ].ToString();

		nameLabel.text = data.Name;
		staminaLabel.text = string.Format( "STAMINA {0}", data.Stamina);
		floorLabel.text = string.Format( "FLOOR {0}", data.Floor);
		expLabel.text = data.RewardExp.ToString();
		coinLabel.text = data.RewardMoney.ToString();
	}

	private void FindUIElement(){
		bgSpr = transform.FindChild("Sprite_Boss_Avatar").GetComponent<UISprite>();
		nameLabel = transform.FindChild("Label_Quest_Name").GetComponent<UILabel>();
		staminaLabel = transform.FindChild("Label_Stamina").GetComponent<UILabel>();
		floorLabel = transform.FindChild("Label_Floor").GetComponent<UILabel>();
		expLabel = transform.FindChild("Label_Exp").GetComponent<UILabel>();
		coinLabel = transform.FindChild("Label_Coin").GetComponent<UILabel>();
	}

	private void AddEventListener(){
		if(data == null)
			UIEventListener.Get(this.gameObject).onClick = null;
		else
			UIEventListener.Get(this.gameObject).onClick = ClickItem;
	}

	private void ClickItem(GameObject item){
		Debug.Log(string.Format("QuestItemView.ClickItem(), Picking quest...questID is {0}, quest name is : {1}", data.ID, data.Name));
		QuestItemView thisQuestItemView = this.GetComponent<QuestItemView>();
		UIManager.Instance.ChangeScene(SceneEnum.FriendSelect);//before
		MsgCenter.Instance.Invoke(CommandEnum.OnPickQuest, thisQuestItemView);//after

		//Record picked StageInfo
		ConfigBattleUseData.Instance.currentStageInfo = stageInfo;
		ConfigBattleUseData.Instance.currentQuestInfo = data;
	}

}
