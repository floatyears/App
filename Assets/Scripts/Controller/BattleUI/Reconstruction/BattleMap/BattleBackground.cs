using UnityEngine;
using System.Collections.Generic;

public class BattleBackground : UIBaseUnity {
	private UITexture background;
	private Camera bottomCamera;
	private Material[] actor;
	private UISprite[] spSprite;
	private UISpriteAnimationCustom spriteAnimation;
	private GameObject battleBottom;

	private UISlider bloodBar;
	private UILabel label;
	private int initBlood = -1;
	private int initEnergyPoint = -1;
	private int currentEnergyPoint = -1;

//	private static Dictionary<string, Vector3> attackPosition = new Dictionary<string, Vector3> ();
//	public static Dictionary<string, Vector3> AttackPosition {
//		get { return attackPosition; }
//	}

	private static Vector3 actorPosition = Vector3.zero;
	public static Vector3 ActorPosition	{
		get {
			return actorPosition;
		}
	}

	private BattleBottom _battleBottomScript;
	public BattleBottom battleBottomScript {
		get { return _battleBottomScript; }
	}

	private BattleQuest battleQuest;

	public override void Init (string name){
		base.Init (name);
		bottomCamera = Main.Instance.bottomCamera;
		battleBottom = FindChild<Transform>("BattleBottom").gameObject;
		_battleBottomScript = battleBottom.AddComponent<BattleBottom> ();
		_battleBottomScript.Init (bottomCamera);
		actorPosition = transform.Find ("Position").localPosition;

		actor = new Material[5];
		spSprite = new UISprite[20];
		string path;

		for (int i = 0; i < actor.Length; i++) {
			path = "BattleBottom/Actor/" + i.ToString();
			actor[i] = 	transform.Find(path).renderer.material;
		}

		for (int i = spSprite.Length; i > 0; i--) {
			path = "Panel/"+ i;
			spSprite[spSprite.Length - i] = battleBottom.transform.Find(path).GetComponent<UISprite>();
		}

		spriteAnimation = battleBottom.transform.Find ("Panel/HP").GetComponent<UISpriteAnimationCustom> ();
		bloodBar = battleBottom.transform.Find("Panel/Slider").GetComponent<UISlider>();
		label = battleBottom.transform.Find("Panel/Label").GetComponent<UILabel>();

		InitTransform ();
	}

	public void SetBattleQuest (BattleQuest bq) {
		battleQuest = bq;
		_battleBottomScript.battleQuest = bq;
	}

	void InitTransform() {
		TUnitParty upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		Dictionary<int,TUserUnit> userUnitInfo = upi.UserUnit;
		for (int i = 0; i < userUnitInfo.Count; i++) {
			TUserUnit tuu = userUnitInfo[i];
			if(tuu == null) {
				continue;
			}
//			Vector3 pos =  FindChild<Transform>("Bottom/" + (i + 1)).localPosition;
//			Debug.LogError("i : " + i + " tuu : " + tuu.MakeUserUnitKey());
//			attackPosition.Add(tuu.MakeUserUnitKey(), pos);
		}
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		battleBottom.SetActive (true);
		AddListener ();
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void HideUI () {
		base.HideUI ();
		gameObject.SetActive (false);
		battleBottom.SetActive (false);

		RemoveListener ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		Destroy (battleBottom);
		_battleBottomScript = null;
//		attackPosition.Clear ();
	}

	public void InitData (int blood, int maxBlood, int energyPoint) {
		initBlood = maxBlood;
		tempNum = blood;
		currentEnergyPoint = initEnergyPoint = energyPoint;
		SetBlood (tempNum); 
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

	private int tempNum = 0;
	void SetBlood (int num) {
		string info = "HP:" + num + "/" + initBlood;
//		Debug.LogError ("SetBlood : " + num);
		label.text = info;
		if (num > tempNum) {
			spriteAnimation.Reset();
		}
		bloodBar.value = DGTools.IntegerSubtriction(num,initBlood);
	}

	int preBlood = 0;
	void ListenUnitBlood (object data) {
		int currentBlood = (int)data;
//		Debug.LogError ("currentBlood : " + currentBlood);
		SetBlood (currentBlood);
	}

	void ListenEnergyPoint (object data) {
		int energyPoint = (int) data;

		for (int i = 0; i < spSprite.Length; i++) {
			UISprite sprite = spSprite[i];
//			Debug.LogError(" ListenEnergyPoint : " + (i < energyPoint) + " sprite : " + sprite);
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

	void AddListener () {
		MsgCenter.Instance.AddListener (CommandEnum.UnitBlood, ListenUnitBlood);
		MsgCenter.Instance.AddListener (CommandEnum.EnergyPoint, ListenEnergyPoint);
//		Debug.LogError("listen energy point");
	}

	void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitBlood, ListenUnitBlood);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnergyPoint, ListenEnergyPoint);
	}
}
