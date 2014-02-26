using UnityEngine;
using System.Collections.Generic;

public class BattleBottom : MonoBehaviour {
	private Camera bottomCamera;
	private RaycastHit rch;
	private UnitPartyInfo upi;
	private Dictionary<int,GameObject> actorObject = new Dictionary<int,GameObject>();
	private static Dictionary<uint,Vector3> rolePosition = new Dictionary<uint, Vector3> ();
	public static Dictionary<uint,Vector3> RolePosition {
		get{ return rolePosition; }
	}

	public void Init(Camera bottomCamera) {
		this.bottomCamera = bottomCamera;
		if (upi == null) {
			upi = ModelManager.Instance.GetData(ModelEnum.UnitPartyInfo,new ErrorMsg()) as UnitPartyInfo;		
		}
		for (int i = 1; i < 6; i++) {
			GameObject tex = transform.Find("Actor/" + i).gameObject;	
			actorObject.Add(i,tex);
		}
		Dictionary<int,UserUnitInfo> userUnitInfo = upi.GetPosUnitInfo ();
		foreach (var item in userUnitInfo) {
			TUnitInfo tui = GlobalData.tempUnitInfo[item.Value.GetUnitID];
			actorObject[item.Key].renderer.material.SetTexture("_MainTex",tui.GetAsset(UnitAssetType.Profile));
			rolePosition.Add(item.Value.GetID,actorObject[item.Key].transform.position);
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
			UserUnitInfo uui = upi.UserUnit [id];
			MsgCenter.Instance.Invoke(CommandEnum.LaunchActiveSkill, uui);
		}
	}
}
