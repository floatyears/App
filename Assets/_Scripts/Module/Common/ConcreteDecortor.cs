
using UnityEngine;
//--------------------------------Decorator-----------------------------------

//--------------------------------Role Select------------------------------------------
public class SelectRoleScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public SelectRoleScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_ROLE_SELECT));
//		NoviceGuideStepEntityManager.Instance ().StartStep ();
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.SceneInfoBarModule);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);
		
		SelectRoleModule unitSelect = AddModuleToScene<SelectRoleModule>(ModuleEnum.SelectRoleModule);
//		unitSelect.SetComponent(sceneInfoBar);
//		controllerList.Add (unitSelect);
		
//		lastDecorator = unitSelect;
//		lastDecorator.CreatUIAsyn (this);
	}
}

//--------------------------------Start---------------------------------------
public class StartScene : SceneBase {
	private SceneInfoBarModule sceneInfoBar;
	public StartScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
//		Debug.LogError("start decorator show scene");
		base.ShowScene();
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		PlayerInfoBarModule playerInfoBar = AddModuleToScene<PlayerInfoBarModule>(ModuleEnum.PlayerInfoBarModule);
//		playerInfoBar.SetComponent(decorator);
//		controllerList.Add (playerInfoBar);

		BgComponent background = AddModuleToScene< BgComponent >(ModuleEnum.MainBackgroundModule);
//		background.SetComponent(playerInfoBar);
//		controllerList.Add (background);
		
		MainMenuModule bottom = AddModuleToScene< MainMenuModule >(ModuleEnum.MainMenuModule);
//		bottom.SetComponent(background);
//		controllerList.Add (bottom);

		TipsBarComponent tipsBar = AddModuleToScene<TipsBarComponent>(ModuleEnum.TipsBarModule);
//		tipsBar.SetComponent(bottom);
//		controllerList.Add (tipsBar);

//		lastDecorator = tipsBar;
//		lastDecorator.CreatUIAsyn (this);
	}
}

//--------------------------------Loading---------------------------------------
public class LoadingDecorator : SceneBase{
    public LoadingDecorator(ModuleEnum sEnum) : base(sEnum){}
    
    public override void ShowScene(){
        base.ShowScene();
    }
    
    public override void HideScene(){
        base.HideScene();
    }
    
    public override void DestoryScene(){
        base.DestoryScene();
    }
    
    public override void InitSceneList(){
		LoadingModule background = AddModuleToScene< LoadingModule >(ModuleEnum.LoadingModule);
//		background.SetComponent(decorator);

//		MsgWindowLogic noteWindow = AddModuleToScene<MsgWindowLogic>(ModuleEnum.);
//		noteWindow.SetComponent(background);

//		NoviceMsgWindowLogic guideWindow = AddModuleToScene<NoviceMsgWindowLogic>(ModuleEnum.noviceGuideWindowName);
//		guideWindow.SetComponent(noteWindow);
		
//		MaskController maskController = AddModuleToScene<MaskController>(ModuleEnum.screenMaskName);
//		maskController.SetComponent(guideWindow);

//		controllerList.Add (maskController);
		
//		lastDecorator = maskController;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator = background;
//		lastDecorator.CreatUIAsyn(lastDecorator);
    }
}


//--------------------------------Home---------------------------------------
public class HomeScene : SceneBase{
//	private SceneInfoComponent sceneInfoBar;
	public HomeScene(ModuleEnum sEnum) : base(sEnum) { }
	
	public override void ShowScene() {
//		sceneInfoBar.SetBackScene(ModuleEnum.None);
//		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_HOME));
		base.ShowScene();
	}
		
	public override void HideScene(){
		base.HideScene();
	}
		
	public override void DestoryScene(){
		base.DestoryScene();
	}

	public override void InitSceneList(){
//		sceneInfoBar = CreatComponent< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);

		QuestModule quest = AddModuleToScene< QuestModule >(ModuleEnum.QuestSelectModule);
//		quest.SetComponent(decorator);
//		controllerList.Add (quest);

//		lastDecorator = quest;
//		lastDecorator.CreatUIAsyn (this);
	}
}

