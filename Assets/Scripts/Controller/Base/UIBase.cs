using UnityEngine;
using System.Collections.Generic;

public class UIBase : IUIInterface
{
	#region IUIInterface implementation

	protected ControllerManager controllerManger;

	protected Main main;

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

	protected Dictionary<string,UIBaseUnity> currentUIDic = new Dictionary<string, UIBaseUnity>();

	public Dictionary<string,UIBaseUnity> CurrentUIDic
	{
		get{return currentUIDic;}
	}

	public GameObject insUIObject;

	public UIBase(string uiName)
	{
		this.uiName = uiName;

		currentState = UIState.UIInit;

		main = Main.Instance;

		controllerManger = ControllerManager.Instance;
	}

	public virtual void CreatUI ()
	{
		currentState = UIState.UICreat;
	}

	public virtual void ShowUI ()
	{
		currentState = UIState.UIShow;
	}

	public virtual void HideUI ()
	{
		currentState = UIState.UIHide;
	}

	public virtual void DestoryUI ()
	{
		currentState = UIState.UIDestory;
	}

	#endregion


}
