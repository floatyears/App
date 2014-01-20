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

	private Font dynamicFont;

	public Font DynamicFont
	{
		get{return dynamicFont;}
	}

	public void Init(GameObject ui)
	{
		mainUIRoot = ui;
		
		mainUICamera = mainUIRoot.GetComponentInChildren<UICamera>();
		
		parentPanel = mainUIRoot.transform.Find("Bottom").gameObject;

		topPanel = mainUIRoot.transform.Find ("Top/Panel").gameObject;

		bottomPanel = mainUIRoot.transform.Find ("Bottom/Panel").gameObject;

		centerPanel = mainUIRoot.transform.Find ("Anchor/Panel").gameObject;

		dynamicFont = Resources.Load("Font/Faerytale Woods",typeof(Font)) as Font;
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

	//-----------------------------------------------------------------------------------------------------------------------
	// new 
	//-----------------------------------------------------------------------------------------------------------------------

	private IUIComponent temp = null;
	
	private Dictionary<string,IUIComponent> UIComponentDic = new Dictionary<string, IUIComponent>();
	
	private static Vector3 hidePos = new Vector3(0f,10000f,10000f);
	public static Vector3 HidePos {
		get {
			return hidePos;
		}
	}
	
	public void AddComponent(IUIComponent component) {
		if (component == null) {
			return;	
		}
		
		UIInsConfig config = component.uiConfig;
		string name = config.uiName;
		
		if (UIComponentDic.TryGetValue (name, out temp)) {
			UIComponentDic [name] = component;	
			temp = null;
		}
		else {
			UIComponentDic.Add(name,component);
		}
	}
	
	public IUIComponent GetComponent(string name) {
		if (UIComponentDic.ContainsKey (name)) {
			return UIComponentDic [name];	
		}
		else {
			return null;
		}
	}
	
	public void RemoveComponent(string name) {
		if (!UIComponentDic.ContainsKey (name)) {
			return;
		}
		
		UIComponentDic.Remove (name);
	}
	
	public void DeleteComponent(string name) {
		if (UIComponentDic.TryGetValue (name,out temp)) {
			temp.DestoryUI ();
			UIComponentDic.Remove(name);
			temp = null;
		}
	}
}