//--------------------------------Friend---------------------------------------
public class FriendScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public FriendScene(ModuleEnum sEnum) : base(sEnum){ }
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.None);
		sceneInfoBar.SetCurSceneName( TextCenter.GetText( TextConst.SCENE_NAME_FRIEND ) );
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.SceneInfoBarModule);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);

		FriendModule friend = AddModuleToScene< FriendModule >(ModuleEnum.FriendsModule);
//		friend.SetComponent(sceneInfoBar);
//		controllerList.Add (friend);

//		lastDecorator = friend;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Scratch--------------------------------------
public class ScratchScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public ScratchScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_SCRATCH));

		if(ModuleEnum.ScratchModule == currentDecoratorScene)
			NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
	}
	
	public override void HideScene() {
		base.HideScene();
	}
	
	public override void DestoryScene() {
		base.DestoryScene();
	}
	
	public override void InitSceneList() {
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.SceneInfoBarModule);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);


		ScratchModule scratch = AddModuleToScene< ScratchModule >(ModuleEnum.ScratchModule);
//		scratch.SetComponent(sceneInfoBar);
//		controllerList.Add (scratch);

//        lastDecorator = scratch;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

public class ShowNewCardScene : SceneBase {
	public ShowNewCardScene(ModuleEnum sEnum) : base(sEnum) { }

	public override void ShowScene () {
		base.ShowScene ();
	}

	public override void HideScene () {
		base.HideScene ();
	}

	public override void DestoryScene () {
		base.DestoryScene ();
	}

	public override void InitSceneList () {
		ShowNewCardModule sn = AddModuleToScene<ShowNewCardModule> (ModuleEnum.ShowCardEffectModule);
//		controllerList.Add (sn);

//		lastDecorator = sn;
//		lastDecorator.CreatUIAsyn (this);
	}
}

public class VictoryScene : SceneBase {
	public VictoryScene(ModuleEnum sEnum) : base (sEnum) { }

	public override void ShowScene () {
		base.ShowScene ();
	}
	
	public override void HideScene () {
		base.HideScene ();
	}
	
	public override void DestoryScene () {
		base.DestoryScene ();
	}
	
	public override void InitSceneList () {
		VicotoryEffectControl sn = AddModuleToScene<VicotoryEffectControl> (ModuleEnum.VictoryModule);
//		controllerList.Add (sn);
//		lastDecorator = sn;
//		lastDecorator.CreatUIAsyn (this);
	}
}

public class GachaWindowScene : SceneBase{
    private SceneInfoBarModule sceneInfoBar;
    public GachaWindowScene(ModuleEnum sEnum) : base(sEnum){
		MsgCenter.Instance.AddListener (CommandEnum.ShowGachaWindow, SetKeepState);
	}
    
    public override void ShowScene(){
        base.ShowScene();

//		if (UIManager.Instance.baseScene.PrevScene != ModuleEnum.UnitDetail && UIManager.Instance.baseScene.PrevScene != ModuleEnum.ShowCardEffect) {
//			sceneInfoBar.SetBackScene(ModuleEnum.Scratch);
//		}
	
		//LogHelper.Log ("gacha window decorator:" + currentDecoratorScene);
		if(currentDecoratorScene == ModuleEnum.ScratchModule)
			NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
    }
    
    public override void HideScene(){
        base.HideScene();
    }
    
    public override void DestoryScene(){
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowGachaWindow, SetKeepState);
        base.DestoryScene();
    }
    
    public override void InitSceneList(){ 
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
//        sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);

        GachaModule gachaWin;
//        switch () {
//        case ModuleEnum.FightReadyModule:
			gachaWin = AddModuleToScene< FriendGachaWindowModule >(currentDecoratorScene);
//            break;
//        case ModuleEnum.RareScratchModule:
//            gachaWin = AddModuleToScene< FriendGachaWindowLogic >(ModuleEnum.gachaWindowName);
//            break;
//        case ModuleEnum.EventScratchModule:
//            gachaWin = AddModuleToScene< EventGachaWindowLogic >(ModuleEnum.gachaWindowName);
//            break;
//        default:
//            gachaWin = AddModuleToScene< GachaWindowLogic >(ModuleEnum.gachaWindowName);
//            break;
//        }
//        gachaWin.SetComponent(sceneInfoBar);
//		controllerList.Add (gachaWin);
//        lastDecorator = gachaWin;
//		lastDecorator.CreatUIAsyn (this);
    }
}

