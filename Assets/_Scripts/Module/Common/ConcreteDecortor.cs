
using UnityEngine;
//--------------------------------Decorator-----------------------------------

//--------------------------------Role Select------------------------------------------
public class SelectRoleScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);
		
		SelectRoleController unitSelect = CreatComponent<SelectRoleController>(UIConfig.selectRoleWindowName);
//		unitSelect.SetComponent(sceneInfoBar);
//		controllerList.Add (unitSelect);
		
//		lastDecorator = unitSelect;
//		lastDecorator.CreatUIAsyn (this);
	}
}

//--------------------------------Start---------------------------------------
public class StartScene : SceneBase {
	private SceneInfoComponent sceneInfoBar;
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
		PlayerInfoBarComponent playerInfoBar = CreatComponent<PlayerInfoBarComponent>(UIConfig.topBackgroundName);
//		playerInfoBar.SetComponent(decorator);
//		controllerList.Add (playerInfoBar);

		BgComponent background = CreatComponent< BgComponent >(UIConfig.HomeBackgroundName);
//		background.SetComponent(playerInfoBar);
//		controllerList.Add (background);
		
		MainMenuController bottom = CreatComponent< MainMenuController >(UIConfig.MainMenuName);
//		bottom.SetComponent(background);
//		controllerList.Add (bottom);

		TipsBarComponent tipsBar = CreatComponent<TipsBarComponent>(UIConfig.TipsBarName);
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
		LoadingLogic background = CreatComponent< LoadingLogic >(UIConfig.loadingWindowName);
//		background.SetComponent(decorator);

		MsgWindowLogic noteWindow = CreatComponent<MsgWindowLogic>(UIConfig.commonNoteWindowName);
//		noteWindow.SetComponent(background);

		NoviceMsgWindowLogic guideWindow = CreatComponent<NoviceMsgWindowLogic>(UIConfig.noviceGuideWindowName);
//		guideWindow.SetComponent(noteWindow);
		
		MaskController maskController = CreatComponent<MaskController>(UIConfig.screenMaskName);
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
//		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);

		QuestController quest = CreatComponent< QuestController >(UIConfig.homeWindowName);
//		quest.SetComponent(decorator);
//		controllerList.Add (quest);

//		lastDecorator = quest;
//		lastDecorator.CreatUIAsyn (this);
	}
}

//--------------------------------Friend---------------------------------------
public class FriendScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);

		FriendComponent friend = CreatComponent< FriendComponent >(UIConfig.friendWindowName);
//		friend.SetComponent(sceneInfoBar);
//		controllerList.Add (friend);

//		lastDecorator = friend;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Scratch--------------------------------------
public class ScratchScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
	public ScratchScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.None);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_SCRATCH));

		if(ModuleEnum.Scratch == currentDecoratorScene)
			NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
	}
	
	public override void HideScene() {
		base.HideScene();
	}
	
	public override void DestoryScene() {
		base.DestoryScene();
	}
	
	public override void InitSceneList() {
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);


        ScratchLogic scratch = CreatComponent< ScratchLogic >(UIConfig.scratchWindowName);
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
		ShowNewCard sn = CreatComponent<ShowNewCard> (UIConfig.showNewCardName);
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
		VicotoryEffectControl sn = CreatComponent<VicotoryEffectControl> (UIConfig.victoryName);
//		controllerList.Add (sn);
//		lastDecorator = sn;
//		lastDecorator.CreatUIAsyn (this);
	}
}

public class GachaWindowScene : SceneBase{
    private SceneInfoComponent sceneInfoBar;
    public GachaWindowScene(ModuleEnum sEnum) : base(sEnum){
		MsgCenter.Instance.AddListener (CommandEnum.ShowGachaWindow, SetKeepState);
	}
    
    public override void ShowScene(){
        base.ShowScene();

//		if (UIManager.Instance.baseScene.PrevScene != ModuleEnum.UnitDetail && UIManager.Instance.baseScene.PrevScene != ModuleEnum.ShowCardEffect) {
//			sceneInfoBar.SetBackScene(ModuleEnum.Scratch);
//		}
	
		//LogHelper.Log ("gacha window decorator:" + currentDecoratorScene);
		if(currentDecoratorScene == ModuleEnum.Scratch)
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
        sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//        sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);

        GachaWindowLogic gachaWin;
        switch (currentDecoratorScene) {
        case ModuleEnum.FriendScratch:
            gachaWin = CreatComponent< FriendGachaWindowLogic >(UIConfig.gachaWindowName);
            break;
        case ModuleEnum.RareScratch:
            gachaWin = CreatComponent< FriendGachaWindowLogic >(UIConfig.gachaWindowName);
            break;
        case ModuleEnum.EventScratch:
            gachaWin = CreatComponent< EventGachaWindowLogic >(UIConfig.gachaWindowName);
            break;
        default:
            gachaWin = CreatComponent< GachaWindowLogic >(UIConfig.gachaWindowName);
            break;
        }
//        gachaWin.SetComponent(sceneInfoBar);
//		controllerList.Add (gachaWin);
//        lastDecorator = gachaWin;
//		lastDecorator.CreatUIAsyn (this);
    }
}

