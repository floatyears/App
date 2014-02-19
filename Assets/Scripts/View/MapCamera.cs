using UnityEngine;
using System.Collections;

public class MapCamera : MonoBehaviour {
	private Camera camera ;
	private GameInput gameInput;
	private bool isClick = true;

	void Awake () {
		camera = GetComponent<Camera> ();
		gameInput = Main.Instance.GInput;
		GameInput.OnUpdate += HandleOnUpdate;
	}

	void OnEnable () {
		MsgCenter.Instance.AddListener (CommandEnum.MeetEnemy, MeetEnemy);
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
	}

	void Disable () {
		MsgCenter.Instance.RemoveListener (CommandEnum.MeetEnemy, MeetEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
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
		if(Physics.Raycast(ray, out rayCastHit)) {
			GameObject go = rayCastHit.collider.gameObject;
			go.SendMessage("OnClick",SendMessageOptions.DontRequireReceiver);
		}
	}
}
