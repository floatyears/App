using UnityEngine;
using System.Collections.Generic;

public class StoreBattleData  {
	public int colorIndex;
	public int hp;
	public int sp;
	/// <summary>
	/// 0 == not battle, 1 == battle enemy, 2 == battle boss;
	/// </summary>
	public int isBattle;		
	public Coordinate roleCoordinate;
	public List<ClearQuestParam> questData = new List<ClearQuestParam>();
	public List<TEnemyInfo> enemyInfo = new List<TEnemyInfo> ();
}