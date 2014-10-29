using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectMotionTrail : MonoBehaviour {

	public bool IsEffectStart = false;

	public GameObject target;

	public GameObject instance;

	private Queue<GameObject> objPool;

	private List<GameObject> currObjs;

	public int frameSep = 10;

	private int currentFrame;

	public float lastTime = 1f;

	// Use this for initialization
	void Start () {
		objPool = new Queue<GameObject> ();
		currObjs = new List<GameObject> ();
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
		GameObject obj = objPool.Count > 0 ? objPool.Dequeue() : Instantiate(instance) as GameObject;
		currObjs.Add (obj);
		obj.SetActive (true);
		obj.transform.localScale = Vector3.one;
		obj.transform.FindChild("Finger").GetComponent<TweenAlpha> ().enabled = true;
		obj.transform.FindChild("Finger").GetComponent<TweenAlpha> ().ResetToBeginning ();
		obj.SetActive (true);
		obj.transform.localPosition = target.transform.localPosition;
		obj.transform.parent = transform;
		yield return new WaitForSeconds(lastTime);
		obj.SetActive (false);
		objPool.Enqueue(obj);
	}

	public void Stop(){
		foreach (var item in currObjs) {
			Destroy(item);
		}
		currObjs.Clear ();
		objPool.Clear ();
		IsEffectStart = false;
	}
}
