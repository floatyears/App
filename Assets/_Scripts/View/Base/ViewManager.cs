using UnityEngine;
using System.Collections.Generic;

public class ViewManager {
	private static ViewManager instance;

	public static ViewManager Instance {
		get {
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

	private GameObject popupPanel;
	
	public GameObject PopupPanel
	{
		get{ return popupPanel; }
	}

	private GameObject bottomLeftPanel;
	public GameObject BottomLeftPanel {
		get { return bottomLeftPanel; }
	}

	public int manualHeight;

	private GameObject effectPanel;
	public GameObject EffectPanel {
		get { return effectPanel; }
	}
	
	private UICamera mainUICamera;
	
	public UICamera MainUICamera
	{
		set{mainUICamera = value;}
		get{return mainUICamera;}
	}

	private UICamera battleCamera;
	public UICamera BattleCamera {
		set { battleCamera = value; }
		get { return battleCamera; }
	}

	private GameObject battleBottom;
	/// <summary>
	/// battle bottom parent object.
	/// </summary>
	/// <value>The battle bottom.</value>
	public GameObject BattleBottom {
		get { return battleBottom; }
		set { battleBottom = value; }
	}

//	private Font dynamicFont;
//
//	public Font DynamicFont {
//		get{return dynamicFont;}
//	}

	private static Vector3 hidePos = new Vector3(0f,10000f,10000f);
	public static Vector3 HidePos {
		get {
			return hidePos;
		}
	}


	private GameObject popUpBg;

	public void Init(GameObject ui){
		mainUIRoot = ui;		
		mainUICamera = mainUIRoot.GetComponentInChildren<UICamera>();
		Transform trans = mainUIRoot.transform;
		parentPanel = trans.Find("Bottom").gameObject;
		popupPanel = trans.Find ("PopUp").gameObject;
		popUpBg = trans.Find ("PopUp/BackGround").gameObject;
		popUpBg.SetActive (false);
		topPanel = trans.Find ("Top").gameObject;
		bottomPanel = trans.Find ("Bottom").gameObject;
		effectPanel = trans.Find ("Anchor/EffectPanel").gameObject;
		centerPanel = trans.Find ("Center").gameObject;


		bottomLeftPanel =  trans.Find ("BottomLeft").gameObject;

//		ResourceManager.Instance.LoadLocalAsset("Font/Dimbo Regular", o =>{
//			dynamicFont = o as Font;
//			manualHeight = mainUIRoot.GetComponent<UIRoot>().manualHeight;
//		}
//		);
	}
	
	public void TogglePopUpWindow(bool show){
		popUpBg.SetActive(show);
	}


//	private Dictionary<string,UIBaseUnity> uiObjectDic = new Dictionary<string, UIBaseUnity>();

//	public void RegistUIBaseUnity(UIComponentUnity obj) {
//		if(uiObjectDic.ContainsKey(obj.uiConfig.uiName))
//			uiObjectDic[obj.uiConfig.uiName] = obj;
//		else
//			uiObjectDic.Add(obj.uiConfig.uiName,obj);
//	}

//	public void GetViewObject(string name, ResourceCallback callback) {
////		if(uiObjectDic.ContainsKey(name)) {	
////			return uiObjectDic[name];
////		}
//		CreatObject(name, callback);
//	}
//
//	public void GetBattleMap (string name, ResourceCallback callback) {
//		CreatNoUIObject (name,callback);
//	}
//
//	void CreatNoUIObject (string name,ResourceCallback callback) {
//
//		ResourceManager.Instance.LoadLocalAsset ("Prefabs/" + name, o=>{
//			GameObject go = GameObject.Instantiate (o) as GameObject;
//			ViewBase goScript = go.GetComponent<ViewBase>();
//			callback(goScript);
//		});
//
//
//
////		uiObjectDic.Add(name,goScript);
//	}
//
//	void CreatObject(string name,ResourceCallback callback) {	
//		ResourceManager.Instance.LoadLocalAsset ("Prefabs/" + name, o => {
//			GameObject sourceObject = o as GameObject;
//			GameObject go = NGUITools.AddChild (centerPanel, sourceObject);
//			ViewBase goScript = go.GetComponent<ViewBase> ();
////			uiObjectDic.Add(name,goScript);
//			callback (goScript);
//		});
//
//	}

//	public void DestoryUI(UIComponentUnity ui) {
////		RemoveUI(ui.uiConfig.uiName);
//		GameObject.Destroy(ui.gameObject);
//	}

//	void RemoveUI(string uiName)
//	{
//		if(uiObjectDic.ContainsKey(uiName))
//			uiObjectDic.Remove(uiName);
//	}

	//-----------------------------------------------------------------------------------------------------------------------
	// new 
	//-----------------------------------------------------------------------------------------------------------------------

//	private IUIComponent temp = null;

}