//--------------------------------Shop-----------------------------------------
public class ShopScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public ShopScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_SHOP));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.s);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);

		ShopModule shop = AddModuleToScene< ShopModule >(ModuleEnum.ShopModule);
//		shop.SetComponent(sceneInfoBar);
//		controllerList.Add (shop);

//		lastDecorator = shop;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();

	}
}

//--------------------------------Others---------------------------------------
public class OthersScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public OthersScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_OTHERS));	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);

		OthersModule others = AddModuleToScene<OthersModule>(ModuleEnum.OthersModule);
//		others.SetComponent(sceneInfoBar);
//		controllerList.Add (others);

//		lastDecorator = others;
//		lastDecorator.CreatUIAsyn (this);
//		others.CreatUI();

	}
}

//--------------------------------Units----------------------------------------
public class UnitsScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public UnitsScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_UNITS));	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		UnitsModule units = AddModuleToScene< UnitsModule >(ModuleEnum.UnitsModule);
		//UnitInfoLogic partyInfo = CreatComponent<UnitInfoLogic>(ModuleEnum.unitsInfoPanelName);

//		sceneInfoBar.SetComponent(decorator);
		//partyInfo.SetComponent(sceneInfoBar);
//		units.SetComponent(sceneInfoBar);
//		controllerList.Add (sceneInfoBar);
//		controllerList.Add (units);
//
//		lastDecorator = units;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------StageSelect----------------------------------------
public class StageSelectScene : SceneBase{

	private SceneInfoBarModule sceneInfoBar;

	public StageSelectScene(ModuleEnum sEnum) : base(sEnum){
        MsgCenter.Instance.AddListener(CommandEnum.QuestSelectSaveState, SetKeepState);
	}

	public override void ShowScene(){
		base.ShowScene();
//		Debug.LogError ("sceneInfoBar : " + sceneInfoBar);
		sceneInfoBar.SetBackScene(ModuleEnum.HomeModule);
//		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_STAGE_SELECT));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		MsgCenter.Instance.RemoveListener(CommandEnum.QuestSelectSaveState, SetKeepState);
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);

		StageSelectModule questSelect = AddModuleToScene< StageSelectModule >(ModuleEnum.StageSelectModule);
//		questSelect.SetComponent(sceneInfoBar);
//		controllerList.Add (questSelect);

//		lastDecorator = questSelect;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------FriendSelect----------------------------------------
public class FriendSelectScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	
	public FriendSelectScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.QuestSelectModule);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_FRIEND_SELECT));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);
		
		FriendHelperController friendSelect = AddModuleToScene< FriendHelperController >(ModuleEnum.FriendSelectModule);
//		friendSelect.SetComponent(sceneInfoBar);
//		controllerList.Add (friendSelect);

//		lastDecorator = friendSelect;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
		
	}
}

//--------------------------------Party----------------------------------------
public class PartyScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public PartyScene(ModuleEnum sEnum) : base(sEnum){
        MsgCenter.Instance.AddListener(CommandEnum.PartySaveState, SetKeepState);
    }
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.UnitsModule);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_PARTY));	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		SortController sortPanel = AddModuleToScene<SortController>(ModuleEnum.SortModule);
		ItemCounterModule counter = AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
//		PartyInfoLogic partyInfo = CreatComponent<PartyInfoLogic>(ModuleEnum.partyInfoPanelName);
		PartyPartyPage partyPage = AddModuleToScene<PartyPartyPage>(ModuleEnum.PartyModule);

//		sceneInfoBar.SetComponent(decorator);
//		sortPanel.SetComponent(sceneInfoBar);
//		partyInfo.SetComponent(sortPanel);
//		counter.SetComponent(sortPanel);
//		partyPage.SetComponent(counter);
//		controllerList.Add (sceneInfoBar);
//		controllerList.Add (sortPanel);

	
//		lastDecorator = partyPage;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------LevelUp----------------------------------------
public class LevelUpScene : SceneBase {
	private SceneInfoBarModule sceneInfoBar;
	public LevelUpScene(ModuleEnum sEnum) : base(sEnum){
//		Debug.LogWarning ("levelup AddListener SetKeepState");
		MsgCenter.Instance.AddListener(CommandEnum.LevelUpSaveState, SetKeepState);
	}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.UnitsModule);
