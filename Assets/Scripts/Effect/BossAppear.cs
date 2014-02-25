using UnityEngine;
using System.Collections;

public class BossAppear : UIBaseUnity {
	private UISprite topSprite;
	private UISprite bottomSprite;
	private UILabel infoLabel;
	private Vector3 position;
	private Vector3 startPosition;

	private Vector3 bottomPosition;
	private Vector3 topPosition;
	private Vector3 bottomTargetPosition;
	private Vector3 topTargetPosition;
	private const int addNum = 70;
	private const float time = 0.2f;
	private const string content = "          Boss\n              Appear";
	private Callback callback;

	public override void Init (string name) {
		base.Init (name);
		position = transform.localPosition;
		float xOffset = -Screen.width;
		startPosition = new Vector3 (position.x + xOffset, position.y, position.z);
		topSprite = FindChild<UISprite>("Top");
		bottomSprite = FindChild<UISprite>("Bottom");
		infoLabel = FindChild<UILabel>("Label");
		bottomPosition = bottomSprite.transform.localPosition;
		topPosition = topSprite.transform.localPosition;
		bottomTargetPosition = new Vector3 (bottomPosition.x, bottomPosition.y - addNum, bottomPosition.z);
		topTargetPosition = new Vector3 (topPosition.x, topPosition.y + addNum, topPosition.z);
		HideUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
	}

	public override void HideUI () {
		base.HideUI ();
		transform.localPosition = startPosition;
		gameObject.SetActive (false);
		topSprite.transform.localPosition = topPosition;
		bottomSprite.transform.localPosition = bottomPosition;
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		Destroy (gameObject);
	}

	void StartMoveUI (string func) {
		iTween.MoveTo(gameObject,iTween.Hash("position",position,"time",0.5f,"islocal",true,"easetype",iTween.EaseType.easeInQuart,"oncomplete", func,"oncompletetarget", gameObject));
	}
	
	public void PlayBossAppera(Callback cb) {
		if (currentState == UIState.UIHide) {
			ShowUI();	
		}
		callback = cb;
		StartMoveUI ("BossAppearAnim");
	}

	void BossAppearAnim() {
		iTween.ScaleFrom (infoLabel.gameObject, iTween.Hash ("y",0f,"easetype",iTween.EaseType.easeInQuart,"time",time));
		iTween.MoveTo(topSprite.gameObject,iTween.Hash("position",topTargetPosition,"islocal",true,"easetype",iTween.EaseType.easeInQuart,"time",time));
		iTween.MoveTo(bottomSprite.gameObject,iTween.Hash("position",bottomTargetPosition,"islocal",true,"easetype",iTween.EaseType.easeInQuart,"time",time,"oncomplete","PlayEnd","oncompletetarget",gameObject));
		
		infoLabel.text = content;
		infoLabel.fontStyle = FontStyle.Italic;
	}

	void PlayEnd () {
		GameTimer.GetInstance ().AddCountDown (1f, End);
	}

	void End(){
		if (callback != null) {
			callback();
		}
		HideUI ();
	}
}
