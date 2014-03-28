using UnityEngine;
using System.Collections.Generic;

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

	public string UIName {
		get {
			return uiName;
		}
	}

	protected UIState currentState;

	public UIState GetState {
		get {
			return currentState;
		}
	}

	protected ViewManager vManager;

	protected GameObject tempObject;

	public virtual void Init (string name) {
		uiName = name;
		gameObject.name = name;
		currentState = UIState.UIInit;
		vManager = ViewManager.Instance;
	}
	
	public virtual void ShowUI () {
		currentState = UIState.UIShow;
	}
	
	public virtual void HideUI () {
		currentState = UIState.UIHide;
	}
	
	public virtual void DestoryUI () {
		currentState = UIState.UIDestory;

		//vManager.DestoryUI(this);
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

public class UIComponentUnity : MonoBehaviour,IUIComponentUnity,IUICallback {

	protected UIInsConfig config = null;
	public UIInsConfig uiConfig {
		get {
			return config;
		}
	}

	protected IUICallback origin;

	public virtual void Init(UIInsConfig config,IUICallback origin = null) {
		if(this.config == config)
			return;

		this.origin = origin;
		this.config = config;
		if (config.parent != null) {
			transform.parent = config.parent;	
			transform.localScale = Vector3.one;
		}
		InitHide ();
	}

	public virtual void ShowUI() {
		transform.localPosition = config.localPosition;
	}

	public virtual void HideUI() {
		InitHide();
	}

    public virtual void ResetUIState() {
    }

	private void InitHide() {
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

	protected Vector3 CaculateReallyPoint (Vector3 distance, Vector3 parentPosition) {
		Vector3 point = distance + transform.localPosition +  parentPosition;
		Vector3 targetpoint = point * Main.Instance.uiRoot.transform.localScale.y;
		return targetpoint;
	}
	
	public virtual void Callback (object data) {

	}
	
	protected void ExcuteCallback (object data) {
		if (origin != null) {
			origin.Callback (data);	
		}
	}

}











