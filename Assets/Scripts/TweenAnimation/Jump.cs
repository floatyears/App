using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {
	Vector3 topPosition = Vector3.zero;
	Vector3 bottomPosition = Vector3.zero;

	public void Init(Vector3 position) {
		topPosition = position;
		transform.localPosition = position;
	}

	public void GameStart (Vector3 point) {
		bottomPosition = point;
		iTween.MoveTo(gameObject,iTween.Hash("position",bottomPosition,"time",0.3f,"easetype",iTween.EaseType.easeInCubic,"islocal",true));
	}
	
	public void JumpAnim() {
//		iTween.MoveTo(gameObject,iTween.Hash("position",topPosition,"time",0.5f,"easetype",iTween.EaseType.easeOutQuart,"oncomplete","JumpTop","oncompletetarget",gameObject,"islocal",true));
	}

	void JumpTop() {
//		iTween.MoveTo(gameObject,iTween.Hash("position",bottomPosition,"time",0.5f,"easetype",iTween.EaseType.easeInCubic,"oncomplete","JumpDone","oncompletetarget",gameObject,"islocal",true));
	}

	void JumpDone() {
		Debug.LogError("jumpdone");
	}
}
