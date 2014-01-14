using UnityEngine;
using System.Collections.Generic;
using System;

public class DecoratorBase {

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
		if(lastDecorator != null)
			lastDecorator.ShowUI ();
	}

	public virtual void DestoryScene () {
		if(lastDecorator != null)
			lastDecorator.DestoryUI ();
	}

	public void SetDecorator (IUIComponent decorator) {
		this.decorator = decorator;
	}

	protected T CreatComponent<T>(string name) where T : ConcreteComponent {
		T component = ViewManager.Instance.GetComponent (name) as T;
		if (component == null) {
			component = Activator.CreateInstance(typeof(T), name) as T;
		}
		
		return component;
	}
}

