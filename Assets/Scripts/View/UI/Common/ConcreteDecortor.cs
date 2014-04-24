
using UnityEngine;
//--------------------------------Decorator-----------------------------------

//--------------------------------SelectRole------------------------------------------
public class SelectRoleDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public SelectRoleDecorator(SceneEnum sEnum) : base(sEnum){}
	
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
		
		BgComponent background = CreatComponent< BgComponent >(UIConfig.menuBackgroundName);
		background.SetComponent(sceneInfoBar);
		
		PlayerInfoBarComponent playerInfoBar = CreatComponent<PlayerInfoBarComponent>(UIConfig.topBackgroundName);
		playerInfoBar.SetComponent(background);
		
		SelectRoleController unitSelect = CreatComponent<SelectRoleController>(UIConfig.selectRoleWindowName);
		unitSelect.SetComponent(playerInfoBar);
		
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
//		sceneInfoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		BgComponent background = CreatComponent< BgComponent >(UIConfig.menuBackgroundName);
        background.SetComponent(decorator);
		
		MainMenuController bottom = CreatComponent< MainMenuController >(UIConfig.menuBottomName);
		bottom.SetComponent(background);
		
		PlayerInfoBarComponent playerInfoBar = CreatComponent<PlayerInfoBarComponent>(UIConfig.topBackgroundName);
		playerInfoBar.SetComponent(bottom);

		TipsBarComponent tipsBar = CreatComponent<TipsBarComponent>(UIConfig.TipsBarName);
		tipsBar.SetComponent(playerInfoBar);

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
        
        MaskController maskController = CreatComponent<MaskController>(UIConfig.screenMaskName);
        maskController.SetComponent(noteWindow);

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

		//QuestController quest = CreatComponent< QuestController >(UIConfig.questWindowName);
		QuestController quest = CreatComponent< QuestController >(UIConfig.homeWorldMapName);
		quest.SetComponent(sceneInfoBar);

		lastDecorator = quest;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Friend---------------------------------------
public class FriendDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public FriendDecorator(SceneEnum sEnum) : base(sEnum)
	{
	}
	
	public override void ShowScene()
	{
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene()
	{
		base.HideScene();
	}
	
	public override void DestoryScene()
	{
		base.DestoryScene();
	}
	
	public override void DecoratorScene()
	{
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		FriendComponent friend = CreatComponent< FriendComponent >(UIConfig.friendWindowName);
		friend.SetComponent(sceneInfoBar);

		lastDecorator = friend;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Scratch--------------------------------------
public class ScratchDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public ScratchDecorator(SceneEnum sEnum) : base(sEnum)
	{
	}
	
	public override void ShowScene()
	{
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene()
	{
		base.HideScene();
	}
	
	public override void DestoryScene()
	{
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
    }
    
    public override void HideScene()
    {
        base.HideScene();
    }
    
    public override void DestoryScene()
    {
        base.DestoryScene();
    }
    
    public override void DecoratorScene()
    {
        
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
public class ShopDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public ShopDecorator(SceneEnum sEnum) : base(sEnum)
	{
	}
	
	public override void ShowScene()
	{
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene()
	{
		base.HideScene();
	}
	
	public override void DestoryScene()
	{
		base.DestoryScene();
	}
	
	public override void DecoratorScene()
	{

		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		ShopComponent shop = CreatComponent< ShopComponent >(UIConfig.shopWindowName);
		shop.SetComponent(sceneInfoBar);

		lastDecorator = shop;
		lastDecorator.CreatUI();

	}
}

//--------------------------------Others---------------------------------------
public class OthersDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public OthersDecorator(SceneEnum sEnum) : base(sEnum)
	{
	}
	
	public override void ShowScene()
	{
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene()
	{
//		UnityEngine.Debug.LogError("HideScene");
		base.HideScene();
	}
	
	public override void DestoryScene()
	{
		base.DestoryScene();
	}
	
	public override void DecoratorScene()
	{
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

		QuestSelectController questSelect = CreatComponent< QuestSelectController >(UIConfig.stageSlidePanelName);
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
		sceneInfoBar.SetBackScene(SceneEnum.QuestSelect);
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
		PartyInfoLogic infoPanel = CreatComponent<PartyInfoLogic>(UIConfig.partyInfoPanelName);
		FriendHelperBriefInfo helperBriefInfo = CreatComponent<FriendHelperBriefInfo>(UIConfig.userBriefInfoWindowName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.friendUnitSortPanelName);

		infoPanel.SetComponent(sceneInfoBar);
		friendSelect.SetComponent(infoPanel);
		sortPanel.SetComponent(friendSelect);
		helperBriefInfo.SetComponent(sortPanel);

		lastDecorator = helperBriefInfo;
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
		PartyInfoLogic partyInfo = CreatComponent<PartyInfoLogic>(UIConfig.partyInfoPanelName);
		PartyPartyPage partyPage = CreatComponent<PartyPartyPage>(UIConfig.PartyWindowName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.userUnitSortPanelName);

		sceneInfoBar.SetComponent(decorator);
		partyInfo.SetComponent(sceneInfoBar);
		counter.SetComponent(partyInfo);
		sortPanel.SetComponent(counter);
		partyPage.SetComponent(sortPanel);
	
		lastDecorator = partyPage;
		lastDecorator.CreatUI();

	}
}

//--------------------------------LevelUp----------------------------------------
public class LevelUpDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public LevelUpDecorator(SceneEnum sEnum) : base(sEnum){
		//LogHelper.Log("LevelUpDecorator : ");
        MsgCenter.Instance.AddListener(CommandEnum.LevelUpSaveState, SetKeepState);
	}
	
	public override void ShowScene()
	{
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene()
	{
		base.HideScene();
	}
	
	public override void DestoryScene()
	{
		base.DestoryScene();
	}
	
	public override void DecoratorScene()
	{
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);

		LevelUpBaseUI friendPanel = CreatComponent<LevelUpBaseUI>(UIConfig.levelUpFriendWindowName);
		LevelUpBaseUI basePanel = CreatComponent<LevelUpBaseUI>(UIConfig.levelUpBasePanelName);
		LevelUpReadyPoolUI readyPanel = CreatComponent<LevelUpReadyPoolUI>(UIConfig.levelUpReadyPanelName);

		sceneInfoBar.SetComponent(decorator);
		friendPanel.SetComponent(sceneInfoBar);
		basePanel.SetComponent(friendPanel);
		readyPanel.SetComponent(basePanel);


		lastDecorator = readyPanel;
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
	public EvolveDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
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

		EvolveComponent evolve = CreatComponent< EvolveComponent >(UIConfig.evolveWindowName);
		evolve.SetComponent(sceneInfoBar);

		UnitDisplay unitdisplay = CreatComponent< UnitDisplay >(UIConfig.unitDisplay);
		unitdisplay.SetComponent(evolve);

		LevelUpBaseUI friendPanel = CreatComponent<LevelUpBaseUI>(UIConfig.evolveFriend);
		friendPanel.SetComponent (unitdisplay);

		lastDecorator = friendPanel;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Catalog------------------------------------------
public class CatalogDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public CatalogDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
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

//--------------------------------SearchFriend------------------------------------------
public class SearchFriendDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public SearchFriendDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
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

//--------------------------------YourID------------------------------------------
public class UserIDDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public UserIDDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
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



//--------------------------------UnitDetail------------------------------------------
public class UnitDetailDecorator : DecoratorBase{
	private SceneInfoComponent sceneInfoBar;
	public UnitDetailDecorator(SceneEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.LevelUp);
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


