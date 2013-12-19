using UnityEngine;
using System.Collections;

public class GameInput : MonoBehaviour 
{
	public static event System.Action OnPressEvent;

	public static event System.Action OnReleaseEvent;

	public static event System.Action<Vector2> OnDragEvent;

	public static event System.Action OnStationaryEvent;
	
	public static event System.Action OnUpdate;

	private bool isCheckInput = true;

	public bool IsCheckInput
	{
		set{isCheckInput = value;}
		get{return isCheckInput;}
	}

	private Vector2 lastPosition = Vector2.zero;

	private Vector2 currentPosition = Vector2.zero;

	private Vector2 deltaPosition = Vector2.zero;

	private float startTime = -1f;

	private float stationarIntervTime = 2f;

	void Update()
	{

		if(Time.timeScale < 0.5f)
			return;

		if(OnUpdate != null)
			OnUpdate();

		if(!isCheckInput)
			return;
//#if UNITY_IPHONE || UNITY_ANDROID
		ProcessTouch();
//#elif UNITY_EDITOR 
		//ProcessMouse();
//#endif
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


	void ProcessMouse()
	{
		if(Input.GetMouseButtonDown(0))
		{
			lastPosition = Input.mousePosition;
			
			currentPosition = Input.mousePosition;
			
			OnPress();
		}
		else if(Input.GetMouseButtonUp(0))
		{
			OnRelease();
		}
		else if(Input.GetMouseButton(0))
		{
			currentPosition = Input.mousePosition;
			
			if(currentPosition != lastPosition)
			{
				deltaPosition = currentPosition;// currentPosition - lastPosition;
				
				OnDrag();
				
				lastPosition = currentPosition;
			}
			else
			{
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
			OnDragEvent(deltaPosition);
	}

	void OnRelease()
	{
		InitCountTime();
		if(OnReleaseEvent != null)
			OnReleaseEvent();
	}

	void OnStationary()
	{
		if(startTime < 0)
			startTime = Time.realtimeSinceStartup;

		if(Time.realtimeSinceStartup - startTime >= stationarIntervTime)
		{
			InitCountTime();

			if(OnStationaryEvent != null)
				OnStationaryEvent();
		}
	}

	void InitCountTime()
	{
		startTime = -1f;
	}
}

