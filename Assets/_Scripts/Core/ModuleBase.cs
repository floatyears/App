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

	protected Dictionary<string,object> data = null;
	
	public ModuleBase(UIConfigItem uiConfig, params object[] args){
		config = uiConfig;
		if (args.Length > 0) {
			Dictionary<string,object> dic = new Dictionary<string, object>();
			if (args.Length %2 != 0) {
				Debug.LogError("Tween Error: Hash requires an even number of arguments!"); 
			}else{
				int i = 0;
				while(i < args.Length - 1) {
					dic.Add(args[i].ToString(), args[i+1]);
					i += 2;
				}
			}
			data = dic;	
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

				switch (state) {
					case ModuleState.Show:
						ShowUI();
						break;
					case ModuleState.Hide:
						HideUI();
						break;
					case ModuleState.Destroy:
						DestoryUI();
						break;
					default:
						break;
				}

			});	
		}
	}

	public virtual void InitUI(){

	}

	public virtual void ShowUI() {
		state = ModuleState.Show;

		if (view != null) {
			view.ShowUI();
		}else{
			Debug.LogError("UI is NULL: " + UIConfig.resourcePath);
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

		if (view != null) {
			view.DestoryUI();
			view = null;
		}
	}

	public virtual void OnReceiveMessages(params object[] data){
		if (view != null)
		{
			view.CallbackView(data);
		}
	}
}

internal enum ModuleState{
	None,
	Show,
	Hide,
	Destroy
}