using UnityEngine;
using System.Collections.Generic;
using System;

public class SceneBase {

//	public WindowType windowType = WindowType.Scene;

    private bool resetStateFlag = true;
    public bool ResetStateFlag {
        get { return resetStateFlag; }
        set { resetStateFlag = value; }
    }

	public SceneBase(ModuleEnum sEnum) {
		currentDecoratorScene = sEnum;
	}

	protected ModuleEnum currentDecoratorScene;

	public ModuleEnum CurrentDecoratorScene{
		get{
			return currentDecoratorScene;
		}
	}

	protected List<ModuleBase> controllerList = new List<ModuleBase>();

//	protected IUIComponent _lastDecorator = null;
//
//	protected IUIComponent lastDecorator {
//		get { return _lastDecorator; }
//		set { 
//			_lastDecorator = value; 
////			if(value != null) { 
////				_lastDecorator.CreatUIAsyn(value);
////			}
//		}
//	}

//	protected IUIComponent decorator = null;
	
	public virtual void InitSceneList () { }
	
	public virtual void HideScene () {
//		if(lastDecorator != null)
//			lastDecorator.HideUI ();
		foreach (var item in controllerList) {
			item.HideUI();
		}
	}

	public virtual void ShowScene () {
//		Debug.LogError("show scene 1");
//        ResetSceneState();
//		if(lastDecorator != null)
//			lastDecorator.ShowUI ();
//		Debug.LogError("show scene 2");
		foreach (var item in controllerList) {
			item.ShowUI();
		}
	}

//    public virtual void ResetSceneState () {
//        if(lastDecorator != null){
//            ConcreteComponent controller = lastDecorator as ConcreteComponent;
//			//Debug.LogError(controller + " resetStateFlag : " + resetStateFlag);
//            if (controller != null){
//                controller.ResetUIState(resetStateFlag);
//            }
//            resetStateFlag = true;
//        }
//    }

	public virtual void DestoryScene () {
//		if(lastDecorator != null)
//			lastDecorator.DestoryUI ();
		foreach (var item in controllerList) {
			item.DestoryUI();
		}
		controllerList.Clear ();
	}

//	public void SetDecorator (IUIComponent decorator) {
//		this.decorator = decorator;
//	}

    /// <summary>
    /// Sets the state of the keep. call when sub class will keep state
    /// </summary>
    public void SetKeepState(object args){
//        Debug.Log(this +  "SetKeepState, clear to true");
        ResetStateFlag = false;
    }

	protected T AddModuleToScene<T>(ModuleEnum name) where T : ModuleBase {
		T component = ModuleManger.Instance.GetOrCreateModule (name) as T;

		controllerList.Add (component);
		return component;
	}

	public bool CheckIsPopUpWindow(){
		ModuleBase controller = controllerList [0];
		if (controller != null && controller.View != null && controller.View.gameObject != null && controller.View.gameObject.transform.parent != null) {
			return controller.View.gameObject.transform.parent == ViewManager.Instance.PopupPanel.transform;	
		}else{
			return false;
		}

	}
}


