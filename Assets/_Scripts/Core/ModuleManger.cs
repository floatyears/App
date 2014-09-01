using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class ModuleManger {

	private static Dictionary<ModuleEnum,ModuleBase> moduleDic = new Dictionary<ModuleEnum, ModuleBase>();

	private static ModuleManger instance;

	public static ModuleManger Instance{
		get{
			return instance;
		}
	}

	public static void SendMessage(ModuleEnum module, object data){
		if(moduleDic.ContainsKey(module)){
			moduleDic[module].CallbackView(data);
		}
	}

//	private Dictionary<string,ModuleBase> UIComponentDic = new Dictionary<string, ModuleBase>();

	public void AddModule(ModuleBase component) {
		if (component == null) {
			return;	
		}
		
		UIConfigItem config = component.UIConfig;
		ModuleEnum name = config.moduleName;
		
//		ModuleBase temp = null;
		if (moduleDic.ContainsKey (name)) {
			moduleDic [name] = component;	
//			temp = null;
		}
		else {
			moduleDic.Add(name,component);
		}
	}
	
	public ModuleBase GetModule(ModuleEnum name) {
		if (moduleDic.ContainsKey (name)) {
			return moduleDic [name];	
		}
		else {
			System.Type moduleType = System.Type.GetType(name.ToString());
			ModuleBase module = Activator.CreateInstance(moduleType, name.ToString()) as ModuleBase;
			
			ModuleManger.Instance.AddModule(module);

			LogHelper.Log ("component: " + module);

			return module;
		}
	}
	
	public void RemoveModule(ModuleEnum name) {
		if (!moduleDic.ContainsKey (name)) {
			return;
		}	
		moduleDic.Remove (name);
	}
	
	public void DeleteComponent(ModuleEnum name) {
		if (moduleDic.ContainsKey (name)) {
			ModuleBase temp = moduleDic[name];
			temp.DestoryUI ();
			moduleDic.Remove(name);
			temp = null;
		}
	}
	
	public void ClearModules () {
		List<ModuleBase> cclist = new List<ModuleBase> ();
		List<ModuleEnum> ccID = new List<ModuleEnum> ();
//		System.Type ty = typeof(MsgWindowLogic);
//		System.Type ty1 = typeof(MaskController);
//		System.Type ty2 = typeof(NoviceMsgWindowLogic);
//		foreach (var item in moduleDic) {
//			ModuleEnum key = item.Key;
//			ModuleBase cc = item.Value as ModuleBase;
//			System.Type tempType = cc.GetType();
//			
//			if(tempType == ty || tempType == ty1 || tempType == ty2) {
//				continue;
//			}
//			
//			ccID.Add(key);
//			cclist.Add(cc);
//		}
		for (int i = 0; i < ccID.Count; i++) {
			moduleDic.Remove(ccID[i]);
		}
		for (int i = cclist.Count - 1; i >= 0; i--) {
			//			Debug.LogError("CleartComponent : " + cclist[i]);
			cclist[i].DestoryUI();
		}
		cclist.Clear ();
	}

}
