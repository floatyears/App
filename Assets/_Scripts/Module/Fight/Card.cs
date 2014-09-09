using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour 
{
	public bool canMove = true;

	public enum Styles
	{
		wind,
		fire,
		water,
		blood
	}
	public Styles style;

	void Start()
	{
		this.gameObject.layer = SceneConfig.CARD_UNPICKED_LAYER;
	}
}
