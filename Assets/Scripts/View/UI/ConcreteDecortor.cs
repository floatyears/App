
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

		playerInfoBar.CreatUI();
		lastDecorator = playerInfoBar;
	}
}

//--------------------------------Quest---------------------------------------
public class QuestDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public QuestDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.None);
	}
		
	public override void HideScene () {
		base.HideScene ();
	}
		
	public override void DestoryScene () {
		base.DestoryScene ();
	}

	public override void DecoratorScene () {

		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		QuestComponent quest = CreatComponent< QuestComponent > ( UIConfig.questWindowName );
		quest.SetComponent ( sceneInfoBar );

		lastDecorator = quest;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Friend---------------------------------------
public class FriendDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public FriendDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		FriendComponent friend = CreatComponent< FriendComponent >( UIConfig.friendWindowName );
		friend.SetComponent( sceneInfoBar );

		lastDecorator = friend;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Scratch--------------------------------------
public class ScratchDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public ScratchDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {

		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		OthersComponent scratch = CreatComponent< OthersComponent >( UIConfig.scratchWindowName );
		scratch.SetComponent( sceneInfoBar );

		lastDecorator = scratch;
		lastDecorator.CreatUI();

	}
}

//--------------------------------Shop-----------------------------------------
public class ShopDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public ShopDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {

		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		ShopComponent shop = CreatComponent< ShopComponent >( UIConfig.shopWindowName );
		shop.SetComponent( sceneInfoBar );

		lastDecorator = shop;
		lastDecorator.CreatUI();

	}
}

//--------------------------------Others---------------------------------------
public class OthersDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public OthersDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		OthersComponent others = CreatComponent< OthersComponent >( UIConfig.othersWindowName );
		others.SetComponent( sceneInfoBar );

		lastDecorator = others;
		others.CreatUI ();

	}
}

//--------------------------------Units----------------------------------------
public class UnitsDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public UnitsDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.None);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		UnitsComponent units = CreatComponent< UnitsComponent >( UIConfig.unitsWindowName );
		units.SetComponent( sceneInfoBar );
	
		lastDecorator = units;
		lastDecorator.CreatUI();
	
	}
}

//--------------------------------Party----------------------------------------
public class PartyDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public PartyDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		PartyComponent party = CreatComponent< PartyComponent >( UIConfig.partyWindowName);
		party.SetComponent( sceneInfoBar );

		lastDecorator = party;
		lastDecorator.CreatUI();
	}
}

//--------------------------------LevelUp----------------------------------------
public class LevelUpDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public LevelUpDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		LevelUpComponent levelUp = CreatComponent< LevelUpComponent >( UIConfig.levelUpWindowName);
		levelUp.SetComponent( sceneInfoBar );

		lastDecorator = levelUp;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Sell------------------------------------------
public class SellDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public SellDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		SellComponent sell = CreatComponent< SellComponent >( UIConfig.sellWindowName);
		sell.SetComponent( sceneInfoBar );

		lastDecorator = sell;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Evolve------------------------------------------
public class EvolveDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public EvolveDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		EvolveComponent evolve = CreatComponent< EvolveComponent >( UIConfig.evolveWindowName);
		evolve.SetComponent( sceneInfoBar );

		lastDecorator = evolve;
		lastDecorator.CreatUI();
	}
}

//--------------------------------Catalog------------------------------------------
public class CatalogDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public CatalogDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		CatalogComponent catalog = CreatComponent< CatalogComponent >( UIConfig.catalogWindowName);
		catalog.SetComponent( sceneInfoBar );
	
		lastDecorator = catalog;
		lastDecorator.CreatUI();
	}
}

//--------------------------------UnitList------------------------------------------
public class UnitListDecorator : DecoratorBase {
	private SceneInfoComponent infoBar;
	public UnitListDecorator(SceneEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene () {
		base.ShowScene ();
		infoBar.SetBackScene(SceneEnum.Units);
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void DecoratorScene () {
		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( decorator );
		infoBar = sceneInfoBar;

		UnitListComponent list = CreatComponent< UnitListComponent >( UIConfig.unitListWindowName);
		list.SetComponent( sceneInfoBar );

		lastDecorator = list;
		lastDecorator.CreatUI();
	}
}
