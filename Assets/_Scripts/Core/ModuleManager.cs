using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class ModuleManager {

	private static Dictionary<ModuleEnum,ModuleBase> moduleDic = new Dictionary<ModuleEnum, ModuleBase>();

	private static Dictionary<SceneEnum, SceneBase> sceneDic = new Dictionary<SceneEnum, SceneBase>();

	private static int[] moduleGroup = new int[(int)ModuleGroup.GROUP_NUM  + 1]{0,0,0,0,0,0,0,0};

	private static GroupType[] typeGroup = new GroupType[(int)ModuleGroup.GROUP_NUM  + 1]{GroupType.None,GroupType.None,GroupType.None,GroupType.None,GroupType.None,GroupType.None,GroupType.None,GroupType.None};

	private static ModuleManager instance;

	public static ModuleManager Instance{
		get{
			if(instance == null){
				instance = new ModuleManager();
			}
			return instance;
		}
	}
	private ModuleManager(){

	}

	/// <summary>
	/// Sends messages to the specified module.
	/// </summary>
	/// <param name="module">Module.</param>
	/// <param name="data">Data.</param>
	public static void SendMessage(ModuleEnum module, params object[] args){

		if(moduleDic.ContainsKey(module)){
			Debug.Log ("Msg Send to: [[[---" + module + "---]]] args: " + args[0].ToString());
			moduleDic[module].OnReceiveMessages(args);

		}else{
			Debug.LogWarning("SendMsg To Module Err: no reciever [[[---" + module + "---]]]");
		}
	}

	/// <summary>
	/// Shows the module.
	/// </summary>
	/// <param name="name">Name.</param>
	public void ShowModule(ModuleEnum name, params object[] args){
		if (name == ModuleEnum.None)
			return;
//		Debug.Log("Show Module: [[[---" + name + "---]]]");
		//hide the prev ui within same group
		int group = (int)DataCenter.Instance.GetConfigUIItem (name).group;
//		Debug.Log ("name: " + name + " group: " + group);

		if (group != (int)ModuleGroup.NONE) {
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

			moduleGroup [group] = (int)name;
			typeGroup [group] = GroupType.Module;
		}

		ModuleManager.SendMessage (ModuleEnum.SceneInfoBarModule, name,ModuleOrScene.Module);	

		if (moduleDic.ContainsKey (name)) {
			if(args.Length > 0){
				moduleDic[name].SetModuleData(args);
			}
			moduleDic[name].ShowUI();	
		}else{
			GetOrCreateModule(name,args).ShowUI();
		}


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
		if (group != (int)ModuleGroup.NONE) {
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

			moduleGroup [(int)scene.Group] = (int)name;
			typeGroup [(int)scene.Group] = GroupType.Scene;
		}

		ModuleManager.SendMessage (ModuleEnum.SceneInfoBarModule, name,ModuleOrScene.Scene);
		scene.ShowScene();


	}

	/// <summary>
	/// Hides the scene.
	/// </summary>
	/// <param name="name">Name.</param>
	public void HideScene(SceneEnum name){

		if (sceneDic.ContainsKey (name)) {
			SceneBase scene = sceneDic[name];
			scene.HideScene();
			if(scene.Group != ModuleGroup.NONE){
				moduleGroup [(int)scene.Group] = (int)ModuleEnum.None;
				typeGroup[(int)scene.Group] = GroupType.None;
			}

		}
	}
	/// <summary>
	/// Hides the module.
	/// </summary>
	/// <param name="name">Name.</param>
	public void HideModule(ModuleEnum name){
//		Debug.Log("Hide Module: [[[---" + name + "---]]]");
		int group = (int)DataCenter.Instance.GetConfigUIItem (name).group;
		if (group != (int)ModuleGroup.NONE) {
			moduleGroup [group] = (int)ModuleEnum.None;
			typeGroup [group] = (int)GroupType.None;	
		}

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
	public ModuleBase GetOrCreateModule(ModuleEnum name, params object[] args) {
		if (moduleDic.ContainsKey (name)) {
			return moduleDic [name];	
		}
		else {
			System.Type moduleType = System.Type.GetType(name.ToString());
			ModuleBase module = null;
			if(args.Length > 0){
				module = Activator.CreateInstance(moduleType, DataCenter.Instance.GetConfigUIItem(name),args) as ModuleBase;
			}else{
				module = Activator.CreateInstance(moduleType, DataCenter.Instance.GetConfigUIItem(name)) as ModuleBase;
			}
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

//	public void AddModuleToDic(ModuleBase module){
//		moduleDic.Add (module.UIConfig.moduleName, module);
//	}

	public T GetModule<T>(ModuleEnum name) where T : ModuleBase{
		if (moduleDic.ContainsKey (name)) {
			return moduleDic[name] as T;
		}
		return null;
	}

	/// <summary>
	/// Removes the module.
	/// </summary>
	/// <param name="name">Name.</param>
	public void DestroyModule(ModuleEnum name) {

		if (moduleDic.ContainsKey (name)) {
			ModuleBase temp = moduleDic[name];

			int group = (int)temp.UIConfig.group;
			if(group != (int)ModuleGroup.NONE){
				moduleGroup [group] = (int)ModuleEnum.None;
				typeGroup [group] = GroupType.None;
			}

			temp.DestoryUI ();
			moduleDic.Remove(name);
		}
	}

	public void DestroyScene(SceneEnum name){
		if (sceneDic.ContainsKey (name)) {
			SceneBase temp = sceneDic[name];

			int group = (int)temp.Group;
			if(temp.Group != ModuleGroup.NONE){
				moduleGroup [group] = (int)ModuleEnum.None;
				typeGroup [group] = GroupType.None;
			}

			temp.DestoryScene ();
			sceneDic.Remove(name);
		}
	}

	/// <summary>
	/// Clears the modules.
	/// </summary>
	public void ClearModulesAndScenes () {

		foreach (var item in sceneDic.Values) {
			item.DestoryScene();
		}

		foreach (var item in moduleDic.Values) {
			item.DestoryUI();
		}
			//			Debug.LogError("CleartComponent : " + cclist[i]);
		moduleDic.Clear ();
		sceneDic.Clear ();

		moduleGroup = new int[(int)ModuleGroup.GROUP_NUM  + 1]{0,0,0,0,0,0,0,0};
		
		typeGroup = new GroupType[(int)ModuleGroup.GROUP_NUM  + 1]{GroupType.None,GroupType.None,GroupType.None,GroupType.None,GroupType.None,GroupType.None,GroupType.None,GroupType.None};

	}

	public void EnterMainScene(){
		ModuleManager.Instance.ShowModule(ModuleEnum.MainBackgroundModule);
		ModuleManager.Instance.ShowScene(SceneEnum.MainScene);
		ModuleManager.Instance.ShowModule(ModuleEnum.SceneInfoBarModule);
		ModuleManager.Instance.ShowModule(ModuleEnum.HomeModule);
		ModuleManager.instance.GetOrCreateModule (ModuleEnum.MsgWindowModule);
		ModuleManager.instance.GetOrCreateModule (ModuleEnum.MaskModule);
	}

	/// <summary>
	/// Exits the battle.
	/// </summary>
	public void ExitBattle(){
		ClearModulesAndScenes ();
		BattleConfigData.Instance.ClearData ();
	}

	/// <summary>
	/// Enters the battle.
	/// </summary>
	public void EnterBattle(){
//		ClearAllUIObject ();
		ClearModulesAndScenes ();
		Resources.UnloadUnusedAssets ();

		MsgCenter.Instance.Invoke (CommandEnum.EnterBattle, null);

//		ShowScene (SceneEnum.BattleScene);

		GetOrCreateModule(ModuleEnum.BattleFullScreenTipsModule);
		GetOrCreateModule(ModuleEnum.BattleAttackEffectModule);
		GetOrCreateModule (ModuleEnum.MsgWindowModule);
		ShowModule(ModuleEnum.BattleBottomModule);
		ShowModule(ModuleEnum.BattleTopModule);
		ShowModule(ModuleEnum.BattleMapModule);
//
//
//		ShowModule(ModuleEnum.BattleManipulationModule);


//		ShowModule(ModuleEnum.BattleSkillModule);

//		HideModule (ModuleEnum.BattleManipulationModule);
//		HideModule (ModuleEnum.BattleSkillModule);
//		HideModule (ModuleEnum.BattleEnemyModule);
//		HideModule (ModuleEnum.BattleAttackEffectModule);
	}

}


internal enum GroupType{
	None,
	Module,
	Scene,
}
