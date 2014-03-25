
using UnityEngine;
//--------------------------------Decorator-----------------------------------

//--------------------------------Start---------------------------------------
public class StartDecorator : DecoratorBase{
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
		BgComponent background = CreatComponent< BgComponent >(UIConfig.menuBackgroundName);
		background.SetComponent(decorator);
		
		MenuBtnsComponent bottom = CreatComponent< MenuBtnsComponent >(UIConfig.menuBottomName);
		bottom.SetComponent(background);
		
		PlayerInfoBarComponent playerInfoBar = CreatComponent<PlayerInfoBarComponent>(UIConfig.topBackgroundName);
		playerInfoBar.SetComponent(bottom);

        MsgWindowLogic noteWindow = CreatComponent<MsgWindowLogic>(UIConfig.commonNoteWindowName);
        noteWindow.SetComponent(playerInfoBar);
        
        MaskController maskController = CreatComponent<MaskController>(UIConfig.screenMaskName);
        maskController.SetComponent(noteWindow);

		TipsBarComponent tipsBar = CreatComponent<TipsBarComponent>(UIConfig.TipsBarName);
        tipsBar.SetComponent(maskController);

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
public class QuestDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public QuestDecorator(SceneEnum sEnum) : base(sEnum)
	{
	}
	
	public override void ShowScene()
	{
		sceneInfoBar.SetBackScene(SceneEnum.None);

		base.ShowScene();

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

//		UnityEngine.Debug.Log("QuestDecorator");

		QuestComponent quest = CreatComponent< QuestComponent >(UIConfig.questWindowName);
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


public class GachaWindowDecorator : DecoratorBase
{
    private SceneInfoComponent sceneInfoBar;
    public GachaWindowDecorator(SceneEnum sEnum) : base(sEnum)
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
public class UnitsDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public UnitsDecorator(SceneEnum sEnum) : base(sEnum)
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

		UnitsComponent units = CreatComponent< UnitsComponent >(UIConfig.unitsWindowName);
		PartyInfoLogic partyInfo = CreatComponent<PartyInfoLogic>(UIConfig.partyInfoPanelName);
		UnitPartyPage partyPage = CreatComponent<UnitPartyPage>(UIConfig.partyPagePanelName);

		partyInfo.SetComponent(sceneInfoBar);
		partyPage.SetComponent(partyInfo);
		units.SetComponent(partyPage);
		lastDecorator = units;
		lastDecorator.CreatUI();
	}
}

//--------------------------------QuestSelect----------------------------------------
public class QuestSelectDecorator : DecoratorBase
{

	private SceneInfoComponent sceneInfoBar;

	public QuestSelectDecorator(SceneEnum sEnum) : base(sEnum)
	{
	}
	
	public override void ShowScene()
	{
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Quest);
	}
	
	public override void HideScene()
	{
		base.HideScene();
	}
	
	public override void DestoryScene()
	{
		base.DestoryScene();
	}
	
	public override void DecoratorScene(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		sceneInfoBar.SetComponent(decorator);

		QuestSelectComponent questSelect = CreatComponent< QuestSelectComponent >(UIConfig.questSelectWindowName);
		questSelect.SetComponent(sceneInfoBar);

		lastDecorator = questSelect;
		lastDecorator.CreatUI();

	}
}

//--------------------------------FriendSelect----------------------------------------
public class FriendSelectDecorator : DecoratorBase
{
	
	private SceneInfoComponent sceneInfoBar;
	
	public FriendSelectDecorator(SceneEnum sEnum) : base(sEnum)
	{
	}
	
	public override void ShowScene()
	{
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.QuestSelect);
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
		
		FriendHelperController friendSelect = CreatComponent< FriendHelperController >(UIConfig.friendSelectWindowName);

		PartyInfoLogic infoPanel = CreatComponent<PartyInfoLogic>(UIConfig.partyInfoPanelName);

		QuestPartyPage page = CreatComponent<QuestPartyPage>(UIConfig.partyPagePanelName);

		FriendHelperBriefInfo helperBriefInfo = CreatComponent<FriendHelperBriefInfo>(UIConfig.userBriefInfoWindowName);

		infoPanel.SetComponent(sceneInfoBar);

		page.SetComponent(infoPanel);

		friendSelect.SetComponent(page);

		helperBriefInfo.SetComponent(friendSelect);