//		Debug.LogError ("LevelUpDecorator show scene : " + sceneInfoBar.backScene);
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
	
	public override void InitSceneList() {
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);

		ItemCounterModule counter = AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
//		counter.SetComponent (sceneInfoBar);

		SortController sc = AddModuleToScene<SortController> (ModuleEnum.SortModule);
//		sc.SetComponent (counter);

		LevelUpModule luou = AddModuleToScene<LevelUpModule> (ModuleEnum.LevelUpModule);
//		luou.SetComponent (sc);

//		sceneInfoBar.checkUiState = luou;

//		lastDecorator = luou;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}
//--------------------------------Sell------------------------------------------
public class SellScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public SellScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
//		Debug.LogError ("SellDecorator ShowScene 1");
		base.ShowScene();
//		Debug.LogError ("SellDecorator ShowScene 2");
        MsgCenter.Instance.AddListener(CommandEnum.SellUnitSaveState, SetKeepState);
		sceneInfoBar.SetBackScene(ModuleEnum.UnitsModule);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_UNIT_SELL));	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		ItemCounterModule counter = AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
		SellModule sell = AddModuleToScene< SellModule >(ModuleEnum.SellModule);
		SortController sortPanel = AddModuleToScene<SortController>(ModuleEnum.SortModule);

//		sceneInfoBar.SetComponent(decorator);
//		counter.SetComponent(sceneInfoBar);
//		sortPanel.SetComponent(counter);
//		sell.SetComponent(sortPanel);

//		lastDecorator = sell;
//		lastDecorator.CreatUIAsyn (this);
	}
}

//--------------------------------Evolve------------------------------------------
public class EvolveScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public EvolveScene(ModuleEnum sEnum) : base(sEnum){
		MsgCenter.Instance.AddListener (CommandEnum.EvolveSaveState, SetKeepState);
	}
	
	public override void ShowScene(){
//		Debug.Log("EvolveDecorator ShowScene 1");
		base.ShowScene();
//		Debug.Log("EvolveDecorator ShowScene 2 sceneInfoBar : " + sceneInfoBar);
		sceneInfoBar.SetBackScene(ModuleEnum.UnitsModule);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_EVOLVE));
//		Debug.Log("EvolveDecorator ShowScene 3");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		MsgCenter.Instance.RemoveListener (CommandEnum.EvolveSaveState, SetKeepState);
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);

//		ItemCounterController counter = CreatComponent<ItemCounterController>(ModuleEnum.itemCounterBarName);
//		counter.SetComponent (sceneInfoBar);

		SortController sc = AddModuleToScene<SortController> (ModuleEnum.SortModule);
//		sc.SetComponent (sceneInfoBar);

		EvolveModule evolve = AddModuleToScene< EvolveModule >(ModuleEnum.EvolveModule);
//		evolve.SetComponent(sc);

//		sceneInfoBar.checkUiState = evolve;

		UnitDisplayModule unitdisplay = AddModuleToScene< UnitDisplayModule >(ModuleEnum.UnitsModule);
//		unitdisplay.SetComponent (evolve);
//		lastDecorator = unitdisplay;
//		lastDecorator.CreatUIAsyn (this);
		EvolveView edu = evolve.View as EvolveView;
		edu.SetUnitDisplay (unitdisplay.View.gameObject);

	}

//	void EvolveSaveState(object data) {
//		ResetSceneState();
//	}
}

//--------------------------------Catalog------------------------------------------
public class CatalogScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public CatalogScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.UnitsModule);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_CATALOG));	
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);

		ItemCounterModule counter = AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
//		counter.SetComponent(sceneInfoBar);
		CatalogModule catalog = AddModuleToScene< CatalogModule >(ModuleEnum.UnitCatalogModule);
//		catalog.SetComponent(counter);
	
//		lastDecorator = catalog;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------UnitList------------------------------------------
public class UnitListScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public UnitListScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.UnitsModule);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_UNIT_LIST));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.SceneInfoBarModule);
		ItemCounterModule counter = AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
		MyUnitListModule unitList = AddModuleToScene< MyUnitListModule >(ModuleEnum.UnitListModule);
		SortController sortPanel = AddModuleToScene<SortController>(ModuleEnum.SortModule);

