using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleBottomView : ViewBase {
	private Camera bottomCamera;
	private RaycastHit rch;
	private TUnitParty upi;
	private Dictionary<int, UITexture> actorObject = new Dictionary<int, UITexture>();
	private Dictionary<int, ActiveSkill> unitInfoPos = new Dictionary<int,ActiveSkill> ();

	private GameObject activeEnableEffect;
	private Dictionary<int, GameObject> enableSKillPos = new Dictionary<int, GameObject> ();
	private List<int> enablePos = new List<int> ();

	public bool IsUseLeaderSkill = false;

	public static bool noviceGuideNotClick = false;

	public static bool notClick = false;

	private static int setPos = -1;
	private static bool storeShiedInput = false;

	private UITexture[] actor;
	private UISprite[] spSprite;
	private UISpriteAnimationCustom spriteAnimation;
	
	private UISlider bloodBar;
	private UILabel label;
	private int initBlood = -1;
	private int initEnergyPoint = -1;
	private int currentEnergyPoint = -1;
	
	private static Vector3 actorPosition = Vector3.zero;
	public static Vector3 ActorPosition	{
		get {
			return actorPosition;
		}
	}

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

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		this.bottomCamera = Main.Instance.bottomCamera;
		RemoveAllSkill ();

		InitTransform ();

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


		actorPosition = transform.Find ("Position").localPosition;
		
		actor = new UITexture[5];
		spSprite = new UISprite[20];
		string path;
		
		for (int i = 0; i < actor.Length; i++) {
			path = "Actor/" + i.ToString();
			actor[i] = 	transform.Find(path).GetComponent<UITexture>();
		}
		
		GameObject panel = FindChild("Panel");
		panel.layer = GameLayer.BottomInfo;
		for (int i = spSprite.Length; i > 0; i--) {
			path = "Panel/"+ i;
			spSprite[spSprite.Length - i] = transform.Find(path).GetComponent<UISprite>();
		}
		
		spriteAnimation = FindChild<UISpriteAnimationCustom> ("Panel/HP");
		bloodBar = FindChild<UISlider>("Panel/Slider");
		label = FindChild<UILabel>("Panel/HPLabel");
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
	

	void OnDisable () {
		GameInput.OnUpdate -= OnRealease;
	}

	void OnEnable () {
		GameInput.OnUpdate += OnRealease;
	}

	public void PlayerDead() {
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
//		if (upi == null || battleQuest.role.isMove) {
//			return;	
//		}

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
//				battleQuest.topUI.SheildInput(false);
//				battleSkillObject.SetActive(true);
//				battleSkill.Refresh(tuu, Boost, Close);
				ModuleManager.SendMessage(ModuleEnum.BattleSkillModule,"refresh",tuu, Boost as Callback, Close as Callback);
//				BattleMapView.waitMove = true;
//				battleQuest.battle.ShieldGameInput(false);
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

//		battleQuest.topUI.SheildInput(true);
		ModuleManager.SendMessage (ModuleEnum.BattleTopModule, "ban", true);
//		BattleMapView.waitMove = false;
//		if (battleQuest.battle.isShowEnemy) {
//			battleQuest.battle.ShieldGameInput(true);
//		}
//		battleSkillObject.SetActive(false);
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
	

//	public void SetBattleQuest (BattleMapModule bq) {
//		battleQuest = bq;
//		//		_battleBottomScript.battleQuest = bq;
//	}
	
	void InitTransform() {
		TUnitParty upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		Dictionary<int,TUserUnit> userUnitInfo = upi.UserUnit;
		for (int i = 0; i < userUnitInfo.Count; i++) {
			TUserUnit tuu = (userUnitInfo.ContainsKey(i) ? userUnitInfo[i] : null);
			if(tuu == null) {
				continue;
			}
		}
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		AddListener ();
	}
	//
	//	public override void CreatUI () {
	//		base.CreatUI ();
	//	}
	
	public override void HideUI () {
		base.HideUI ();
		gameObject.SetActive (false);
		//		_battleBottomScript.RemoveAllSkill ();
		RemoveListener ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
		//		_battleBottomScript = null;
	}
	
	public void InitData (BattleBaseData data) {
		initBlood = data.maxBlood;
		currentBlood = data.Blood;
		currentEnergyPoint = initEnergyPoint = data.EnergyPoint;
		SetBlood (currentBlood); 
		InitSP ();
	}
	
	void InitSP () {
		for (int i = 0; i < spSprite.Length; i++) {
			if(i >= initEnergyPoint) {
				spSprite[i].enabled = false;
			} else {
				spSprite[i].enabled = true;
			}
		}
	}
	
	private int currentBlood = 0;
	void SetBlood (int num) {
		string info = num + "/" + initBlood;
		label.text = info;
		//		if (num > currentBlood || num == initBlood) {
		//			spriteAnimation.Reset();
		//		}
		currentBlood = num;
		bloodBar.value = DGTools.IntegerSubtriction(num, initBlood);
	}
	
	int preBlood = 0;
	void ListenUnitBlood (object data) {
		int blood = (int)data;
		SetBlood (blood);
	}
	
	void ListenEnergyPoint (object data) {
		int energyPoint = (int) data;
		for (int i = 0; i < spSprite.Length; i++) {
			UISprite sprite = spSprite[i];
			if(i < energyPoint) {
				if(!sprite.enabled) {
					sprite.enabled = true;
				}
			} else {
				if(sprite.enabled) {
					sprite.enabled = false;
				}
			}
		}
	}
	
	void RecoverHP(object data) {
		//		AttackInfo ai = data as AttackInfo;
		//
		//		if (ai.AttackRange == 2) {
		//			spriteAnimation.Reset();
		//		}
	}
	
	void ShowHPAnimation(object data) {
		AudioManager.Instance.PlayAudio(AudioEnum.sound_hp_recover);
		spriteAnimation.Reset();
	}
	
	void AddListener () {
		MsgCenter.Instance.AddListener (CommandEnum.UnitBlood, ListenUnitBlood);
		MsgCenter.Instance.AddListener (CommandEnum.EnergyPoint, ListenEnergyPoint);
		//		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, RecoverHP);
		MsgCenter.Instance.AddListener (CommandEnum.ShowHPAnimation, ShowHPAnimation);
	}
	
	void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitBlood, ListenUnitBlood);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnergyPoint, ListenEnergyPoint);
		//		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, RecoverHP);
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowHPAnimation, ShowHPAnimation);
	}
}
