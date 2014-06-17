using UnityEngine;
using System.Collections.Generic;

public class RootComponent
{
	protected UIInsConfig config = null;

	public UIInsConfig uiConfig
	{
		get
		{
			return config;
		}
	}

	protected UIComponentUnity viewComponent;

	public UIComponentUnity ViewComponent {
		get { return viewComponent; }
	}

	protected ErrorMsg errMsg = new ErrorMsg();

	public RootComponent()
	{

	}

	public RootComponent(string name)
	{
		config = GetUIInsConfig(name, errMsg);
		//Debug.Log ("config: " + config + " name: " + name);
	}

	public static UIInsConfig GetUIInsConfig(string name, ErrorMsg errMsg)
	{
		UIIns ins = ModelManager.Instance.GetData(ModelEnum.UIInsConfig, errMsg) as UIIns;
		return ins.GetData(name);
	}
}

/// <summary>
/// decorator class
/// </summary>
public class BaseComponent :RootComponent, IUIComponent
{

	public BaseComponent(string name) : base (name)	{

	}

	public void CreatUIAsyn (IUIComponent componnet) {
		throw new System.NotImplementedException ();
	}

	public virtual void CreatUI() {

	}

	public virtual void ShowUI(){ }

	public virtual void HideUI(){ }

	public virtual void DestoryUI(){ }
	
}

/// <summary>
/// concrete decorate class
/// </summary>
public class ConcreteComponent : RootComponent, IUIComponent ,IUICallback{	
    protected bool willClearState = true;
    public bool WillClearState{
        get { return willClearState; }
        set { willClearState = value; }
    }

	public ConcreteComponent(string name) : base(name){
		ViewManager.Instance.AddComponent(this);
	}

	private IUIComponent uiComponent = null;

	public void CreatUIAsyn(IUIComponent uiComponent) {
		this.uiComponent = uiComponent;
		CreatUI ();
	}

	public virtual void CreatUI() {
		if (component != null){
			component.CreatUI();
		}
		CreatViewComponent();
	}

	public virtual void ShowUI() {
		if (component != null) {
			component.ShowUI();		
		}
//		Debug.LogError("ConcreteComponent  component ShowUI : " + component + "  this : " + this + " viewComponent : " + viewComponent);
		if (viewComponent != null) {
			viewComponent.ShowUI();
		}
	}

	public virtual void HideUI() {
		if (component != null) {
			component.HideUI();		
		}
//		Debug.LogError("viewComponent : " + viewComponent);

		if (viewComponent != null) {
			viewComponent.HideUI ();
		}
	}

	public virtual void DestoryUI() {
//		if (component != null) {
//			component.DestoryUI();
//		}

		if (viewComponent != null) {
			viewComponent.DestoryUI();
		}

		if (viewComponent != null) {
			GameObject.Destroy (viewComponent.gameObject);	
		}

		ViewManager.Instance.RemoveComponent(uiConfig.uiName);

		UIManager.Instance.RemoveUI ();
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
			ResourceManager.Instance.LoadLocalAsset(uiConfig.resourcePath, CreateCallback);
		}
	}

	private void CreateCallback(Object o){
		if (o == null){
			LogHelper.LogError("there is no ui with the path:" + uiConfig.resourcePath);
			return;
		}
		Debug.LogError ("o : " +  o.name + "type : " + o.GetType() );
		GameObject go = GameObject.Instantiate(o) as GameObject;
		viewComponent = go.GetComponent<UIComponentUnity>();
		if (viewComponent == null){
			LogHelper.LogError("the component of the ui:{0} is null",uiConfig.resourcePath);
			return;
		}
		
		viewCallback = viewComponent;
		viewComponent.Init(uiConfig, this);

		if (uiComponent != null) {
			uiComponent.ShowUI();
		}
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