//--------------------------------Shop-----------------------------------------
public class ShopScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);

		ShopComponent shop = CreatComponent< ShopComponent >(UIConfig.shopWindowName);
//		shop.SetComponent(sceneInfoBar);
//		controllerList.Add (shop);

//		lastDecorator = shop;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();

	}
}

//--------------------------------Others---------------------------------------
public class OthersScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);

		OthersComponent others = CreatComponent<OthersComponent>(UIConfig.othersWindowName);
//		others.SetComponent(sceneInfoBar);
//		controllerList.Add (others);

//		lastDecorator = others;
//		lastDecorator.CreatUIAsyn (this);
//		others.CreatUI();

	}
}

//--------------------------------Units----------------------------------------
public class UnitsScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		UnitsController units = CreatComponent< UnitsController >(UIConfig.unitsWindowName);
		//UnitInfoLogic partyInfo = CreatComponent<UnitInfoLogic>(UIConfig.unitsInfoPanelName);

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

	private SceneInfoComponent sceneInfoBar;

	public StageSelectScene(ModuleEnum sEnum) : base(sEnum){
        MsgCenter.Instance.AddListener(CommandEnum.QuestSelectSaveState, SetKeepState);
	}

	public override void ShowScene(){
		base.ShowScene();
//		Debug.LogError ("sceneInfoBar : " + sceneInfoBar);
		sceneInfoBar.SetBackScene(ModuleEnum.Home);
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);

		StageSelectController questSelect = CreatComponent< StageSelectController >(UIConfig.stageWindowName);
//		questSelect.SetComponent(sceneInfoBar);
//		controllerList.Add (questSelect);

//		lastDecorator = questSelect;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------FriendSelect----------------------------------------
public class FriendSelectScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
	
	public FriendSelectScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.QuestSelect);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_FRIEND_SELECT));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);
//		controllerList.Add (sceneInfoBar);
		
		FriendHelperController friendSelect = CreatComponent< FriendHelperController >(UIConfig.friendSelectWindowName);
//		friendSelect.SetComponent(sceneInfoBar);
//		controllerList.Add (friendSelect);

//		lastDecorator = friendSelect;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
		
	}
}

//--------------------------------Party----------------------------------------
public class PartyScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
	public PartyScene(ModuleEnum sEnum) : base(sEnum){
        MsgCenter.Instance.AddListener(CommandEnum.PartySaveState, SetKeepState);
    }
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.Units);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_PARTY));	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.userUnitSortPanelName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
//		PartyInfoLogic partyInfo = CreatComponent<PartyInfoLogic>(UIConfig.partyInfoPanelName);
		PartyPartyPage partyPage = CreatComponent<PartyPartyPage>(UIConfig.PartyWindowName);

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
	private SceneInfoComponent sceneInfoBar;
	public LevelUpScene(ModuleEnum sEnum) : base(sEnum){
//		Debug.LogWarning ("levelup AddListener SetKeepState");
		MsgCenter.Instance.AddListener(CommandEnum.LevelUpSaveState, SetKeepState);
	}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.Units);
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);

		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
//		counter.SetComponent (sceneInfoBar);

		SortController sc = CreatComponent<SortController> (UIConfig.userUnitSortPanelName);
//		sc.SetComponent (counter);

		levelUpOperateUI luou = CreatComponent<levelUpOperateUI> (UIConfig.levelUpView);
//		luou.SetComponent (sc);

		sceneInfoBar.checkUiState = luou;

//		lastDecorator = luou;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}
//--------------------------------Sell------------------------------------------
public class SellScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
	public SellScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
//		Debug.LogError ("SellDecorator ShowScene 1");
		base.ShowScene();
