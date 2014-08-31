//using UnityEngine;
//using System.Collections.Generic;
//
//public class UIBase : IUIInterface
//{
//	#region IUIInterface implementation
//
//	protected ControllerManager controllerManger;
//
//	protected Main main;
//
//	protected ViewManager viewManager;
//
//	protected string uiName;
//
//	public string UIName 
//	{
//		get
//		{
//			return uiName;
//		}
//	}
//
//	private UIState currentState;
//
//	public UIState GetState
//	{
//		get
//		{
//			return currentState;
//		}
//	}
//
//	#region IUIInterface implementation
//	protected SceneEnum sEnum;
//	public SceneEnum GetScene {
//		get {
//			return sEnum;
//		}
//		set{
//			sEnum = value;
//		}
//	}
//
//	#endregion
//
//	protected Dictionary<string,IUIInterface> currentUIDic = new Dictionary<string, IUIInterface>();
//
//	public Dictionary<string,IUIInterface> CurrentUIDic {
//		get{return currentUIDic;}
//	}
//
//	protected void AddSelfObject(IUIInterface ui) {
//		currentUIDic.Add (ui.UIName, ui);
//	}
//
//	public GameObject insUIObject;
//
//	protected int initCount = 0;
////	protected bool battleInitEnd = false;
//
//	protected void BattleInitEnd () {
//		initCount++;
//	}
//
//	public UIBase(string uiName) {
//		GameInput.OnUpdate += InitEnd;
//
//		initCount = 0;
//		this.uiName = uiName;
//		currentState = UIState.UIInit;
//		main = Main.Instance;
//		controllerManger = ControllerManager.Instance;
//		viewManager = ViewManager.Instance;
//	}
//
//	void InitEnd() {
//		if (initCount==5) {
//			GameInput.OnUpdate -= InitEnd;
//			ShowUI();
//		}
//	}
//
//	public virtual void CreatUI () {
//		currentState = UIState.UICreat;
//
//		foreach (var item in currentUIDic.Values){
//			item.CreatUI();
//		}
//	}
//
//	public virtual void ShowUI () {
//		currentState = UIState.UIShow;
//
//		foreach (var item in currentUIDic.Values){
//			item.ShowUI();
//		}
//	}
//
//	public virtual void HideUI () {
//		currentState = UIState.UIHide;
//		foreach (var item in currentUIDic.Values){
//			item.HideUI();
//		}
//	}
//
//	public virtual void DestoryUI () {
//		currentState = UIState.UIDestory;
//		foreach (var item in currentUIDic.Values){
//			item.DestoryUI();
//		}
//		currentUIDic.Clear ();
//	}
//
//	protected void ChangeScene(SceneEnum se) {
//		controllerManger.ChangeScene(se);
//	}
//
//	#endregion
//
//
//}
