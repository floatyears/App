
using UnityEngine;
//--------------------------------Decorator-----------------------------------

//--------------------------------Role Select------------------------------------------
public class SelectRoleDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public SelectRoleDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_ROLE_SELECT));
//		NoviceGuideStepEntityManager.Instance ().StartStep ();
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
		
		SelectRoleController unitSelect = CreatComponent<SelectRoleController>(UIConfig.selectRoleWindowName);
		unitSelect.SetComponent(sceneInfoBar);
		
		lastDecorator = unitSelect;
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Start---------------------------------------
public class StartDecorator : DecoratorBase {
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

		lastDecorator = tipsBar;
//		lastDecorator.CreatUIAsyn(lastDecorator);
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

//		guideWindow.SetComponent(noteWindow);
		
		MaskController maskController = CreatComponent<MaskController>(UIConfig.screenMaskName);
		maskController.SetComponent(noteWindow);
		
		lastDecorator = maskController;

//		lastDecorator = background;
//		lastDecorator.CreatUIAsyn(lastDecorator);
        
    }
}


//--------------------------------Home---------------------------------------
public class HomeDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public HomeDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_HOME));
		base.ShowScene();
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

		QuestController quest = CreatComponent< QuestController >(UIConfig.homeWindowName);
		quest.SetComponent(sceneInfoBar);

		lastDecorator = quest;
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Friend---------------------------------------
public class FriendDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public FriendDecorator(SceneEnum sEnum) : base(sEnum){ }
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName( TextCenter.GetText( TextConst.SCENE_NAME_FRIEND ) );
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
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Scratch--------------------------------------
public class ScratchDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public ScratchDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_SCRATCH));

		if(SceneEnum.Scratch == currentDecoratorScene)
			NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
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


        ScratchLogic scratch = CreatComponent< ScratchLogic >(UIConfig.scratchWindowName);
		scratch.SetComponent(sceneInfoBar);

        lastDecorator = scratch;
//		lastDecorator.CreatUI();

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
			NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
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
//        lastDecorator.CreatUI();
        
    }
}

//--------------------------------Shop-----------------------------------------
public class ShopDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public ShopDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_SHOP));
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
//		lastDecorator.CreatUI();

	}
}

//--------------------------------Others---------------------------------------
public class OthersDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public OthersDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_OTHERS));	}
	
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
//		others.CreatUI();

	}
}

//--------------------------------Units----------------------------------------
public class UnitsDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public UnitsDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_UNITS));	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		UnitsController units = CreatComponent< UnitsController >(UIConfig.unitsWindowName);
		//UnitInfoLogic partyInfo = CreatComponent<UnitInfoLogic>(UIConfig.unitsInfoPanelName);

		sceneInfoBar.SetComponent(decorator);
		//partyInfo.SetComponent(sceneInfoBar);
		units.SetComponent(sceneInfoBar);

		lastDecorator = units;
//		lastDecorator.CreatUI();
	}
}

//--------------------------------StageSelect----------------------------------------
public class StageSelectDecorrator : DecoratorBase{

	private SceneInfoComponent sceneInfoBar;

	public StageSelectDecorrator(SceneEnum sEnum) : base(sEnum){
        MsgCenter.Instance.AddListener(CommandEnum.QuestSelectSaveState, SetKeepState);
	}

	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Home);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_STAGE_SELECT));
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

		StageSelectController questSelect = CreatComponent< StageSelectController >(UIConfig.stageWindowName);
		questSelect.SetComponent(sceneInfoBar);

		lastDecorator = questSelect;
//		lastDecorator.CreatUI();

	}
}

//--------------------------------FriendSelect----------------------------------------
public class FriendSelectDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	
	public FriendSelectDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.QuestSelect);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_FRIEND_SELECT));
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

		friendSelect.SetComponent(sceneInfoBar);

		lastDecorator = friendSelect;
