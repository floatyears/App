using UnityEngine;
using System.Collections.Generic;

public class ViewManager 
{
	private string path = "Prefabs/"; 

	private static ViewManager instance;

	public static ViewManager Instance
	{
		get
		{
			if(instance == null)
				instance = new ViewManager();
			return instance;
		}
	}

	private Dictionary<string,UIBaseUnity> uiObjectDic = new Dictionary<string, UIBaseUnity>();

	public UIBaseUnity GetViewObject(string name)
	{
		if(uiObjectDic.ContainsKey(name))
		{	
			return uiObjectDic[name];
		}

		return CreatObject(name);
	}

	UIBaseUnity CreatObject(string name)
	{
		string reallyPath = path + name;
		
		Object sourceObject = Resources.Load(reallyPath);
		
		GameObject go = GameObject.Instantiate(sourceObject) as GameObject;
		
		UIBaseUnity goScript = go.GetComponent<UIBaseUnity>();

		goScript.Init(name);

		uiObjectDic.Add(name,goScript);

		return goScript;
	}
}
