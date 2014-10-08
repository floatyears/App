using UnityEngine;
using System.Collections;

public class TaskModule : ModuleBase {
	
	public TaskModule(UIConfigItem config):base(config){
		CreateUI<TaskView> ();
	}
}
