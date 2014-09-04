﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class ModuleManger {

	private static Dictionary<ModuleEnum,ModuleBase> moduleDic = new Dictionary<ModuleEnum, ModuleBase>();

	private static Dictionary<SceneEnum, SceneBase> sceneDic = new Dictionary<SceneEnum, SceneBase>();

	private static int[] moduleGroup = new int[(int)ModuleGroup.GROUP_NUM  + 1]{0,0,0,0,0,0};

	private static GroupType[] typeGroup = new GroupType[(int)ModuleGroup.GROUP_NUM  + 1]{GroupType.None,GroupType.None,GroupType.None,GroupType.None,GroupType.None,GroupType.None};

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
	public static void SendMessage(ModuleEnum module, params object[] args){
		if(moduleDic.ContainsKey(module)){
			moduleDic[module].OnReceiveMessages(args);
		}
	}

	/// <summary>
	/// Shows the module.
	/// </summary>
	/// <param name="name">Name.</param>
	public void ShowModule(ModuleEnum name){
		//hide the prev ui within same group
		int group = (int)DataCenter.Instance.GetConfigUIItem (name).group;
		Debug.Log ("name: " + name + " group: " + group);

		if (typeGroup [group] == GroupType.Module) {
			ModuleEnum prevName = (ModuleEnum)moduleGroup [group];
			if(prevName == name)
				return;
			if (prevName != ModuleEnum.None) {
				HideModule (prevName);
			}	
		}else if(typeGroup[group] == GroupType.Scene){
			SceneEnum prevName = (SceneEnum)moduleGroup [group];
			if (prevName != SceneEnum.None) {
				HideScene (prevName);
			}
		}
			
		if (moduleDic.ContainsKey (name)) {
			moduleDic[name].ShowUI();	
		}else{
			GetOrCreateModule(name).ShowUI();
		}
		ModuleManger.SendMessage (ModuleEnum.PlayerInfoBarModule, name,GroupType.Module);
		moduleGroup [group] = (int)name;
		typeGroup [group] = GroupType.Module;
	}

	/// <summary>
	/// Shows the scene.
	/// </summary>
	/// <param name="name">Name.</param>
	public void ShowScene(SceneEnum name){
//		moduleGroup [(int)DataCenter.Instance.GetConfigUIItem (name).group] = ModuleEnum.None;

		SceneBase scene = null;

		if (sceneDic.ContainsKey (name)) {
			scene = sceneDic[name];
		}else{
			System.Type sceneType = System.Type.GetType(name.ToString());
			scene = Activator.CreateInstance(sceneType, name) as SceneBase;
			if(scene == null){
				Debug.Log ("Scene Create Err: there is no [[[---" + scene + "---]]]");
				return;
			}
			sceneDic.Add(name,scene);
		}

		int group = (int)scene.Group;
		if (typeGroup [group] == GroupType.Module) {
			ModuleEnum prevName = (ModuleEnum)moduleGroup [group];
			if (prevName != ModuleEnum.None) {
				HideModule (prevName);
			}	
		}else if(typeGroup[group] == GroupType.Scene){
			SceneEnum prevName = (SceneEnum)moduleGroup [group];
			if(prevName == name)
				return;
			if (prevName != SceneEnum.None) {
				HideScene (prevName);
			}
		}

		scene.ShowScene();
		ModuleManger.SendMessage (ModuleEnum.PlayerInfoBarModule, name,GroupType.Scene);

		moduleGroup [(int)scene.Group] = (int)name;
		typeGroup [(int)scene.Group] = GroupType.Scene;

	}

	/// <summary>
	/// Hides the scene.
	/// </summary>
	/// <param name="name">Name.</param>
	public void HideScene(SceneEnum name){

		if (sceneDic.ContainsKey (name)) {
			SceneBase scene = sceneDic[name];
			scene.HideScene();
			moduleGroup [(int)scene.Group] = (int)ModuleEnum.None;
			typeGroup[(int)scene.Group] = GroupType.None;
		}
	}
	/// <summary>
	/// Hides the module.
	/// </summary>
	/// <param name="name">Name.</param>
	public void HideModule(ModuleEnum name){
		int group = (int)DataCenter.Instance.GetConfigUIItem (name).group;
		moduleGroup [group] = (int)ModuleEnum.None;
		typeGroup [group] = (int)GroupType.None;

		if (moduleDic.ContainsKey (name)) {
			moduleDic[name].HideUI();
		}
	}

	private void HideModuleOrScene(int group){

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
			if(module == null){
				Debug.LogError ("Module Create Err: there is no [[[---" + module + "---]]]");
				return null;
			}
			moduleDic.Add(name,module);

//			Debug.Log ("Module Create: [[[---" + module + "---]]]");

			return module;
		}
	}

	/// <summary>
	/// Removes the module.
	/// </summary>
	/// <param name="name">Name.</param>
	public void DestroyModule(ModuleEnum name) {

		if (moduleDic.ContainsKey (name)) {
			ModuleBase temp = moduleDic[name];

			int group = (int)temp.UIConfig.group;
			moduleGroup [group] = (int)ModuleEnum.None;
			typeGroup [group] = GroupType.None;

			temp.DestoryUI ();
			moduleDic.Remove(name);
			temp = null;
		}
	}

	public void DestroyScene(SceneEnum name){
		if (sceneDic.ContainsKey (name)) {
			SceneBase temp = sceneDic[name];

			int group = (int)temp.Group;
			moduleGroup [group] = (int)ModuleEnum.None;
			typeGroup [group] = GroupType.None;

			temp.DestoryScene ();
			sceneDic.Remove(name);
			temp = null;
		}
	}

	/// <summary>
	/// Clears the modules.
	/// </summary>
	public void ClearModules () {
		List<ModuleBase> cclist = new List<ModuleBase> ();
		List<ModuleEnum> ccID = new List<ModuleEnum> ();

		for (int i = 0; i < ccID.Count; i++) {
			moduleDic.Remove(ccID[i]);
		}
		for (int i = cclist.Count - 1; i >= 0; i--) {
			//			Debug.LogError("CleartComponent : " + cclist[i]);
			cclist[i].DestoryUI();
		}
		cclist.Clear ();
	}

	/// <summary>
	/// Exits the battle.
	/// </summary>
	public void ExitBattle(){

	}

	/// <summary>
	/// Enters the battle.
	/// </summary>
	public void EnterBattle(){

	}

}


internal enum GroupType{
	None,
	Module,
	Scene,
}
