using bbproto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bbproto{
	public partial class QuestDungeonData : ProtoBuf.IExtensible {

		private List<DropUnit> dropUnit;

		public List<byte> Colors;
		
		public void assignData() {
			convertColors ();
			Floors = new  List< List<QuestGrid> > ();
			dropUnit = new List<DropUnit>();

			DropUnit bossDrop = null;
			foreach(DropUnit drop in _drop) {
				dropUnit.Add (drop);
				if (drop.dropId==0) { // is boss drop
					bossDrop = drop;
				} 
			}

			for(int i=0; i< boss.Count; i++) {
				EnemyInfo e = boss[i];
				e.currentHp = e.GetInitBlood();
				e.currentNext = e.GetInitRound();
				e.IsBoss = true;
				if (bossDrop!=null && bossDrop.dropPos == i ) {
					e.drop = bossDrop;
				}

	//			this.Boss.Add(e);
			}

			for(int nFloor=0; nFloor < floors.Count; nFloor++){
				List<QuestGrid> floor = new List<QuestGrid> ();
							
				LogHelper.Log ("===fill floor[{0}]", nFloor);
				for(int f=0; f< floors[nFloor].gridInfo.Count; f++){
					QuestGrid grid = floors[nFloor].gridInfo[f];

					//assign DropUnit
					for(int i=0; i<drop.Count;i++){
						if ( grid.dropId == 0){
							continue;
						}

						if ( drop[i].dropId == grid.dropId ){
							grid.Drop = drop [i];
							break;
						}
					}

					// assign EnemyInfo
					for(int g=0; g<grid.enemyId.Count;g++){
						for(int i=0; i<enemys.Count;i++){
							if ( grid.enemyId[g] == enemys[i].enemyId ){
								enemys[i].currentHp = enemys[i].hp;
								enemys[i].currentNext = enemys[i].nextAttack;
								EnemyInfo tei =  CopyEnemyInfo(enemys[i] ) ;
								tei.drop = grid.Drop;
								grid.Enemy.Add(tei);
								break;
							}
						}
					}
					Debug.Log ("===floor["+nFloor+"].Add grid" + floor.Count + " enemy count: " + grid.enemyId.Count + " coins: " + grid.coins);
					floor.Add (grid);

				} //end of gridInfo...

				Floors.Add (floor);
			}
		}
			
		EnemyInfo CopyEnemyInfo(EnemyInfo ei) {
			EnemyInfo enemyInfo = new EnemyInfo();
			enemyInfo.attack = ei.attack;
			enemyInfo.currentHp = ei.currentHp;
			enemyInfo.currentNext = ei.currentNext;
			enemyInfo.defense = ei.defense;
			enemyInfo.enemyId = ei.enemyId;
			enemyInfo.hp = ei.hp;
			enemyInfo.nextAttack = ei.nextAttack;
			enemyInfo.type = ei.type;
			enemyInfo.unitId = ei.unitId;
			return enemyInfo;
		}

		private void convertColors() {
			Colors = new List<byte> ();

			const byte HEAD_BIT1 = 0x80 ; // 10000000b
			const byte HEAD_BIT2 = 0xc0 ; // 11000000b


			const byte TAIL_BIT1 = 0x1 ; // 001
			const byte TAIL_BIT2 = 0x3 ; // 011
			const byte BIT3 	 = 0x7 ; // 111
				
			for(int i=0; i <= colors.Length-3; i+=3){
				byte b1 = colors [i];
				byte b2 = colors [i+1];
				byte b3 = colors [i+2];
				LogHelper.Log("bbb:{0},{1},{2}",b1,b2,b3);
				byte b = (byte)((b1 >> 5) & BIT3);
				Colors.Add (b);

				b = (byte) ((b1 >> 2) & BIT3);
				Colors.Add ( b );

				b = (byte)( ((b1 & TAIL_BIT2) << 1) + ((b2 & HEAD_BIT1)>>7) );
				Colors.Add ( b );

				b = (byte) ( (b2 >> 4) & BIT3);
				Colors.Add ( b );

				b = (byte) ( (b2 >> 1) & BIT3);
				Colors.Add ( b );

				b = (byte) ( ((b2 & TAIL_BIT1) << 2) + ((b3 & HEAD_BIT2)>>6) );
				Colors.Add ( b );

				b = (byte) ((b3 >> 3) & BIT3);
				Colors.Add ( b );

				b = (byte) (b3 & BIT3);
				Colors.Add ( b );
			}


	//		for( int i=0; i<Colors.Count;i++){
	//			LogHelper.Log ("b[{0}]: {1}", i, colors[i]);
	//		}

		}

		public List<EnemyInfo>	Boss {
			get { 
				return boss;
			} 
			set { 
				boss.Clear();
				boss.AddRange(value);
			} 
		}
//	EnemyInfo		enemys	

		public List< List<QuestGrid> >	Floors;
		public int currentFloor = 0;

		public int GetGridIndex(Coordinate coor) {
			if (coor.y == 0 && coor.x == 2) {
				return -1;	
			}
			int index = coor.y * 5 + coor.x - 1 + currentFloor * 24;
			if (index < 2) {
				index++;		
			}
			return index;
		}

		public QuestGrid GetFloorDataByCoor(Coordinate coor) {
			if (coor.y == 0 && coor.x == 2) {
				return null;	
			}

			int index = coor.y * 5 + coor.x - 1;
			if (coor.y == 0 && coor.x < 3) {
				index++;
			} 
	//		UnityEngine.Debug.LogError ("currentFloor : " + currentFloor);
			return Floors [currentFloor] [index];
		}

		public bool isLastCell(){
			return currentFloor == Floors.Count - 1;
		}

		public Coordinate GetGridCoordinate(uint index) {
			int indexValue = (int)index;
			indexValue -= currentFloor * 24;
			int y = GetYCoordinate (indexValue);
			if (y == -1) { 
				UnityEngine.Debug.LogError(" get coordinate error : " + indexValue + " y : " + y);
			}

			int x = (indexValue - y * 5) + 1;
	//		UnityEngine.Debug.LogError(" get coordinate error : " + indexValue + " x : " + x);
			if (indexValue < 2) {
				x --;	
			}
			return new Coordinate (x, y);
		}

		int GetYCoordinate(int index) {
			if (index >= 0 && index <= 3) {
				return 0;		
			} else if (index <= 8) {
				return 1;		
			} else if (index <= 13) {
				return 2;
			} else if (index <= 18) {
				return 3;	
			} else if (index <= 23) {
				return 4;	
			}

			return -1;
		}


	
	}


	public partial class QuestGrid : ProtoBuf.IExtensible {

		public QuestGrid() { 
	//		LogHelper.LogError ("TQuestGrid :: instance.trap id : " + instance.trapId);
			Enemy = new List<EnemyInfo> ();
		}

		private DropUnit drop;
		public DropUnit	Drop { 
			get { return this.drop; } 
			set { this.drop = value; } 
		}
		private List<EnemyInfo> enemy;
		public List<EnemyInfo> Enemy{
			get{
				return enemy;
			}
			set{
				enemy = value;
			}
		}

		public TrapBase	TrapInfo { get { 
				if (!DataCenter.Instance.BattleData.TrapInfo.ContainsKey(trapId) ) {
	//				UnityEngine.Debug.LogError("instance.trapId : " + instance.trapId);
					LogHelper.LogError("TQuestGrid.TrapInfo :: cannot find trapId({0}) in Global.trapInfo.", trapId);
					return null;
				}
					
				return DataCenter.Instance.BattleData.TrapInfo[trapId]; 
			} 
		}
	}


public partial class DropUnit : ProtoBuf.IExtensible {

	public UnitInfo UnitInfo {
		get { return DataCenter.Instance.UnitData.GetUnitInfo (unitId); }
	}
}
}

