using UnityEngine;
using System.Collections;

public class BattleBackground : UIBaseUnity {

	private UITexture background;

	private Camera bottomCamera;

	private Material[] actor;

	private UISprite[] spSprite;

	private GameObject battleBottom;

	private UISlider bloodBar;

	public override void Init (string name){
		base.Init (name);

		background = FindChild<UITexture> ("Center/Texture");

		bottomCamera = FindChild<Camera> ("BottomCamera");

		Object o = LoadAsset.Instance.LoadAssetFromResources ("BattleBottom", ResourceEuum.Prefab);

		battleBottom = Instantiate (o) as GameObject;
	
		battleBottom.GetComponent<UIAnchor> ().uiCamera = ViewManager.Instance.MainUICamera.camera;
		
		actor = new Material[5];
		spSprite = new UISprite[20];
		string path;
		for (int i = 0; i < actor.Length; i++) {
			path = "Actor/" + (i + 1).ToString();
			actor[i] = 	battleBottom.transform.Find(path).renderer.material;			//FindChild<MeshRenderer>(i.ToString()).material;
		}

		for (int i = 0; i < spSprite.Length; i++) {
			path = "Panel/Sprite/"+(i + 1).ToString();
			spSprite[i] = battleBottom.transform.Find(path).GetComponent<UISprite>();	// <UISprite>("Sprite/" + i.ToString());
		}

		bloodBar = battleBottom.transform.Find("Panel/Sprite/Slider").GetComponent<UISlider>();
	}

	public override void ShowUI ()
	{
		base.ShowUI ();
		background.transform.localPosition = Vector3.zero;
		gameObject.SetActive (true);
		battleBottom.SetActive (true);
	}

	public override void CreatUI (){
		base.CreatUI ();
	}

	public override void HideUI (){
		base.HideUI ();
		gameObject.SetActive (false);
		battleBottom.SetActive (false);
	}

	public override void DestoryUI (){
		base.DestoryUI ();
	}
}
