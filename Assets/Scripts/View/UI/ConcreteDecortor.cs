
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

		SceneInfoComponent sceneInfoBar = CreatComponent< SceneInfoComponent >( UIConfig.sceneInfoBarName );
		sceneInfoBar.SetComponent( playerInfoBar );

		sceneInfoBar.CreatUI ();
		
		lastDecorator = sceneInfoBar;
	}
}

//--------------------------------Quest---------------------------------------
public class QuestDecorator : DecoratorBase {

	public QuestDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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

		QuestComponent quest = CreatComponent< QuestComponent > ( UIConfig.questWindowName );
		quest.SetComponent ( decorator );

		quest.CreatUI ();
		lastDecorator = quest;

	}
}

//--------------------------------Friend---------------------------------------
public class FriendDecorator : DecoratorBase {
	
	public FriendDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
		FriendComponent friend = CreatComponent< FriendComponent >( UIConfig.friendWindowName );
		friend.SetComponent( decorator );

		friend.CreatUI ();
		lastDecorator = friend;

		
	}
}

//--------------------------------Scratch--------------------------------------
public class ScratchDecorator : DecoratorBase {
	
	public ScratchDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
		OthersComponent scratch = CreatComponent< OthersComponent >( UIConfig.scratchWindowName );
		scratch.SetComponent( decorator );
		
		scratch.CreatUI ();
		lastDecorator = scratch;
		
		
	}
}

//--------------------------------Shop-----------------------------------------
public class ShopDecorator : DecoratorBase {
	
	public ShopDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
		ShopComponent shop = CreatComponent< ShopComponent >( UIConfig.shopWindowName );
		shop.SetComponent( decorator );
		
		shop.CreatUI ();
		lastDecorator = shop;
		
		
	}
}

//--------------------------------Others---------------------------------------
public class OthersDecorator : DecoratorBase {
	
	public OthersDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
		
		OthersComponent others = CreatComponent< OthersComponent >( UIConfig.othersWindowName );
		others.SetComponent( decorator );
		
		others.CreatUI ();
		lastDecorator = others;
		
	}
}

//--------------------------------Units----------------------------------------
public class UnitsDecorator : DecoratorBase {
	
	public UnitsDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
		
		UnitsComponent units = CreatComponent< UnitsComponent >( UIConfig.unitsWindowName );
		units.SetComponent( decorator );
		
		units.CreatUI ();
		lastDecorator = units;
		
	}
}

//--------------------------------Party----------------------------------------
public class PartyDecorator : DecoratorBase {
	
	public PartyDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
	
		PartyComponent party = CreatComponent< PartyComponent >( UIConfig.partyWindowName);
		party.SetComponent( decorator );

		party.CreatUI();
		lastDecorator = party;
		
	}
}

//--------------------------------LevelUp----------------------------------------
public class LevelUpDecorator : DecoratorBase {
	
	public LevelUpDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
		
		LevelUpComponent levelUp = CreatComponent< LevelUpComponent >( UIConfig.levelUpWindowName);
		levelUp.SetComponent( decorator );
		
		levelUp.CreatUI();
		lastDecorator = levelUp;
		
	}
}

//--------------------------------Sell------------------------------------------
public class SellDecorator : DecoratorBase {
	
	public SellDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
		
		SellComponent sell = CreatComponent< SellComponent >( UIConfig.sellWindowName);
		sell.SetComponent( decorator );
		
		sell.CreatUI();
		lastDecorator = sell;
		
	}
}

//--------------------------------Evolve------------------------------------------
public class EvolveDecorator : DecoratorBase {
	
	public EvolveDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
		
		EvolveComponent evolve = CreatComponent< EvolveComponent >( UIConfig.evolveWindowName);
		evolve.SetComponent( decorator );
		
		evolve.CreatUI();
		lastDecorator = evolve;
		
	}
}

//--------------------------------Catalog------------------------------------------
public class CatalogDecorator : DecoratorBase {
	
	public CatalogDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
		
		CatalogComponent catalog = CreatComponent< CatalogComponent >( UIConfig.catalogWindowName);
		catalog.SetComponent( decorator );
		
		catalog.CreatUI();
		lastDecorator = catalog;
		
	}
}

//--------------------------------UnitList------------------------------------------
public class UnitListDecorator : DecoratorBase {
	
	public UnitListDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
		
		UnitListComponent list = CreatComponent< UnitListComponent >( UIConfig.unitListWindowName);
		list.SetComponent( decorator );
		
		list.CreatUI();
		lastDecorator = list;
		
	}
}
