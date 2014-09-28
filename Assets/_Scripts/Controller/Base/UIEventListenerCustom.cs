using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIEventListenerCustom : MonoBehaviour {

	private static List<ModuleEnum> FocusModules = new List<ModuleEnum>();

	public delegate void VoidDelegate (GameObject go);
	public delegate void BoolDelegate (GameObject go, bool state);
	public delegate void FloatDelegate (GameObject go, float delta);
	public delegate void VectorDelegate (GameObject go, Vector2 delta);
	public delegate void StringDelegate (GameObject go, string text);
	public delegate void ObjectDelegate (GameObject go, GameObject draggedObject);
	public delegate void KeyCodeDelegate (GameObject go, KeyCode key);
	
	public VoidDelegate onSubmit;
	public VoidDelegate onClick;
	public VoidDelegate onDoubleClick;
	public BoolDelegate onHover;
	public BoolDelegate onPress;
	public BoolDelegate onSelect;
	public FloatDelegate onScroll;
	public VectorDelegate onDrag;
	public ObjectDelegate onDrop;
	public StringDelegate onInput;
	public KeyCodeDelegate onKey;

	public delegate void LongPressDelegate(GameObject go);
	public LongPressDelegate  LongPress;


	void OnSubmit (){ 
		if (onSubmit != null && CheckFocus()) 
			onSubmit(gameObject); 
	}

	void OnDoubleClick (){ 
		if (onDoubleClick != null && CheckFocus()) 
			onDoubleClick(gameObject); 
	}

	void OnHover (bool isOver){ 
		if (onHover != null && CheckFocus()) 
			onHover(gameObject, isOver);
	}

	void OnSelect (bool selected){ 
		if (onSelect != null && CheckFocus()) 
			onSelect(gameObject, selected); 
	}

	void OnScroll (float delta){ 
		if (onScroll != null && CheckFocus()) 
			onScroll(gameObject, delta); 
	}

	void OnDrag (Vector2 delta){ 

		GameTimer.GetInstance ().ExitCountDonw(CountDown);
		if (onDrag != null && CheckFocus())
			onDrag(gameObject, delta); 
	}

	void OnDrop (GameObject go){ 
		if (onDrop != null && CheckFocus()) 
			onDrop(gameObject, go); 
	}

	void OnInput (string text){ 
		if (onInput != null && CheckFocus()) 
			onInput(gameObject, text); 
	}

	void OnKey (KeyCode key){ 
		if (onKey != null && CheckFocus()) 
			onKey(gameObject, key); 
	}
	
	void OnPress (bool isPressed) { 

		if (!CheckFocus ())
			return;
		if (onPress != null) {
			onPress(gameObject, isPressed); 	
		}
		
		if (isPressed) {
			GameTimer.GetInstance ().AddCountDown (0.6f, CountDown);
		} else {
			bool b =GameTimer.GetInstance ().ExitCountDonw(CountDown);
//			Debug.LogError("b : " + b + " onclick : " + onClick);
			if(b){
				if (onClick != null) onClick (gameObject);
			}
		}
	}
	
	void CountDown() {
		if (LongPress != null) {
			LongPress(gameObject);
		}
	}

	bool CheckFocus(){
		Debug.Log ("check ui focus");
		if (FocusModules.Count == 0)
			return true;
		foreach (var item in gameObject.GetComponentsInParent<Transform>()) {
			foreach (var module in FocusModules) {
				if(item == ModuleManager.Instance.GetModule<ModuleBase>(module).View.transform){
					return true;
				}
			}
		}
	   	return false;
	}
	
	static public UIEventListenerCustom Get (GameObject go)
	{
		UIEventListenerCustom listener = go.GetComponent<UIEventListenerCustom>();
		if (listener == null) listener = go.AddComponent<UIEventListenerCustom>();
		return listener;
	}

	static public void SetFocusModule(ModuleEnum module, bool isFocus){
		if (isFocus) {
			if(!FocusModules.Contains(module)){
				FocusModules.Add(module);
			}	
		}else{
			if(FocusModules.Contains(module)){
				FocusModules.Remove(module);
			}
		}
	}
}