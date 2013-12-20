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

	private GameObject topPanel;

	public GameObject TopPanel
	{
		get{return topPanel;}
	}

	private GameObject bottomPanel;

	public GameObject BottomPanel
	{
		get{return bottomPanel;}
	}
	
	private GameObject centerPanel;
	
	public GameObject CenterPanel
	{
		get{ return centerPanel; }
	}

	private GameObject parentPanel;
	
	public GameObject ParentPanel
	{
		get{ return parentPanel; }
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
		
		parentPanel = mainUIRoot.transform.Find("Bottom").gameObject;

		topPanel = mainUIRoot.transform.Find ("Top/Panel").gameObject;

		bottomPanel = mainUIRoot.transform.Find ("Bottom/Panel").gameObject;

		centerPanel = mainUIRoot.transform.Find ("Anchor/Panel").gameObject;
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
	
		GameObject go = NGUITools.AddChild(centerPanel,sourceObject);

		UIBaseUnity goScript = go.GetComponent<UIBaseUnity>();

		uiObjectDic.Add(name,goScript);

		return goScript;
	}

	public void DestoryUI(UIBaseUnity ui)
	{
		RemoveUI(ui.name);

		GameObject.Destroy(ui.gameObject);
	}

	void RemoveUI(string uiName)
	{
		if(uiObjectDic.ContainsKey(uiName))
			uiObjectDic.Remove(uiName);
	}
}
