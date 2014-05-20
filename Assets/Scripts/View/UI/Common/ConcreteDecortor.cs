
using UnityEngine;
//--------------------------------Decorator-----------------------------------

//--------------------------------SelectRole------------------------------------------
public class SelectRoleDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public SelectRoleDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
		NoviceGuideStepEntityManager.Instance ().StartStep ();
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

//		PlayerInfoBarComponent playerInfoBar = CreatComponent<PlayerInfoBarComponent>(UIConfig.topBackgroundName);
//		playerInfoBar.SetComponent(sceneInfoBar);
//
//		BgComponent background = CreatComponent< BgComponent >(UIConfig.HomeBackgroundName);
//		background.SetComponent(playerInfoBar);

		SelectRoleController unitSelect = CreatComponent<SelectRoleController>(UIConfig.selectRoleWindowName);
		unitSelect.SetComponent(sceneInfoBar);
		
		lastDecorator = unitSelect;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Start---------------------------------------
public class StartDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public StartDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		PlayerInfoBarComponent playerInfoBar = CreatComponent<PlayerInfoBarComponent>(UIConfig.topBackgroundName);
		playerInfoBar.SetComponent(decorator);

		BgComponent background = CreatComponent< BgComponent >(UIConfig.HomeBackgroundName);
		background.SetComponent(playerInfoBar);
		
		MainMenuController bottom = CreatComponent< MainMenuController >(UIConfig.MainMenuName);
		bottom.SetComponent(background);

		TipsBarComponent tipsBar = CreatComponent<TipsBarComponent>(UIConfig.TipsBarName);
		tipsBar.SetComponent(bottom);

		UnitBriefInfoLogic selectUnitInfo = CreatComponent<UnitBriefInfoLogic>(UIConfig.unitBriefInfoWindowName);
		selectUnitInfo.SetComponent(tipsBar);

		lastDecorator = selectUnitInfo;
		lastDecorator.CreatUI();

	}
}

//--------------------------------Loading---------------------------------------
public class LoadingDecorator : DecoratorBase{
    public LoadingDecorator(SceneEnum sEnum) : base(sEnum){}
    
    public override void ShowScene(){
        base.ShowScene();
    }
    
    public override void HideScene(){
        base.HideScene();
    }
    
    public override void DestoryScene(){
        base.DestoryScene();
    }
    
    public override void DecoratorScene(){
		LoadingLogic background = CreatComponent< LoadingLogic >(UIConfig.loadingWindowName);
		background.SetComponent(decorator);

        MsgWindowLogic noteWindow = CreatComponent<MsgWindowLogic>(UIConfig.commonNoteWindowName);
		noteWindow.SetComponent(background);
        
		NoviceMsgWindowLogic guideWindow = CreatComponent<NoviceMsgWindowLogic>(UIConfig.noviceGuideWindowName);
		guideWindow.SetComponent(noteWindow);

        MaskController maskController = CreatComponent<MaskController>(UIConfig.screenMaskName);
		maskController.SetComponent(guideWindow);



		lastDecorator = maskController;
        lastDecorator.CreatUI();
        
    }
}


//--------------------------------Quest---------------------------------------
public class QuestDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public QuestDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName("Quest");
		base.ShowScene();

		//NoviceGuideStepEntityManager.Instance ().StartStep ();
	}
		
	public override void HideScene(){
		base.HideScene();
	}
		
	public override void DestoryScene(){
		base.DestoryScene();
	}

	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		//QuestController quest = CreatComponent< QuestController >(UIConfig.questWindowName);
		QuestController quest = CreatComponent< QuestController >(UIConfig.homeWorldMapName);
		quest.SetComponent(sceneInfoBar);

		lastDecorator = quest;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Friend---------------------------------------
public class FriendDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public FriendDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName("Friend");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		FriendComponent friend = CreatComponent< FriendComponent >(UIConfig.friendWindowName);
		friend.SetComponent(sceneInfoBar);

		lastDecorator = friend;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Scratch--------------------------------------