//		sceneInfoBar.SetComponent(decorator);
//		counter.SetComponent(sceneInfoBar);
//		sortPanel.SetComponent(counter);
//		unitList.SetComponent(sortPanel);
//
//		lastDecorator = unitList;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------FriendList------------------------------------------
public class FriendListScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public FriendListScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
//		Debug.LogError("ShowScene 1");
		base.ShowScene();
//		Debug.LogError("ShowScene 2");
		sceneInfoBar.SetBackScene(ModuleEnum.FriendsModule);
//		Debug.LogError("ShowScene 3");
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_FRIEND_LIST));
//		Debug.LogError("ShowScene 4");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene<SceneInfoComponent>(ModuleEnum.sceneInfoBarName);
		ItemCounterModule counter = AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);

		SortController sortPanel = AddModuleToScene<SortController>(ModuleEnum.SortModule);

//		FriendListUserBriefInfo briefInfo = AddModuleToScene<FriendListUserBriefInfo>(ModuleEnum.friend);

		FriendListModule friendList = AddModuleToScene<FriendListModule>(ModuleEnum.FriendListModule);

//		sceneInfoBar.SetComponent(decorator);
//		counter.SetComponent(sceneInfoBar);
//		sortPanel.SetComponent(counter);
//		briefInfo.SetComponent(sortPanel);
//		friendList.SetComponent(briefInfo);
//
//		lastDecorator = friendList;
//		lastDecorator.CreatUIAsyn (this);
	}
}

//--------------------------------Information------------------------------------------
public class InformationScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public InformationScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.FriendsModule);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_INFORMATION));
         
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		InformationModule infoWindow = AddModuleToScene< InformationModule >(ModuleEnum.InformationModule);

//		sceneInfoBar.SetComponent(decorator);
//		infoWindow.SetComponent(sceneInfoBar);
//
//		lastDecorator = infoWindow;
//		lastDecorator.CreatUIAsyn (this);
////		lastDecorator.CreatUI();

	}
}

//--------------------------------Friend Search------------------------------------------
public class FriendSearchScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public FriendSearchScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.FriendsModule);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_FRIEND_SEARCH));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		SearchFriendModule searchMainUI = AddModuleToScene< SearchFriendModule >(ModuleEnum.SearchFriendModule);
		RequestFriendApply requestApply = AddModuleToScene<RequestFriendApply>(ModuleEnum.RequestFriendModule);

//		sceneInfoBar.SetComponent(decorator);
//		searchMainUI.SetComponent(sceneInfoBar);
//
//		requestApply.SetComponent(searchMainUI);
//
//		lastDecorator = requestApply;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Apply------------------------------------------
public class ApplyScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public ApplyScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.FriendsModule);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_APPLY));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		ItemCounterModule counter = AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
		SortController sortPanel = AddModuleToScene<SortController>(ModuleEnum.SortModule);
		ApplyModule applyWindow = AddModuleToScene< ApplyModule >(ModuleEnum.ApplyModule);
		DeleteFriendApply deleteApply = AddModuleToScene<DeleteFriendApply>(ModuleEnum.DeleteFriendModule);

//		sceneInfoBar.SetComponent(decorator);
//		counter.SetComponent(sceneInfoBar);
//		sortPanel.SetComponent(counter);
//		applyWindow.SetComponent(counter);
//		deleteApply.SetComponent(applyWindow);
//
//		lastDecorator = deleteApply;
//		lastDecorator.CreatUIAsyn (this);
	}
}

//--------------------------------Reception------------------------------------------
public class ReceptionScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public ReceptionScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
//		Debug.LogError ("ReceptionDecorator ShowScene 1");
		base.ShowScene();
//		Debug.LogError ("ReceptionDecorator ShowScene 2");
		sceneInfoBar.SetBackScene(ModuleEnum.FriendsModule);
//		Debug.LogError ("ReceptionDecorator ShowScene 3");
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_RECEPTION));
//		Debug.LogError ("ReceptionDecorator ShowScene 4");
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		SortController sortPanel = AddModuleToScene<SortController>(ModuleEnum.SortModule);
		ItemCounterModule counter = AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
		ReceptionModule recptionWin = AddModuleToScene< ReceptionModule >(ModuleEnum.ReceptionModule);
		AccpetFriendApply acceptApply = AddModuleToScene<AccpetFriendApply>(ModuleEnum.AccpetFriendApplyModule);

