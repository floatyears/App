using UnityEngine;
using System.Collections.Generic;
using System;

public class MsgCenter 
{
	private static MsgCenter instance;

	private MsgCenter() {	}

	public static MsgCenter Instance
	{
		get
		{
			if(instance == null)
				instance = new MsgCenter();
			return instance;
		}
	}
	
	private Dictionary<DataEnum,Delegate> msgDic = new Dictionary<DataEnum, Delegate>();
	
	private bool OnAdd(DataEnum mEnum,Delegate listener)
	{
		if(!msgDic.ContainsKey(mEnum))
			msgDic.Add(mEnum,null);
		
		Delegate d = msgDic[mEnum];
		
		if(d != null && d.GetType() != listener.GetType())
		{
			return false;
		}
		
		return true;
	}
	
	private bool OnRemove(DataEnum mEnum, Delegate listener)
	{
		if(!msgDic.ContainsKey(mEnum))
			return false;
		
		Delegate de = msgDic[mEnum];
		
		if(de == null || de.GetType() != listener.GetType())
			return false;
		
		return true;
	}
	
	private void OnRemoveEnd(DataEnum mEnum)
	{
		if(msgDic[mEnum] == null)
			msgDic.Remove(mEnum);
	}
	
	public void AddListener(DataEnum mEnum,DataListener func)
	{
		if(OnAdd(mEnum,func))
		{
			msgDic[mEnum] = (DataListener)msgDic[mEnum] + func;
		}
	}
	
	public void RemoveNoParameterMsg(DataEnum mEnum,DataListener func)
	{
		if(OnRemove(mEnum,func))
		{
			msgDic[mEnum] = (DataListener)msgDic[mEnum] - func;
			OnRemoveEnd(mEnum);
		}
	}
	
	public void Invoke(DataEnum mEnum,object data)
	{
		if(!msgDic.ContainsKey(mEnum))
			return;

		if(msgDic[mEnum] != null)
		{
			DataListener df = (DataListener)msgDic[mEnum];
			df(data);
		}
	}
}
