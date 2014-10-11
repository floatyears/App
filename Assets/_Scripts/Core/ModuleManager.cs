using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class ModuleManager {

	private static Dictionary<ModuleEnum,ModuleBase> moduleDic = new Dictionary<ModuleEnum, ModuleBase>();

	private static ModuleEnum[] moduleGroup = new ModuleEnum[(int)ModuleGroup.GROUP_NUM  + 1]{ModuleEnum.None,ModuleEnum.None,ModuleEnum.None,ModuleEnum.None};
	
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
		Debug.Log("Show Module: [[[---" + name + "---]]]");
		//hide the prev ui within same group
		ModuleGroup group = DataCenter.Instance.GetConfigUIItem (name).group;
//		Debug.Log ("name: " + name + " group: " + group);

		if (group != ModuleGroup.NONE) {
				ModuleEnum prevName = moduleGroup [(int)group];
				if(prevName == name)
					return;
				if (prevName != ModuleEnum.None) {
					HideModule (prevName);
				}	

			moduleGroup [(int)group] = name;
		}

		ModuleManager.SendMessage (ModuleEnum.SceneInfoBarModule, name);	

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
	/// Hides the module.
	/// </summary>
	/// <param name="name">Name.</param>
	public void HideModule(ModuleEnum name){
		Debug.Log("Hide Module: [[[---" + name + "---]]]");
		ModuleGroup group = DataCenter.Instance.GetConfigUIItem (name).group;
		if (group != ModuleGroup.NONE) {
			moduleGroup [(int)group] = ModuleEnum.None;
		}

		if (moduleDic.ContainsKey (name)) {
			moduleDic[name].HideUI();
		}
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

			ModuleGroup group = temp.UIConfig.group;
			if(group != ModuleGroup.NONE){
				moduleGroup [(int)group] = ModuleEnum.None;
			}

			temp.DestoryUI ();
			moduleDic.Remove(name);
		}
	}

	/// <summary>
	/// Clears the modules.
	/// </summary>
	public void ClearModulesAndScenes () {
	
		foreach (var item in moduleDic.Values) {
			item.DestoryUI();
		}
			//			Debug.LogError("CleartComponent : " + cclist[i]);
		moduleDic.Clear ();

		moduleGroup = new ModuleEnum[(int)ModuleGroup.GROUP_NUM  + 1]{ModuleEnum.None,ModuleEnum.None,ModuleEnum.None,ModuleEnum.None};

	}

	public void EnterMainScene(){
		ModuleManager.Instance.ShowModule(ModuleEnum.MainBackgroundModule);
		ModuleManager.Instance.ShowModule (ModuleEnum.MainMenuModule);
		ModuleManager.instance.ShowModule (ModuleEnum.PlayerInfoBarModule);
		ModuleManager.instance.ShowModule (ModuleEnum.SceneInfoBarModule);

		ModuleManager.Instance.ShowModule(ModuleEnum.HomeModule);
		ModuleManager.instance.GetOrCreateModule (ModuleEnum.MsgWindowModule);
		ModuleManager.instance.GetOrCreateModule (ModuleEnum.MaskModule);
	}

	/// <summary>
	/// Exits the battle.
	/// </summary>
	public void ExitBattle(){
		ClearModulesAndScenes ();

		BattleAttackManager.Instance.ResetSkill();

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

		ShowModule(ModuleEnum.BattleFullScreenTipsModule);
		ShowModule(ModuleEnum.BattleAttackEffectModule);
		GetOrCreateModule (ModuleEnum.MsgWindowModule);
		ShowModule(ModuleEnum.BattleBottomModule);
		ShowModule(ModuleEnum.BattleTopModule);
		ShowModule(ModuleEnum.BattleMapModule);
//
//
//		ShowModule(ModuleEnum.BattleManipulationModule);
		HideModule(ModuleEnum.BattleFullScreenTipsModule);

//		ShowModule(ModuleEnum.BattleSkillModule);

//		HideModule (ModuleEnum.BattleManipulationModule);
//		HideModule (ModuleEnum.BattleSkillModule);
//		HideModule (ModuleEnum.BattleEnemyModule);
//		HideModule (ModuleEnum.BattleAttackEffectModule);
	}

	public bool IsModuleShow(ModuleEnum name){
		if(moduleDic.ContainsKey(name)){
			return moduleDic[name].View.gameObject.activeSelf;
		}
		return false;
	}

	public static bool CheckIsModuleFocusUIEvent(UIConfigItem config){
		if (config.group == ModuleGroup.POPUP || config.moduleName == ModuleEnum.MsgWindowModule || config.moduleName == ModuleEnum.MaskModule) 
			return true;
		return false;
	}
}