//		sceneInfoBar.SetComponent(decorator);
//
//		counter.SetComponent(sceneInfoBar);
//		sortPanel.SetComponent(counter);
//		recptionWin.SetComponent(sortPanel);
//		acceptApply.SetComponent(recptionWin);
//
//		lastDecorator = acceptApply;
//		lastDecorator.CreatUIAsyn (this);
	}
}

//--------------------------------YourID------------------------------------------
public class UserIDScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public UserIDScene(ModuleEnum sEnum) : base(sEnum){ }
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.FriendsModule);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_YOUR_ID));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		UserIDModule userIDWindow = AddModuleToScene< UserIDModule >(ModuleEnum.UserIDModule);

//		sceneInfoBar.SetComponent(decorator);
//		userIDWindow.SetComponent(sceneInfoBar);
//
//		lastDecorator = userIDWindow;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}



//--------------------------------UnitDetail------------------------------------------
public class UnitDetailScene : SceneBase{
	private UnitDetailModule unitDetail;
	public UnitDetailScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
//		sceneInfoBar.SetBackScene(ModuleEnum.LevelUp);
//		sceneInfoBar.SetCurSceneName(TextCenter.Instace.GetCurrentText(TextConst.SCENE_NAME_UNIT_DETAIL));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){

//		UnitDetailTopComponent unitDetailTop = CreatComponent< UnitDetailTopComponent >(ModuleEnum.unitDetailTopPanelName);
//		unitDetailTop.SetComponent(decorator);
//
//		UnitDetailCenterComponent unitDetailCenter = CreatComponent<UnitDetailCenterComponent> (ModuleEnum.unitDetailCenterPanelName);
//		unitDetailCenter.SetComponent (unitDetailTop);

		unitDetail = AddModuleToScene< UnitDetailModule >(ModuleEnum.UnitDetailModule);
//		unitDetail.SetComponent(unitDetailCenter);
//
//		lastDecorator = unitDetail;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}

}

//--------------------------------Result------------------------------------------
public class ResultScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public ResultScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_RESULT));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.SceneInfoBarModule);
//		sceneInfoBar.SetComponent(decorator);
		
		ResultModule resultWindow = AddModuleToScene<ResultModule>(ModuleEnum.ResultModule);
//		resultWindow.SetComponent(sceneInfoBar);
		
//		lastDecorator = resultWindow;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Fight Ready------------------------------------------
public class FightReadyScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public FightReadyScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.FriendSelectModule);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_FIGHT_READY));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		FightReadyModule standByWindow = AddModuleToScene<FightReadyModule>(ModuleEnum.FightReadyModule);

//		sceneInfoBar.SetComponent(decorator);
//		standByWindow.SetComponent(sceneInfoBar);
//
//		lastDecorator = standByWindow;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}


//--------------------------------Quest Select------------------------------------------
public class QuestSelectScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public QuestSelectScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.StageSelectModule);
//		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_QUEST_SELECT));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		QuestSelectModule questSelect = AddModuleToScene<QuestSelectModule>(ModuleEnum.QuestSelectModule);
		
//		sceneInfoBar.SetComponent(decorator);
//		questSelect.SetComponent(sceneInfoBar);
//		
//		lastDecorator = questSelect;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Operation Notice------------------------------------------
public class OperationNoticeScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public OperationNoticeScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
//		sceneInfoBar.SetBackScene(ModuleEnum.Home);
//		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_OPERATION_NOTICE));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = CreatComponent< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		OperationNoticeModule operationNotice = AddModuleToScene<OperationNoticeModule>(ModuleEnum.OperationNoticeModule);
		
//		sceneInfoBar.SetComponent(decorator);
//		operationNotice.SetComponent (decorator);//sceneInfoBar);
//		
//		lastDecorator = operationNotice;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Reward------------------------------------------
public class RewardScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public RewardScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
//		sceneInfoBar.SetBackScene (UIManager.Instance.baseScene.PrevScene);//ModuleEnum.Home);
		LogHelper.Log ("reward scene name: " + TextCenter.GetText(TextConst.SCENE_NAME_REWARD));
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_REWARD));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		RewardModule reward = AddModuleToScene<RewardModule>(ModuleEnum.RewardModule);
//		
//		sceneInfoBar.SetComponent(decorator);
//		reward.SetComponent (sceneInfoBar);
//		
//		lastDecorator = reward;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Raider------------------------------------------
public class GameRaiderScene : SceneBase{
	private SceneInfoBarModule sceneInfoBar;
	public GameRaiderScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.OthersModule);
		//LogHelper.Log ("reward scene name: " + TextCenter.GetText(TextConst.SCENE_NAME_REWARD));
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_RAIDER));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sc);
		GameRaiderModule raider = AddModuleToScene<GameRaiderModule>(ModuleEnum.GameRaiderModule);
		
