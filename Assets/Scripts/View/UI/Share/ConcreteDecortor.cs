
//--------------------------------Decorator-----------------------------------

//--------------------------------Start---------------------------------------
public class StartDecorator : DecoratorBase {
	public StartDecorator (SceneEnum sEnum) : base(sEnum) {
	}
	
	public override void ShowScene () {
		base.ShowScene ();
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {

		BgComponent background = CreatComponent< BgComponent >( UIConfig.menuBackgroundName );
		background.SetComponent ( decorator );
		
		MenuBtnsComponent bottom = CreatComponent< MenuBtnsComponent > (UIConfig.menuBottomName);
		bottom.SetComponent (background);
		
		PlayerInfoBarComponent playerInfoBar = CreatComponent<PlayerInfoBarComponent> (UIConfig.topBackgroundName);
		playerInfoBar.SetComponent (bottom);

		TipsBarComponent tipsBar = CreatComponent<TipsBarComponent> (UIConfig.TipsBarName);
		tipsBar.SetComponent (playerInfoBar);

		tipsBar.CreatUI();
		lastDecorator = tipsBar;
	}
}

//--------------------------------Quest---------------------------------------
public class QuestDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public QuestDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		sceneInfoBar.SetBackScene(SceneEnum.None);

		base.ShowScene ();

	}
		
	public override void HideScene () {
		base.HideScene ();
	}
		
	public override void DestoryScene () {
		base.DestoryScene ();
	}

	public override void DecoratorScene () {

		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		QuestComponent quest = CreatComponent< QuestComponent > ( UIConfig.questWindowName );
		quest.SetComponent ( sceneInfoBar );

		lastDecorator = quest;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Friend---------------------------------------
public class FriendDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public FriendDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		FriendComponent friend = CreatComponent< FriendComponent >( UIConfig.friendWindowName );
		friend.SetComponent( sceneInfoBar );

		lastDecorator = friend;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Scratch--------------------------------------
public class ScratchDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public ScratchDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {

		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		ScratchComponent scratch = CreatComponent< ScratchComponent >( UIConfig.scratchWindowName );
		scratch.SetComponent( sceneInfoBar );

		lastDecorator = scratch;
		lastDecorator.CreatUI();

	}
}

//--------------------------------Shop-----------------------------------------
public class ShopDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public ShopDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {

		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		ShopComponent shop = CreatComponent< ShopComponent >( UIConfig.shopWindowName );
		shop.SetComponent( sceneInfoBar );

		lastDecorator = shop;
		lastDecorator.CreatUI();

	}
}

//--------------------------------Others---------------------------------------
public class OthersDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public OthersDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		OthersComponent others = CreatComponent< OthersComponent >( UIConfig.othersWindowName );
		others.SetComponent( sceneInfoBar );

		lastDecorator = others;
		others.CreatUI ();

	}
}

//--------------------------------Units----------------------------------------
public class UnitsDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public UnitsDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		UnitsComponent units = CreatComponent< UnitsComponent >( UIConfig.unitsWindowName );
		units.SetComponent( sceneInfoBar );
	
		lastDecorator = units;
		lastDecorator.CreatUI();
	
	}
}

//--------------------------------QuestSelect----------------------------------------
public class QuestSelectDecorator : DecoratorBase {

	private SceneInfoComponent sceneInfoBar;

	public QuestSelectDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Quest);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		QuestSelectComponent questSelect = CreatComponent< QuestSelectComponent >( UIConfig.questSelectWindowName );
		questSelect.SetComponent( sceneInfoBar );

		lastDecorator = questSelect;
		lastDecorator.CreatUI();

	}
}

//--------------------------------FriendSelect----------------------------------------
public class FriendSelectDecorator : DecoratorBase {
	
	private SceneInfoComponent sceneInfoBar;
	
	public FriendSelectDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.QuestSelect);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		
		FriendSelectComponent friendSelect = CreatComponent< FriendSelectComponent >( UIConfig.friendSelectWindowName );
		friendSelect.SetComponent( sceneInfoBar );
		
		lastDecorator = friendSelect;
		lastDecorator.CreatUI();
		
	}
}

//--------------------------------Party----------------------------------------
public class PartyDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public PartyDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		PartyComponent party = CreatComponent< PartyComponent >( UIConfig.partyWindowName);
		party.SetComponent( sceneInfoBar );

		lastDecorator = party;
		lastDecorator.CreatUI();
	}
}

//--------------------------------LevelUp----------------------------------------
public class LevelUpDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public LevelUpDecorator(SceneEnum sEnum) : base(sEnum) { LogHelper.Log ("LevelUpDecorator : ");}
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {

//		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
//		LevelUpReadyPoolUI readyPanel = CreatComponent<LevelUpReadyPoolUI>(UIConfig.levelUpReadyPanelName);
//		LevelUpInfoPanelUI infoPanel = CreatComponent<LevelUpInfoPanelUI>(UIConfig.levelUpInfoPanelName);
//		LevelUpBaseUI basePanel = CreatComponent<LevelUpBaseUI>(UIConfig.levelUpBasePanelName);
//		LevelUpBaseUI friendListPanel = CreatComponent<LevelUpBaseUI>(UIConfig.levelUpFriendWindowName);
//		LevelUpMaterialUI materialPanel = CreatComponent<LevelUpMaterialUI>(UIConfig.levelUpMaterialWindowName);
//
//		sceneInfoBar.SetComponent(decorator);
//		readyPanel.SetComponent(sceneInfoBar);
//		infoPanel.SetComponent(readyPanel);
//		readyPanel.SetComponent(infoPanel);
//		basePanel.SetComponent(readyPanel);
//		friendListPanel.SetComponent(basePanel);
//		materialPanel.SetComponent(friendListPanel);
//
//		lastDecorator = materialPanel;
//		lastDecorator.CreatUI();

		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		LevelUpReadyPoolUI readyPanel = CreatComponent<LevelUpReadyPoolUI>(UIConfig.levelUpReadyPanelName);
		LevelUpInfoPanelUI infoPanel = CreatComponent<LevelUpInfoPanelUI>(UIConfig.levelUpInfoPanelName);
		LevelUpBaseUI basePanel = CreatComponent<LevelUpBaseUI>(UIConfig.levelUpBasePanelName);
		LevelUpMaterialUI materialPanel = CreatComponent<LevelUpMaterialUI>(UIConfig.levelUpMaterialWindowName);
		LevelUpBaseUI friendPanel = CreatComponent<LevelUpBaseUI>(UIConfig.levelUpFriendWindowName);

		sceneInfoBar.SetComponent(decorator);
		readyPanel.SetComponent(sceneInfoBar);
		infoPanel.SetComponent(readyPanel);
		basePanel.SetComponent(infoPanel);
		materialPanel.SetComponent(basePanel);
		friendPanel.SetComponent(materialPanel);

		lastDecorator = friendPanel;
		lastDecorator.CreatUI();

	}
}

