using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class ModuleManger {

	private static Dictionary<ModuleEnum,ModuleBase> moduleDic = new Dictionary<ModuleEnum, ModuleBase>();

	private static Dictionary<SceneEnum, SceneBase> sceneDic = new Dictionary<SceneEnum, SceneBase>();

	private static ModuleManger instance;

	public static ModuleManger Instance{
		get{
			if(instance == null){
				instance = new ModuleManger();
			}
			return instance;
		}
	}
	private ModuleManger(){

	}

	/// <summary>
	/// Sends messages to the specified module.
	/// </summary>
	/// <param name="module">Module.</param>
	/// <param name="data">Data.</param>
	public static void SendMessage(ModuleEnum module, object data){
		if(moduleDic.ContainsKey(module)){
			moduleDic[module].OnReceiveMessages(data);
		}
	}

	/// <summary>
	/// Shows the module.
	/// </summary>
	/// <param name="name">Name.</param>
	public void ShowModule(ModuleEnum name){
		if (moduleDic.ContainsKey (name)) {
			moduleDic[name].ShowUI();	
		}else{
			GetOrCreateModule(name).ShowUI();
		}
	}

	/// <summary>
	/// Shows the scene.
	/// </summary>
	/// <param name="name">Name.</param>
	public void ShowScene(SceneEnum name){

	}

	/// <summary>
	/// Gets or create the module.
	/// </summary>
	/// <returns>The or create module.</returns>
	/// <param name="name">Name.</param>
	public ModuleBase GetOrCreateModule(ModuleEnum name) {
		if (moduleDic.ContainsKey (name)) {
			return moduleDic [name];	
		}
		else {
			System.Type moduleType = System.Type.GetType(name.ToString());
			ModuleBase module = Activator.CreateInstance(moduleType, DataCenter.Instance.GetConfigUIItem(name)) as ModuleBase;
//			module.UIConfig = ;

			moduleDic.Add(name,module);

			Debug.Log ("Module Create: [[[---" + module + "---]]]");

			return module;
		}
	}

	/// <summary>
	/// Removes the module.
	/// </summary>
	/// <param name="name">Name.</param>
	public void RemoveModule(ModuleEnum name) {
		if (moduleDic.ContainsKey (name)) {
			ModuleBase temp = moduleDic[name];
			temp.DestoryUI ();
			moduleDic.Remove(name);
			temp = null;
		}

	}

	/// <summary>
	/// Clears the modules.
	/// </summary>
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
