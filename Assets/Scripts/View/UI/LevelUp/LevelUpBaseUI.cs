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

    bool CheckIfSaveStatusOrNot(SceneEnum nextScene){
        bool ret = false;
        if (UIManager.Instance.baseScene.CurrentScene == SceneEnum.UnitDetail || nextScene == SceneEnum.UnitDetail){
            ret = true;
        }
        return ret;
    }

    void ResetUI(object args){
        SceneEnum nextScene = (SceneEnum)args;
        LogHelper.Log("ResetUI(), nextScene {0}", nextScene);
        LevelUpBasePanel view = viewComponent as LevelUpBasePanel;
        if (!CheckIfSaveStatusOrNot(nextScene)){
            if (view != null){
                view.NeedInit = false;
            }
            return;
        }
        Debug.LogError(viewComponent);
        if (view != null){
            view.ClearData();
            view.NeedInit = true;
        }
    }

}

