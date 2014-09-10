using UnityEngine;
using System.Collections;

public class MessageAdapt : MonoBehaviour {

	public const string MessageCallbackName = "MessageCallback";

	private Callback DelegateCallback;

	public void AddCallback(Callback callBack) {
		DelegateCallback = callBack;
	}

	public void MessageCallback () {
		if (DelegateCallback != null) {
			DelegateCallback ();
		}
	}

}
