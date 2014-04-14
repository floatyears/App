using UnityEngine;
using System.Collections;

public class FloorRotate : MonoBehaviour {
	int addPoint = -10;
	float time = 0.6f;
	float multipetime = 0.8f;
	GameObject targetObject;
	Vector3 rotatePoint = Vector3.zero;
	float manyProbability = 0.5f;
	float countDownTime = 1f;
	public Vector3 currentPoint = Vector3.zero;
	bool isRotate =false ;
	float oneangle = 0f;
	float manyAngle = 0f;

	public void Init() {
		targetObject = gameObject;
		currentPoint = targetObject.transform.position;
		currentPoint = new Vector3 (currentPoint.x, currentPoint.y, currentPoint.z - 2f);
		Vector3 scale = targetObject.transform.localScale;
		rotatePoint = transform.Find ("RotatePoint").transform.position;
	}

	void OnEnable () {
		isRotate = false;
	}

	Callback temp;

	public void RotateFloor (Callback call) {
		if (isRotate) {
			return;	
		}
		if (targetObject == null) { 
			return;	
		}
		temp = call;
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
		countDownTime = multipetime;
		oneangle = 225f;
		manyAngle = 900f;
	}

	public void RotateOne () {
		rotateState = 0;
		countDownTime = time;	
		oneangle = 300f;
	}

	int rotateState = -1;

	public bool isShowBox = false;
	GameObject box;
	public void ShowBox () {
		box = BattleMap.Box;
		box.SetActive (true);
		box.transform.localPosition = transform.localPosition;
		GameTimer.GetInstance ().AddCountDown (0.75f, HideBox);
	}

	void HideBox() {
		box.SetActive (false);
	}

	void Falling () {
		if (isShowBox) {
			ShowBox();
		}
		Vector3 angle = transform.rotation.eulerAngles;
		Vector3 newAngle = new Vector3 (0f, angle.y, angle.z);
		transform.eulerAngles = newAngle;//Quart
		iTween.MoveTo (gameObject, iTween.Hash("position",currentPoint,"time",0.5f,"easetype",iTween.EaseType.easeOutCubic,"oncomplete","FallComplete","oncompletetarget",gameObject));
	}

	void FallComplete() {
		if (temp != null) {
			temp();	
		}
		MsgCenter.Instance.Invoke (CommandEnum.RotateDown, null);
	}

	void Update () {
		if (rotateState == -1) {
			return;	
		}

		if(countDownTime >= 0) {
			transform.RotateAround(rotatePoint,Vector3.right,oneangle * Time.deltaTime);
			if(rotateState == 1) {
				transform.Rotate(Vector3.right, manyAngle * Time.deltaTime);
//				iTween.RotateTo(gameObject,
			}
			countDownTime -= Time.deltaTime;
		}else{
			Falling();
			rotateState = -1;
		}
	}
}
