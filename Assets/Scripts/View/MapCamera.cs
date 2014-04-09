using UnityEngine;
using System.Collections;

public class MapCamera : MonoBehaviour {
	private Camera mapCamera ;
	private GameInput gameInput;
	private static bool isClick = true;
	public static bool IsClick {
		set { SetIsClick (value); } //Debug.LogError("MapCamera value : " + value);
		get { return isClick; }
	}
	
	void Awake () {
		mapCamera = GetComponent<Camera> ();
		gameInput = Main.Instance.GInput;

	}

	void OnEnable () {
		MsgCenter.Instance.AddListener (CommandEnum.MeetEnemy, MeetEnemy);
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.AddListener (CommandEnum.StopInput, StopInput);
		GameInput.OnUpdate += HandleOnUpdate;
		SetIsClick (false);
	}

	void OnDisable () {
		MsgCenter.Instance.RemoveListener (CommandEnum.MeetEnemy, MeetEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.StopInput, StopInput);
		GameInput.OnUpdate -= HandleOnUpdate;
		SetIsClick (false);
	}

	void StopInput(object data) {
		if (data == null) {
			SetIsClick (false);	
			return;
		}
		bool b = (bool)data;
		SetIsClick (b);	
//		Debug.LogError ("isClick : " + isClick +" time: "+ Time.realtimeSinceStartup);
	}

	void HandleOnUpdate () {
		if (!isClick) {
			return;
		}
		ProcessMouse ();
	}

	void MeetEnemy (object data) {
		SetIsClick (false);
	}

	void BattleEnd (object data) {
		SetIsClick (true);
	}

	static void SetIsClick (bool b) {
		isClick = b;
	}

	void ProcessMouse() {
		if(Input.GetMouseButtonDown(0)) {
			Press();
		}
	}

	void ProcessTouch() {
		if(Input.touchCount > 0) {
			Touch touch = Input.touches[0];
			switch (touch.phase) {
			case TouchPhase.Began:
				Press();
				break;
			default:
				break;
			}
		}
	}

	RaycastHit rayCastHit;

	void Press () {
//		Debug.LogError ("Press : " + isClick +" time: "+ Time.realtimeSinceStartup);
		Ray ray = mapCamera.ScreenPointToRay (Input.mousePosition);
		bool haveObject = Physics.Raycast (ray, out rayCastHit);
		if(haveObject) {
			GameObject go = rayCastHit.collider.gameObject;
			go.SendMessage("OnClick",SendMessageOptions.DontRequireReceiver);
		}
	}
}
