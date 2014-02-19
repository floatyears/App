using UnityEngine;
using System.Collections;

public class MapDoor : UIBaseUnity {
	private GameObject leftDoor;
	private GameObject rightDoor;
	public override void Init (string name) {
		base.Init (name);
		leftDoor = transform.Find("Left").gameObject;
		rightDoor = transform.Find ("Right").gameObject;
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
		//MsgCenter.Instance.AddListener (CommandEnum.OpenDoor, OpenDoor);
	}

	public override void HideUI () {
		base.HideUI ();
		//MsgCenter.Instance.RemoveListener (CommandEnum.OpenDoor, OpenDoor);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void SetPosition(Vector3 pos) {
		transform.localPosition = pos;
	}

	void OpenDoor (object data) {
//		Debug.LogError("opendoor  data");
		iTween.RotateTo (leftDoor, new Vector3 (-90f, -120f, 0f), 2f);
		iTween.RotateTo (rightDoor, new Vector3 (90f, 120f, 0f), 2f);
	}
}
