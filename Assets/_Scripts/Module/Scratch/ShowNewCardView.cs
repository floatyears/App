using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ShowNewCardView : ViewBase {
	public override void Init (UIConfigItem config, Dictionary<string, object> data = null){
		base.Init (config,data);
		InitComponent ();
//		MsgCenter.Instance.AddListener (CommandEnum.ShowNewCard, ShowProfile);
	}

	public override void ShowUI () {
		base.ShowUI ();
		ActiveButton (false);
		ActiveEffect (true);

		if (viewData.ContainsKey ("data")) {
			ShowProfile(viewData["data"] as UserUnit);
		}
	}

	public override void HideUI () {
		ActiveEffect (false);
		base.HideUI ();

		profileTexture.mainTexture = null;
		for (int i = 0; i < starList.Count; i++) {
			Destroy(starList[i]);
		}
		starList.Clear ();
	}

//	private ModuleEnum sEnum;

	private GameObject backEffect;
	private GameObject bombEffect;
	private UITexture profileTexture;

	private UserUnit userUnit;

	private UIButton detailButton;
	private UIButton returnButton;
	private UILabel detailButtonLabel;
	private UILabel returnButtonLabel;
	private UISprite starSpr;
	private UISprite starBgSpr;
	private Vector3 starPosition = Vector3.zero;

	private UILabel cardNameLabel;

	private int starSprWidth = 0;
	private GameObject starParent;

	private List<GameObject> starList = new List<GameObject> ();

	private Vector3 DoubleScale = new Vector3 (2f, 2f, 2f);
	private Vector3 TribleScale = new Vector3 (3f, 3f, 3f);

	void InitComponent() {
		backEffect = transform.Find ("scratch1").gameObject;
		bombEffect = transform.Find ("scratch2").gameObject;
		bombEffect.gameObject.SetActive (false);
		profileTexture = FindChild<UITexture> ("TexturePanel/Texture");
		cardNameLabel = FindChild<UILabel>("Star/CardName");
		starSpr = FindChild<UISprite>("Star/Star1");
		starBgSpr = FindChild<UISprite> ("Star/Starbg1");
		starParent = starSpr.transform.parent.gameObject;
		starSprWidth = starSpr.width;
		starPosition = starBgSpr.transform.localPosition;

		detailButton = FindChild<UIButton>("DetailButton");
		detailButtonLabel = FindChild<UILabel>("DetailButton/Label");
		returnButton = FindChild<UIButton>("ReturnButton");
		returnButtonLabel = FindChild<UILabel> ("ReturnButton/Label");

		detailButtonLabel.text = TextCenter.GetText ("Btn_ToDetail");
		returnButtonLabel.text = TextCenter.GetText ("Btn_SceneBack");

		UIEventListenerCustom.Get (detailButton.gameObject).onClick = DetailButtonCallback;
		UIEventListenerCustom.Get (returnButton.gameObject).onClick = ReturnButtonCallback;
	}

	void ShowProfile(UserUnit userUnit) {

		if (userUnit== null) {
			return;	
		}

		cardNameLabel.text = userUnit.UnitInfo.name;

		int maxRare = userUnit.UnitInfo.maxStar == 0 ? userUnit.UnitInfo.rare : userUnit.UnitInfo.maxStar;

		if (maxRare <= 0) {
			return;	
		}
		
		if (DGTools.IsOddNumber (maxRare)) {
			int halfCount = (maxRare - 1) >> 1;
			SortBG(false ,halfCount, starPosition);
		} else {
			int halfCount = maxRare >> 1;
			Vector3 startPosition = new Vector3(starPosition.x + starBgSpr.width * 0.5f, starPosition.y, starPosition.z);
			SortBG(true ,halfCount, startPosition);
		}

		GameTimer.GetInstance ().AddCountDown (0.5f, ()=>{
			Debug.Log("show new card effect end1");
			ResourceManager.Instance.GetAvatar (UnitAssetType.Profile, userUnit.unitId, texture => {
				Texture2D tex = texture as Texture2D;
				DGTools.ShowTexture(profileTexture, tex);
				iTween.ScaleFrom(profileTexture.gameObject, iTween.Hash("scale", TribleScale, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuart));
				GameTimer.GetInstance().AddCountDown(0.5f, ()=>{
					Debug.Log("show new card effect end");
					bombEffect.SetActive (true);
					
					StartCoroutine (ShowStar (userUnit.UnitInfo.rare));
				});
			});
		});
	}

	void SortBG(bool isOdd, int endIndex, Vector3 startPosition) {
		int realStartIndex = -endIndex, 
		realEndIndex = isOdd ? endIndex - 1 : endIndex;

		for (int i = realStartIndex; i <= realEndIndex; i++) {
			GameObject go = NGUITools.AddChild(starParent, starBgSpr.gameObject);
			go.SetActive(true);
			starList.Add(go);
			float xOffset = i * starBgSpr.width;
			go.transform.localPosition =  new Vector3(startPosition.x + xOffset, startPosition.y, startPosition.z);
		}
	}

	void DetailButtonCallback(GameObject go) {
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule,"unit",userUnit);
		ModuleManager.Instance.HideModule (ModuleEnum.ShowNewCardModule);
	}

	void ReturnButtonCallback(GameObject go) {
		ModuleManager.Instance.HideModule (ModuleEnum.ShowNewCardModule);
	}
	

	void ActiveButton(bool b) {
		if (detailButton != null) {
			detailButton.gameObject.SetActive(b);
		}

		if (returnButton != null) {
			returnButton.gameObject.SetActive(b);
		}
	}

	void ActiveEffect(bool b) {
		if (backEffect != null && backEffect.activeSelf != b) {
			backEffect.SetActive (b);
		}

		if (bombEffect != null && bombEffect.activeSelf != b) {
			bombEffect.SetActive (b);
		}
	}
	
	IEnumerator ShowStar(int rare) {
		for (int i = 0; i < rare; i++) {
			GameObject go = NGUITools.AddChild(starParent, starSpr.gameObject);

			go.SetActive(true);
//			Debug.LogError("ShowStar : i" + i + " bgcount : " + (starList.Count - i + 1) + " rare ; " + rare + " max rare : " + userUnit.UnitInfo.MaxRare);
			go.transform.localPosition = starList[i].transform.localPosition;

			AudioManager.Instance.PlayAudio(AudioEnum.sound_star_appear);

			iTween.ScaleFrom(go, iTween.Hash("scale", TribleScale, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuart));

			starList.Add(go);

			yield return new WaitForSeconds(0.2f);
		}

		ActiveButton (true);
	}
}
