using UnityEngine;
using System.Collections;
using bbproto;

public class UnitSourceItemView : DragPanelItemBase {

	private UnitGetWay data;

	private GameObject goBtn;
	private UILabel nameLabel;
	private UILabel infoLabel1;
	private UILabel infoLabel2;

	private object goData1;
	private object goData2;

	private UISprite bossAvatarSpr;
	private UISprite borderSpr;
	private UISprite avatarBgSpr;	
	private UISprite mask;
	private UISprite lockSpr;

	void Init(){
		goBtn = transform.FindChild ("GoBtn").gameObject;
		transform.FindChild ("GoBtn/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("Btn_GoTo");
		UIEventListenerCustom.Get (goBtn).onClick = GotoGet;
		nameLabel = transform.FindChild ("Lable_Name").GetComponent<UILabel> ();
		infoLabel1 = transform.FindChild ("Label_Info1").GetComponent<UILabel> ();
		infoLabel2 = transform.FindChild ("Label_Info2").GetComponent<UILabel> ();

		bossAvatarSpr = transform.FindChild("Sprite_Boss_Avatar").GetComponent<UISprite>();
		borderSpr = transform.FindChild("Sprite_Boss_Avatar_Border").GetComponent<UISprite>();
		avatarBgSpr = transform.FindChild("Sprite_Boss_Avatar_Bg").GetComponent<UISprite>();
		mask = transform.FindChild ("Mask").GetComponent<UISprite> ();
		lockSpr = transform.FindChild ("LockImg").GetComponent<UISprite> ();
	}

	public override void ItemCallback (params object[] args)
	{
		throw new System.NotImplementedException ();
	}

	public override void SetData<T> (T d, params object[] args)
	{
		if (nameLabel == null)
			Init ();
		data = d as UnitGetWay;

		switch (data.getType) {
		
		case EUnitGetType.E_EVENT_QUEST:
		case  EUnitGetType.E_NORMAL_QUEST:
		
			//				CityInfo cityInfo = DataCenter.Instance.QuestData.GetCityInfo(cityId);
			QuestInfo questInfo = DataCenter.Instance.QuestData.GetQuestInfo(data.getPath);
			if ( questInfo!=null) {

				ShowQuestInfo(questInfo);
			}
			break;
		case EUnitGetType.E_STAGE:
			StageInfo stageInfo = DataCenter.Instance.QuestData.GetStageInfo(data.getPath);
			if ( stageInfo!=null) {
				ShowStageInfo(stageInfo);
			}
			break;
		case EUnitGetType.E_GACHA_EVENT:
			break;
		case EUnitGetType.E_GACHA_NORMAL:
			break;
		case EUnitGetType.E_BUY:
			break;
		case EUnitGetType.E_BONUS:
			break;
		}

	}

	void GotoGet(GameObject obj){
		switch (data.getType) {
		case EUnitGetType.E_EVENT_QUEST:
			ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"event");
			ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"unti_source");
			break;
		case EUnitGetType.E_STAGE:
			ModuleManager.Instance.ShowModule(ModuleEnum.QuestSelectModule,"data",goData1 as StageInfo);
			ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"unti_source");
			break;
		case  EUnitGetType.E_NORMAL_QUEST:
			if(!mask.enabled){
				ModuleManager.Instance.ShowModule(ModuleEnum.QuestSelectModule,"data",goData1 as StageInfo);
				ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"unti_source");
//				ModuleManager.Instance.ShowModule(ModuleEnum.FightReadyModule,"QuestInfo",goData1 as QuestInfo);
//				ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"unti_source");
			}
//			else{
////				ModuleManager.Instance.ShowModule(ModuleEnum.FightReadyModule,"QuestInfo",goData1 as QuestInfo);
////				ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"unti_source");
//			}
			break;
		case EUnitGetType.E_GACHA_EVENT:
			break;
		case EUnitGetType.E_GACHA_NORMAL:
			break;
		case EUnitGetType.E_BUY:
			break;
		case EUnitGetType.E_BONUS:
			break;
		}
	}

	private void ShowQuestInfo(QuestInfo data){
		goData1 = data;
		nameLabel.text = data.name;
		infoLabel1.text = TextCenter.GetText("Stamina") + " " +  data.stamina;
		infoLabel2.text = "";
		ResourceManager.Instance.GetAvatarAtlas(data.bossId[ 0 ], bossAvatarSpr);
		
		//staminaLabel.text = string.Format( "STAMINA {0}", data.Stamina);
		//floorLabel.text = string.Format( "FLOOR {0}", data.Floor);
		//		floorLabel.text = TextCenter.GetText("Floor") + " " + data.Floor;
		/*Debug.Log("QuestItemView.ShowQuestInfo(), stageID = " + stageID + ", questID = " + data.ID 
		          + ", isClear = " + isClear);*/
		
		//		if (DataCenter.gameState == GameState.Evolve) {
		//			isClear = false;
		//		}
		
		//		clearFlagLabel.text = isClear ? TextCenter.GetText("clearQuest") : "";
		
		UnitInfo bossUnitInfo = DataCenter.Instance.UnitData.GetUnitInfo(data.bossId[ 0 ]);
		avatarBgSpr.spriteName = bossUnitInfo.GetUnitBackgroundName();
		borderSpr.spriteName = bossUnitInfo.GetUnitBorderSprName();
		
		//		enabled = (data.state != EQuestState.QS_NEW);
		bool isLocked = (data.state == EQuestState.QS_NEW);
		mask.enabled = isLocked;
		lockSpr.gameObject.SetActive( isLocked );
	}

	private void ShowStageInfo(StageInfo data){
		goData1 = data;
		nameLabel.text = data.stageName;
		infoLabel1.text = "";
		infoLabel2.text = "";
		bool isLocked = (data.state == EQuestState.QS_NEW);
//		mask.enabled = isLocked;
		lockSpr.gameObject.SetActive( isLocked );
	}

}
