using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using bbproto;

public class BattleBottomView : ViewBase {
//	private Camera bottomCamera;
//	private RaycastHit rch;
//	private TUnitParty upi;
//	private Dictionary<GameObject, UITexture> actorObject = new Dictionary<GameObject, UITexture>();
	private Dictionary<int, GameObject> enableSKillPos = new Dictionary<int, GameObject> ();
	private List<int> enablePos = new List<int> ();

	private Dictionary<GameObject, ActiveSkill> unitInfoPos = new Dictionary<GameObject,ActiveSkill> ();

	private UITexture[] actor;
	private UISprite[] spSprite;
	private UISpriteAnimationCustom spriteAnimation;
	
	private UISlider bloodBar;
	private UILabel label;
	private int initBlood = -1;
	private int initEnergyPoint = -1;
	private int currentEnergyPoint = -1;

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		List<UserUnit> userUnitInfo = DataCenter.Instance.UnitData.PartyInfo.CurrentParty.UserUnit;

		Transform actorTrans = transform.Find ("Actor");
		actor = new UITexture[5];
		for (int i = 0; i < 5; i++) {
			GameObject temp = actorTrans.Find(i.ToString()).gameObject;	

			UISprite tex =  actorTrans.Find(i.ToString() + "_Border").GetComponent<UISprite>();
			UISprite bgSpr = actorTrans.Find(i.ToString() + "_bg").GetComponent<UISprite>();
			UISprite skillBGSpr = actorTrans.Find(i.ToString()+ "_skillBg").GetComponent<UISprite>();
			UISprite skillSpr = actorTrans.Find(i.ToString() + "_skill").GetComponent<UISprite>();
			UITexture texture = actor[i] = temp.GetComponent<UITexture>();

//			actorObject.Add(temp, texture);
//			Debug.Log("battle attack: " + userUnitInfo[i] + " i: " + i);
			if(userUnitInfo[i] == null) {
				temp.gameObject.SetActive(false);
				tex.enabled = false;
				bgSpr.enabled = false;
				skillBGSpr.enabled = false;
				skillSpr.enabled = false;

			} else {
				UnitInfo tui = userUnitInfo[i].UnitInfo;

				SkillBase sbi = DataCenter.Instance.BattleData.GetSkill (userUnitInfo[i].MakeUserUnitKey (), tui.activeSkill, SkillType.ActiveSkill);
				if(sbi != null){
					ActiveSkill activeSkill =  sbi as ActiveSkill;
					unitInfoPos.Add(temp, activeSkill);
					activeSkill.AddListener(ActiveSkillCallback);
				}

				ResourceManager.Instance.GetAvatar(UnitAssetType.Profile,tui.id, o=>{
					if(o != null) {
						texture.mainTexture = o as Texture2D;
//						float scale = 80f/105f;
//						float h = texture.mainTexture.height*tui.ShowPos.h/scale;
						float height = ((i == 0) ? 135f : 110f)/(115f*105f/80f)*tui.showPos.h;
						float y = tui.showPos.y + tui.showPos.h - height;
						if(y > 1)
							y = y - 1;
//						float x = tui.ShowPos.x - tui.ShowPos.w*25f/160f;
						texture.uvRect = new Rect( tui.showPos.x, y, tui.showPos.w, height);
					}
				});
				
				tex.spriteName = "avatar_border_" + (i == 0 ? "l" : "f") + "_" + ((int)tui.type).ToString();//GetUnitTypeSpriteName(i, tui.Type);
				bgSpr.spriteName = "avatar_bg_" + (i == 0 ? "l" : "f") + "_"  + ((int)tui.type).ToString();
				skillSpr.spriteName = "icon_skill_" + ((int)tui.type).ToString();
			}
			UIEventListenerCustom.Get(temp).onClick += ClickItem;
		}
		

		spSprite = new UISprite[20];

		//		FindChild("Board").layer = GameLayer.BottomInfo;
		for (int i = spSprite.Length; i > 0; i--) {
			spSprite[spSprite.Length - i] = transform.Find("Board/"+ i).GetComponent<UISprite>();
		}
		
