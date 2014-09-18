using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// concrete decorate class
/// </summary>
public class ModuleBase{	
    private ModuleState state = ModuleState.None;

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

	protected Dictionary<string,object> moduleData = null;

	public ModuleBase(UIConfigItem uiConfig, params object[] args){
		config = uiConfig;
		if (args.Length > 0) {
			Dictionary<string,object> dic = new Dictionary<string, object>();
			if (args.Length %2 != 0) {
				Debug.LogError("Module Create Args Error: Dictionary requires an even number of arguments!"); 
			}else{
				int i = 0;
				while(i < args.Length - 1) {
					dic.Add(args[i].ToString(), args[i+1]);
					i += 2;
				}
			}
			moduleData = dic;	
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
				
				view.Init(UIConfig, moduleData);

				switch (state) {
					case ModuleState.Show:
						view.ShowUI();
						break;
					case ModuleState.Hide:
						view.HideUI();
						break;
					case ModuleState.Destroy:
						view.DestoryUI();
						break;
					default:
						break;
				}

			});	
		}
	}

	protected void CreateUI<T>(T extraView, UIConfigItem viewUIConfig, Dictionary<string, object> data = null) where T : ViewBase{
		ResourceManager.Instance.LoadLocalAsset (viewUIConfig.resourcePath, o=>{
			if (o == null){
				Debug.LogError("there is no ui with the path:" + viewUIConfig.resourcePath);
				return;
			}
			
			GameObject go = GameObject.Instantiate(o) as GameObject;
			extraView = go.GetComponent<T>();
			
			if (extraView == null){
				Debug.LogError("the component of the ui:"+viewUIConfig.resourcePath+" is null" );
				return;
			}
			
			extraView.Init(viewUIConfig, data);
			
			switch (state) {
			case ModuleState.Show:
				extraView.ShowUI();
				break;
			case ModuleState.Hide:
				extraView.HideUI();
				break;
			case ModuleState.Destroy:
				extraView.DestoryUI();
				break;
			default:
				break;
			}
		});	
	}

	public virtual void InitUI(){

	}

	public virtual void ShowUI() {
		state = ModuleState.Show;

		if (view != null) {
			view.ShowUI();
		}else{
			Debug.LogError("UI is NULL: " + config.resourcePath);
		}
	}

	public virtual void HideUI() {
		state = ModuleState.Hide;

		if (view != null) {
			view.HideUI ();
		}
	}

	public virtual void DestoryUI() {
		state = ModuleState.Destroy;

//		Debug.Log ("Desctroy UI: " + config.moduleName);
		if (view != null) {
			view.DestoryUI();
			view = null;
		}
		if (moduleData != null) {
			moduleData.Clear();
			moduleData = null;
		}
		config = null;
	}

	public virtual void OnReceiveMessages(params object[] data){
		if (view != null)
		{
			view.CallbackView(data);
		}else{
			Debug.Log("send msg fail: "+config.moduleName+"'s ui hasn't been inited" );
		}
	}

	public void SetModuleData(params object[] args){
		if (args.Length > 0) {
			Dictionary<string,object> dic = new Dictionary<string, object>();
			if (args.Length %2 != 0) {
				Debug.LogError("Module Data Set Error: Dictionary requires an even number of arguments!"); 
			}else{
				int i = 0;
				while(i < args.Length - 1) {
					dic.Add(args[i].ToString(), args[i+1]);
					i += 2;
				}
			}
			moduleData = dic;
			if(view != null){
				view.SetViewData(dic);
			}else{
				Debug.Log("View Data Set Failed: " + config.moduleName);
			}
		}
	}
}

internal enum ModuleState{
	None,
	Show,
	Hide,
	Destroy
}