//--------------------------------Sell------------------------------------------
public class SellDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public SellDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		SellComponent sell = CreatComponent< SellComponent >( UIConfig.sellWindowName);
		sell.SetComponent( sceneInfoBar );

		lastDecorator = sell;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Evolve------------------------------------------
public class EvolveDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public EvolveDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		EvolveComponent evolve = CreatComponent< EvolveComponent >( UIConfig.evolveWindowName);
		evolve.SetComponent( sceneInfoBar );

		lastDecorator = evolve;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Catalog------------------------------------------
public class CatalogDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public CatalogDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		CatalogComponent catalog = CreatComponent< CatalogComponent >( UIConfig.catalogWindowName);
		catalog.SetComponent( sceneInfoBar );
	
		lastDecorator = catalog;
		lastDecorator.CreatUI();
	}
}

//--------------------------------UnitList------------------------------------------
public class UnitListDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public UnitListDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

		UnitListComponent list = CreatComponent< UnitListComponent >( UIConfig.unitListWindowName);
		list.SetComponent( sceneInfoBar );

		lastDecorator = list;
		lastDecorator.CreatUI();
	}
}

//--------------------------------FriendList------------------------------------------
public class FriendListDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public FriendListDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		FriendListComponent friendListPanel = CreatComponent< FriendListComponent >( UIConfig.friendListPanelName);

		sceneInfoBar.SetComponent( decorator );
		friendListPanel.SetComponent( sceneInfoBar );

		lastDecorator = friendListPanel;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Information------------------------------------------
public class InformationDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public InformationDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		InformationComponent infoWindow = CreatComponent< InformationComponent >( UIConfig.informationWindowName );

		sceneInfoBar.SetComponent( decorator );
		infoWindow.SetComponent( sceneInfoBar );

		lastDecorator = infoWindow;
		lastDecorator.CreatUI();

	}
}

//--------------------------------SearchFriend------------------------------------------
public class SearchFriendDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public SearchFriendDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		SearchFriendUI searchMainUI = CreatComponent< SearchFriendUI >(UIConfig.searchMainWindowName );
		SearchInfoUI infoUI = CreatComponent<  SearchInfoUI >( UIConfig.searchInfoWindowName );

		TempNetwork.infoUI = infoUI;

		sceneInfoBar.SetComponent( decorator );
		searchMainUI.SetComponent(sceneInfoBar);
		infoUI.SetComponent( searchMainUI );

		lastDecorator = infoUI;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Apply------------------------------------------
public class ApplyDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public ApplyDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {

		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		ApplyComponent applyWindow = CreatComponent< ApplyComponent >( UIConfig.friendListPanelName );

		sceneInfoBar.SetComponent( decorator );
		applyWindow.SetComponent( sceneInfoBar );
		lastDecorator = applyWindow;

		lastDecorator.CreatUI();
	}
}

//--------------------------------Reception------------------------------------------
public class ReceptionDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public ReceptionDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		ReceptionComponent receptionWindow = CreatComponent< ReceptionComponent >( UIConfig.friendListPanelName );

		sceneInfoBar.SetComponent( decorator );
		receptionWindow.SetComponent( sceneInfoBar );
		
		lastDecorator = receptionWindow;
		lastDecorator.CreatUI();
	}
}

//--------------------------------YourID------------------------------------------
public class UserIDDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public UserIDDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.Friends);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		UserIDComponent userIDWindow = CreatComponent< UserIDComponent >( UIConfig.userIDWindowName);

		sceneInfoBar.SetComponent( decorator );
		userIDWindow.SetComponent( sceneInfoBar );

		lastDecorator = userIDWindow;
		lastDecorator.CreatUI();
	}
}



//--------------------------------UnitDetail------------------------------------------
public class UnitDetailDecorator : DecoratorBase {
	private SceneInfoComponent sceneInfoBar;
	public UnitDetailDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		sceneInfoBar.SetBackScene(SceneEnum.LevelUp);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );

//		PlayerInfoBarComponent playerInfoBar = CreatComponent< PlayerInfoBarComponent >( UIConfig.topBackgroundName );
//		playerInfoBar.SetComponent( sceneInfoBar );

		UnitDetailComponent  unitDetailPanel = CreatComponent< UnitDetailComponent >( UIConfig.unitDetailPanelName );
		unitDetailPanel.SetComponent( sceneInfoBar );

		lastDecorator = unitDetailPanel;
		lastDecorator.CreatUI();
	}
}
