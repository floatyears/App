
using System.Collections.Generic;

//-----------------------------------------------------------------------------------------

public class CommandBase {
//	private MsgCenter msgCenter;
//
//	protected List<CommandEnum> listenerCommandList ;
//
//	public CommandBase(){
//		msgCenter = MsgCenter.Instance;
//
//		listenerCommandList = new List<CommandEnum> ();
//	}
//
//	public virtual void RegisterCommand(CommandEnum command,DataListener callback) {
//		msgCenter.AddListener (command,callback);
//		listenerCommandList.Add (command);
//	}
//
//	public virtual void InvokeCommand(CommandEnum command,object data) {
//		msgCenter.Invoke (command, data);
//	}
//
//	public virtual void ClearCommand() {
//		for (int i = 0; i < listenerCommandList.Count; i++) {
//			msgCenter.RemoveListener(listenerCommandList[i]);
//			listenerCommandList.RemoveAt(i);
//		}
//	}
//
//	public virtual void RemoveCommand(CommandEnum command, DataListener func) {
//		if (listenerCommandList.Contains (command)) {
//			listenerCommandList.Remove(command);
//			msgCenter.RemoveListener(command,func);
//		}
//	}

}

public class SingleCommandBase {
	private MsgCenter msgCenter;

	private KeyValuePair<CommandEnum,DataListener> singleCommand;

	private CommandEnum excuteCommand;

	public SingleCommandBase() {
		msgCenter = MsgCenter.Instance;
	}

	public virtual void ExcuteCommand() {
		
	}
}
//-----------------------------------------------------------------------------------------