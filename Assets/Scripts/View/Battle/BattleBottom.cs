using UnityEngine;
using System.Collections.Generic;

public class BattleBottom : MonoBehaviour {
	private Camera bottomCamera;
	private RaycastHit rch;
	private TUnitParty upi;
	private Dictionary<int,GameObject> actorObject = new Dictionary<int,GameObject>();
	private GameObject battleSkillObject;
	private BattleSkill battleSkill;
//	public BattleSkill BattleSkillIns{
//		get { return battleSkill; }
//	}

	public bool IsUseLeaderSkill = false;

	public static bool notClick = false;

//	[HideInInspector]
	private BattleQuest _battleQuest;
	public BattleQuest battleQuest {
		set {
			_battleQuest = value;
			battleSkill.battleQuest = battleQuest;
		}
		get{
			return _battleQuest;
		}
	}

	public void Init(Camera bottomCamera) {
		this.bottomCamera = bottomCamera;
		battleSkillObject = Resources.Load("Prefabs/BattleSkill") as GameObject;
		battleSkillObject = NGUITools.AddChild (ViewManager.Instance.CenterPanel, battleSkillObject);
		battleSkill = battleSkillObject.GetComponent<BattleSkill> ();
		battleSkill.Init ("BattleSkill");
		battleSkillObject.SetActive (false);

		if (upi == null) {
//			Debug.LogError("Battle bottom init : " + DataCenter.Instance.PartyInfo.CurrentPartyId);
			upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		}
		Dictionary<int,TUserUnit> userUnitInfo = upi.UserUnit;
//		Debug.LogError("bottom unitparty id : " + upi.ID);
//		Debug.LogError ("Battle bottom : " + userUnitInfo.Count);
		for (int i = 0; i < 5; i++) {
			GameObject temp = transform.Find("Actor/" + i).gameObject;	
			if(userUnitInfo[i] == null) {
				temp.gameObject.SetActive(false);
				continue;
			}

			TUnitInfo tui = userUnitInfo[i].UnitInfo;

//			Debug.LogError("BattleBottom : " + userUnitInfo[i].ID + " pos : " + i);
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

	void OnDestroy() {
		if(battleSkill != null) 
			Destroy (battleSkill.gameObject);
		battleSkill = null;
	} 

	void OnDisable () {
		GameInput.OnUpdate -= OnRealease;
	}

	void OnEnable () {
		GameInput.OnUpdate += OnRealease;
	}

	void OnRealease () {
		if (notClick) {
			return;	
		}

		if (Input.GetMouseButtonDown (0)) {
			Ray ray = bottomCamera.ScreenPointToRay (Input.mousePosition);

			int layermask = Main.Instance.NguiCamera.eventReceiverMask;

			//LogHelper.Log("----------------battle bottom layermask: " + layermask);

			int receiveMask = 0;

//			int noviint = GameLayer.LayerToInt(GameLayer.NoviceGuide);
//			int blocker =GameLayer.LayerToInt(GameLayer.blocker);

//			int layer = noviint | blocker;
//			Debug.LogError("noviint : " + noviint + " blockerlayer : " + blocker + " layer : " + layer);
			//bool novi = (layermask & GameLayer.LayerToInt(GameLayer.NoviceGuide)) == GameLayer.LayerToInt(GameLayer.NoviceGuide);
//			Debug.LogError(layermask + " GameLayer.NoviceGuid : " + GameLayer.LayerToInt(GameLayer.NoviceGuide));
//			Debug.LogError("novi : " +novi + " GameLayer.NoviceGuid : " + (layermask & GameLayer.LayerToInt(GameLayer.NoviceGuide)) + " GameLayer.NoviceGuide : " + GameLayer.LayerToInt(GameLayer.NoviceGuide));
			if(IsUseLeaderSkill) {
				receiveMask = GameLayer.LayerToInt(GameLayer.NoviceGuide);
			}
			else{
				receiveMask = GameLayer.LayerToInt(GameLayer.Bottom);
			}
			if (Physics.Raycast (ray, out rch, 100f, receiveMask)) {
				string name = rch.collider.name;
				CheckCollider(name);
			}
		}
	}

	TUserUnit tuu;
	void CheckCollider (string name) {
		if (upi == null || battleQuest.role.isMove) {
			Debug.LogError("upi is null");
			return;	
		}
		try{
			int id = System.Int32.Parse (name);
			if (upi.UserUnit.ContainsKey (id)) {
				foreach (var item in actorObject.Values) {
					if(item.name == name) {
						item.renderer.material.color = Color.white;
						continue;
					}
					item.renderer.material.color = Color.gray;
				}

				if(IsUseLeaderSkill && id == 0){
					LogHelper.Log("--------use leader skill command");
					MsgCenter.Instance.Invoke(CommandEnum.UseLeaderSkill,null);
				}


				tuu = upi.UserUnit [id];
				battleQuest.topUI.SheildInput(false);
				battleSkillObject.SetActive(true);
				battleSkill.Refresh(tuu, Boost, Close);
				BattleMap.waitMove = true;
				battleQuest.battle.ShieldGameInput(false);
			}
		}
		catch(System.Exception ex) {
			Debug.LogError("exception : " + ex.Message + " name : " + name);
		}
	}

	public void Boost() {
		CloseSkillWindow ();
		MsgCenter.Instance.Invoke(CommandEnum.LaunchActiveSkill, tuu);
	}

	public void Close () {
		CloseSkillWindow ();
	}

	void CloseSkillWindow () {
		foreach (var item in actorObject.Values) {
			item.renderer.material.color = Color.white;
		}
		battleQuest.topUI.SheildInput(true);
		BattleMap.waitMove = false;
		if (battleQuest.battle.isShowEnemy) {
			battleQuest.battle.ShieldGameInput(true);
		}
		battleSkillObject.SetActive(false);
	}

	public void SetLeaderToNoviceGuide(bool isInNoviceGuide){
		if (isInNoviceGuide) {
			GameObject temp = transform.Find ("Actor/0").gameObject;
			temp.layer = LayerMask.NameToLayer ("NoviceGuide");	
		} else {
			GameObject temp = transform.Find ("Actor/0").gameObject;
			temp.layer = LayerMask.NameToLayer ("Bottom");	
		}

	}
}