//		lastDecorator.CreatUI();
		
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
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_PARTY));	}
	
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
		//PartyInfoLogic partyInfo = CreatComponent<PartyInfoLogic>(UIConfig.partyInfoPanelName);
		PartyPartyPage partyPage = CreatComponent<PartyPartyPage>(UIConfig.PartyWindowName);

		sceneInfoBar.SetComponent(decorator);
		sortPanel.SetComponent(sceneInfoBar);
		//partyInfo.SetComponent(sortPanel);
		counter.SetComponent(sortPanel);
		partyPage.SetComponent(counter);
	
		lastDecorator = partyPage;
//		lastDecorator.CreatUI();
	}
}

//--------------------------------LevelUp----------------------------------------
public class LevelUpDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public LevelUpDecorator(SceneEnum sEnum) : base(sEnum){
//		Debug.LogWarning ("levelup AddListener SetKeepState");
		MsgCenter.Instance.AddListener(CommandEnum.LevelUpSaveState, SetKeepState);
	}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_LEVEL_UP));	
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
//		Debug.LogWarning ("levelup RemoveListener SetKeepState");
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUpSaveState, SetKeepState);
		base.DestoryScene();
	}
	
	public override void DecoratorScene() {
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		SortController sc = CreatComponent<SortController> (UIConfig.userUnitSortPanelName);
		sc.SetComponent (sceneInfoBar);

		levelUpOperateUI luou = CreatComponent<levelUpOperateUI> (UIConfig.levelUpView);
		luou.SetComponent (sc);

		lastDecorator = luou;
//		lastDecorator.CreatUI();
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
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_UNIT_SELL));	}
	
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
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Evolve------------------------------------------
public class EvolveDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public EvolveDecorator(SceneEnum sEnum) : base(sEnum){
		MsgCenter.Instance.AddListener (CommandEnum.EvolveSaveState, SetKeepState);
	}
	
	public override void ShowScene(){
//		Debug.LogError("EvolveDecorator show scene begin");
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_EVOLVE));
//		Debug.LogError("EvolveDecorator show scene end");
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

		SortController sc = CreatComponent<SortController> (UIConfig.userUnitSortPanelName);
		sc.SetComponent (sceneInfoBar);

		EvolveComponent evolve = CreatComponent< EvolveComponent >(UIConfig.evolveWindowName);
		evolve.SetComponent(sc);

		UnitDisplay unitdisplay = CreatComponent< UnitDisplay >(UIConfig.unitDisplay);
		unitdisplay.SetComponent (evolve);

		lastDecorator = unitdisplay;
//		lastDecorator.CreatUI();

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
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_CATALOG));	
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
	
//		lastDecorator.CreatUI();
	}
}

//--------------------------------UnitList------------------------------------------
public class UnitListDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public UnitListDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_UNIT_LIST));
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
//		lastDecorator.CreatUI();
	}
}

//--------------------------------FriendList------------------------------------------
public class FriendListDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public FriendListDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_FRIEND_LIST));
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
		briefInfo.SetComponent(counter);
		sortPanel.SetComponent(briefInfo);
		friendList.SetComponent(sortPanel);

		lastDecorator = friendList;

//		lastDecorator.CreatUI();
	}
}

//--------------------------------Information------------------------------------------
public class InformationDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public InformationDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_INFORMATION));
         
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
//		lastDecorator.CreatUI();

	}
}

//--------------------------------Friend Search------------------------------------------
public class FriendSearchDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public FriendSearchDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_FRIEND_SEARCH));
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
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Apply------------------------------------------
public class ApplyDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public ApplyDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_APPLY));
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
		sortPanel.SetComponent(sceneInfoBar);
		counter.SetComponent(sortPanel);
		applyWindow.SetComponent(counter);
		deleteApply.SetComponent(applyWindow);
		//sortPanel.SetComponent(deleteApply);

		lastDecorator = deleteApply;

//		lastDecorator.CreatUI();
	}
}

//--------------------------------Reception------------------------------------------
public class ReceptionDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public ReceptionDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_RECEPTION));
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
//		lastDecorator.CreatUI();
	}
}

//--------------------------------YourID------------------------------------------
public class UserIDDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public UserIDDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_YOUR_ID));
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
//		lastDecorator.CreatUI();
	}
}



