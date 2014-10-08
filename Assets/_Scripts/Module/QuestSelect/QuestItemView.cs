using UnityEngine;
using System.Collections;
using bbproto;

public class QuestItemView : DragPanelItemBase {
	private UISprite bossAvatarSpr;
	private UISprite borderSpr;
	private UISprite avatarBgSpr;	
	private UILabel nameLabel;
	private UILabel staminaLabel;
	private UILabel floorLabel;
	private UILabel expLabel;
	private UILabel coinLabel;
	private UILabel clearFlagLabel;
	
//	private static GameObject prefab;
//	public static GameObject Prefab{
//		get{
//			if(prefab == null){
//				string sourcePath = "Prefabs/UI/Quest/QuestItemPrefab";
//				prefab = ResourceManager.Instance.LoadLocalAsset(sourcePath, null) as GameObject;
//			}
//			return prefab;
//		}
//	}

//	public static QuestItemView Inject(GameObject view){
//		QuestItemView stageItemView = view.GetComponent<QuestItemView>();
//		if(stageItemView == null) stageItemView = view.AddComponent<QuestItemView>();
//		return stageItemView;
//	}

	private uint stageID;
	public uint StageID{
		get{
			return stageID;
		}
		set{
			stageID = value;
		}
	}

	private QuestInfo data;
	public QuestInfo Data{
		get{
			return data;
		}
	}

	public override void SetData<T> (T data, params object[] args)
	{
		this.data = data as QuestInfo;
		if(data == null){
			Debug.LogError("QuestItemView, Data is NULL!");
			return;
		}
		FindUIElement();
		ShowQuestInfo();
		AddEventListener();
	}

	public override void ItemCallback (params object[] args)
	{
//		throw new System.NotImplementedException ();
	}

	private StageInfo _stageInfo;
	public StageInfo stageInfo{
		set { _stageInfo = value; stageID = _stageInfo.id; }
		get {return _stageInfo;}
	}

	public Callback evolveCallback;
	
	private void ShowQuestInfo(){
		ResourceManager.Instance.GetAvatarAtlas(data.bossId[ 0 ], bossAvatarSpr);
	
		nameLabel.text = data.name;
		//staminaLabel.text = string.Format( "STAMINA {0}", data.Stamina);
		staminaLabel.text = TextCenter.GetText("Stamina") + " " + data.stamina;
		//floorLabel.text = string.Format( "FLOOR {0}", data.Floor);
//		floorLabel.text = TextCenter.GetText("Floor") + " " + data.Floor;

		expLabel.text = data.rewardExp.ToString();
		coinLabel.text = data.rewardMoney.ToString();
		bool isClear = DataCenter.Instance.QuestData.QuestClearInfo.IsStoryQuestClear(stageID, data.id);

		/*Debug.Log("QuestItemView.ShowQuestInfo(), stageID = " + stageID + ", questID = " + data.ID 
		          + ", isClear = " + isClear);*/

//		if (DataCenter.gameState == GameState.Evolve) {
//			isClear = false;
//		}

		clearFlagLabel.text = isClear ? TextCenter.GetText("clearQuest") : "";

		UnitInfo bossUnitInfo = DataCenter.Instance.UnitData.GetUnitInfo(data.bossId[ 0 ]);
		avatarBgSpr.spriteName = bossUnitInfo.GetUnitBackgroundName();
//		Debug.Log("avatarBgSpr.spriteName : " + avatarBgSpr.spriteName);
		borderSpr.spriteName = bossUnitInfo.GetUnitBorderSprName();
//		Debug.Log("bossAvatarSpr.spriteName : " + bossAvatarSpr.spriteName);

//		enabled = (data.state != EQuestState.QS_NEW);
		GetComponent<UIButton>().isEnabled = (data.state != EQuestState.QS_NEW);

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
			UIEventListenerCustom.Get(this.gameObject).onClick = null;
		else
			UIEventListenerCustom.Get(this.gameObject).onClick = ClickItem;
	}

	private void ClickItem(GameObject item){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		if(CheckStaminaEnough()){
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
//			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetStaminaLackMsgParams());
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("StaminaLackNoteTitle"),TextCenter.GetText("StaminaLackNoteContent"),TextCenter.GetText("OK"));
			return;
		}

		QuestItemView thisQuestItemView = this.GetComponent<QuestItemView>();
		BattleConfigData.Instance.currentStageInfo = stageInfo;
		BattleConfigData.Instance.currentQuestInfo = data;

//		if (DataCenter.gameState == GameState.Evolve && evolveCallback != null) {
//			evolveCallback ();
//		} else {
			ModuleManager.Instance.ShowModule(ModuleEnum.FriendSelectModule,"type","quest","data",thisQuestItemView);//before
//			MsgCenter.Instance.Invoke(CommandEnum.OnPickQuest, thisQuestItemView);//after
//		}
	}

	private bool CheckStaminaEnough(){
		int staminaNeed = Data.stamina;
		int staminaNow = DataCenter.Instance.UserData.UserInfo.staminaNow;

		if(staminaNeed > staminaNow) 
			return true;
		else 
			return false;
	}
}
