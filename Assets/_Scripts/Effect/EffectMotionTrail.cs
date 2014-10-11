using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectMotionTrail : MonoBehaviour {

	public bool IsEffectStart = false;

	public GameObject target;

	public Queue<GameObject> objPool;

	public int frameSep = 10;

	private int currentFrame;

	public float lastTime = 1f;

	// Use this for initialization
	void Start () {
		objPool = new Queue<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (IsEffectStart) {
			if(currentFrame == 0){
				currentFrame = frameSep;
				StartCoroutine(Emmit());
			}
			currentFrame--;
		}

	}

	IEnumerator Emmit(){
//		Debug.Log ("Emmit");
		GameObject obj = objPool.Count > 0 ? objPool.Dequeue() : Instantiate(target) as GameObject;
		obj.transform.localScale = Vector3.one;
		obj.GetComponent<TweenAlpha> ().enabled = true;
		obj.GetComponent<TweenAlpha> ().ResetToBeginning ();
		obj.SetActive (true);
		obj.transform.localPosition = target.transform.localPosition;
		obj.transform.parent = transform;
		yield return new WaitForSeconds(lastTime);
		obj.SetActive (false);
		objPool.Enqueue(obj);
	}
}