//		sceneInfoBar.SetComponent(decorator);
//		raider.SetComponent (sceneInfoBar);
//		
//		lastDecorator = raider;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

////--------------------------------Raider------------------------------------------
//public class GameCurrencyScene : SceneBase{
//	private SceneInfoComponent sceneInfoBar;
//	public GameCurrencyScene(ModuleEnum sEnum) : base(sEnum){}
//	
//	public override void ShowScene(){
//		base.ShowScene();
//		sceneInfoBar.SetBackScene(ModuleEnum.HomeModule);
//		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_CURRENCY));
//	}
//	
//	public override void HideScene(){
//		base.HideScene();
//	}
//	
//	public override void DestoryScene(){
//		base.DestoryScene();
//	}
//	
//	public override void InitSceneList(){
////		sceneInfoBar = AddModuleToScene< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
//		GameCurrencyComponent currency = AddModuleToScene<GameCurrencyComponent>(ModuleEnum.ga);
//		
////		sceneInfoBar.SetComponent(decorator);
////		currency.SetComponent (sceneInfoBar);
////		
////		lastDecorator = currency;
////		lastDecorator.CreatUIAsyn (this);
////		lastDecorator.CreatUI();
//	}
//}

//--------------------------------Music------------------------------------------
public class MusicScene : SceneBase{
//	private SceneInfoComponent sceneInfoBar;
	public MusicScene(ModuleEnum sEnum) : base(sEnum){
	}
	
	public override void ShowScene(){
		base.ShowScene();
//		sceneInfoBar.SetBackScene(ModuleEnum.Others);
//		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_MUSIC));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = CreatComponent< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		MusicModule currency = AddModuleToScene<MusicModule>(ModuleEnum.MusicModule);
		
//		sceneInfoBar.SetComponent(decorator);
//		currency.SetComponent (decorator);
//		
//		lastDecorator = currency;
//		lastDecorator.CreatUIAsyn (this);
//		//		lastDecorator.CreatUI();
	}
}

//--------------------------------Raider------------------------------------------
public class NicknameScene : SceneBase{
//	private SceneInfoComponent sceneInfoBar;
	public NicknameScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
//		sceneInfoBar.SetBackScene(ModuleEnum.Others);
//		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_Nickname));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
//		sceneInfoBar = CreatComponent< SceneInfoComponent >(ModuleEnum.sceneInfoBarName);
		NicknameModule currency = AddModuleToScene<NicknameModule>(ModuleEnum.NickNameModule);
		
//		sceneInfoBar.SetComponent(decorator);
//		currency.SetComponent (decorator);
//		
//		lastDecorator = currency;
//		lastDecorator.CreatUIAsyn (this);
		//		lastDecorator.CreatUI();
	}
}

	//--------------------------------Preface------------------------------------------
public class PrefaceScene : SceneBase{
	//	private SceneInfoComponent sceneInfoBar;
	public PrefaceScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();

	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		PrefaceModule preface = AddModuleToScene<PrefaceModule>(ModuleEnum.PrefaceModule);

//		preface.SetComponent (decorator);
//		
//		lastDecorator = preface;
//		lastDecorator.CreatUIAsyn (this);
	}
}

//--------------------------------ResourceDownload------------------------------------------
public class ResourceDownloadScene : SceneBase{
	//	private SceneInfoComponent sceneInfoBar;
	public ResourceDownloadScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		ResourceDownloadModule rsdownload = AddModuleToScene<ResourceDownloadModule>(ModuleEnum.ResourceDownloadModule);
		
//		rsdownload.SetComponent (decorator);
//		
//		lastDecorator = rsdownload;
//		lastDecorator.CreatUIAsyn (this);
	}
}
