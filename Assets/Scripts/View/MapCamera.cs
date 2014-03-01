using UnityEngine;
using System.Collections;

public class MapCamera : MonoBehaviour {
	private Camera camera ;
	private GameInput gameInput;
	private bool isClick = true;



	void Awake () {
		camera = GetComponent<Camera> ();
		gameInput = Main.Instance.GInput;

	}

	void OnEnable () {
		MsgCenter.Instance.AddListener (CommandEnum.MeetEnemy, MeetEnemy);
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.AddListener (CommandEnum.StopInput, StopInput);
		GameInput.OnUpdate += HandleOnUpdate;
		isClick = true;
	}

	void OnDisable () {
		MsgCenter.Instance.RemoveListener (CommandEnum.MeetEnemy, MeetEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.StopInput, StopInput);
		GameInput.OnUpdate -= HandleOnUpdate;
		isClick = false;
	}

	void StopInput(object data) {
		isClick = false;
//		Debug.LogError ("isClick : " + isClick);
	}

	void HandleOnUpdate () {
		if (!isClick) {
			return;
		}
		ProcessMouse ();
	}

	void MeetEnemy (object data) {

		isClick = false;
	}

	void BattleEnd (object data) {
		isClick = true;
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
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);
		bool haveObject = Physics.Raycast (ray, out rayCastHit);
//		Debug.LogError ("haveObject : " + haveObject);
		if(haveObject) {
			GameObject go = rayCastHit.collider.gameObject;
			go.SendMessage("OnClick",SendMessageOptions.DontRequireReceiver);
		}
	}
}
