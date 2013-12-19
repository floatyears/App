using UnityEngine;
using System.Collections.Generic;

public class ViewManager 
{
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


	public void RegistUIBaseUnity(UIBaseUnity obj)
	{
		if(uiObjectDic.ContainsKey(obj.UIName))
			uiObjectDic[obj.UIName] = obj;
		else
			uiObjectDic.Add(obj.UIName,obj);
	}

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
		GameObject sourceObject = LoadAsset.Instance.LoadAssetFromResources(name,ResourceEuum.Prefab) as GameObject;
	
		GameObject go = NGUITools.AddChild(parentPanel,sourceObject);

		UIBaseUnity goScript = go.GetComponent<UIBaseUnity>();

		goScript.Init(name);

		uiObjectDic.Add(name,goScript);

		return goScript;
	}
}
