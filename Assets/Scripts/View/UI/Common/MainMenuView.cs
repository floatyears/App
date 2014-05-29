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
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		RemoveListener ();
	}

    private void AddListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ModifiedParty, UpdateLeaderAvatar);
        MsgCenter.Instance.AddListener(CommandEnum.EnableMenuBtns, SetMenuValid);
    }

    private void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ModifiedParty, UpdateLeaderAvatar);
        MsgCenter.Instance.RemoveListener(CommandEnum.EnableMenuBtns, SetMenuValid);
    }


	private void InitButton() {
		GameObject go = FindChild ("ImgBtn_Friends");
		buttonInfo.Add (go, SceneEnum.Friends);

		go = FindChild ("ImgBtn_Home");
		buttonInfo.Add (go, SceneEnum.Home);

		go = FindChild ("ImgBtn_Scratch");
		buttonInfo.Add (go, SceneEnum.Scratch);

		go = FindChild ("ImgBtn_Shop");
		buttonInfo.Add (go, SceneEnum.Shop);

		go = FindChild ("ImgBtn_Others");
		buttonInfo.Add (go, SceneEnum.Others);

		go = FindChild ("ImgBtn_Units");
		buttonInfo.Add (go, SceneEnum.Units);

		foreach (var item in buttonInfo.Keys) {
			UIEventListener.Get(item).onClick = ClickMenuBtn;
		}

		leaderAvatarTex = transform.FindChild("ImgBtn_Units/Texture_Avatar_Leader").GetComponent<UITexture>();
		leaderAvatarSpr = transform.FindChild("ImgBtn_Units/Sprite_Border").GetComponent<UISprite>();
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
			Debug.LogError("newestLeaderUnit is NULL, return...");
			return;
		}

		if(leaderUnitInfo == null){
			//first step in
//			Debug.Log("UpdateLeaderAvatar(), Leader data is FRIST assigned.");
			leaderAvatarTex.mainTexture = newestLeaderUnit.UnitInfo.GetAsset(UnitAssetType.Profile);

			SetUVByConfig();
			leaderUnitInfo = newestLeaderUnit.UnitInfo;
			return;
		}

	 	if(leaderUnitInfo.ID != newestLeaderUnit.UnitInfo.ID){
//			Debug.LogError("else if  leaderUnitInfo.ID  : " + leaderUnitInfo.ID + " newestLeaderUnit.UnitInfo.ID : " +newestLeaderUnit.UnitInfo.ID);
			//changed
//			Debug.Log("UpdateLeaderAvatar(), Leader data CHANGED." + newestLeaderUnit.UnitInfo.ID + " leaderUnitInfo: " + leaderUnitInfo.ID);
			leaderAvatarTex.mainTexture = newestLeaderUnit.UnitInfo.GetAsset(UnitAssetType.Profile);
 
			SetUVByConfig();
			leaderUnitInfo = newestLeaderUnit.UnitInfo;
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
