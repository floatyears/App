using UnityEngine;
using System.Collections;

public class UIEventListenerCustom : MonoBehaviour {
	public delegate void VoidDelegate (GameObject go);
	public delegate void BoolDelegate (GameObject go, bool state);
	public delegate void FloatDelegate (GameObject go, float delta);
	public delegate void VectorDelegate (GameObject go, Vector2 delta);
	public delegate void StringDelegate (GameObject go, string text);
	public delegate void ObjectDelegate (GameObject go, GameObject draggedObject);
	public delegate void KeyCodeDelegate (GameObject go, KeyCode key);
	
	public object parameter;
	
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

	public bool ShieldInput = false;

	public delegate void LongPressDelegate(GameObject go);
	public LongPressDelegate  LongPress;
	void OnSubmit ()				{ if (ShieldInput) return; if (onSubmit != null) onSubmit(gameObject); }
	void OnDoubleClick ()			{ if (ShieldInput) return; if (onDoubleClick != null) onDoubleClick(gameObject); }
	void OnHover (bool isOver)		{ if (ShieldInput) return; if (onHover != null) onHover(gameObject, isOver);}
	void OnSelect (bool selected)	{ if (ShieldInput) return; if (onSelect != null) onSelect(gameObject, selected); }
	void OnScroll (float delta)		{ if (ShieldInput) return; if (onScroll != null) onScroll(gameObject, delta); }
	void OnDrag (Vector2 delta)		{ if (ShieldInput) return; GameTimer.GetInstance ().ExitCountDonw(CountDown); if (onDrag != null) onDrag(gameObject, delta); }
	void OnDrop (GameObject go)		{ if (ShieldInput) return; if (onDrop != null) onDrop(gameObject, go); }
	void OnInput (string text)		{ if (ShieldInput) return; if (onInput != null) onInput(gameObject, text); }
	void OnKey (KeyCode key)		{ if (ShieldInput) return; if (onKey != null) onKey(gameObject, key); }
	
	void OnPress (bool isPressed) { 
		if (onPress != null) {
			onPress(gameObject, isPressed); 	
		}
		
		if (isPressed) {
			GameTimer.GetInstance ().AddCountDown (0.6f, CountDown);
		} else {
			bool b =GameTimer.GetInstance ().ExitCountDonw(CountDown);
			Debug.LogError("b : " + b + " onclick : " + onClick);
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
	
	static public UIEventListenerCustom Get (GameObject go)
	{
		UIEventListenerCustom listener = go.GetComponent<UIEventListenerCustom>();
		if (listener == null) listener = go.AddComponent<UIEventListenerCustom>();
		return listener;
	}
}