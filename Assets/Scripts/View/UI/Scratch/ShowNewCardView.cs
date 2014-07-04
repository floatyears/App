using UnityEngine;
using System.Collections;

public class ShowNewCardView : UIComponentUnity {
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
		InitComponent ();
	}

	public override void ShowUI () {
		base.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

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

	private Vector3 DoubleScale = new Vector3 (2f, 2f, 2f);
	private Vector3 TribleScale = new Vector3 (3f, 3f, 3f);

	void InitComponent() {
		backEffect = transform.Find ("scratch01").gameObject;
		bombEffect = transform.Find ("Scatch02").gameObject;
		bombEffect.gameObject.SetActive (false);
		profileTexture = FindChild<UITexture> ("Texture");
		starSpr = FindChild<UISprite>("Star2/Star1");
		starParent = starSpr.transform.parent.gameObject;
		starSprWidth = starSpr.width;

		detailButton = FindChild<UIButton>("DetailButton");
		detailButtonLabel = FindChild<UILabel>("DetailButton/Label");
		returnButton = FindChild<UIButton>("ReturnButton");
		returnButtonLabel = FindChild<UILabel> ("ReturnButton/Label");

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
			Texture tex = texture as Texture;
			profileTexture.mainTexture = tex;
			iTween.ScaleFrom(profileTexture.gameObject, iTween.Hash("scale", TribleScale,"time","1f","easetype",iTween.EaseType.easeInOutBack));
			GameTimer.GetInstance().AddCountDown(0.8f, ShowBombEffect);
		});
	}

	void DetailButtonCallback(GameObject go) {
		UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke (CommandEnum.ShowUnitDetail, userUnit);
	}

	void ReturnButtonCallback(GameObject go) {

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
			go.transform.localPosition = initPosition + Vector3.right * starSprWidth;
			iTween.ScaleFrom(go, iTween.Hash("scale", DoubleScale,"time",0.5f,"easetype",iTween.EaseType.easeInQuad));
			yield return 0.2f;
		}
	}
}
