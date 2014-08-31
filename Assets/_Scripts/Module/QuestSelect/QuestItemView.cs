using UnityEngine;
using System.Collections;

public class QuestItemView : MonoBehaviour {
	private UISprite bossAvatarSpr;
	private UISprite borderSpr;
	private UISprite avatarBgSpr;	
	private UILabel nameLabel;
	private UILabel staminaLabel;
	private UILabel floorLabel;
	private UILabel expLabel;
	private UILabel coinLabel;
	private UILabel clearFlagLabel;
	
	private static GameObject prefab;
	public static GameObject Prefab{
		get{
			if(prefab == null){
				string sourcePath = "Prefabs/UI/Quest/QuestItemPrefab";
				prefab = ResourceManager.Instance.LoadLocalAsset(sourcePath, null) as GameObject;
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

	private TStageInfo _stageInfo;
	public TStageInfo stageInfo{
		set { _stageInfo = value; stageID = _stageInfo.ID; }
		get {return _stageInfo;}
	}

	public Callback evolveCallback;
	
	private void ShowQuestInfo(){
		DataCenter.Instance.GetAvatarAtlas(data.BossID[ 0 ], bossAvatarSpr);
	
		nameLabel.text = data.Name;
		//staminaLabel.text = string.Format( "STAMINA {0}", data.Stamina);
		staminaLabel.text = TextCenter.GetText("Stamina") + " " + data.Stamina;
		//floorLabel.text = string.Format( "FLOOR {0}", data.Floor);
//		floorLabel.text = TextCenter.GetText("Floor") + " " + data.Floor;

		expLabel.text = data.RewardExp.ToString();
		coinLabel.text = data.RewardMoney.ToString();
		bool isClear = DataCenter.Instance.QuestClearInfo.IsStoryQuestClear(stageID, data.ID);

		/*Debug.Log("QuestItemView.ShowQuestInfo(), stageID = " + stageID + ", questID = " + data.ID 
		          + ", isClear = " + isClear);*/

		if (DataCenter.gameState == GameState.Evolve) {
			isClear = false;
		}

		clearFlagLabel.text = isClear ? TextCenter.GetText("clearQuest") : "";

		TUnitInfo bossUnitInfo = DataCenter.Instance.GetUnitInfo(data.BossID[ 0 ]);
		avatarBgSpr.spriteName = bossUnitInfo.GetUnitBackgroundName();
//		Debug.Log("avatarBgSpr.spriteName : " + avatarBgSpr.spriteName);
		borderSpr.spriteName = bossUnitInfo.GetUnitBorderSprName();
//		Debug.Log("bossAvatarSpr.spriteName : " + bossAvatarSpr.spriteName);

	}

	private void FindUIElement(){
		bossAvatarSpr = transform.FindChild("Sprite_Boss_Avatar").GetComponent<UISprite>();
		nameLabel = transform.FindChild("Label_Quest_Name").GetComponent<UILabel>();
		staminaLabel = transform.FindChild("Label_Stamina").GetComponent<UILabel>();
		floorLabel = transform.FindChild("Label_Floor").GetComponent<UILabel>();
		floorLabel.enabled = false;
		expLabel = transform.FindChild("Label_Exp").GetComponent<UILabel>();
		coinLabel = transform.FindChild("Label_Coin").GetComponent<UILabel>();
		clearFlagLabel = transform.FindChild("Label_Clear_Flag").GetComponent<UILabel>();
		clearFlagLabel.text = TextCenter.GetText ("StageStateClear");
		borderSpr = transform.FindChild("Sprite_Boss_Avatar_Border").GetComponent<UISprite>();
		avatarBgSpr = transform.FindChild("Sprite_Boss_Avatar_Bg").GetComponent<UISprite>();
	}

	private void AddEventListener(){
		if(data == null)
			UIEventListener.Get(this.gameObject).onClick = null;
		else
			UIEventListener.Get(this.gameObject).onClick = ClickItem;
	}

	private void ClickItem(GameObject item){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		if(CheckStaminaEnough()){
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetStaminaLackMsgParams());
			return;
		}

		QuestItemView thisQuestItemView = this.GetComponent<QuestItemView>();
		ConfigBattleUseData.Instance.currentStageInfo = stageInfo;
		ConfigBattleUseData.Instance.currentQuestInfo = data;

		if (DataCenter.gameState == GameState.Evolve && evolveCallback != null) {
			evolveCallback ();
		} else {
			UIManager.Instance.ChangeScene(SceneEnum.FriendSelect);//before
			MsgCenter.Instance.Invoke(CommandEnum.OnPickQuest, thisQuestItemView);//after
		}
	}

	private bool CheckStaminaEnough(){
		int staminaNeed = Data.Stamina;
		int staminaNow = DataCenter.Instance.UserInfo.StaminaNow;

		if(staminaNeed > staminaNow) 
			return true;
		else 
			return false;
	}
	private MsgWindowParams GetStaminaLackMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.GetText("StaminaLackNoteTitle");
		msgParams.contentText = TextCenter.GetText("StaminaLackNoteContent");
		msgParams.btnParam = new BtnParam();
		return msgParams;
	}
}
