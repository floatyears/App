using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleBottom : MonoBehaviour {
	private Camera bottomCamera;
	private RaycastHit rch;
	private TUnitParty upi;
	private Dictionary<int, UITexture> actorObject = new Dictionary<int, UITexture>();
	private Dictionary<int, ActiveSkill> unitInfoPos = new Dictionary<int,ActiveSkill> ();
	private GameObject battleSkillObject;
	private BattleSkill battleSkill;

	private GameObject activeEnableEffect;
	private Dictionary<int, GameObject> enableSKillPos = new Dictionary<int, GameObject> ();
	private List<int> enablePos = new List<int> ();

	public bool IsUseLeaderSkill = false;

	public static bool noviceGuideNotClick = false;

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

	private static int setPos = -1;
	private static bool storeShiedInput = false;
	

//	void LaunchActiveSkill(object data) {
//
//	}

	public static void SetClickItem(int pos) {
		if (pos < 0 || pos > 4 || pos == -1) {
			setPos = -1;
			notClick = storeShiedInput;
			return;
		}

		storeShiedInput = notClick;
		notClick = false;
		setPos = pos;
	}

	public void Init(Camera bottomCamera) {
		this.bottomCamera = bottomCamera;
		RemoveAllSkill ();

		ResourceManager.Instance.LoadLocalAsset("Prefabs/BattleSkill", o => {
			GameObject go = o as GameObject;
			battleSkillObject = NGUITools.AddChild (ViewManager.Instance.CenterPanel, go);
			battleSkillObject.layer = GameLayer.BottomInfo;
			battleSkill = battleSkillObject.GetComponent<BattleSkill> ();
//			battleSkill.Init ("BattleSkill");
			battleSkillObject.transform.localPosition = new Vector3(2f, -110f, 0f);
			battleSkillObject.SetActive (false);
		});

		if (upi == null) {
			upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		}

		CheckActiveEnableEffect ();

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
			if(!userUnitInfo.ContainsKey(i) || userUnitInfo[i] == null) {
				temp.gameObject.SetActive(false);
				tex.enabled = false;
				bgSpr.enabled = false;
				skillBGSpr.enabled = false;
				skillSpr.enabled = false;
			} else {
				TUnitInfo tui = userUnitInfo[i].UnitInfo;

				SkillBaseInfo sbi = DataCenter.Instance.GetSkill (userUnitInfo[i].MakeUserUnitKey (), tui.ActiveSkill, SkillType.ActiveSkill);
				if(sbi != null){
					ActiveSkill activeSkill =  sbi as ActiveSkill;
					unitInfoPos.Add(i, activeSkill);
					activeSkill.AddListener(ActiveSkillCallback);
				}

				tui.GetAsset(UnitAssetType.Profile, o=>{
					if(o != null) {
						texture.mainTexture = o as Texture2D;
//						float scale = 80f/105f;
//						float h = texture.mainTexture.height*tui.ShowPos.h/scale;
						float height = ((i == 0) ? 135f : 110f)/(115f*105f/80f)*tui.ShowPos.h;
						float y = tui.ShowPos.y + tui.ShowPos.h - height;
						if(y > 1)
							y = y - 1;
//						float x = tui.ShowPos.x - tui.ShowPos.w*25f/160f;
						texture.uvRect = new Rect( tui.ShowPos.x, y, tui.ShowPos.w, height);
					}
				});
				
				tex.spriteName = GetUnitTypeSpriteName(i, tui.Type);
				bgSpr.spriteName = GetBGSpriteName(i, tui.Type);
				skillSpr.spriteName = GetSkillSpriteName(tui.Type);
			}
		}
	}

	void ActiveSkillCallback(object data) {
		ActiveSkill activeSKill = data as ActiveSkill;
		foreach (var item in unitInfoPos) {
			if(item.Value.skillBase.id == activeSKill.skillBase.id && item.Value.CoolingDone) {
				ActiveSkillEffect(item.Key);
			}
		}
	}

	void CheckActiveEnableEffect() {
		if (activeEnableEffect == null) {
			EffectManager.Instance.GetOtherEffect(EffectManager.EffectEnum.ActiveSkill, o => activeEnableEffect = o as GameObject);
		}
	}

	void ActiveSkillEffect(int pos) {
		CheckActiveEnableEffect ();
		AddActivePos (pos);
	}

	public void RemoveAllSkill() {
		enableSKillPos.Clear ();
		enablePos.Clear ();
	}

	void RemoveSkillEffect (int pos) {
		if(enableSKillPos.ContainsKey(pos))
			enableSKillPos.Remove (pos);
		if(enablePos.Contains(pos))
			enablePos.Remove (pos);
	}

	void AddActivePos(int pos) {
		if (enableSKillPos.ContainsKey (pos)) {
			return;	
		}

		enableSKillPos.Add (pos, null);
		enablePos.Add (pos);
	}

	readonly Vector3 effectScale = new Vector3 (30f, 30f, 30f);
	readonly Vector3 effectOffset = new Vector3 (35f, 15f, 0f);

	void LateUpdate () {
		for (int i = 0; i < enablePos.Count; i++) {
			int pos = enablePos[i];
			GameObject go = enableSKillPos[pos];
			if(go == null) {
				Transform trans = actorObject[pos].transform;
				go = NGUITools.AddChild(trans.parent.gameObject, activeEnableEffect);
				go.layer = GameLayer.EffectLayer;
				go.transform.localPosition = trans.localPosition + effectOffset;
				go.transform.localScale = effectScale;
				enableSKillPos[pos] = go;
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
		MsgCenter.Instance.RemoveListener (CommandEnum.PlayerDead, PlayerDead);
	}

	void OnEnable () {
		GameInput.OnUpdate += OnRealease;
		MsgCenter.Instance.AddListener (CommandEnum.PlayerDead, PlayerDead);
	}

	void PlayerDead(object data) {
		RemoveAllSkill ();
	}

	void OnRealease () {
		if (noviceGuideNotClick && NoviceGuideStepEntityManager.isInNoviceGuide()) {
			return;	
		}

		if (notClick) {
			return;	
		}

		if (Input.GetMouseButtonDown (0)) {
			Ray ray = bottomCamera.ScreenPointToRay (Input.mousePosition);
			int receiveMask = GameLayer.LayerToInt(GameLayer.Default);
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
//			Debug.LogError("CheckCollider id : " + id);
			if(setPos != -1 && id != setPos) {
				return;
			}

			if (upi.UserUnit.ContainsKey (id)) {
				if(id == prevID) {
					CloseSkillWindow();
					prevID = -1;
					return;
				}
				prevID = id;
				MaskCard (name, true);
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
		RemoveSkillEffect (prevID);
//		Debug.LogError("Boost Active Skill : " + tuu);
		MsgCenter.Instance.Invoke(CommandEnum.LaunchActiveSkill, tuu);
	}

	public void Close () {
		CloseSkillWindow ();
	}

	void MaskCard(string name,bool mask) {
		foreach (var item in actorObject.Values) {
			if(name == item.name) {
				item.color = !mask ? Color.gray : Color.white;
			} else {
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
		return;
		if (isInNoviceGuide) {
			GameObject temp = transform.Find ("Actor/0").gameObject;
			temp.layer = LayerMask.NameToLayer ("NoviceGuide");	
		} else {
			GameObject temp = transform.Find ("Actor/0").gameObject;
			temp.layer = LayerMask.NameToLayer ("Bottom");	
		}
	}
}
