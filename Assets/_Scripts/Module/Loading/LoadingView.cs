// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;


public class LoadingView : ViewBase {
	private UILabel tapLogin;

	private bool initComplete = false;

	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null) {
        base.Init (config, data);
		tapLogin = FindChild ("ClickLabel").GetComponent<UILabel>();
		
		tapLogin.enabled = false;
    }
    
    public override void ShowUI () {
//		GameDataStore.Instance.StoreData (GameDataStore.UUID, "");
//		GameDataStore.Instance.StoreData (GameDataStore.USER_ID, "");

		base.ShowUI ();

		//友盟初始化
		Umeng.GA.StartWithAppKeyAndChannelId (ServerConfig.UmengAppKey, ServerConfig.Channel);

		if (string.IsNullOrEmpty (GameDataPersistence.USER_ID)) {
			GameDataAnalysis.Event(GameDataAnalysisEventType.FirstStart,new Dictionary<string,string>(){{"DeviceInfo",SystemInfo.deviceUniqueIdentifier}});
		}

		#if !UNITY_EDITOR
		Debug.Log("device info: " + SystemInfo.deviceUniqueIdentifier);
		//		Debug.Log("GetDeviceInfo: " + Umeng.GA.GetDeviceInfo());
		#endif

    }

	private void CouldLogin(){
		Debug.Log ("load complete, could login");

		ResourceManager.Instance.Init (o => {
			EffectManager em = EffectManager.Instance;
//			ConfigDragPanel dragPanelConfig = new ConfigDragPanel();
			
			TextCenter.Instance.Init (o1=>{
				
				AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_home);
//				DataCenter.Instance.Init();
				
				initComplete = true;
				//				Debug.Log("init complete: " + initComplete);

				UIEventListenerCustom.Get(this.gameObject).onClick = ClickToLogin;
				tapLogin.enabled = true;
			});
		});

		tapLogin.text = 
#if LANGUAGE_CN
	ServerConfig.touchToLogin;
#elif LANGUAGE_EN
	"TAP SCREEN TO START";
#else
	"TAP SCREEN TO START";
#endif

		//TextCenter.GetText("Text_TapToLogin");

	}

    private bool CheckIfFirstLogin(){
        bool ret = false;
        uint userId = GameDataPersistence.Instance.GetUInt(GameDataPersistence.USER_ID);
        string uuid = GameDataPersistence.Instance.GetData(GameDataPersistence.UUID);
        if (userId == 0 && uuid.Length == 0) {
            return true;
        }
        return ret;
    }

    private void ClickToLogin(GameObject btn){
//		Debug.Log("click to login: " + initComplete);
		if(initComplete)
			Login();
    }

	private void Login(){
		if (CheckIfFirstLogin()){
			Debug.Log("firstLogin");
			SelectRoleFirst();
		}
		else {
			Debug.Log("login directly");
			LoginDirectly();

		}
	}
	
	private void LoginDirectly(){
		Umeng.GA.Event ("Login");
//		LoadingLogic loadingLogic = origin as LoadingLogic;
//        loadingLogic.StartLogin();
		ModuleManager.SendMessage (ModuleEnum.LoadingModule,"StartLogin");
    }

    private void SelectRoleFirst(){
//		ModuleManger.Instance.ShowModule (ModuleEnum.Preface);
		ModuleManager.Instance.ShowModule (ModuleEnum.SelectRoleModule);
    }

}
