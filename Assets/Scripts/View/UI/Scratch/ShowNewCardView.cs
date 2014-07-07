using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowNewCardView : UIComponentUnity {
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
		InitComponent ();
		MsgCenter.Instance.AddListener (CommandEnum.ShowNewCard, ShowProfile);
	}

	public override void ShowUI () {
		UIManager.Instance.HideBaseScene ();

		sEnum = UIManager.Instance.prevScene;
		base.ShowUI ();
		ActiveButton ();
	}

	public override void HideUI () {
		UIManager.Instance.ShowBaseScene ();

		base.HideUI ();
		profileTexture.mainTexture = null;
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowNewCard, ShowProfile);
	}
	private SceneEnum sEnum;

	private GameObject backEffect;
	private GameObject bombEffect;
	private UITexture profileTexture;

	private TUserUnit userUnit;

	private UIButton detailButton;
	private UIButton returnButton;
	private UILabel detailButtonLabel;
	private UILabel returnButtonLabel;
	private UISprite starSpr;
	private int starSprWidth = 0;
	private GameObject starParent;

	private List<GameObject> starList = new List<GameObject> ();

	private Vector3 DoubleScale = new Vector3 (2f, 2f, 2f);
	private Vector3 TribleScale = new Vector3 (3f, 3f, 3f);

	void InitComponent() {
		backEffect = transform.Find ("scratch1").gameObject;
		bombEffect = transform.Find ("scratch2").gameObject;
		bombEffect.gameObject.SetActive (false);
		profileTexture = FindChild<UITexture> ("Texture");
		starSpr = FindChild<UISprite>("Star/Star1");
		starParent = starSpr.transform.parent.gameObject;
		starSprWidth = starSpr.width;

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
		GameTimer.GetInstance ().AddCountDown (0.5f, ShowTexture);
	}

	void ShowTexture() {
		DataCenter.Instance.GetProfile (userUnit.Object.unitId, null, texture => {
			Texture2D tex = texture as Texture2D;
			DGTools.ShowTexture(profileTexture, tex);
			iTween.ScaleFrom(profileTexture.gameObject, iTween.Hash("scale", TribleScale, "time", 0.8f, "easetype", iTween.EaseType.easeOutBounce));
			GameTimer.GetInstance().AddCountDown(0.6f, ShowBombEffect);
		});
	}

	void DetailButtonCallback(GameObject go) {

		UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke (CommandEnum.ShowUnitDetail, userUnit);
		UIManager.Instance.baseScene.PrevScene = sEnum;

		ClearStar ();
	}

	void ReturnButtonCallback(GameObject go) {
		UIManager.Instance.ChangeScene (sEnum);

		ClearStar ();
	}

	void ClearStar() {
		for (int i = 0; i < starList.Count; i++) {
			Destroy(starList[i]);
		}
		starList.Clear ();
	}

	void ActiveButton() {
		if (detailButton != null) {
			detailButton.gameObject.SetActive(false);
			detailButton.gameObject.SetActive(true);
		}

		if (returnButton != null) {
			returnButton.gameObject.SetActive(false);
			returnButton.gameObject.SetActive(true);
		}
	}

	void ShowBombEffect() {
		bombEffect.SetActive (true);

		int star = userUnit.UnitInfo.Rare;

		Vector3 starPosition = starSpr.transform.localPosition;
		int positionIndex = star >> 1;
		Vector3 initPosition = Vector3.zero;
		if (DGTools.IsOddNumber (star)) {
			initPosition = new Vector3 (starPosition.x - starSprWidth * positionIndex, starPosition.y, starPosition.z);
		} else {
			float xCoor = starSprWidth * positionIndex - starSprWidth * 0.5f;
			initPosition = new Vector3(starPosition.x - xCoor, starPosition.y, starPosition.z);
		}

		StartCoroutine (ShowStar (star, initPosition));
	}

	IEnumerator ShowStar(int rare, Vector3 initPosition) {
		for (int i = 0; i < rare; i++) {
			GameObject go = NGUITools.AddChild(starParent, starSpr.gameObject);
			starList.Add(go);
			go.SetActive(false);
			go.transform.localPosition = initPosition + Vector3.right * starSprWidth;
			iTween.ScaleFrom(go, iTween.Hash("scale", DoubleScale,"time",0.5f,"easetype",iTween.EaseType.easeInQuad));
			yield return 0.2f;
		}
	}
}
