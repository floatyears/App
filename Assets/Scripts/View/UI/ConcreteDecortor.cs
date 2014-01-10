
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
		
		MenuBottom bottom = CreatComponent<MenuBottom> (UIConfig.menuBottomName);
		bottom.SetComponent (decorator);
		
		Top playerInfoBar = CreatComponent<Top> (UIConfig.topBackgroundName);
		playerInfoBar.SetComponent (bottom);
		
		playerInfoBar.CreatUI ();
		
		lastDecorator = playerInfoBar;
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
		
		
		
	}
}

//--------------------------------QuestSelect----------------------------------
public class QuestSelectDecorator : DecoratorBase {
	
	public QuestSelectDecorator(SceneEnum sEnum) : base(sEnum) { }
	
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
		
		
		
	}
}


