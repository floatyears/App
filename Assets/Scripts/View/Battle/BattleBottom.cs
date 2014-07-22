using UnityEngine;
using System.Collections.Generic;

public class BattleBottom : MonoBehaviour {
	private Camera bottomCamera;
	private RaycastHit rch;
	private TUnitParty upi;
	private Dictionary<int, UITexture> actorObject = new Dictionary<int, UITexture>();
	private GameObject battleSkillObject;
	private BattleSkill battleSkill;

	public bool IsUseLeaderSkill = false;

	public static bool notClick = false;

	private BattleQuest _battleQuest;
	public BattleQuest battleQuest {
		set {
			_battleQuest = value;
			battleSkill.battleQuest = battleQuest;
		}
		get {
			return _battleQuest;
		}
	}

	public void Init(Camera bottomCamera) {
		this.bottomCamera = bottomCamera;
		ResourceManager.Instance.LoadLocalAsset("Prefabs/BattleSkill", o => {
			GameObject go = o as GameObject;
			battleSkillObject = NGUITools.AddChild (ViewManager.Instance.CenterPanel, go);
			battleSkillObject.layer = GameLayer.BottomInfo;
			battleSkill = battleSkillObject.GetComponent<BattleSkill> ();
			battleSkill.Init ("BattleSkill");
			battleSkillObject.transform.localPosition = new Vector3(2f, -110f, 0f);
			battleSkillObject.SetActive (false);
		});

		if (upi == null) {
			upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		}

		Dictionary<int,TUserUnit> userUnitInfo = upi.UserUnit;
		Transform actorTrans = transform.Find ("Actor");
		for (int i = 0; i < 5; i++) {
			GameObject temp = actorTrans.Find(i.ToString()).gameObject;	
			UISprite tex =  actorTrans.Find(i.ToString() + "_Border").GetComponent<UISprite>();
			UISprite bgSpr = actorTrans.Find(i.ToString() + "_bg").GetComponent<UISprite>();
			UISprite skillBGSpr = actorTrans.Find(i.ToString()+ "_skillBg").GetComponent<UISprite>();
			UISprite skillSpr = actorTrans.Find(i.ToString() + "_skill").GetComponent<UISprite>();
			UITexture texture = temp.GetComponent<UITexture>();

			actorObject.Add(i, texture);

			if(userUnitInfo[i] == null) {
				temp.gameObject.SetActive(false);
				tex.enabled = false;
				bgSpr.enabled = false;
				skillBGSpr.enabled = false;
				skillSpr.enabled = false;
			} else{
				TUnitInfo tui = userUnitInfo[i].UnitInfo;
				tui.GetAsset(UnitAssetType.Profile, o=>{
					if(o != null)
						texture.mainTexture = o as Texture2D;
				});
				
				tex.spriteName = GetUnitTypeSpriteName(i, tui.Type);
				bgSpr.spriteName = GetBGSpriteName(i, tui.Type);
				skillSpr.spriteName = GetSkillSpriteName(tui.Type);
			}
		}
	}

	string GetSkillSpriteName( bbproto.EUnitType type) {
		string suffixName = "icon_skill_";

		int typeNumber = (int)type;
		suffixName += typeNumber.ToString ();
		return suffixName;
	}

	string GetBGSpriteName(int pos, bbproto.EUnitType type) {
		string suffixName = "avatar_bg_";
		if (pos == 0) {
			suffixName += "l_";
		} else {
			suffixName += "f_";
		}
		
		int typeNumber = (int)type;
		suffixName += typeNumber.ToString ();
		return suffixName;
	}

	string GetUnitTypeSpriteName(int pos, bbproto.EUnitType type) {
		string suffixName = "avatar_border_";
		if (pos == 0) {
			suffixName += "l_";
		} else {
			suffixName += "f_";
		}

		int typeNumber = (int)type;
		suffixName += typeNumber.ToString ();
		return suffixName;
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
			int receiveMask = 0;

			if(IsUseLeaderSkill) {
				receiveMask = GameLayer.LayerToInt(GameLayer.NoviceGuide);
			} else {
				receiveMask = GameLayer.LayerToInt(GameLayer.Default);
			}

			if (Physics.Raycast (ray, out rch, 100f, receiveMask)) {
				string name = rch.collider.name;
				CheckCollider(name);
			}
		}
	}

	TUserUnit tuu;
	int prevID = -1;
	void CheckCollider (string name) {
		if (upi == null || battleQuest.role.isMove) {
			return;	
		}

		try {
			int id = System.Int32.Parse (name);
			if (upi.UserUnit.ContainsKey (id)) {
				if(id == prevID) {
					CloseSkillWindow();
					prevID = -1;
					return;
				}
				prevID = id;
				MaskCard (name, true);
//				foreach (var item in actorObject.Values) {
//					if(item.name == name) {
//						item.renderer.material.color = Color.white;
//						continue;
//					}
//					item.renderer.material.color = Color.gray;
//				}
				if(IsUseLeaderSkill && id == 0) {
					LogHelper.Log("--------use leader skill command");
					MsgCenter.Instance.Invoke(CommandEnum.UseLeaderSkill, null);
				}
				tuu = upi.UserUnit [id];
				battleQuest.topUI.SheildInput(false);
				battleSkillObject.SetActive(true);
				battleSkill.Refresh(tuu, Boost, Close);
				BattleMap.waitMove = true;
				battleQuest.battle.ShieldGameInput(false);
			}
		} catch(System.Exception ex) {
//			Debug.LogError("exception : " + ex.Message + " name : " + name);
		}
	}

	public void Boost() {
		CloseSkillWindow ();
		MsgCenter.Instance.Invoke(CommandEnum.LaunchActiveSkill, tuu);
	}

	public void Close () {
		CloseSkillWindow ();
	}

	void MaskCard(string name,bool mask) {
		foreach (var item in actorObject.Values) {
			if(name == item.name) {
				item.color = !mask ? Color.gray : Color.white;
			}
			else{
				item.color = mask ? Color.gray : Color.white;
			}
		}	
	}

	void CloseSkillWindow () {
		MaskCard ("", false);

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
