using UnityEngine;
using System.Collections.Generic;

public class MainMenuView : UIComponentUnity{
	private UITexture leaderAvatarTex;
	private UISprite leaderAvatarSpr;

	private Dictionary<GameObject,SceneEnum> buttonInfo = new Dictionary<GameObject, SceneEnum> ();
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
		InitButton ();
		UpdateLeaderAvatar(null);
	}

	public override void ShowUI () {
		base.ShowUI ();
        AddListener();
	}

	public override void HideUI () {
		base.HideUI ();
		RemoveListener ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		RemoveListener ();
	}

    private void AddListener(){
//		Debug.LogError("main menu addlistener");
		MsgCenter.Instance.AddListener(CommandEnum.ModifiedParty, UpdateLeaderAvatar);
        MsgCenter.Instance.AddListener(CommandEnum.EnableMenuBtns, SetMenuValid);
    }

    private void RemoveListener(){
//		Debug.LogError("main menu removeListener");
		MsgCenter.Instance.RemoveListener(CommandEnum.ModifiedParty, UpdateLeaderAvatar);
        MsgCenter.Instance.RemoveListener(CommandEnum.EnableMenuBtns, SetMenuValid);
    }


	private void InitButton() {
		GameObject go = FindChild ("Btn_Friends");
		FindChild ("Btn_Friends/Label").GetComponent<UILabel> ().text = TextCenter.GetText("SCENE_NAME_FRIEND");
		buttonInfo.Add (go, SceneEnum.Friends);

		go = FindChild ("Btn_Home");
		FindChild ("Btn_Home/Label").GetComponent<UILabel> ().text = TextCenter.GetText("SCENE_NAME_HOME");
		buttonInfo.Add (go, SceneEnum.Home);

		go = FindChild ("Btn_Scratch");
		FindChild ("Btn_Scratch/Label").GetComponent<UILabel> ().text = TextCenter.GetText("SCENE_NAME_SCRATCH");
		buttonInfo.Add (go, SceneEnum.Scratch);

		go = FindChild ("Btn_Shop");
		FindChild ("Btn_Shop/Label").GetComponent<UILabel> ().text = TextCenter.GetText("SCENE_NAME_SHOP");
		buttonInfo.Add (go, SceneEnum.Shop);

		go = FindChild ("Btn_Others");
		FindChild ("Btn_Others/Label").GetComponent<UILabel> ().text = TextCenter.GetText("SCENE_NAME_OTHERS");
		buttonInfo.Add (go, SceneEnum.Others);

		go = FindChild ("Btn_Units");
		FindChild ("Btn_Units/Label").GetComponent<UILabel> ().text = TextCenter.GetText("SCENE_NAME_UNITS");
		buttonInfo.Add (go, SceneEnum.Units);

		foreach (var item in buttonInfo.Keys) {
			UIEventListener.Get(item).onClick = ClickMenuBtn;
		}

		leaderAvatarTex = transform.FindChild("Btn_Units/Texture_Avatar_Leader").GetComponent<UITexture>();
		leaderAvatarSpr = transform.FindChild("Btn_Units/Sprite_Border").GetComponent<UISprite>();
	}

	private void ClickMenuBtn( GameObject btn ) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		SceneEnum targetScene = buttonInfo [ btn ];
		UIManager.Instance.ChangeScene(targetScene);

		Umeng.GA.Event ("BottomMenu",targetScene.ToString ());

	}

	private void SetMenuValid(object args){
        bool valid = (bool)args;
        foreach (var item in buttonInfo.Keys) {
          UIButtonScale btnScale = item.GetComponent<UIButtonScale>() ;
			if(btnScale == null)
				continue;
            btnScale.enabled = valid;
            if(valid)  UIEventListener.Get(item).onClick += ClickMenuBtn; 
            else UIEventListener.Get(item).onClick -= ClickMenuBtn;
        }
    }

	private TUnitInfo leaderUnitInfo;
	private void UpdateLeaderAvatar(object msg){
		TUserUnit newestLeaderUnit = DataCenter.Instance.PartyInfo.CurrentParty.GetUserUnit()[ 0 ];
//		Debug.LogError ((newestLeaderUnit == null) + "  leaderUnitInfo == null : " + (leaderUnitInfo == null));
		if(newestLeaderUnit == null){
			leaderAvatarTex.mainTexture = null;
//			Debug.LogError("newestLeaderUnit is NULL, return...");
			return;
		}

		if(leaderUnitInfo == null){
			//first step in
//			Debug.Log("UpdateLeaderAvatar(), Leader data is FRIST assigned.");
			newestLeaderUnit.UnitInfo.GetAsset(UnitAssetType.Profile,o=>{
				leaderAvatarTex.mainTexture = o as Texture2D;
				SetUVByConfig();
				leaderUnitInfo = newestLeaderUnit.UnitInfo;
			} );

			return;
		}

	 	if(leaderUnitInfo.ID != newestLeaderUnit.UnitInfo.ID){
//			Debug.LogError("else if  leaderUnitInfo.ID  : " + leaderUnitInfo.ID + " newestLeaderUnit.UnitInfo.ID : " +newestLeaderUnit.UnitInfo.ID);
			//changed
//			Debug.Log("UpdateLeaderAvatar(), Leader data CHANGED." + newestLeaderUnit.UnitInfo.ID + " leaderUnitInfo: " + leaderUnitInfo.ID);
			newestLeaderUnit.UnitInfo.GetAsset(UnitAssetType.Profile, o =>{
				leaderAvatarTex.mainTexture = o as Texture2D;
				SetUVByConfig();
				leaderUnitInfo = newestLeaderUnit.UnitInfo;
			});
 

		}
		else{
			//not changed
//			Debug.LogError("else   leaderUnitInfo.ID  : " + leaderUnitInfo.ID + " newestLeaderUnit.UnitInfo.ID : " +newestLeaderUnit.UnitInfo.ID);
//			Debug.Log("UpdateLeaderAvatar(), Leader data NOT CHANGED.");
		}
	}

	private void SetUVByConfig(){
		//Debug.Log("SetUVByConfig()...");
		//TODO read from uv config file
		float x = 0.21f;
		float y = 0.35f;
		float w = 0.52f;
		float h = 0.55f;
		leaderAvatarTex.uvRect = new Rect(x, y, w, h);
	}
}
