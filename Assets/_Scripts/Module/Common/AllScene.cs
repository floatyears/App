using UnityEngine;

//--------------------------------Start---------------------------------------
public class MainScene : SceneBase {
	public MainScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.MAIN;
	}
	
	protected override void InitSceneList(){

		AddModuleToScene< PlayerInfoBarModule >(ModuleEnum.PlayerInfoBarModule);

		AddModuleToScene< MainMenuModule >(ModuleEnum.MainMenuModule);
	}
}


//--------------------------------Party----------------------------------------
public class PartyScene : SceneBase{
	public PartyScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.DEFAULT;
    }
	
	protected override void InitSceneList(){
		AddModuleToScene<SortController>(ModuleEnum.SortModule);
		AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
		AddModuleToScene<PartyModule>(ModuleEnum.PartyModule);
	}
}

//--------------------------------LevelUp----------------------------------------
public class LevelUpScene : SceneBase {
	public LevelUpScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.DEFAULT;
	}
	
	protected override void InitSceneList() {

		AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);

		AddModuleToScene<SortController> (ModuleEnum.SortModule);

		AddModuleToScene<LevelUpModule> (ModuleEnum.LevelUpModule);
	}
}
//--------------------------------Sell------------------------------------------
public class SellScene : SceneBase{
	public SellScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.DEFAULT;
	}

	protected override void InitSceneList(){
		AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
		AddModuleToScene< SellUnitModule >(ModuleEnum.SellUnitModule);
		AddModuleToScene<SortController>(ModuleEnum.SortModule);
	}
}

//--------------------------------Evolve------------------------------------------
public class EvolveScene : SceneBase{

	public EvolveScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.DEFAULT;
	}

	protected override void InitSceneList(){

		AddModuleToScene<SortController> (ModuleEnum.SortModule);


//		EvolveModule evolve = AddModuleToScene< EvolveModule >(ModuleEnum.EvolveModule);
//
//
//		UnitDisplayModule unitdisplay = AddModuleToScene< UnitDisplayModule >(ModuleEnum.UnitsModule);
//
//		EvolveView edu = evolve.View as EvolveView;
//		edu.SetUnitDisplay (unitdisplay.View.gameObject);

	}
	
}

//--------------------------------Catalog------------------------------------------
public class CatalogScene : SceneBase{
	public CatalogScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.DEFAULT;
	}
	
	protected override void InitSceneList(){
		AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
		AddModuleToScene< CatalogModule >(ModuleEnum.CatalogModule);
	}
}

//--------------------------------UnitList------------------------------------------
public class UnitListScene : SceneBase{
	public UnitListScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.DEFAULT;

	}

	protected override void InitSceneList(){
		AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
		AddModuleToScene< MyUnitsListModule >(ModuleEnum.MyUnitsListModule);
		AddModuleToScene<SortController>(ModuleEnum.SortModule);

	}
}

//--------------------------------FriendList------------------------------------------
public class FriendListScene : SceneBase{
	public FriendListScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.DEFAULT;
	}
	
	protected override void InitSceneList(){
		AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
		AddModuleToScene<SortController>(ModuleEnum.SortModule);
		AddModuleToScene<FriendListModule>(ModuleEnum.FriendListModule);

	}
}


//--------------------------------Friend Search------------------------------------------
public class FriendSearchScene : SceneBase{
	public FriendSearchScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.DEFAULT;
	}

	
	protected override void InitSceneList(){

		AddModuleToScene< SearchFriendModule >(ModuleEnum.SearchFriendModule);
		AddModuleToScene<RequestFriendApply>(ModuleEnum.RequestFriendModule);

	}
}

//--------------------------------Apply------------------------------------------
public class ApplyScene : SceneBase{
	public ApplyScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.DEFAULT;
	}

	protected override void InitSceneList(){
		AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
		AddModuleToScene<SortController>(ModuleEnum.SortModule);
		AddModuleToScene<ApplyModule >(ModuleEnum.ApplyModule);
		AddModuleToScene<DeleteFriendApply>(ModuleEnum.DeleteFriendModule);

	}
}

//--------------------------------Reception------------------------------------------
public class ReceptionScene : SceneBase{
	public ReceptionScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.DEFAULT;
	}

	protected override void InitSceneList(){
		AddModuleToScene<SortController>(ModuleEnum.SortModule);
		AddModuleToScene<ItemCounterModule>(ModuleEnum.ItemCounterModule);
		AddModuleToScene< ReceptionModule >(ModuleEnum.ReceptionModule);
		AddModuleToScene<AccpetFriendApply>(ModuleEnum.AccpetFriendApplyModule);

	}
}

public class BattleScene: SceneBase{
	public BattleScene(SceneEnum sEnum) : base(sEnum){
		group = ModuleGroup.BATTLE;
	}
	
	protected override void InitSceneList(){
		AddModuleToScene<BattleBottomModule>(ModuleEnum.BattleBottomModule);
		AddModuleToScene<BattleTopModule>(ModuleEnum.BattleTopModule);
		AddModuleToScene< BattleMapModule >(ModuleEnum.BattleMapModule);
		AddModuleToScene<BattleManipulationModule>(ModuleEnum.BattleManipulationModule);
		AddModuleToScene<BattleFullScreenTipsModule> (ModuleEnum.BattleFullScreenTipsModule);
		AddModuleToScene<BattleAttackEffectModule> (ModuleEnum.BattleAttackEffectModule);
		AddModuleToScene<BattleSkillModule> (ModuleEnum.BattleSkillModule);
	}
	
}

