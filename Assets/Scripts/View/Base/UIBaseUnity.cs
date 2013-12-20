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

	private UIState currentState;

	public UIState GetState 
	{
		get 
		{
			return currentState;
		}
	}

	protected ViewManager vManager;

	protected GameObject tempObject;

	public virtual void Init (string name)
	{
		uiName = name;

		gameObject.name = name;

		currentState = UIState.UIInit;

		vManager = ViewManager.Instance;
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

		vManager.DestoryUI(this);
	}

	/// <summary>
	/// find child script component
	/// </summary>
	/// <returns>The child.</returns>
	/// <param name="path">Path.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>

	protected T FindChild<T>(string path) where T : MonoBehaviour
	{
		if(string.IsNullOrEmpty(path))
			return null;

		return transform.Find(path).GetComponent<T>();
	}

	public virtual void CreatUI ()
	{
		currentState = UIState.UICreat;
	}

	#endregion
}
