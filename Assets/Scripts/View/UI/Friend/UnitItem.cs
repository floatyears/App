using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitItem : MonoBehaviour {

	public UILabel unitItemInfoLabel;
	private showTurn turn;
	private string firstFadeText;
	private string secondFadeText;

	private float timer;
	private float alternateTime;

	void Start () {
		MsgCenter.Instance.AddListener( CommandEnum.CrossFade, CrossFade);
		turn = showTurn.FirstTurn;
		timer = 0;
		alternateTime = 1f;
		unitItemInfoLabel.text = string.Format( "Lv{0}", firstFadeText );
	}
	
	void CrossFade(object data){
		List<int> fadeList = data as List<int>;

		firstFadeText = fadeList[0].ToString();
		Debug.Log("UnitItem CrossFade(), firstFadeText : " + firstFadeText);
		secondFadeText = fadeList[1].ToString();
		Debug.Log("UnitItem CrossFade(), secondFadeText : " + secondFadeText);

	}

	void Update () {
		timer += Time.deltaTime;
		if( timer < alternateTime )	return;
		switch( turn ){
			case showTurn.FirstTurn:
				turn = showTurn.FirstTurn;
				unitItemInfoLabel.text = string.Format( "Lv:{0}", firstFadeText );
				unitItemInfoLabel.color = Color.white;
				timer = 0f;
				break;
			case showTurn.SecondTurn:
				turn = showTurn.SecondTurn;
				unitItemInfoLabel.text = string.Format( "+{0}", secondFadeText );
				unitItemInfoLabel.color = Color.yellow;
				timer = 0f;
				break;
			default:
				break;
		}
	}

	IEnumerator CrossFade(string str1, string str2, float dt, UILabel label) {
		yield return new WaitForSeconds(dt);
	}
}
