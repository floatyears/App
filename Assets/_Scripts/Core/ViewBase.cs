using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ViewBase : MonoBehaviour {

	protected UIConfigItem config;

	protected Dictionary<string, object> viewData;

	public virtual void Init(UIConfigItem uiconfig, Dictionary<string, object> data = null) {

		config = uiconfig;

		if (config != null && config.parent != null) {
			transform.parent = config.parent;	
			transform.localScale = Vector3.one;
			transform.localPosition = new Vector3(-10000,-3000,0);
			gameObject.SetActive(false);
		}

		viewData = data;

	}

	public virtual void ShowUI() {
		if (ModuleManager.CheckIsModuleFocusUIEvent (config)) {
			gameObject.layer = GameLayer.blocker;
			InputManager.Instance.SetBlockWithinModule(config.moduleName,true);
		}
		ToggleAnimation (true);
	}

	public virtual void HideUI() {
		if (ModuleManager.CheckIsModuleFocusUIEvent (config)) {
			InputManager.Instance.SetBlockWithinModule(config.moduleName,false);
		}
		ToggleAnimation (false);
	}
	
	public virtual void DestoryUI() {

		if (viewData != null) {
			viewData.Clear();
			viewData = null;
		}
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
	
	public virtual void CallbackView (params object[] args) {

	}


	protected virtual void ToggleAnimation(bool isShow){
		if (isShow) {
//			Debug.Log("Show Module!: [[[---" + config.moduleName + "---]]]pos: " + config.localPosition.x + " " + config.localPosition.y);
			gameObject.SetActive(true);
			transform.localPosition = new Vector3(config.localPosition.x, config.localPosition.y, 0);
//			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
		}else{
//			Debug.Log("Hide Module!: [[[---" + config.moduleName + "---]]]");
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
//			iTween.MoveTo(gameObject, iTween.Hash("x", -1000, "time", 0.4f, "islocal", true,"oncomplete","AnimationComplete","oncompletetarget",gameObject));
		}

	}

	protected void AnimationComplete(){
		gameObject.SetActive (false);
	}

	public void SetViewData(Dictionary<string, object> data){
		viewData = data;
	}


}