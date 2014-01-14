using UnityEngine;
using System.Collections;

public class UIBaseUnity : MonoBehaviour ,IUIInterface
{
	#region IUIInterface implementation
	protected SceneEnum sEnum;

	public SceneEnum GetScene {
		get {
			return sEnum;
		}
		set{
			sEnum = value;
		}
	}

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

	protected T FindChild<T>(string path) where T : Component
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

public class UIComponentUnity : MonoBehaviour,IUIComponentUnity {
	protected UIInsConfig config = null;
	public UIInsConfig UIConfig {
		get {
			return config;
		}
	}

	protected IUIOrigin origin;

	public virtual void Init(UIInsConfig config,IUIOrigin origin = null) {
		if(this.config == config)
			return;

		this.origin = origin;
		this.config = config;
		if (config.parent != null) {
			transform.parent = config.parent;	
			transform.localScale = Vector3.one;
		}

		HideUI ();
	}

	public virtual void ShowUI() {
		transform.localPosition = config.localPosition;
	}

	public virtual void HideUI() {
		transform.localPosition = ViewManager.HidePos;
	}

	public virtual void DestoryUI() {
		Destroy (gameObject);
	}

	protected T FindChild<T> (string path) where T : Component {
		if (!string.IsNullOrEmpty (path)) {
			return transform.Find(path).GetComponent<T>();
		}

		return GetComponent<T>();
	}

	protected GameObject FindChild(string path) {
		if (!string.IsNullOrEmpty (path)) {
			return transform.Find(path).gameObject;
		}

		return gameObject;
	}

	public static T FindChild<T> (GameObject root, string path) where T : Component {
		if (root == null || string.IsNullOrEmpty (path)) {
			return null;
		}

		return root.transform.Find (path).GetComponent<T> ();
	}
}
