using UnityEngine;
using System.Collections;

public class UIBaseUnity : MonoBehaviour ,IUIInterface
{
	#region IUIInterface implementation

	protected string uiName;

	public string UIName
	{
		get 
		{
			return uiName;
		}
	}

	protected ViewManager vManager;

	public virtual void Init (string name)
	{
		uiName = name;

		vManager = ViewManager.Instance;
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
