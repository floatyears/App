using UnityEngine;
using System.Collections.Generic;

public class UIBase : IUIInterface
{
	#region IUIInterface implementation

	protected ControllerManager controllerManger;

	protected string uiName;

	public string UIName 
	{
		get
		{
			return uiName;
		}
	}

	protected Dictionary<string,UIBaseUnity> currentUIDic;

	public Dictionary<string,UIBaseUnity> CurrentUIDic
	{
		get{return currentUIDic;}
	}

	public GameObject insUIObject;

	public UIBase(string uiName)
	{
		this.uiName = uiName;

		controllerManger = ControllerManager.Instance;
	}

	public virtual void CreatUI ()
	{

	}

	public virtual void ShowUI ()
	{

	}

	public virtual void HideUI ()
	{

	}

	public virtual void DestoryUI ()
	{

	}

	#endregion


}