public class ScratchDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public ScratchDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName("Scratch");

		if(SceneEnum.Scratch == currentDecoratorScene)
			NoviceGuideStepEntityManager.Instance ().StartStep ();
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene()
	{
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);


        ScratchLogic scratch = CreatComponent< ScratchLogic >(UIConfig.scratchWindowName);
		scratch.SetComponent(sceneInfoBar);

        lastDecorator = scratch;
		lastDecorator.CreatUI();

	}
}


public class GachaWindowDecorator : DecoratorBase{
    private SceneInfoComponent sceneInfoBar;
    public GachaWindowDecorator(SceneEnum sEnum) : base(sEnum){}
    
    public override void ShowScene(){
        base.ShowScene();
        sceneInfoBar.SetBackScene(SceneEnum.Scratch);

		//LogHelper.Log ("gacha window decorator:" + currentDecoratorScene);
		if(currentDecoratorScene == SceneEnum.Scratch)
			NoviceGuideStepEntityManager.Instance ().StartStep ();
    }
    
    public override void HideScene(){
        base.HideScene();
    }
    
    public override void DestoryScene(){
        base.DestoryScene();
    }
    
    public override void DecoratorScene(){ 
        sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
        sceneInfoBar.SetComponent(decorator);

        GachaWindowLogic gachaWin;
        switch (currentDecoratorScene) {
        case SceneEnum.FriendScratch:
            gachaWin = CreatComponent< FriendGachaWindowLogic >(UIConfig.gachaWindowName);
            break;
        case SceneEnum.RareScratch:
            gachaWin = CreatComponent< FriendGachaWindowLogic >(UIConfig.gachaWindowName);
            break;
        case SceneEnum.EventScratch:
            gachaWin = CreatComponent< EventGachaWindowLogic >(UIConfig.gachaWindowName);
            break;
        default:
            gachaWin = CreatComponent< GachaWindowLogic >(UIConfig.gachaWindowName);
            break;
        }
        gachaWin.SetComponent(sceneInfoBar);
        lastDecorator = gachaWin;
        lastDecorator.CreatUI();
        
    }
}

//--------------------------------Shop-----------------------------------------
public class ShopDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public ShopDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		ShopComponent shop = CreatComponent< ShopComponent >(UIConfig.shopWindowName);
		shop.SetComponent(sceneInfoBar);

		lastDecorator = shop;
		lastDecorator.CreatUI();

	}
}

//--------------------------------Others---------------------------------------
public class OthersDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public OthersDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName("Others");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		OthersComponent others = CreatComponent<OthersComponent>(UIConfig.othersWindowName);
		others.SetComponent(sceneInfoBar);

		lastDecorator = others;
		others.CreatUI();

	}
}

//--------------------------------Units----------------------------------------
public class UnitsDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public UnitsDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName("Units");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		UnitsController units = CreatComponent< UnitsController >(UIConfig.unitsWindowName);
		PartyInfoLogic partyInfo = CreatComponent<PartyInfoLogic>(UIConfig.partyInfoPanelName);

		sceneInfoBar.SetComponent(decorator);
		partyInfo.SetComponent(sceneInfoBar);
		units.SetComponent(partyInfo);

		lastDecorator = units;
		lastDecorator.CreatUI();
	}
}

//--------------------------------QuestSelect----------------------------------------
public class QuestSelectDecorator : DecoratorBase{

	private SceneInfoComponent sceneInfoBar;

	public QuestSelectDecorator(SceneEnum sEnum) : base(sEnum){
        MsgCenter.Instance.AddListener(CommandEnum.QuestSelectSaveState, SetKeepState);
	}

	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.World);
		sceneInfoBar.SetCurSceneName("Quest Select");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		MsgCenter.Instance.RemoveListener(CommandEnum.QuestSelectSaveState, SetKeepState);
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		QuestSelectController questSelect = CreatComponent< QuestSelectController >(UIConfig.stageMapName);
		questSelect.SetComponent(sceneInfoBar);

		lastDecorator = questSelect;
		lastDecorator.CreatUI();

	}
}

