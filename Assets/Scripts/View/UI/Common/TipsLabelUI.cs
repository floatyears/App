using UnityEngine;
using System.Collections;

public class TipsLabelUI : MonoBehaviour {
	private UILabel showInfoLabel;
	private TweenScale tweenScale;
	private TweenAlpha tweenAlpha;

	void Awake() {
		showInfoLabel = GetComponent<UILabel> ();
		tweenScale = GetComponent<TweenScale> ();
		tweenAlpha = GetComponent<TweenAlpha> ();
	}

	public void ShowInfo(string text) {
		showInfoLabel.text = text;
		tweenScale.enabled = true;
		tweenScale.ResetToBeginning ();
	}

	public void ScaleEnd () {
		tweenScale.enabled = false;
		tweenAlpha.enabled = true;
		tweenAlpha.ResetToBeginning ();
	}

	public void AlphaEnd () {
		showInfoLabel.text = "";
		showInfoLabel.alpha = 1;
		showInfoLabel.transform.localScale = Vector3.zero;
		tweenAlpha.enabled = false;
	}
}
