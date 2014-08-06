using UnityEngine;
using System.Collections;

public class GameInput : MonoBehaviour  {
	public static event System.Action OnPressEvent;
	public static event System.Action OnReleaseEvent;
	public static event System.Action<Vector2> OnDragEvent;
	public static event System.Action OnStationaryEvent;
	public static event System.Action OnUpdate;
	public static event System.Action OnLateUpdate;
	public static event System.Action OnPressContinued;
	private bool isCheckInput = false;
	public bool IsCheckInput {
		set{ isCheckInput = value; }//Debug.LogError("set ischeck input : " + isCheckInput); }
		get{ return isCheckInput; }
	}

	private bool _noviceGInput = true;
	private bool noviceGuideShileInput {
		set { _noviceGInput = value; }//Debug.LogError("_noviceGInput : " + _noviceGInput  + " time : " + Time.realtimeSinceStartup); }
		get { return _noviceGInput; }
	}

	/// <summary>
	/// shield all custom input.
	/// </summary>
	private bool shieldInput = false;

	private Vector2 lastPosition = Vector2.zero;

	private Vector2 currentPosition = Vector2.zero;

	private Vector2 deltaPosition = Vector2.zero;

	private float startTime = -1f;

	private float stationarIntervTime = 2f;
	
	void OnEnable () {
		Application.RegisterLogCallback (CatchException);
		MsgCenter.Instance.AddListener (CommandEnum.StopInput, StopInput);
		MsgCenter.Instance.AddListener (CommandEnum.ShiledInput, ShiledInput);
	}

	void OnDisable () {
		Application.RegisterLogCallback (null);
		MsgCenter.Instance.RemoveListener (CommandEnum.StopInput, StopInput);
		MsgCenter.Instance.RemoveListener (CommandEnum.ShiledInput, ShiledInput);
	}

	void StopInput(object data) {
		if (data == null) {
			IsCheckInput = false;
			return;
		}
		bool b = (bool)data;
		IsCheckInput = b;
	}

	void ShiledInput(object data) {
		bool sInput = (bool)data;

		noviceGuideShileInput = !sInput;
		BattleBottom.noviceGuideNotClick = sInput;
	}

	void CatchException(string condition, string stackInfo, LogType lt) {
		Debug.LogError ("DG : " + " " + lt +  " " + condition + " " + stackInfo + "    " + TimeHelper.FormattedTimeNow ());
	}

	void Update() {
//		if(Input.GetKey(KeyCode.Escape)){
//			Application.Quit();
//		}

		if(Time.timeScale < 0.5f)
			return;

		if(OnUpdate != null)
			OnUpdate();
//		Debug.LogError ("noviceGuideShileInput : " + noviceGuideShileInput + "NoviceGuideStepEntityManager.isInNoviceGuide() : " + NoviceGuideStepEntityManager.isInNoviceGuide ());
		if (!noviceGuideShileInput && NoviceGuideStepEntityManager.isInNoviceGuide()) {
//			Debug.LogError("!noviceGuideShileInput && DGTools.IsNoviceGuide () ");
			return;	
		}

		if (!isCheckInput) {
			Debug.LogError ("game input update : " + noviceGuideShileInput + " isCheckInput : " + isCheckInput);
			return;
		}
//		Debug.LogError ("game input update ");
//#if UNITY_IPHONE || UNITY_ANDROID
       	//ProcessTouch();
//#elif UNITY_EDITOR 
		ProcessMouse();
//#endif
	}

	void LateUpdate() {
		if (OnLateUpdate != null) {
			OnLateUpdate();
		}
	}

	void ProcessTouch()
	{
		if(Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			
			switch (touch.phase) 
			{
			case TouchPhase.Began:
				OnPress();
				break;
			case TouchPhase.Moved:
				deltaPosition = touch.position;
				OnDrag();
				break;
			case TouchPhase.Ended:
				OnRelease();
				break;
			case TouchPhase.Stationary:
				break;
			default:
				break;
			}
		}
	}


	void ProcessMouse() {
		if(Input.GetMouseButtonDown(0)) {
			OnPress();
		} else if(Input.GetMouseButtonUp(0)) {
			OnRelease();
		} else if(Input.GetMouseButton(0)) {
			currentPosition = Input.mousePosition;
			
			if(currentPosition != lastPosition) {
				deltaPosition = currentPosition;// currentPosition - lastPosition;
				
				OnDrag();
				
				lastPosition = currentPosition;
			}
			else {
				OnStationary();
			}
		}
	}

	void OnPress()
	{
		if(OnPressEvent != null)
			OnPressEvent();
	}

	void OnDrag()
	{
		startTime = Time.realtimeSinceStartup;

		if(OnDragEvent != null)
			OnDragEvent(Input.mousePosition);
	}

	void OnRelease()
	{
		InitCountTime();
		if(OnReleaseEvent != null)
			OnReleaseEvent ();
	}

	void OnStationary()
	{
		if(startTime < 0)
			startTime = Time.realtimeSinceStartup;

		if(Time.realtimeSinceStartup - startTime >= stationarIntervTime) {
			InitCountTime();

			if(OnStationaryEvent != null)
				OnStationaryEvent();
		}
	}

	void InitCountTime() {
		startTime = -1f;
	}
}
