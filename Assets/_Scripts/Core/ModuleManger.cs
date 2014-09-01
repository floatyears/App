using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
}
