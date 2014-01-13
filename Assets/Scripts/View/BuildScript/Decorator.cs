using UnityEngine;
using System.Collections.Generic;
using System;

public class DecoratorBase {

//	private Dictionary<string,IUIComponent> currentComponentDic = new Dictionary<string, IUIComponent>();
//
//	public Dictionary<string, IUIComponent> CurrentComponentDic {
//		get {
//			return currentComponentDic;
//		}
//	}
	public DecoratorBase(SceneEnum sEnum) {
		currentDecoratorScene = sEnum;
	}

	private SceneEnum currentDecoratorScene;

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

//-------------------------------------------------------------------------------------------
// Example
//-------------------------------------------------------------------------------------------


public class DecoratorInitScene : DecoratorBase {
	public DecoratorInitScene (SceneEnum sEnum) : base(sEnum) {
	}

	public override void ShowScene () {
		base.ShowScene ();
	}

	public override void HideScene () {
		base.HideScene ();
	}

	public override void DestoryScene () {
		base.DestoryScene ();
	}

	public override void DecoratorScene () {
		ConcreteComponent bottom = CreatComponent<MenuBottom> (UIConfig.menuBottomName);
		bottom.SetComponent (decorator);


		ConcreteComponent t = CreatComponent<Top> (UIConfig.topBackgroundName);
		t.SetComponent (bottom);

		//xxx

		t.CreatUI ();

		lastDecorator = t;

	}
}

public class DecoratorUIScene : DecoratorBase {

	public DecoratorUIScene(SceneEnum sEnum) : base(sEnum) {
	}

	public override void DecoratorScene () {
		if (decorator == null) {
			return;	
		}

		Background back = CreatComponent<Background> (UIConfig.menuBackgroundName);

		back.SetComponent (decorator);
		back.CreatUI ();

		lastDecorator = back;
	}

	public override void ShowScene () {
		base.ShowScene ();
	}

	public override void HideScene () {
		base.HideScene ();
	}

	public override void DestoryScene () {
		base.DestoryScene ();
	}
}

public class TestUIQuest : DecoratorBase {

	public TestUIQuest(SceneEnum sEnum) : base(sEnum) {
	}

	public override void DecoratorScene () {
		ConcreteComponent top = CreatComponent<Top> (UIConfig.topBackgroundName);
		top.SetComponent (decorator);

		ConcreteComponent bottom = CreatComponent<MenuBottom> (UIConfig.menuBottomName);

		bottom.SetComponent (top);

		bottom.CreatUI ();

		lastDecorator = bottom;
	}

	public override void ShowScene () {
		base.ShowScene ();
	}

	public override void HideScene () {
		base.HideScene ();
	}

	public override void DestoryScene () {
		base.DestoryScene ();
	}

}
