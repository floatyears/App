using UnityEngine;
using System.Collections;

public class BattleBackground : UIBaseUnity {

	private UITexture background;

	private Camera bottomCamera;

	private Material[] actor;

	private UISprite[] spSprite;


	public override void Init (string name){
		base.Init (name);

		background = FindChild<UITexture> ("Center/Texture");

		bottomCamera = FindChild<Camera> ("BottomCamera");

		actor = new Material[5];
		spSprite = new UISprite[20];

		for (int i = 0; i < actor.Length; i++) {
			actor[i] = FindChild<MeshRenderer>(i.ToString()).material;
		}

		for (int i = 0; i < spSprite.Length; i++) {
			spSprite[i] = FindChild<UISprite>("Sprite/" + i.ToString());
		}
	}

	public override void ShowUI ()
	{
		base.ShowUI ();

		gameObject.SetActive (true);
	}

	public override void CreatUI (){
		base.CreatUI ();
	}

	public override void HideUI (){
		base.HideUI ();
		gameObject.SetActive (false);
	}

	public override void DestoryUI (){
		base.DestoryUI ();
	}
}