		spriteAnimation = FindChild<UISpriteAnimationCustom> ("Board/HP");
		bloodBar = FindChild<UISlider>("Board/Slider");
		label = FindChild<UILabel>("Board/HPLabel");
	}

	public override void CallbackView (params object[] args)
	{
		switch (args[0].ToString()) {
		case "init_data":
			InitData((int)args[1],(int)args[2],(int)args[3]);
			break;
		case "player_dead":
//			PlayerDead();
			MaskCard("",false);
			break;
		case "energy_point":
			ListenEnergyPoint(args[1]);
			break;
		case "update_blood":
			int blood = (int)args[1];
			SetBlood (blood);
			if(args.Length > 2 && (bool)args[2]){ // recover
				AudioManager.Instance.PlayAudio(AudioEnum.sound_hp_recover);
				spriteAnimation.Reset();
			}
			break;
		case "close_skill_window":
			MaskCard("",false);
			break;
		default:
			break;
		}
	}

	void InitData (int blood , int maxBlood,int enegyPoint) {
		initBlood = maxBlood;
		currentBlood = blood;
		currentEnergyPoint = initEnergyPoint = enegyPoint;
		SetBlood (currentBlood); 
		
		for (int i = 0; i < spSprite.Length; i++) {
			if(i >= initEnergyPoint) {
				spSprite[i].enabled = false;
			} else {
				spSprite[i].enabled = true;
			}
		}
	}

	void ActiveSkillCallback(object data) {
		ActiveSkill activeSKill = data as ActiveSkill;
		foreach (var item in unitInfoPos) {
			if(item.Value.GetBaseInfo().id == activeSKill.GetBaseInfo().id && item.Value.CoolingDone) {
				EffectManager.Instance.PlayEffect("activeskill_enabled",item.Key.transform);
//				if (enableSKillPos.ContainsKey (item.Key)) {
//					return;	
//				}
//				
//				enableSKillPos.Add (item.Key, null);
//				enablePos.Add (item.Key);

			}
		}
	}	

	public void Boost() {
//		RemoveSkillEffect (prevID);
		//		Debug.LogError("Boost Active Skill : " + tuu);
//		MsgCenter.Instance.Invoke(CommandEnum.LaunchActiveSkill, tuu);
	}

	void RemoveSkillEffect (int pos) {
//		if(enableSKillPos.ContainsKey(pos))
//			enableSKillPos.Remove (pos);
//		if(enablePos.Contains(pos))
//			enablePos.Remove (pos);
	}

	void ClickItem (GameObject obj) {
		UITexture tex = actor [int.Parse (obj.name)];
		if (tex != null && tex.color == Color.white) {
			MaskCard (obj.name, true);
			ModuleManager.Instance.ShowModule(ModuleEnum.BattleSkillModule,"show_skill_window",DataCenter.Instance.UnitData.PartyInfo.CurrentParty.UserUnit [int.Parse(obj.name)]);

		}

	}

	void MaskCard(string name,bool mask) {
		foreach (var item in actor) {
			if(name == item.name) {
				item.color = !mask ? Color.gray : Color.white;
			} else {
				item.color = mask ? Color.gray : Color.white;
			}
		}
	}

	private int currentBlood = 0;
	void SetBlood (int num) {
		string info = num + "/" + initBlood;
		label.text = info;
		currentBlood = num;
		bloodBar.value = DGTools.IntegerSubtriction(num, initBlood);
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

	public void SetLeaderToNoviceGuide(bool isInNoviceGuide){
		if (isInNoviceGuide) {
			GameObject temp = transform.Find ("Actor/0").gameObject;
			temp.layer = LayerMask.NameToLayer ("NoviceGuide");	
		} else {
			GameObject temp = transform.Find ("Actor/0").gameObject;
			temp.layer = LayerMask.NameToLayer ("Bottom");	
		}
	}



	
	public override void DestoryUI () {
		base.DestoryUI ();
	}
}
