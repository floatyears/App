using UnityEngine;
using System.Collections.Generic;

public class BattleBottom : MonoBehaviour {
	private Camera bottomCamera;
	private RaycastHit rch;
	private TUnitParty upi;
	private Dictionary<int,GameObject> actorObject = new Dictionary<int,GameObject>();
//	private static Dictionary<uint,Transform> rolePosition = new Dictionary<uint, Transform> ();
//	public static Dictionary<uint,Transform> RolePosition {
//		get{ return rolePosition; }
//	}

	public void Init(Camera bottomCamera) {
		this.bottomCamera = bottomCamera;
		if (upi == null) {
			upi = ModelManager.Instance.GetData(ModelEnum.UnitPartyInfo,new ErrorMsg()) as TUnitParty;		
		}
		for (int i = 1; i < 6; i++) {
			GameObject tex = transform.Find("Actor/" + i).gameObject;	
			actorObject.Add(i,tex);
		}
		Dictionary<int,TUserUnit> userUnitInfo = upi.GetPosUnitInfo ();
		foreach (var item in userUnitInfo) {
//			Debug.LogError("item.Value.UnitID : " + item.Value.UnitID);
			TUnitInfo tui = GlobalData.unitInfo[item.Value.UnitID];
			actorObject[item.Key].renderer.material.SetTexture("_MainTex",tui.GetAsset(UnitAssetType.Profile));
//			rolePosition.Add(item.Value.GetID,actorObject[item.Key].transform);
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
			TUserUnit uui = upi.UserUnit [id];
			MsgCenter.Instance.Invoke(CommandEnum.LaunchActiveSkill, uui);
		}
	}
}