//--------------------------------FriendSelect----------------------------------------
public class FriendSelectDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	
	public FriendSelectDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Quest);
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);
		
		FriendHelperController friendSelect = CreatComponent< FriendHelperController >(UIConfig.friendSelectWindowName);
		//SortController sortPanel = CreatComponent<SortController>(UIConfig.friendUnitSortPanelName);

		friendSelect.SetComponent(sceneInfoBar);
		//sortPanel.SetComponent(friendSelect);

		lastDecorator = friendSelect;
		lastDecorator.CreatUI();
		
	}
}

//--------------------------------Party----------------------------------------
public class PartyDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public PartyDecorator(SceneEnum sEnum) : base(sEnum){
        MsgCenter.Instance.AddListener(CommandEnum.PartySaveState, SetKeepState);
    }
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
		sceneInfoBar.SetCurSceneName("Party");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.userUnitSortPanelName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
		PartyInfoLogic partyInfo = CreatComponent<PartyInfoLogic>(UIConfig.partyInfoPanelName);
		PartyPartyPage partyPage = CreatComponent<PartyPartyPage>(UIConfig.PartyWindowName);


		sceneInfoBar.SetComponent(decorator);
		sortPanel.SetComponent(sceneInfoBar);
		partyInfo.SetComponent(sortPanel);
		counter.SetComponent(partyInfo);
		partyPage.SetComponent(counter);
	
		lastDecorator = partyPage;
		lastDecorator.CreatUI();

	}
}

//--------------------------------LevelUp----------------------------------------
public class LevelUpDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public LevelUpDecorator(SceneEnum sEnum) : base(sEnum){
        MsgCenter.Instance.AddListener(CommandEnum.LevelUpSaveState, SetKeepState);
	}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
		sceneInfoBar.SetCurSceneName("Level Up");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUpSaveState, SetKeepState);
		base.DestoryScene();
	}
	
	public override void DecoratorScene() {
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);

//		LevelUpBaseUI friendPanel = CreatComponent<LevelUpBaseUI>(UIConfig.levelUpFriendWindowName);
//		LevelUpBaseUI basePanel = CreatComponent<LevelUpBaseUI>(UIConfig.levelUpBasePanelName);
//		LevelUpReadyPoolUI readyPanel = CreatComponent<LevelUpReadyPoolUI>(UIConfig.levelUpReadyPanelName);
		//SortController sortPanel = CreatComponent<SortController>(UIConfig.userUnitSortPanelName);
		levelUpOperateUI luou = CreatComponent<levelUpOperateUI> (UIConfig.levelUpView);

		sceneInfoBar.SetComponent(decorator);
		//sortPanel.SetComponent (sceneInfoBar);
		luou.SetComponent (sceneInfoBar);
//		friendPanel.SetComponent(sceneInfoBar);
//		basePanel.SetComponent(friendPanel);
//		readyPanel.SetComponent(basePanel);

		lastDecorator = luou;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Sell------------------------------------------
public class SellDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public SellDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
        MsgCenter.Instance.AddListener(CommandEnum.SellUnitSaveState, SetKeepState);
		sceneInfoBar.SetBackScene(SceneEnum.Units);
		sceneInfoBar.SetCurSceneName("Sell");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
		SellController sell = CreatComponent< SellController >(UIConfig.sellWindowName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.userUnitSortPanelName);

		sceneInfoBar.SetComponent(decorator);
		counter.SetComponent(sceneInfoBar);
		sortPanel.SetComponent(counter);
		sell.SetComponent(sortPanel);

		lastDecorator = sell;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Evolve------------------------------------------
