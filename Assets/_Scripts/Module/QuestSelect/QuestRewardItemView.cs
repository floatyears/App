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

		/*Debug.Log("QuestItemView.ShowQuestInfo(), stageID = " + stageID + ", questID = " + data.ID 
		          + ", isClear = " + isClear);*/

//		if (DataCenter.gameState == GameState.Evolve) {
//			isClear = false;
//		}

//		clearFlagLabel.text = isClear ? TextCenter.GetText("clearQuest") : "";

		bool isClear = DataCenter.Instance.QuestData.QuestClearInfo.IsStoryQuestClear(stageID, data.id, stageInfo.CopyType);


		ShowQuestStar(isClear);

		UnitInfo bossUnitInfo = DataCenter.Instance.UnitData.GetUnitInfo(data.bossId[ 0 ]);
		avatarBgSpr.spriteName = bossUnitInfo.GetUnitBackgroundName();
		borderSpr.spriteName = bossUnitInfo.GetUnitBorderSprName();

//		enabled = (data.state != EQuestState.QS_NEW);
		mask.enabled = (data.state == EQuestState.QS_NEW);

		clearFlagLabel.text = (data.state == EQuestState.QS_QUESTING) ? "New!" : "";
	}

	private void ShowQuestStar(bool isClear) {

		UISprite star = transform.FindChild("Star").GetComponent<UISprite>();
		star.gameObject.SetActive(isClear);
		if ( isClear ) {
			CopyPassInfo passinfo = ( (stageInfo.CopyType == ECopyType.CT_NORMAL) ? DataCenter.Instance.NormalCopyInfo : DataCenter.Instance.EliteCopyInfo);
			
			int stageStar = passinfo.GetQuestStar(data.id);
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
	}

	private void AddEventListener(){
		if(data == null)
			UIEventListenerCustom.Get(this.gameObject).onClick = null;
		else
			UIEventListenerCustom.Get(this.gameObject).onClick = ClickItem;
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
		ModuleManager.Instance.ShowModule(ModuleEnum.FightReadyModule,"QuestInfo", data);
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
