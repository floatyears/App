using UnityEngine;
using System.Collections;

public class LevelUpBaseUI : ConcreteComponent {
	public LevelUpBaseUI(string uiName):base(uiName) {
        MsgCenter.Instance.AddListener(CommandEnum.ChangeScene, ResetUI);
    }
	public override void CreatUI(){
		base.CreatUI();
	}
	
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
	}
	
	public override void DestoryUI(){
		base.DestoryUI();
	}
    void ResetUI(object args){
        SceneEnum nextScene = (SceneEnum)args;
        LogHelper.Log("ResetUI(), nextScene {0}", nextScene);
        if (UIManager.Instance.baseScene.CurrentScene != SceneEnum.LevelUp){
            return;
        }
        Debug.LogError(viewComponent);
        LevelUpBasePanel view = viewComponent as LevelUpBasePanel;
        if (nextScene == SceneEnum.UnitDetail){
            if (view != null){
                view.NeedReInit = false;
            }
            return;
        }
        if (view != null){
            view.NeedReInit = true;
            view.ClearData();
        }
    }

}