//		Debug.LogError ("SellDecorator ShowScene 2");
        MsgCenter.Instance.AddListener(CommandEnum.SellUnitSaveState, SetKeepState);
		sceneInfoBar.SetBackScene(ModuleEnum.Units);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_UNIT_SELL));	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
		SellController sell = CreatComponent< SellController >(UIConfig.sellWindowName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.userUnitSortPanelName);

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
	private SceneInfoComponent sceneInfoBar;
	public EvolveScene(ModuleEnum sEnum) : base(sEnum){
		MsgCenter.Instance.AddListener (CommandEnum.EvolveSaveState, SetKeepState);
	}
	
	public override void ShowScene(){
//		Debug.Log("EvolveDecorator ShowScene 1");
		base.ShowScene();
//		Debug.Log("EvolveDecorator ShowScene 2 sceneInfoBar : " + sceneInfoBar);
		sceneInfoBar.SetBackScene(ModuleEnum.Units);
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);

//		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
//		counter.SetComponent (sceneInfoBar);

		SortController sc = CreatComponent<SortController> (UIConfig.userUnitSortPanelName);
//		sc.SetComponent (sceneInfoBar);

		EvolveComponent evolve = CreatComponent< EvolveComponent >(UIConfig.evolveWindowName);
//		evolve.SetComponent(sc);

		sceneInfoBar.checkUiState = evolve;

		UnitDisplay unitdisplay = CreatComponent< UnitDisplay >(UIConfig.unitDisplay);
//		unitdisplay.SetComponent (evolve);
//		lastDecorator = unitdisplay;
//		lastDecorator.CreatUIAsyn (this);
		EvolveDecoratorUnity edu = evolve.ViewComponent as EvolveDecoratorUnity;
		edu.SetUnitDisplay (unitdisplay.ViewComponent.gameObject);

	}

//	void EvolveSaveState(object data) {
//		ResetSceneState();
//	}
}

//--------------------------------Catalog------------------------------------------
public class CatalogScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
	public CatalogScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.Units);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_CATALOG));	
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);

		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
//		counter.SetComponent(sceneInfoBar);
		CatalogController catalog = CreatComponent< CatalogController >(UIConfig.catalogWindowName);
//		catalog.SetComponent(counter);
	
//		lastDecorator = catalog;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------UnitList------------------------------------------
public class UnitListScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
	public UnitListScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.Units);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_UNIT_LIST));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
		MyUnitListLogic unitList = CreatComponent< MyUnitListLogic >(UIConfig.unitListWindowName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.userUnitSortPanelName);

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
	private SceneInfoComponent sceneInfoBar;
	public FriendListScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
//		Debug.LogError("ShowScene 1");
		base.ShowScene();
//		Debug.LogError("ShowScene 2");
		sceneInfoBar.SetBackScene(ModuleEnum.Friends);
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
		sceneInfoBar = CreatComponent<SceneInfoComponent>(UIConfig.sceneInfoBarName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);

		SortController sortPanel = CreatComponent<SortController>(UIConfig.friendUnitSortPanelName);

		FriendListUserBriefInfo briefInfo = CreatComponent<FriendListUserBriefInfo>(UIConfig.userBriefInfoWindowName);

		FriendListLogic friendList = CreatComponent<FriendListLogic>(UIConfig.friendListWindowName);

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
	private SceneInfoComponent sceneInfoBar;
	public InformationScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_INFORMATION));
         
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		InformationComponent infoWindow = CreatComponent< InformationComponent >(UIConfig.informationWindowName);

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
	private SceneInfoComponent sceneInfoBar;
	public FriendSearchScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_FRIEND_SEARCH));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		SearchFriendController searchMainUI = CreatComponent< SearchFriendController >(UIConfig.searchMainWindowName);
		RequestFriendApply requestApply = CreatComponent<RequestFriendApply>(UIConfig.applyMessageWindowName);

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
	private SceneInfoComponent sceneInfoBar;
	public ApplyScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_APPLY));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.friendUnitSortPanelName);
		ApplyController applyWindow = CreatComponent< ApplyController >(UIConfig.applyWindowName);
		DeleteFriendApply deleteApply = CreatComponent<DeleteFriendApply>(UIConfig.applyMessageWindowName);

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
	private SceneInfoComponent sceneInfoBar;
	public ReceptionScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
//		Debug.LogError ("ReceptionDecorator ShowScene 1");
		base.ShowScene();