		lastDecorator = helperBriefInfo;
		lastDecorator.CreatUI();
		
	}
}

//--------------------------------Party----------------------------------------
public class PartyDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public PartyDecorator(SceneEnum sEnum) : base(sEnum)
	{
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
		sceneInfoBar.SetComponent(decorator);

		PartyInfoLogic partyInfo = CreatComponent<PartyInfoLogic>(UIConfig.partyInfoPanelName);
		PartyPartyPage partyPage = CreatComponent<PartyPartyPage>(UIConfig.partyPagePanelName);
		UnitListForPartyLogic dragPanel = CreatComponent<UnitListForPartyLogic>(UIConfig.partyDragPanelName);
	
		partyInfo.SetComponent(sceneInfoBar);
		partyPage.SetComponent(partyInfo);
		dragPanel.SetComponent(partyPage);

		lastDecorator = dragPanel;

		lastDecorator.CreatUI();

	}
}

//--------------------------------LevelUp----------------------------------------
public class LevelUpDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public LevelUpDecorator(SceneEnum sEnum) : base(sEnum)
	{
		LogHelper.Log("LevelUpDecorator : ");
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
		OnSaleUnitsController sell = CreatComponent< OnSaleUnitsController >(UIConfig.sellWindowName);

		sceneInfoBar.SetComponent(decorator);
		counter.SetComponent(sceneInfoBar);
		sell.SetComponent(counter);

		lastDecorator = sell;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Evolve------------------------------------------
public class EvolveDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public EvolveDecorator(SceneEnum sEnum) : base(sEnum)
	{
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
public class CatalogDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public CatalogDecorator(SceneEnum sEnum) : base(sEnum)
	{
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
		sceneInfoBar.SetComponent(decorator);

		CatalogComponent catalog = CreatComponent< CatalogComponent >(UIConfig.catalogWindowName);
		catalog.SetComponent(sceneInfoBar);
	

		lastDecorator = catalog;
		lastDecorator.CreatUI();
	}
}

//--------------------------------UnitList------------------------------------------
public class UnitListDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public UnitListDecorator(SceneEnum sEnum) : base(sEnum)
	{
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
		sceneInfoBar.SetComponent(decorator);

		OwnedUnitListLogic unitList = CreatComponent< OwnedUnitListLogic >(UIConfig.unitListWindowName);
		unitList.SetComponent(sceneInfoBar);

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

		sceneInfoBar.SetComponent(decorator);
		counter.SetComponent(sceneInfoBar);
		friendList.SetComponent(counter);
		briefInfo.SetComponent(friendList);

		lastDecorator = briefInfo;

		lastDecorator.CreatUI();
	}
}

//--------------------------------Information------------------------------------------
public class InformationDecorator : DecoratorBase
{
	private SceneInfoComponent sceneInfoBar;
	public InformationDecorator(SceneEnum sEnum) : base(sEnum)
	{
	}
	
	public override void ShowScene()
	{
		base.ShowScene();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
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
		InformationComponent infoWindow = CreatComponent< InformationComponent >(UIConfig.informationWindowName);

		sceneInfoBar.SetComponent(decorator);
		infoWindow.SetComponent(sceneInfoBar);

		lastDecorator = infoWindow;
		lastDecorator.CreatUI();

	}
}

//--------------------------------SearchFriend------------------------------------------
public class SearchFriendDecorator : DecoratorBase
{
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
		FriendListLogic applyWindow = CreatComponent< FriendListLogic >(UIConfig.friendListWindowName);
		DeleteFriendApply deleteApply = CreatComponent<DeleteFriendApply>(UIConfig.applyMessageWindowName);

		sceneInfoBar.SetComponent(decorator);
		counter.SetComponent(sceneInfoBar);
		applyWindow.SetComponent(counter);
		deleteApply.SetComponent(applyWindow);

		lastDecorator = deleteApply;

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
		FriendListLogic receptionWindow = CreatComponent< FriendListLogic >(UIConfig.friendListWindowName);
		AccpetFriendApply acceptApply = CreatComponent<AccpetFriendApply>(UIConfig.acceptApplyMessageWindowName);

		sceneInfoBar.SetComponent(decorator);
		counter.SetComponent(sceneInfoBar);
		receptionWindow.SetComponent(counter);
		acceptApply.SetComponent(receptionWindow);

		lastDecorator = acceptApply;
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
