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
	
	private GameObject mainUIRoot;
	
	public GameObject MainUIRoot
	{
		get{return mainUIRoot;}
	}
	
	private GameObject parentPanel;
	
	public GameObject ParentPanel
	{
		get{return parentPanel;}
	}
	
	private UICamera mainUICamera;
	
	public UICamera MainUICamera
	{
		set{mainUICamera = value;}
		get{return mainUICamera;}
	}

	public void Init(GameObject ui)
	{
		mainUIRoot = ui;
		
		mainUICamera = mainUIRoot.GetComponentInChildren<UICamera>();
		
		parentPanel = mainUIRoot.transform.Find("Camera/Anchor/Panel").gameObject;
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