//--------------------------------UnitDetail------------------------------------------
public class UnitDetailDecorator : DecoratorBase{
	private UnitDetailComponent unitDetail;
	public UnitDetailDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
//		sceneInfoBar.SetBackScene(SceneEnum.LevelUp);
//		sceneInfoBar.SetCurSceneName(TextCenter.Instace.GetCurrentText(TextConst.SCENE_NAME_UNIT_DETAIL));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){

		UnitDetailTopComponent unitDetailTop = CreatComponent< UnitDetailTopComponent >(UIConfig.unitDetailTopPanelName);
		unitDetailTop.SetComponent(decorator);

		UnitDetailCenterComponent unitDetailCenter = CreatComponent<UnitDetailCenterComponent> (UIConfig.unitDetailCenterPanelName);
		unitDetailCenter.SetComponent (unitDetailTop);

		unitDetail = CreatComponent< UnitDetailComponent >(UIConfig.unitDetailPanelName);
		unitDetail.SetComponent(unitDetailCenter);

		lastDecorator = unitDetail;
//		lastDecorator.CreatUI();
	}

}

//--------------------------------Result------------------------------------------
public class ResultDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public ResultDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.LevelUp);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_RESULT));
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
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Fight Ready------------------------------------------
public class FightReadyDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public FightReadyDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.FriendSelect);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_FIGHT_READY));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		FightReadyController standByWindow = CreatComponent<FightReadyController>(UIConfig.fightReadyWindowName);

		sceneInfoBar.SetComponent(decorator);
		standByWindow.SetComponent(sceneInfoBar);

		lastDecorator = standByWindow;
//		lastDecorator.CreatUI();
	}
}


//--------------------------------Quest Select------------------------------------------
public class QuestSelectDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public QuestSelectDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.StageSelect);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_QUEST_SELECT));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		QuestSelectController questSelect = CreatComponent<QuestSelectController>(UIConfig.questSelectWindowName);
		
		sceneInfoBar.SetComponent(decorator);
		questSelect.SetComponent(sceneInfoBar);
		
		lastDecorator = questSelect;
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Operation Notice------------------------------------------
public class OperationNoticeDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public OperationNoticeDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
//		sceneInfoBar.SetBackScene(SceneEnum.Home);
//		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_OPERATION_NOTICE));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
//		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		OperationNoticeComponent operationNotice = CreatComponent<OperationNoticeComponent>(UIConfig.operationNoticeWindowName);
		
//		sceneInfoBar.SetComponent(decorator);
		operationNotice.SetComponent (decorator);//sceneInfoBar);
		
		lastDecorator = operationNotice;
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Reward------------------------------------------
public class RewardDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public RewardDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Home);
		LogHelper.Log ("reward scene name: " + TextCenter.GetText(TextConst.SCENE_NAME_REWARD));
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_REWARD));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		RewardComponent reward = CreatComponent<RewardComponent>(UIConfig.rewardViewName);
		
		sceneInfoBar.SetComponent(decorator);
		reward.SetComponent (sceneInfoBar);
		
		lastDecorator = reward;
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Raider------------------------------------------
public class GameRaiderDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public GameRaiderDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Home);
		//LogHelper.Log ("reward scene name: " + TextCenter.GetText(TextConst.SCENE_NAME_REWARD));
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_RAIDER));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		GameRaiderComponent raider = CreatComponent<GameRaiderComponent>(UIConfig.gameRaiderWindowName);
		
		sceneInfoBar.SetComponent(decorator);
		raider.SetComponent (sceneInfoBar);
		
		lastDecorator = raider;
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Raider------------------------------------------
public class GameCurrencyDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public GameCurrencyDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Home);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_CURRENCY));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		GameCurrencyComponent currency = CreatComponent<GameCurrencyComponent>(UIConfig.gameCurrencyWindowName);
		
		sceneInfoBar.SetComponent(decorator);
		currency.SetComponent (sceneInfoBar);
		
		lastDecorator = currency;
//		lastDecorator.CreatUI();
	}
}

