using UnityEngine;
using System.Collections;

public class BattleBottom : MonoBehaviour {
	private Camera bottomCamera;
	private RaycastHit rch;
	private UnitPartyInfo upi;

	public void Init(Camera bottomCamera) {
		this.bottomCamera = bottomCamera;
		if (upi == null) {
			upi = ModelManager.Instance.GetData(ModelEnum.UnitPartyInfo,new ErrorMsg()) as UnitPartyInfo;		
		}
	}

	void OnDisable () {
		GameInput.OnReleaseEvent -= OnRealease;
	}

	void OnEnable () {
		GameInput.OnReleaseEvent += OnRealease;
	}

	void OnRealease () {
		Ray ray = bottomCamera.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out rch,100f,GameLayer.Bottom << 31)) {
			string name = rch.collider.name;
			CheckCollider(name);
		}
	}

	void CheckCollider (string name) {
		if (upi == null) {
			Debug.LogError("upi is null");
			return;	
		}
		int id = System.Int32.Parse (name);
		if (upi.UserUnit.ContainsKey (id)) {
			Debug.LogError ("CheckCollider : " + id);
			UserUnitInfo uui = upi.UserUnit [id];
			MsgCenter.Instance.Invoke(CommandEnum.LaunchActiveSkill, uui);
		}
	}
}
