using UnityEngine;
using System.Collections;

public class RootComponent {
	protected UIInsConfig uiConfig = null;

	public UIInsConfig UIConfig {
		get {
			return uiConfig;
		}
	}

	protected UIComponentUnity viewComponent;

	protected ErrorMsg errMsg = new ErrorMsg ();

	public RootComponent() {

	}

	public RootComponent(string name) {

		uiConfig = GetUIInsConfig(name, errMsg);
	}

	public static UIInsConfig GetUIInsConfig(string name,ErrorMsg errMsg) {
		UIIns ins = ModelManager.Instance.GetData (ModelEnum.UIInsConfig, errMsg) as UIIns;
		return ins.GetData (name);
	}
}

/// <summary>
/// decorator class
/// </summary>
public class BaseComponent :RootComponent, IUIComponent {

	public BaseComponent(string name) : base (name) {

	}
	
	public virtual void CreatUI () {
	}

	public virtual void ShowUI () {
	}

	public virtual void HideUI () {
	}

	public virtual void DestoryUI () {
	}
}

/// <summary>
/// concrete decorate class
/// </summary>
public class ConcreteComponent : RootComponent, IUIComponent {	

	public ConcreteComponent (string name) : base(name) {
		ViewManager.Instance.AddComponent (this);
	}
	
	public virtual void CreatUI () {
		if (component != null) {
			component.CreatUI ();
		}

		CreatViewComponent ();
	}

	public virtual void ShowUI () {
		if (component != null) {
			component.ShowUI();		
		}

		if (viewComponent != null) {
			viewComponent.ShowUI ();
		}
	}

	public virtual void HideUI () {
		if (component != null) {
			component.HideUI();		
		}
	
		if (viewComponent != null) {
			viewComponent.HideUI ();
		}
	}

	public virtual void DestoryUI () {
		if (component != null) {
			component.DestoryUI ();
		}

		if (viewComponent != null) {
			viewComponent.DestoryUI ();
		}
	}

	protected void CreatViewComponent() {
		Object o = ResourceManager.Instance.LoadLocalAsset (uiConfig.resourcePath) as Object;

		if(o == null)
			return;

		GameObject go = GameObject.Instantiate (o) as GameObject;
		viewComponent = go.GetComponent<UIComponentUnity> ();

		if(viewComponent == null)
			return;

		IUIOrigin org = null;
		if (this is IUIOrigin) {
			org = this as IUIOrigin;
		}
		viewComponent.Init (uiConfig, org);
	}
	
	#region decorator
	protected IUIComponent component;
	
	/// <summary>
	/// Sets the decorator
	/// </summary>
	/// <param name="component">Component.</param>
	public void SetComponent(IUIComponent component)  {
		this.component = component;
	}
	#endregion
}