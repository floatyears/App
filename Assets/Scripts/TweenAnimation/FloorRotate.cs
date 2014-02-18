using UnityEngine;
using System.Collections;

public class FloorRotate : MonoBehaviour {
	int addPoint = -10;
	float time = 0.8f;
	GameObject targetObject;
	Vector3 rotatePoint = Vector3.zero;
	float manyProbability = 0.5f;
	float countDownTime = 1f;
	Vector3 currentPoint = Vector3.zero;
	bool isRotate =false ;

//	void Start () {
//		RotateFloor ();
//	}

	public void Init() {
		targetObject = gameObject;
		currentPoint = targetObject.transform.position;
		currentPoint = new Vector3 (currentPoint.x, currentPoint.y, currentPoint.z - 2f * 0.001760563f);
		Vector3 scale = targetObject.transform.localScale;
		rotatePoint = transform.Find ("RotatePoint").transform.position;
	}
	
	public void RotateFloor () {
		if (isRotate) {
			return;	
		}
		if (targetObject == null) { 
			return;	
		}
		isRotate = true;
		float index = DGTools.RandomToFloat ();
		if (index <= manyProbability) {
			RotateMany();
		} 
		else {
			RotateOne();
		}
	}

	void RotateMany () {
		rotateState = 1;
		MoveComplete ();
	}

	void RotateOne () {
		MoveComplete ();
		rotateState = 0;
	}

	int rotateState = -1;
	
	void MoveComplete () {
		countDownTime = time;
	}

	void Falling () {
		Vector3 angle = transform.rotation.eulerAngles;
		Vector3 newAngle = new Vector3 (0f, angle.y, angle.z);
		transform.eulerAngles = newAngle;
		iTween.MoveTo (gameObject, iTween.Hash("position",currentPoint,"time",0.8f,"easetype",iTween.EaseType.easeInOutQuart,"oncomplete","FallComplete","oncompletetarget",gameObject));
	}

	void FallComplete() {
//		Debug.LogError ("FallComplete");
		MsgCenter.Instance.Invoke (CommandEnum.RotateDown, null);
	}

	void Update () {
		if (rotateState == -1) {
			return;	
		}

		if(countDownTime >= 0) {
			transform.RotateAround(rotatePoint,Vector3.right,225f * Time.deltaTime);
			if(rotateState == 1) {
				transform.Rotate(Vector3.right,450f * Time.deltaTime);
			}
			countDownTime -= Time.deltaTime;
		}else{
			Falling();
			rotateState = -1;
		}
	}
}
