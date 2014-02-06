using UnityEngine;
using System.Collections;

public class BattleBackground : UIBaseUnity {
	private UITexture background;
	private Camera bottomCamera;
	private Material[] actor;
	private UISprite[] spSprite;
	private UISpriteAnimationCustom spriteAnimation;
	private GameObject battleBottom;
	private BattleBottom battleBottomScript;
	private UISlider bloodBar;
	private UILabel label;
	private int initBlood = -1;
	private int initEnergyPoint = -1;
	private int currentEnergyPoint = -1;

	public override void Init (string name){
		base.Init (name);

		background = FindChild<UITexture> ("Center/Texture");

		bottomCamera = FindChild<Camera> ("BottomCamera");

		Object o = LoadAsset.Instance.LoadAssetFromResources ("BattleBottom", ResourceEuum.Prefab);

		battleBottom = Instantiate (o) as GameObject;
	
		battleBottom.GetComponent<UIAnchor> ().uiCamera = ViewManager.Instance.MainUICamera.camera;
		battleBottomScript = battleBottom.AddComponent<BattleBottom> ();
		battleBottomScript.Init (bottomCamera);
		actor = new Material[5];
		spSprite = new UISprite[20];
		string path;
		for (int i = 0; i < actor.Length; i++) {
			path = "Actor/" + (i + 1).ToString();
			actor[i] = 	battleBottom.transform.Find(path).renderer.material;
		}

		for (int i = spSprite.Length; i > 0; i--) {
			path = "Panel/Sprite/"+ i;
			spSprite[spSprite.Length - i] = battleBottom.transform.Find(path).GetComponent<UISprite>();
		}
		spriteAnimation = battleBottom.transform.Find ("Panel/Sprite/HP").GetComponent<UISpriteAnimationCustom> ();
		bloodBar = battleBottom.transform.Find("Panel/Sprite/Slider").GetComponent<UISlider>();
		label = battleBottom.transform.Find("Panel/Label").GetComponent<UILabel>();
	}

	public override void ShowUI ()
	{
		base.ShowUI ();
		background.transform.localPosition = Vector3.zero;
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
	}

	public void InitData (int blood, int energyPoint) {
		initBlood = blood;
		currentEnergyPoint = initEnergyPoint = energyPoint;
		SetBlood (initBlood);
		InitSP ();
	}

	void InitSP () {
		for (int i = 0; i < spSprite.Length; i++) {
			if(i > initEnergyPoint) {
				spSprite[i].enabled = false;
			}
			else {
				spSprite[i].enabled = true;
			}
		}
	}

	void SetBlood (int num) {
		string info = num + "/" + initBlood;
		label.text = info;
		float value = DGTools.IntegerSubtriction(num,initBlood);
		if (bloodBar.value < value) {
			spriteAnimation.Reset();
		}
		bloodBar.value = value;
	}

	void ListenUnitBlood (object data) {
		int currentBlood = (int)data;
		SetBlood (currentBlood);
	}

	void ListenEnergyPoint (object data) {
		int energyPoint = (int) data;
		int remaining = initEnergyPoint - energyPoint;
		if (remaining < 0) {
			LogHelper.LogException(new System.Exception("energy point number is bigger to energy sprite number"));
			return ;
		}
		for (int i = 0; i < remaining; i++) {
			spSprite[energyPoint + i].enabled = false;
		}
	}

	void AddListener () {
		MsgCenter.Instance.AddListener (CommandEnum.UnitBlood, ListenUnitBlood);
		MsgCenter.Instance.AddListener (CommandEnum.EnergyPoint, ListenEnergyPoint);
	}

	void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitBlood, ListenUnitBlood);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnergyPoint, ListenEnergyPoint);
	}
}
