using UnityEngine;
using System.Collections.Generic;

public class UIBase : IUIInterface
{
	#region IUIInterface implementation

	protected ControllerManager controllerManger;

	protected Main main;

	protected ViewManager viewManager;

	protected string uiName;

	public string UIName 
	{
		get
		{
			return uiName;
		}
	}

	private UIState currentState;

	public UIState GetState
	{
		get
		{
			return currentState;
		}
	}

	protected Dictionary<string,IUIInterface> currentUIDic = new Dictionary<string, IUIInterface>();

	public Dictionary<string,IUIInterface> CurrentUIDic
	{
		get{return currentUIDic;}
	}

	protected void AddSelfObject(IUIInterface ui)
	{
		currentUIDic.Add (ui.UIName, ui);
	}

	public GameObject insUIObject;

	public UIBase(string uiName)
	{
		this.uiName = uiName;

		currentState = UIState.UIInit;

		main = Main.Instance;

		controllerManger = ControllerManager.Instance;

		viewManager = ViewManager.Instance;
	}

	public virtual void CreatUI ()
	{
		currentState = UIState.UICreat;

		foreach (var item in currentUIDic.Values){

			item.CreatUI();
				}
	}

	public virtual void ShowUI ()
	{
		currentState = UIState.UIShow;

		foreach (var item in currentUIDic.Values){
			item.ShowUI();
		}
	}

	public virtual void HideUI ()
	{
		currentState = UIState.UIHide;
		foreach (var item in currentUIDic.Values){
			item.HideUI();
		}
	}

	public virtual void DestoryUI ()
	{
		currentState = UIState.UIDestory;
		foreach (var item in currentUIDic.Values){
			item.DestoryUI();
		}
	}

	#endregion


}
