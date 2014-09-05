using UnityEngine;
using System.Collections;

public class SceneInfoBarModule : ModuleBase {
	
	public SceneInfoBarModule(UIConfigItem config):base(  config) {
		CreateUI<SceneInfoBarView> ();
    }
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void OnReceiveMessages (params object[] data)
	{
		if((ModuleOrScene)data[1] == ModuleOrScene.Module){
			ModuleEnum name = (ModuleEnum)data[0];
			switch (name) {
			case ModuleEnum.HomeModule:
				view.gameObject.SetActive(false);
				break;
			case ModuleEnum.RewardModule:
			case ModuleEnum.MusicModule:
			case ModuleEnum.OperationNoticeModule:
			case ModuleEnum.NicknameModule:
				break;
			case ModuleEnum.FriendMainModule:
			case ModuleEnum.ScratchModule:
			case ModuleEnum.ShopModule:
			case ModuleEnum.OthersModule:
			case ModuleEnum.UnitsMainModule:
				view.gameObject.SetActive(true);
				(view as SceneInfoBarView).SetBackBtnActive(false);
				(view as SceneInfoBarView).SetSceneName(data[0].ToString());
				break;
			default:
				view.gameObject.SetActive(true);
				(view as SceneInfoBarView).SetBackBtnActive(true,GetBackModule(name));
				(view as SceneInfoBarView).SetSceneName(data[0].ToString());
				break;
			}
		}else if((ModuleOrScene)data[1] == ModuleOrScene.Scene){
			(view as SceneInfoBarView).SetSceneName(data[0].ToString());
		}

	}

	private ModuleEnum GetBackModule(ModuleEnum name){
		ModuleEnum backName = ModuleEnum.None;
		switch (name) {
			case ModuleEnum.FriendListModule:
			case ModuleEnum.ApplyModule:
			case ModuleEnum.ReceptionModule:
			case ModuleEnum.SearchFriendModule:
				backName = ModuleEnum.FriendMainModule;
				break;
			case ModuleEnum.GameRaiderModule:
				backName = ModuleEnum.OthersModule;
				break;
			case ModuleEnum.PartyModule:
			case ModuleEnum.LevelUpModule:
			case ModuleEnum.SellUnitModule:
			case ModuleEnum.CatalogModule:
			case ModuleEnum.MyUnitsListModule: 
			case ModuleEnum.EvolveModule:
				backName = ModuleEnum.UnitsMainModule;
				break;
			case ModuleEnum.StageSelectModule:
				backName = ModuleEnum.HomeModule;
				break;
			case ModuleEnum.GachaModule:
				backName = ModuleEnum.ScratchModule;
				break;
			case ModuleEnum.QuestSelectModule:
				backName = ModuleEnum.StageSelectModule;
				break;
			case ModuleEnum.FriendSelectModule:
				backName = ModuleEnum.QuestSelectModule;
				break;
			case ModuleEnum.FightReadyModule:
				backName = ModuleEnum.FriendSelectModule;
				break;
			default:
				backName = ModuleEnum.None;
				break;
		}
		return backName;
	}
}
