using UnityEngine;
using System.Collections.Generic;

public class MainMenuView : UIComponentUnity {
	private Dictionary<GameObject,SceneEnum> buttonInfo = new Dictionary<GameObject, SceneEnum> ();
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
		InitButton ();
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
        MsgCenter.Instance.AddListener(CommandEnum.EnableMenuBtns, SetMenuValid);
    }

    private void RemoveListener(){
        MsgCenter.Instance.RemoveListener(CommandEnum.EnableMenuBtns, SetMenuValid);
    }


	void InitButton() {
		GameObject go = FindChild ("ImgBtn_Friends");
		buttonInfo.Add (go, SceneEnum.Friends);

		go = FindChild ("ImgBtn_Quest");
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
	}

	private void ClickMenuBtn( GameObject btn ) {
		//Debug.Log("ClickMenuBtn(), btn name is : " + btn.name);
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		SceneEnum targetScene = buttonInfo [ btn ];
		UIManager.Instance.ChangeScene(targetScene);

		Umeng.GA.Event ("BottomMenu",targetScene.ToString ());

	}

    void SetMenuValid(object args){
        bool valid = (bool)args;
        foreach (var item in buttonInfo.Keys) {
          UIButtonScale btnScale = item.GetComponent<UIButtonScale>() ;
			if(btnScale == null)
				continue;
            btnScale.enabled = valid;
//            Debug.LogError("SetMenuValid(), btnScale is : " + valid);
            if(valid)  UIEventListener.Get(item).onClick += ClickMenuBtn; 
            else UIEventListener.Get(item).onClick -= ClickMenuBtn;
        }
    }
}
