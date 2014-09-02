using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// concrete decorate class
/// </summary>
public class ModuleBase{	
    private bool show = false;
	private bool destroy = false;

	private UIConfigItem config = null;
	
	public UIConfigItem UIConfig {
		get {
			return config;
		}
		set{
			config = value;
		}
	}

	protected ViewBase view;

	public ViewBase View{
		get{
			return view;
		}
	}

	protected object data = null;
	
	public ModuleBase(UIConfigItem uiConfig, object moduleData = null){
		config = uiConfig;
		if (moduleData != null) {
			data = moduleData;	
		}
	}

	protected void CreateUI<T>() where T : ViewBase{
		if (view == null) {
			ResourceManager.Instance.LoadLocalAsset (UIConfig.resourcePath, o=>{
				if (o == null){
					Debug.LogError("there is no ui with the path:" + UIConfig.resourcePath);
					return;
				}
				
				GameObject go = GameObject.Instantiate(o) as GameObject;
				view = go.GetComponent<T>();
				
				if (view == null){
					Debug.LogError("the component of the ui:"+UIConfig.resourcePath+" is null" );
					return;
				}
				
				view.Init(UIConfig);

				if(show){
					ShowUI();
				}else{
					if(destroy){
						DestoryUI();
					}
				}

			});	
		}
	}

	public virtual void InitUI(){

	}

	public virtual void ShowUI() {
		show = true;

		if (view != null) {
			view.ShowUI();
		}else{
			Debug.LogError("UI didn't show: " + UIConfig.resourcePath);
		}
	}

	public virtual void HideUI() {
		show = false;

		if (view != null) {
			view.HideUI ();
		}
	}

	public virtual void DestoryUI() {
		show = false;
		destroy = true;

		if (view != null) {
			view.DestoryUI();
		}
	}

	public virtual void OnReceiveMessages(object data){
		if (view != null)
		{
			view.CallbackView(data);
		}
	}
}