using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CountDownUnity : ViewBase {
//	private UISprite numberSprite;
	private UILabel numberLabel;
	private UISprite circleSprite;
	private float countDownValue = 1f;
	private bool Stop = true;

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);
//		numberSprite = FindChild<UISprite>("Number");
		numberLabel = FindChild<UILabel>("Number");
		circleSprite = FindChild<UISprite>("Circle");
		transform.localPosition = new Vector3 (0f, 100f, 0f);
		HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void HideUI () {
		base.HideUI ();
		GameInput.OnUpdate -= HandleOnUpdate;
		gameObject.SetActive (false);
		Stop = false;
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		GameInput.OnUpdate += HandleOnUpdate;
	}

	void HandleOnUpdate () {
		if (Stop) {
			countDownValue -= Time.deltaTime;
			circleSprite.fillAmount = countDownValue;
		}

	}

	public void SetCurrentTime (int time) {
		if (time == 0) {
			HideUI ();
			return;
		}

		if (!Stop) {
			Stop = true;	
		}

		AudioManager.Instance.PlayAudio (AudioEnum.sound_count_down);
		iTween.ScaleFrom (gameObject, new Vector3 (1.25f, 1.25f, 1.25f), 0.3f);
		countDownValue = 1f;
		numberLabel.text = time.ToString ();
	}

}