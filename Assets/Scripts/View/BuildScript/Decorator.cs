using UnityEngine;
using System.Collections.Generic;
using System;

public class DecoratorBase {

    private bool resetStateFlag = true;
    public bool ResetStateFlag {
        get { return resetStateFlag; }
        set { resetStateFlag = value; }
    }

	public DecoratorBase(SceneEnum sEnum) {
		currentDecoratorScene = sEnum;
	}

	protected SceneEnum currentDecoratorScene;

	public SceneEnum CurrentDecoratorScene{
		get{
			return currentDecoratorScene;
		}
	}

	protected IUIComponent lastDecorator = null;

	protected IUIComponent decorator = null;
	
	public virtual void DecoratorScene () {
	}
	
	public virtual void HideScene () {
		if(lastDecorator != null)
			lastDecorator.HideUI ();
	}

	public virtual void ShowScene () {
        ResetSceneState();
		if(lastDecorator != null)
			lastDecorator.ShowUI ();
	}

    public virtual void ResetSceneState () {
        if(lastDecorator != null){
            ConcreteComponent controller = lastDecorator as ConcreteComponent;
            if (controller != null){
                controller.ResetUIState(resetStateFlag);
            }
            resetStateFlag = true;
        }
    }

	public virtual void DestoryScene () {
		if(lastDecorator != null)
			lastDecorator.DestoryUI ();
	}

	public void SetDecorator (IUIComponent decorator) {
		this.decorator = decorator;
	}

    /// <summary>
    /// Sets the state of the keep. call when sub class will keep state
    /// </summary>
    public void SetKeepState(object args){
        LogHelper.Log("SetKeepState, clear to true");
        ResetStateFlag = false;
    }

	protected T CreatComponent<T>(string name) where T : ConcreteComponent {
		T component = ViewManager.Instance.GetComponent (name) as T;
		if (component == null) {
			component = Activator.CreateInstance(typeof(T), name) as T;
		}
		return component;
	}
}