public class EvolveDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public EvolveDecorator(SceneEnum sEnum) : base(sEnum){
		MsgCenter.Instance.AddListener (CommandEnum.EvolveSaveState, SetKeepState);
	}
	
	public override void ShowScene(){
//		Debug.LogError("show scene begin");
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
		sceneInfoBar.SetCurSceneName("Evolve");
//		Debug.LogError("show scene end");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		MsgCenter.Instance.RemoveListener (CommandEnum.EvolveSaveState, SetKeepState);
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		EvolveComponent evolve = CreatComponent< EvolveComponent >(UIConfig.evolveWindowName);
		evolve.SetComponent(sceneInfoBar);

		UnitDisplay unitdisplay = CreatComponent< UnitDisplay >(UIConfig.unitDisplay);

		lastDecorator = unitdisplay;
		lastDecorator.CreatUI();

		EvolveDecoratorUnity edu = evolve.ViewComponent as EvolveDecoratorUnity;
		edu.SetUnitDisplay (unitdisplay.ViewComponent.gameObject);
	}

	void EvolveSaveState(object data) {
		ResetSceneState();
	}
}

//--------------------------------Catalog------------------------------------------
public class CatalogDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public CatalogDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
		sceneInfoBar.SetCurSceneName("Catalog");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
		counter.SetComponent(sceneInfoBar);
		CatalogController catalog = CreatComponent< CatalogController >(UIConfig.catalogWindowName);
		catalog.SetComponent(counter);
	
		lastDecorator = catalog;
		lastDecorator.CreatUI();
	}
}

//--------------------------------UnitList------------------------------------------
public class UnitListDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public UnitListDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
		sceneInfoBar.SetCurSceneName("Unit List");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
		MyUnitListLogic unitList = CreatComponent< MyUnitListLogic >(UIConfig.unitListWindowName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.userUnitSortPanelName);

		sceneInfoBar.SetComponent(decorator);
		counter.SetComponent(sceneInfoBar);
		sortPanel.SetComponent(counter);
		unitList.SetComponent(sortPanel);

		lastDecorator = unitList;
		lastDecorator.CreatUI();
	}
}

//--------------------------------FriendList------------------------------------------
public class FriendListDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public FriendListDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName("Friend List");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent<SceneInfoComponent>(UIConfig.sceneInfoBarName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
		FriendListLogic friendList = CreatComponent<FriendListLogic>(UIConfig.friendListWindowName);
		FriendListUserBriefInfo briefInfo = CreatComponent<FriendListUserBriefInfo>(UIConfig.userBriefInfoWindowName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.friendUnitSortPanelName);

		sceneInfoBar.SetComponent(decorator);
		counter.SetComponent(sceneInfoBar);
		friendList.SetComponent(counter);
		briefInfo.SetComponent(friendList);
		sortPanel.SetComponent(briefInfo);

		lastDecorator = sortPanel;

		lastDecorator.CreatUI();
	}
}

//--------------------------------Information------------------------------------------
public class InformationDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public InformationDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName("Information");

	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		InformationComponent infoWindow = CreatComponent< InformationComponent >(UIConfig.informationWindowName);

		sceneInfoBar.SetComponent(decorator);
		infoWindow.SetComponent(sceneInfoBar);

		lastDecorator = infoWindow;
		lastDecorator.CreatUI();

	}
}

//--------------------------------Friend Search------------------------------------------
public class FriendSearchDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public FriendSearchDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.Instace.GetCurrentText(TextConst.SCENE_NAME_FRIEND_SEARCH));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		SearchFriendController searchMainUI = CreatComponent< SearchFriendController >(UIConfig.searchMainWindowName);
		RequestFriendApply requestApply = CreatComponent<RequestFriendApply>(UIConfig.applyMessageWindowName);

		sceneInfoBar.SetComponent(decorator);
		searchMainUI.SetComponent(sceneInfoBar);

		requestApply.SetComponent(searchMainUI);

		lastDecorator = requestApply;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Apply------------------------------------------
