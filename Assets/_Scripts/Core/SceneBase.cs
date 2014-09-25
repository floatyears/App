using UnityEngine;
using System.Collections.Generic;
using System;

public class SceneBase {

	private SceneEnum sceneName;

	protected ModuleGroup group;

	public ModuleGroup Group{
		get{
			return group;
		}
	}

	public SceneBase(SceneEnum scene) {
		sceneName = scene;
		InitSceneList ();
	}

	protected List<ModuleBase> moduleList = new List<ModuleBase>();
	
	public virtual void HideScene () {
		foreach (var item in moduleList) {
			item.HideUI();
		}
	}

	public virtual void ShowScene () {
		foreach (var item in moduleList) {
			item.ShowUI();
		}
	}

	public virtual void DestoryScene () {
		foreach (var item in moduleList) {
			ModuleManager.Instance.DestroyModule(item.UIConfig.moduleName);
		}
		moduleList.Clear ();
	}

	protected virtual void InitSceneList(){
	
	}
	

	protected void AddModuleToScene<T>(ModuleEnum name) where T : ModuleBase {
		T module = ModuleManager.Instance.GetOrCreateModule (name) as T;
		if (module != null) {
			moduleList.Add (module);	
		}

	}
	
}


