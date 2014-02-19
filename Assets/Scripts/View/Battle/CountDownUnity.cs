using UnityEngine;
using System.Collections;

public class CountDownUnity : UIBaseUnity {
	private UISprite numberSprite;
	private UISprite circleSprite;
	private float countDownValue = 1f;
	private bool Stop = true;

	public override void Init (string name) {
		base.Init (name);
		numberSprite = FindChild<UISprite>("Number");
		circleSprite = FindChild<UISprite>("Circle");
		transform.localPosition = new Vector3 (0f, 100f, 0f);
		HideUI ();
	}

	public override void CreatUI () {
		base.CreatUI ();
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

		AudioManager.Instance.PlayAudio (AudioEnum.sound_count_time);
		iTween.ScaleFrom (gameObject, new Vector3 (1.25f, 1.25f, 1.25f), 0.3f);
		countDownValue = 1f;
		numberSprite.spriteName = time.ToString ();
//		numberSprite.width = numberSprite.GetAtlasSprite ().width;
//		numberSprite.height = numberSprite.GetAtlasSprite ().height;
	}

}