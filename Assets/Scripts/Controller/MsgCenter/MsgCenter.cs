using UnityEngine;
using System.Collections.Generic;
using System;

public class MsgCenter 
{
	private static MsgCenter instance;

	private MsgCenter() {	}

	public static MsgCenter Instance {
		get {
			if(instance == null)
				instance = new MsgCenter();
			return instance;
		}
	}
	
	private Dictionary<CommandEnum,Delegate> msgDic = new Dictionary<CommandEnum, Delegate>();
	
	private bool OnAdd(CommandEnum mEnum,Delegate listener) {
		if(!msgDic.ContainsKey(mEnum))
			msgDic.Add(mEnum,null);
		
		Delegate d = msgDic[mEnum];
		
		if(d != null && d.GetType() != listener.GetType())
		{
			return false;
		}
		
		return true;
	}
	
	private bool OnRemove(CommandEnum mEnum, Delegate listener) {
		if(!msgDic.ContainsKey(mEnum))
			return false;
		
		Delegate de = msgDic[mEnum];
		
		if(de == null || de.GetType() != listener.GetType())
			return false;
		
		return true;
	}
	
	private void OnRemoveEnd(CommandEnum mEnum) {
		if(msgDic[mEnum] == null)
			msgDic.Remove(mEnum);
	}
	
	public void AddListener(CommandEnum mEnum,DataListener func) {
		if(OnAdd(mEnum,func)) {
			msgDic[mEnum] = (DataListener)msgDic[mEnum] + func;
		}
	}
	
	public void RemoveListener(CommandEnum mEnum,DataListener func) {
		if(OnRemove(mEnum,func)) {
			msgDic[mEnum] = (DataListener)msgDic[mEnum] - func;
			OnRemoveEnd(mEnum);
		}
	}
	
	public void Invoke(CommandEnum mEnum,object data = null) {
		if(!msgDic.ContainsKey(mEnum))
			return;

		if(msgDic[mEnum] != null) {
			DataListener df = (DataListener)msgDic[mEnum];
			df(data);
		}
	}
}
