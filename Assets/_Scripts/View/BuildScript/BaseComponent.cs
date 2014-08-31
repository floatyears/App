using UnityEngine;
using System.Collections.Generic;

//public class RootComponent
//{
//	protected UIInsConfig config = null;
//
//	public UIInsConfig uiConfig
//	{
//		get
//		{
//			return config;
//		}
//	}
//
//	protected UIComponentUnity viewComponent;
//
//	public UIComponentUnity ViewComponent {
//		get { return viewComponent; }
//	}
//
//	protected ErrorMsg errMsg = new ErrorMsg();
//
//	public RootComponent()
//	{
//
//	}
//
//	public RootComponent(string name)
//	{
//		config = GetUIInsConfig(name, errMsg);
////		Debug.Log ("config: " + config + " name: " + name + config.localPosition);
//	}
//
//	public static UIInsConfig GetUIInsConfig(string name, ErrorMsg errMsg)
//	{
//		UIIns ins = ModelManager.Instance.GetData(ModelEnum.UIInsConfig, errMsg) as UIIns;
//		return ins.GetData(name);
//	}
//}

///// <summary>
///// decorator class
///// </summary>
//public class BaseComponent :RootComponent, IUIComponent
//{
//
//	public BaseComponent(string name) : base (name)	{
//
//	}
//
//	public void CreatUIAsyn (DecoratorBase decoratorBase) {
//		throw new System.NotImplementedException ();
//	}
//
//	public virtual void CreatUI() {
//
//	}
//
//	public virtual void ShowUI(){ }
//
//	public virtual void HideUI(){ }
//
//	public virtual void DestoryUI(){ }
//	
//}

/// <summary>
/// concrete decorate class
/// </summary>
public class ConcreteComponent{	
    protected bool willClearState = true;
    public bool WillClearState{
        get { return willClearState; }
        set { willClearState = value; }
    }

	public ConcreteComponent(string name) : base(name){
		ViewManager.Instance.AddComponent(this);
	}

	private DecoratorBase decoratorBase = null;

	public void CreatUIAsyn(DecoratorBase decoratorBase) {
		this.decoratorBase = decoratorBase;
		CreatUI ();
	}
	
	public virtual void CreatUI() {
//		if(this is UnitDisplay)
//		Debug.Log ("ConcreteComponent CreatUI  " + this + " component : " + component);
		if (component != null){
			component.CreatUI();
		}

		CreatViewComponent();
//		if(this is UnitDisplay)
//		Debug.Log ("ConcreteComponent CreatUI 2 " + this);
	}

	public virtual void ShowUI() {
//		if(this is ItemCounterController)
//			Debug.LogError ("ConcreteComponent ShowUI 1  " + this + " component : " + component);
		if (component != null) {
			component.ShowUI();		
		}

		if (viewComponent != null) {
			viewComponent.ShowUI();
		}
//		if(this is ItemCounterController)
//		Debug.LogError ("ConcreteComponent ShowUI 2  " + this);
	}

	public virtual void HideUI() {
		if (component != null) {
			component.HideUI();		
		}

		if (viewComponent != null) {
			viewComponent.HideUI ();
		}
	}

	public virtual void DestoryUI() {
		if (viewComponent != null) {
			viewComponent.DestoryUI();
		}

		if (viewComponent != null) {
			GameObject.Destroy (viewComponent.gameObject);
		}

		ViewManager.Instance.RemoveComponent(uiConfig.uiName);

		UIManager.Instance.RemoveUI ();

		Resources.UnloadUnusedAssets ();
	}

//	public 

    public virtual void ResetUIState(bool clear = true) {
        if (!clear){
            return;
        }
        if (component != null) {
            ConcreteComponent controller = component as ConcreteComponent;
            if (controller != null){
                controller.ResetUIState(clear);
            }
        }

        if (viewComponent != null) {
            viewComponent.ResetUIState();
        }
    }

	protected void CreatViewComponent() {
		if (viewComponent == null) {
			ResourceManager.Instance.LoadLocalAsset (uiConfig.resourcePath, CreateCallback);
		} else if (decoratorBase != null){
				decoratorBase.ShowScene();	
		}
	}

	private void CreateCallback(Object o){
//		if(this is ItemCounterController)
//		Debug.LogError ("ConcreteComponent CreateCallback 1 " + this + " o : " + o);

		if (o == null){
			LogHelper.LogError("there is no ui with the path:" + uiConfig.resourcePath);
			return;
		}

		GameObject go = GameObject.Instantiate(o) as GameObject;
		viewComponent = go.GetComponent<UIComponentUnity>();

//		if(this is ItemCounterController)
//		Debug.LogError ("ConcreteComponent CreateCallback 2 " + this + " viewComponent : " + viewComponent);

		if (viewComponent == null){
			LogHelper.LogError("the component of the ui:{0} is null",uiConfig.resourcePath);
			return;
		}
		
		viewCallback = viewComponent;
		viewComponent.Init(uiConfig, this);

//		if(this is ItemCounterController)
//		Debug.LogError ("ConcreteComponent CreateCallback 3 " + this + " decoratorBase : " + decoratorBase);

		if (decoratorBase != null){
			decoratorBase.ShowScene();	
		}

//		if(this is ItemCounterController)
//		Debug.LogError ("ConcreteComponent CreateCallback 4 " + this);
	}

	protected IUIComponent component;
	
	/// <summary>
	/// Sets the decorator
	/// </summary>
	/// <param name="component">Component.</param>
	public void SetComponent(IUIComponent component)
	{
		this.component = component;
	}

	/// <summary>
	/// IUICallback implement. UI Part will use 
	/// </summary>
	/// <param name="data">Data.</param>
	public virtual void CallbackView(object data)
	{

	}

	protected IUICallback viewCallback;

	protected void ExcuteCallback(object data)
	{
//		Debug.LogError(viewCallback);
		if (viewCallback != null)
		{
			viewCallback.CallbackView(data);
		}
	}
}