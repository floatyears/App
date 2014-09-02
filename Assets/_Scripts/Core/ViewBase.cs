using UnityEngine;
using System.Collections.Generic;

public class ViewBase : MonoBehaviour {

	protected UIConfigItem config;

	public virtual void Init(UIConfigItem uiconfig) {

		config = uiconfig;

		if (config != null && config.parent != null) {
			transform.parent = config.parent;	
			transform.localScale = Vector3.one;
		}

	}

	public virtual void ShowUI() {
		ToggleAnimation (true);
	}

	public virtual void HideUI() {
		ToggleAnimation (false);
	}


	public virtual void DestoryUI() {
		Destroy (gameObject);
	}

	protected T FindChild<T> (string path) where T : Component {
		if (!string.IsNullOrEmpty (path)) {
			Transform trans = transform.FindChild(path);

			if(trans == null) {
				Debug.LogError("find child is not exist . path is " + path);
				return default(T);
			}

			return transform.FindChild(path).GetComponent<T>();
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
	
	public virtual void CallbackView (object data) {

	}

	
	protected void ToggleAnimation(bool isShow){
		if (isShow) {
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);
			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
		}else{
//			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);
			iTween.MoveTo(gameObject, iTween.Hash("x", -1000, "time", 0.4f, "islocal", true));
		}

	}

	protected void SetGameObjectActive(bool active) {
		if (gameObject.activeSelf != active) {
			gameObject.SetActive(active);		
		}
	}

}