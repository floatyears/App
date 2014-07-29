using UnityEngine;
using System.Collections;

public class LoadingRotate : MonoBehaviour {

	public Vector3 rotateSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.rotation *= Quaternion.Euler (rotateSpeed);
		//transform.rotation = Quaternion.Euler (new Vector3(transform.rotation.x + rotateSpeed.x,transform.rotation.y + rotateSpeed.y,transform.rotation.z + rotateSpeed.z));
	}
}
