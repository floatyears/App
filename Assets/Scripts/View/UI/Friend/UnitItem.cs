using UnityEngine;
using System.Collections;

public class UnitItem : MonoBehaviour {

	public UILabel unitItemInfoLabel;
	private showTurn turn;
	public int addPoint;
	public int level;
	private float timer;
	private float alternateTime;

	void Start () {
		turn = showTurn.levelTurn;
		addPoint = 7;
		level = 32;
		timer = 0;
		alternateTime = 1f;
		unitItemInfoLabel.text = string.Format( "Lv{0}", level );
	}

	void Update () {
		timer += Time.deltaTime;
		if( timer < alternateTime )	return;
		switch( turn ){
			case showTurn.levelTurn:
				turn = showTurn.addPointTurn;
				unitItemInfoLabel.text = string.Format( "Lv:{0}", level );
				unitItemInfoLabel.color = Color.white;
				timer = 0f;
				break;
			case showTurn.addPointTurn:
				if( addPoint == 0 )	return;
				turn = showTurn.levelTurn;
				unitItemInfoLabel.text = string.Format( "+{0}", addPoint );
				unitItemInfoLabel.color = Color.yellow;
				timer = 0f;
				break;
			default:
				break;
		}
	}
}
