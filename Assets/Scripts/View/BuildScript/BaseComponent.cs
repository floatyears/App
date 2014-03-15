using UnityEngine;
using System.Collections.Generic;

public class RootComponent {
	protected UIInsConfig config = null;

	public UIInsConfig uiConfig {
		get {
			return config;
		}
	}

	protected UIComponentUnity viewComponent;

	protected ErrorMsg errMsg = new ErrorMsg ();

	public RootComponent() {

	}

	public RootComponent(string name) {

		config = GetUIInsConfig(name, errMsg);
	}

	public static UIInsConfig GetUIInsConfig(string name,ErrorMsg errMsg) {
		UIIns ins = ModelManager.Instance.GetData (ModelEnum.UIInsConfig, errMsg) as UIIns;
		return ins.GetData (name);
	}
}

/// <summary>
/// decorator class
/// </summary>
public class BaseComponent :RootComponent, IUIComponent{

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
public class ConcreteComponent : RootComponent, IUIComponent ,IUICallback{	

	public ConcreteComponent (string name) : base(name) {
		ViewManager.Instance.AddComponent (this);
	}
	
	public virtual void CreatUI () {
//		Debug.LogError("CreatUI : 111  " + config.uiName);
		if (component != null) {
			component.CreatUI ();
		}
//		Debug.LogError("CreatUI : 222  "  + config.uiName );
		CreatViewComponent ();
//		Debug.LogError("CreatUI : 333  " + config.uiName );
	}

	public virtual void ShowUI () {
//		Debug.LogError ("component : " +component);
		if (component != null) {
			component.ShowUI();		
		}
//		Debug.LogError ("ConcreteComponent : " + viewComponent.gameObject.name);
		if (viewComponent != null) {
			viewComponent.ShowUI ();
		}
	}

	public virtual void HideUI () {
		if (component != null) {
			component.HideUI();		
		}
//		Debug.LogError("viewComponent : " + viewComponent);
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

		ViewManager.Instance.RemoveComponent (uiConfig.uiName);
	}

	protected void CreatViewComponent() {

		if(viewComponent == null) {

			Object o = ResourceManager.Instance.LoadLocalAsset (uiConfig.resourcePath) as Object;
			
			if(o == null)
				return;
			
			GameObject go = GameObject.Instantiate (o) as GameObject;

			viewComponent = go.GetComponent<UIComponentUnity> ();
			if(viewComponent == null)
				return;
			viewCallback = viewComponent;
			viewComponent.Init (uiConfig, this);
		}
	}



	protected IUIComponent component;
	
	/// <summary>
	/// Sets the decorator
	/// </summary>
	/// <param name="component">Component.</param>
	public void SetComponent(IUIComponent component)  {
		this.component = component;
	}

	/// <summary>
	/// IUICallback implement. UI Part will use 
	/// </summary>
	/// <param name="data">Data.</param>
	public virtual void Callback (object data) {

	}

	protected IUICallback viewCallback;

	protected void ExcuteCallback (object data) {
//		Debug.LogError(viewCallback);
		if (viewCallback != null) {
			viewCallback.Callback (data);
		}
	}
}