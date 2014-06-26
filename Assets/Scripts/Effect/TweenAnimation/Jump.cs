using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {
	Vector3 topPosition = Vector3.zero;
	Vector3 bottomPosition = Vector3.zero;

//	void Start(){
//		Init (transform.position);
//		GameStart (Vector3.zero);
//	}

	public void Init(Vector3 position) {
		topPosition = position;
		transform.localPosition = position;
	}

	public void GameStart (Vector3 point) {
//		bottomPosition = point;
//		iTween.MoveTo(gameObject,iTween.Hash("position",bottomPosition,"time",0.3f,"easetype",iTween.EaseType.easeInCubic,"islocal",true));
//		iTween.ScaleFrom (gameObject, new Vector3 (3f, 3f, 3f), 0.5f);
	}
	
	public void JumpAnim() {
//		iTween.ScaleTo (gameObject, iTween.Hash ("scale", new Vector3 (1.5f, 1.5f, 1.5f), "time", 0.3f, "easetype", iTween.EaseType.easeOutCubic, "oncomplete", "JumpEnd", "oncompletetarget", gameObject));
	}

	void JumpEnd() {
//		iTween.ScaleTo (gameObject, iTween.Hash ("scale", new Vector3 (1f, 1f, 1f), "time", 0.3f, "easetype", iTween.EaseType.easeInCubic, "oncomplete", "JumpDone", "oncompletetarget", gameObject));
	}

	void JumpDone() {
//		Debug.LogError("jumpdone");
	}

//	void Update (){
//		if (Input.GetMouseButton (0)) {
//			JumpAnim();
//		}
//	}
}
