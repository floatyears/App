using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInfoBarModule : ModuleBase {
	private SceneInfoBarView v;

	public SceneInfoBarModule(UIConfigItem config):base(  config) {
		CreateUI<SceneInfoBarView> ();
		v = view as SceneInfoBarView;
    }
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void OnReceiveMessages (params object[] data)
	{
		switch (data.Length) {
		case 1:
			if(data[0] is ModuleEnum){
				ModuleEnum name = (ModuleEnum)data[0];
				switch (name) {
				case ModuleEnum.HomeModule:
				case ModuleEnum.GachaModule:
					v.gameObject.SetActive(false);
					v.SetBackBtnActive(false,(ModuleEnum)data[0]);
					break;
				case ModuleEnum.RewardModule:
				case ModuleEnum.MusicModule:
				case ModuleEnum.OperationNoticeModule:
				case ModuleEnum.NicknameModule:
				case ModuleEnum.UnitDetailModule:
				case ModuleEnum.MsgWindowModule:
				case ModuleEnum.NoviceGuideTipsModule:
				case ModuleEnum.NoviceMsgWindowModule:
				case ModuleEnum.MaskModule:
				case ModuleEnum.UnitSortModule:
				case ModuleEnum.ItemCounterModule:
				case ModuleEnum.ApplyMessageModule:
				case ModuleEnum.ShowNewCardModule:
				case ModuleEnum.TaskModule:
				case ModuleEnum.AchieveModule:
				case ModuleEnum.AchieveTipModule:
				case ModuleEnum.ResultModule:
					break;
				case ModuleEnum.FriendMainModule:
				case ModuleEnum.ScratchModule:
				case ModuleEnum.ShopModule:
				case ModuleEnum.OthersModule:
				case ModuleEnum.UnitsMainModule:
					v.gameObject.SetActive(true);
					v.SetBackBtnActive(false,(ModuleEnum)data[0]);
					break;
				default:
//					backModules.
					view.gameObject.SetActive(true);
					v.SetBackBtnActive(true,(ModuleEnum)data[0]);
					break;
				}
			}else if(data[0] is string){
				if(data[0].ToString() == "back_scene"){
					v.BackPreScene(null);
				}
			}
			break;
		case 2:
		 	if(data[0].ToString() == "stage"){
				v.SetSceneName((string)data[1]);
			}else if(data[0].ToString() == "no_back"){
				v.NoBackScene((ModuleEnum)data[1]);
			}
			break;
		default:
			Debug.LogError("Scene Info Args Length Err: ");
			break;
		}

	}


}