//		Debug.LogError ("ReceptionDecorator ShowScene 2");
		sceneInfoBar.SetBackScene(ModuleEnum.Friends);
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		SortController sortPanel = CreatComponent<SortController>(UIConfig.friendUnitSortPanelName);
		ItemCounterController counter = CreatComponent<ItemCounterController>(UIConfig.itemCounterBarName);
		ReceptionController recptionWin = CreatComponent< ReceptionController >(UIConfig.receptionWindowName);
		AccpetFriendApply acceptApply = CreatComponent<AccpetFriendApply>(UIConfig.acceptApplyMessageWindowName);

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
	private SceneInfoComponent sceneInfoBar;
	public UserIDScene(ModuleEnum sEnum) : base(sEnum){ }
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.Friends);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_YOUR_ID));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		UserIDComponent userIDWindow = CreatComponent< UserIDComponent >(UIConfig.userIDWindowName);

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
	private UnitDetailComponent unitDetail;
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

//		UnitDetailTopComponent unitDetailTop = CreatComponent< UnitDetailTopComponent >(UIConfig.unitDetailTopPanelName);
//		unitDetailTop.SetComponent(decorator);
//
//		UnitDetailCenterComponent unitDetailCenter = CreatComponent<UnitDetailCenterComponent> (UIConfig.unitDetailCenterPanelName);
//		unitDetailCenter.SetComponent (unitDetailTop);

		unitDetail = CreatComponent< UnitDetailComponent >(UIConfig.unitDetailPanelName);
//		unitDetail.SetComponent(unitDetailCenter);
//
//		lastDecorator = unitDetail;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}

}

//--------------------------------Result------------------------------------------
public class ResultScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
//		sceneInfoBar.SetComponent(decorator);
		
		ResultController resultWindow = CreatComponent<ResultController>(UIConfig.resultWindowName);
//		resultWindow.SetComponent(sceneInfoBar);
		
//		lastDecorator = resultWindow;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Fight Ready------------------------------------------
public class FightReadyScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
	public FightReadyScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.FriendSelect);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_FIGHT_READY));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		FightReadyController standByWindow = CreatComponent<FightReadyController>(UIConfig.fightReadyWindowName);

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
	private SceneInfoComponent sceneInfoBar;
	public QuestSelectScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.StageSelect);
//		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_QUEST_SELECT));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		QuestSelectController questSelect = CreatComponent<QuestSelectController>(UIConfig.questSelectWindowName);
		
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
	private SceneInfoComponent sceneInfoBar;
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
//		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		OperationNoticeComponent operationNotice = CreatComponent<OperationNoticeComponent>(UIConfig.operationNoticeWindowName);
		
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
	private SceneInfoComponent sceneInfoBar;
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		RewardComponent reward = CreatComponent<RewardComponent>(UIConfig.rewardViewName);
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
	private SceneInfoComponent sceneInfoBar;
	public GameRaiderScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.Others);
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
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		GameRaiderComponent raider = CreatComponent<GameRaiderComponent>(UIConfig.gameRaiderWindowName);
		
//		sceneInfoBar.SetComponent(decorator);
//		raider.SetComponent (sceneInfoBar);
//		
//		lastDecorator = raider;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

//--------------------------------Raider------------------------------------------
public class GameCurrencyScene : SceneBase{
	private SceneInfoComponent sceneInfoBar;
	public GameCurrencyScene(ModuleEnum sEnum) : base(sEnum){}
	
	public override void ShowScene(){
		base.ShowScene();
		sceneInfoBar.SetBackScene(ModuleEnum.Home);
		sceneInfoBar.SetCurSceneName(TextCenter.GetText(TextConst.SCENE_NAME_CURRENCY));
	}
	
	public override void HideScene(){
		base.HideScene();
	}
	
	public override void DestoryScene(){
		base.DestoryScene();
	}
	
	public override void InitSceneList(){
		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		GameCurrencyComponent currency = CreatComponent<GameCurrencyComponent>(UIConfig.gameCurrencyWindowName);
		
//		sceneInfoBar.SetComponent(decorator);
//		currency.SetComponent (sceneInfoBar);
//		
//		lastDecorator = currency;
//		lastDecorator.CreatUIAsyn (this);
//		lastDecorator.CreatUI();
	}
}

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
//		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		MusicComponent currency = CreatComponent<MusicComponent>(UIConfig.settingWindowName);
		
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
//		sceneInfoBar = CreatComponent< SceneInfoComponent >(UIConfig.sceneInfoBarName);
		NicknameComponent currency = CreatComponent<NicknameComponent>(UIConfig.nicknameWindowName);
		
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
		PrefaceComponent preface = CreatComponent<PrefaceComponent>(UIConfig.prefaceWindowName);

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
		ResourceDownloadComponent rsdownload = CreatComponent<ResourceDownloadComponent>(UIConfig.resourceDownloadWindowName);
		
//		rsdownload.SetComponent (decorator);
//		
//		lastDecorator = rsdownload;
//		lastDecorator.CreatUIAsyn (this);
	}
}
