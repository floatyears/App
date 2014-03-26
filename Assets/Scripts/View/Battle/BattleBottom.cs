using UnityEngine;
using System.Collections.Generic;

public class BattleBottom : MonoBehaviour {
	private Camera bottomCamera;
	private RaycastHit rch;
	private TUnitParty upi;
	private Dictionary<int,GameObject> actorObject = new Dictionary<int,GameObject>();

	private GameObject battleSkillObject;
	private BattleSkill battleSkill;

	[HideInInspector]
	public BattleQuest battleQuest;

	public void Init(Camera bottomCamera) {
		this.bottomCamera = bottomCamera;
		battleSkillObject = Resources.Load("Prefabs/BattleSkill") as GameObject;
		battleSkillObject = NGUITools.AddChild (ViewManager.Instance.CenterPanel, battleSkillObject);
		battleSkill = battleSkillObject.GetComponent<BattleSkill> ();
		battleSkill.Init ("BattleSkill");
		battleSkillObject.SetActive (false);

		if (upi == null) {
			upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		}
		Dictionary<int,TUserUnit> userUnitInfo = upi.UserUnit;
		for (int i = 0; i < 5; i++) {
			GameObject temp = transform.Find("Actor/" + i).gameObject;	
			TUnitInfo tui = userUnitInfo[i].UnitInfo;
			temp.renderer.material.SetTexture("_MainTex",tui.GetAsset(UnitAssetType.Profile));
			UITexture tex =  transform.Find("ActorP/" + i).GetComponent<UITexture>();
			tex.color =  DGTools.TypeToColor(tui.Type);
		}

		List<int> haveInfo = new List<int> (userUnitInfo.Keys);
		for (int i = 0; i < 5; i++) {
			if(!haveInfo.Contains(i)) {
				actorObject[i].SetActive(false);
			}
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
		if (Physics.Raycast (ray, out rch,100f, GameLayer.Bottom << 31)) {
			string name = rch.collider.name;
			CheckCollider(name);
		}
	}

	TUserUnit tuu;
	void CheckCollider (string name) {
		if (upi == null) {
			Debug.LogError("upi is null");
			return;	
		}
		Debug.LogError ("name : " + name);
		int id = System.Int32.Parse (name);
		battleQuest.battle.SwitchInput (true);
		if (upi.UserUnit.ContainsKey (id)) {
			tuu = upi.UserUnit [id];
			battleSkillObject.SetActive(true);
			battleSkill.Refresh(tuu, Boost);
		}
	}

	void Boost() {
		battleQuest.battle.SwitchInput (false);
		battleSkillObject.SetActive(false);
		MsgCenter.Instance.Invoke(CommandEnum.LaunchActiveSkill, tuu);
	}
}
