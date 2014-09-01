using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowNewCardView : ViewBase {
	public override void Init (UIConfigItem config) {
		base.Init (config);
		InitComponent ();
		MsgCenter.Instance.AddListener (CommandEnum.ShowNewCard, ShowProfile);
	}

	public override void ShowUI () {
//		UIManager.Instance.HideBaseScene ();

//		sEnum = UIManager.Instance.baseScene.PrevScene;
		base.ShowUI ();
		ActiveButton (false);
		ActiveEffect (true);
	}

	public override void HideUI () {
//		UIManager.Instance.ShowBaseScene ();
		ActiveEffect (false);
		base.HideUI ();
		ClearStar ();
		profileTexture.mainTexture = null;
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowNewCard, ShowProfile);
	}
	private ModuleEnum sEnum;

	private GameObject backEffect;
	private GameObject bombEffect;
	private UITexture profileTexture;

	private TUserUnit userUnit;

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

		UIEventListener.Get (detailButton.gameObject).onClick = DetailButtonCallback;
		UIEventListener.Get (returnButton.gameObject).onClick = ReturnButtonCallback;
	}

	void ShowProfile(object data) {
		userUnit = data as TUserUnit;

		if (userUnit== null) {
			return;	
		}

		cardNameLabel.text = userUnit.UnitInfo.Name;

		int maxRare = userUnit.UnitInfo.MaxRare == 0 ? userUnit.UnitInfo.Rare : userUnit.UnitInfo.MaxRare;

		SortStarBg (maxRare);

		GameTimer.GetInstance ().AddCountDown (0.5f, ShowTexture);
	}

	void SortStarBg(int maxRare) {
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
	}

	void SortBG(bool isOdd, int endIndex, Vector3 startPosition) {
		int realStartIndex = -endIndex, 
		realEndIndex = isOdd ? endIndex - 1 : endIndex;

		for (int i = realStartIndex; i <= realEndIndex; i++) {
			GameObject go = GenerateSprite(starBgSpr);
			float xOffset = i * starBgSpr.width;
			go.transform.localPosition =  new Vector3(startPosition.x + xOffset, startPosition.y, startPosition.z);
		}
	}

	GameObject GenerateSprite(UISprite sprite) {
		GameObject go = NGUITools.AddChild(starParent, sprite.gameObject);
		go.SetActive(true);
		starList.Add(go);
		return go;
	}

	void ShowTexture() {
		ResourceManager.Instance.GetProfile (userUnit.Object.unitId, null, texture => {
			Texture2D tex = texture as Texture2D;
			DGTools.ShowTexture(profileTexture, tex);
			iTween.ScaleFrom(profileTexture.gameObject, iTween.Hash("scale", TribleScale, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuart));
			GameTimer.GetInstance().AddCountDown(0.5f, ShowBombEffect);
		});
	}

	void DetailButtonCallback(GameObject go) {
		UIManager.Instance.ChangeScene (ModuleEnum.UnitDetailModule);
		MsgCenter.Instance.Invoke (CommandEnum.ShowUnitDetail, userUnit);
//		UIManager.Instance.baseScene.PrevScene = sEnum;

//		ClearStar ();
		HideUI ();
	}

	void ReturnButtonCallback(GameObject go) {
		UIManager.Instance.ChangeScene (sEnum);

//		ClearStar ();
	}

	void ClearStar() {
//		Debug.LogError ("SHOW NEW CARD VIEW CLEAR STAR");
		profileTexture.mainTexture = null;
		for (int i = 0; i < starList.Count; i++) {
			Destroy(starList[i]);
		}
		starList.Clear ();
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

	void ShowBombEffect() {
		bombEffect.SetActive (true);

//		int star = userUnit.UnitInfo.Rare;
//
//		Vector3 starPosition = starSpr.transform.localPosition;
//
//		int positionIndex = star >> 1;
//		Vector3 initPosition = Vector3.zero;
//		if (DGTools.IsOddNumber (star)) {
//			initPosition = new Vector3 (starPosition.x - starSprWidth * positionIndex, starPosition.y, starPosition.z);
//		} else {
//			float xCoor = starSprWidth * positionIndex - starSprWidth * 0.5f;
//			initPosition = new Vector3(starPosition.x - xCoor, starPosition.y, starPosition.z);
//		}

		StartCoroutine (ShowStar (userUnit.UnitInfo.Rare));
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