public class ApplyDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public ApplyDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.Instace.GetCurrentText(TextConst.SCENE_NAME_APPLY));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
		ApplyController applyWindow = CreatComponent< ApplyController >(UIConfig.applyWindowName);
		DeleteFriendApply deleteApply = CreatComponent<DeleteFriendApply>(UIConfig.applyMessageWindowName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.friendUnitSortPanelName);

		sceneInfoBar.SetComponent(decorator);
		counter.SetComponent(sceneInfoBar);
		applyWindow.SetComponent(counter);
		deleteApply.SetComponent(applyWindow);
		sortPanel.SetComponent(deleteApply);

		lastDecorator = sortPanel;

		lastDecorator.CreatUI();
	}
}

//--------------------------------Reception------------------------------------------
public class ReceptionDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public ReceptionDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.Instace.GetCurrentText(TextConst.SCENE_NAME_RECEPTION));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
		ReceptionController recptionWin = CreatComponent< ReceptionController >(UIConfig.receptionWindowName);
		AccpetFriendApply acceptApply = CreatComponent<AccpetFriendApply>(UIConfig.acceptApplyMessageWindowName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.friendUnitSortPanelName);

		sceneInfoBar.SetComponent(decorator);
		counter.SetComponent(sceneInfoBar);
		recptionWin.SetComponent(counter);
		acceptApply.SetComponent(recptionWin);
		sortPanel.SetComponent(acceptApply);

		lastDecorator = sortPanel;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Your ID------------------------------------------
public class UserIDDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public UserIDDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.Instace.GetCurrentText(TextConst.SCENE_NAME_YOUR_ID));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		UserIDComponent userIDWindow = CreatComponent< UserIDComponent >(UIConfig.userIDWindowName);

		sceneInfoBar.SetComponent(decorator);
		userIDWindow.SetComponent(sceneInfoBar);

		lastDecorator = userIDWindow;
		lastDecorator.CreatUI();
	}
}



//--------------------------------Unit Detail------------------------------------------
public class UnitDetailDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public UnitDetailDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.LevelUp);
		sceneInfoBar.SetCurSceneName(TextCenter.Instace.GetCurrentText(TextConst.SCENE_NAME_UNIT_DETAIL));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		UnitDetailComponent unitDetailPanel = CreatComponent< UnitDetailComponent >(UIConfig.unitDetailPanelName);
		unitDetailPanel.SetComponent(sceneInfoBar);

		lastDecorator = unitDetailPanel;
		lastDecorator.CreatUI();
	}

}

//--------------------------------Result------------------------------------------
public class ResultDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public ResultDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.LevelUp);
		sceneInfoBar.SetCurSceneName(TextCenter.Instace.GetCurrentText(TextConst.SCENE_NAME_RESULT));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);
		
		ResultController resultWindow = CreatComponent<ResultController>(UIConfig.resultWindowName);
		resultWindow.SetComponent(sceneInfoBar);
		
		lastDecorator = resultWindow;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Fight Ready------------------------------------------
public class FightReadyDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public FightReadyDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.FriendSelect);
		sceneInfoBar.SetCurSceneName(TextCenter.Instace.GetCurrentText(TextConst.SCENE_NAME_FIGHT_READY));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		StandbyController standByWindow = CreatComponent<StandbyController>(UIConfig.fightReadyWindowName);

		sceneInfoBar.SetComponent(decorator);
		standByWindow.SetComponent(sceneInfoBar);

		lastDecorator = standByWindow;
		lastDecorator.CreatUI();
	}
}


//--------------------------------NewQuestSelect------------------------------------------
public class NewQuestSelectDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public NewQuestSelectDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Stage);
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		NewQuestSelectController questSelect = CreatComponent<NewQuestSelectController>(UIConfig.questSelectPanelName);
		
		sceneInfoBar.SetComponent(decorator);
		questSelect.SetComponent(sceneInfoBar);
		
		lastDecorator = questSelect;
		lastDecorator.CreatUI();
	}
}


