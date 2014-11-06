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
	private UISprite mask;
	private UISprite lockSpr;

	private uint stageID;
	public uint StageID{
		get{
			return stageID;
		}
		set{
			stageID = value;
		}
	}
	
	private StageInfo _stageInfo;
	public StageInfo stageInfo{
		set { _stageInfo = value; stageID = _stageInfo.id; }
		get {return _stageInfo;}
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
		tag = "Untagged";
		if (this.data.state == EQuestState.QS_QUESTING) {
			tag = "quest_new";	
		}
		if(data == null){
			Debug.LogError("QuestItemView, Data is NULL!");
			return;
		}
		if (bossAvatarSpr == null) {
			FindUIElement();	
		}

		if(args.Length>0) {
			stageInfo = (args[0] as StageInfo);
		}

		ShowQuestInfo();
		AddEventListener();
	}

	public override void ItemCallback (params object[] args)
	{
//		throw new System.NotImplementedException ();
	}

	public void CollectBonusCallback () {

	}


	public Callback evolveCallback;
	
	private void ShowQuestInfo(){
		ResourceManager.Instance.GetAvatarAtlas(data.bossId[ 0 ], bossAvatarSpr);
	
		nameLabel.text = data.name;
		//staminaLabel.text = string.Format( "STAMINA {0}", data.Stamina);
		//floorLabel.text = string.Format( "FLOOR {0}", data.Floor);
//		floorLabel.text = TextCenter.GetText("Floor") + " " + data.Floor;
		int multiple = (stageInfo.CopyType==ECopyType.CT_NORMAL ? 1 : 2);

		staminaLabel.text = TextCenter.GetText("Stamina") + " " + (data.stamina * multiple);
		expLabel.text = (data.rewardExp * multiple).ToString();
		coinLabel.text = (data.rewardMoney * multiple).ToString();

		/*Debug.Log("QuestItemView.ShowQuestInfo(), stageID = " + stageID + ", questID = " + data.ID 
		          + ", isClear = " + isClear);*/

//		if (DataCenter.gameState == GameState.Evolve) {
//			isClear = false;
//		}

//		clearFlagLabel.text = isClear ? TextCenter.GetText("clearQuest") : "";

		bool isClear = DataCenter.Instance.QuestData.QuestClearInfo.GetStoryQuestState(stageID, data.id, stageInfo.CopyType) == StageState.CLEAR;
		ShowQuestStar(isClear);

		UnitInfo bossUnitInfo = DataCenter.Instance.UnitData.GetUnitInfo(data.bossId[ 0 ]);
		avatarBgSpr.spriteName = bossUnitInfo.GetUnitBackgroundName();
		borderSpr.spriteName = bossUnitInfo.GetUnitBorderSprName();

//		enabled = (data.state != EQuestState.QS_NEW);
		bool isLocked = (data.state == EQuestState.QS_NEW);
		mask.enabled = isLocked;
		lockSpr.gameObject.SetActive( isLocked );

		clearFlagLabel.text = (data.state == EQuestState.QS_QUESTING) ? "New!" : "";
	}

	private void ShowQuestStar(bool isClear) {

		UISprite star = transform.FindChild("Star").GetComponent<UISprite>();
		star.gameObject.SetActive(isClear);
		if ( isClear ) {
			CopyPassInfo passinfo = DataCenter.Instance.GetCopyPassInfo(stageInfo.CopyType);
			
			int stageStar = passinfo.GetQuestStar(StageID, data.id);
			star.width = star.height * stageStar;
		}
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
		mask = transform.FindChild ("Mask").GetComponent<UISprite> ();
		lockSpr = transform.FindChild ("LockImg").GetComponent<UISprite> ();
	}

	private void AddEventListener(){
		if(data == null)
			UIEventListenerCustom.Get(this.gameObject).onClick -= ClickItem;
		else
			UIEventListenerCustom.Get(this.gameObject).onClick += ClickItem;
	}

	private void ClickItem(GameObject item){
		if (mask.enabled)
			return;
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		if(CheckStaminaEnough()){
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("StaminaLackNoteTitle"),TextCenter.GetText("StaminaLackNoteContent"),TextCenter.GetText("OK"));
			return;
		}

		QuestItemView thisQuestItemView = this.GetComponent<QuestItemView>();
		BattleConfigData.Instance.currentStageInfo = stageInfo;
		BattleConfigData.Instance.currentQuestInfo = data;

//		if (NoviceGuideStepManager.Instance.CurrentGuideStep == NoviceGuideStage.NoviceGuideStepB_3) {
//			
//			,"HelperInfo", null);//before
//		} else {
//			ModuleManager.Instance.ShowModule(ModuleEnum.FriendSelectModule,"type","quest","data",thisQuestItemView);//before
//		}
		ModuleManager.Instance.ShowModule(ModuleEnum.FightReadyModule,"QuestInfo", data, "StageInfo", stageInfo);
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
