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
public class ModuleBase{	
    protected bool willClearState = true;

	protected UIConfigItem config = null;
	
	public UIConfigItem UIConfig {
		get {
			return config;
		}
	}

	protected ViewBase view;

	public ViewBase View{
		get{
			return view;
		}
	}

    public bool WillClearState{
        get { return willClearState; }
        set { willClearState = value; }
    }

	public ModuleBase(string name){

	}

//	private DecoratorBase decoratorBase = null;

//	public void CreatUIAsyn(DecoratorBase decoratorBase) {
////		this.decoratorBase = decoratorBase;
//		CreatUI ();
//	}
	
	public virtual void CreatUI() {
//		if(this is UnitDisplay)
//		Debug.Log ("ConcreteComponent CreatUI  " + this + " component : " + component);
//		if (component != null){
//			component.CreatUI();
//		}

		CreatViewComponent();
//		if(this is UnitDisplay)
//		Debug.Log ("ConcreteComponent CreatUI 2 " + this);
	}

	public virtual void ShowUI() {
//		if(this is ItemCounterController)
//			Debug.LogError ("ConcreteComponent ShowUI 1  " + this + " component : " + component);
//		if (component != null) {
//			component.ShowUI();		
//		}

		if (view != null) {
			view.ShowUI();
		}
//		if(this is ItemCounterController)
//		Debug.LogError ("ConcreteComponent ShowUI 2  " + this);
	}

	public virtual void HideUI() {
//		if (component != null) {
//			component.HideUI();		
//		}

		if (view != null) {
			view.HideUI ();
		}
	}

	public virtual void DestoryUI() {
		if (view != null) {
			view.DestoryUI();
		}

		if (view != null) {
			GameObject.Destroy (view.gameObject);
		}

		ModuleManger.Instance.RemoveModule(UIConfig.moduleName);

		UIManager.Instance.RemoveUI ();

		Resources.UnloadUnusedAssets ();
	}

//	public 

//    public virtual void ResetUIState(bool clear = true) {
//        if (!clear){
//            return;
//        }
////        if (component != null) {
////            ConcreteComponent controller = component as ConcreteComponent;
////            if (controller != null){
////                controller.ResetUIState(clear);
////            }
////        }
//
//        if (viewComponent != null) {
//            viewComponent.ResetUIState();
//        }
//    }

	protected void CreatViewComponent() {
		if (view == null) {
						ResourceManager.Instance.LoadLocalAsset (UIConfig.resourcePath, CreateCallback);
				}
//		} else if (decoratorBase != null){
//				decoratorBase.ShowScene();	
//		}
	}

	private void CreateCallback(Object o){
//		if(this is ItemCounterController)
//		Debug.LogError ("ConcreteComponent CreateCallback 1 " + this + " o : " + o);

		if (o == null){
			LogHelper.LogError("there is no ui with the path:" + UIConfig.resourcePath);
			return;
		}

		GameObject go = GameObject.Instantiate(o) as GameObject;
		view = go.GetComponent<ViewBase>();

//		if(this is ItemCounterController)
//		Debug.LogError ("ConcreteComponent CreateCallback 2 " + this + " viewComponent : " + viewComponent);

		if (view == null){
			LogHelper.LogError("the component of the ui:{0} is null",UIConfig.resourcePath);
			return;
		}
		
//		viewCallback = viewComponent;
		view.Init(UIConfig);

//		if(this is ItemCounterController)
//		Debug.LogError ("ConcreteComponent CreateCallback 3 " + this + " decoratorBase : " + decoratorBase);

//		if (decoratorBase != null){
//			decoratorBase.ShowScene();	
//		}

//		if(this is ItemCounterController)
//		Debug.LogError ("ConcreteComponent CreateCallback 4 " + this);
	}

//	protected IUIComponent component;
	
	/// <summary>
	/// Sets the decorator
	/// </summary>
	/// <param name="component">Component.</param>
//	public void SetComponent(IUIComponent component)
//	{
//		this.component = component;
//	}

	/// <summary>
	/// IUICallback implement. UI Part will use 
	/// </summary>
	/// <param name="data">Data.</param>
	public virtual void CallbackView(object data)
	{

	}

//	protected IUICallback viewCallback;

	protected void ExcuteCallback(object data)
	{
//		Debug.LogError(viewCallback);
		if (view != null)
		{
			view.CallbackView(data);
		}
	}
}