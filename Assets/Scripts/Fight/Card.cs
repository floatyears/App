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
		light
	}
	public Styles style;
}
