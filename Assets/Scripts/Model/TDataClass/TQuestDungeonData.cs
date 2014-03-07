using bbproto;
using System.Collections;
using System.Collections.Generic;

public class TQuestDungeonData : ProtobufDataBase {
	public TQuestDungeonData(QuestDungeonData inst) : base (inst) { 
		instance = inst;

		convertColors ();

		assignData ();
	}

	private QuestDungeonData	instance;
	private List<TDropUnit>		dropUnit;
	private List<TEnemyInfo>	boss = new List<TEnemyInfo>();
	private List<byte> colors;

	private void assignData() {
		Floors = new  List< List<TQuestGrid> > ();
		dropUnit = new List<TDropUnit>();

		foreach(DropUnit drop in instance.drop) {
			TDropUnit du = new TDropUnit (drop);
			dropUnit.Add (du);
		}

		foreach(EnemyInfo b in instance.boss) {
			TEnemyInfo e = new TEnemyInfo(b);
			this.boss.Add(e);
		}

		for(int nFloor=0; nFloor < instance.floors.Count; nFloor++){
			List<TQuestGrid> floor = new List<TQuestGrid> ();
						
			LogHelper.Log ("===fill floor[{0}]", nFloor);
			for(int f=0; f< instance.floors[nFloor].gridInfo.Count; f++){
				TQuestGrid grid = new TQuestGrid (instance.floors[nFloor].gridInfo[f]);

				//assign DropUnit
				for(int i=0; i<instance.drop.Count;i++){
					if ( grid.Object.dropId == null || grid.Object.dropId.Count==0){
						//LogHelper.Log ("===floor[{0}].Add grid[{1}/{2}].dropId==null, skip...", nFloor,i,floor.Count);
						continue;
					}

					if ( instance.drop[i].dropId == grid.Object.dropId[0] ){
						LogHelper.Log ("===floor[{0}].Add => grid[{1}/{2}].dropId=={3} unitId:{4}",
						               nFloor,i,floor.Count, instance.drop[i].dropId, instance.drop[i].unitId);
						grid.Drop = new TDropUnit(instance.drop [i]);
						break;
					}
				}

				// assign EnemyInfo
				for(int g=0; g<grid.Object.enemyId.Count;g++){	
					for(int i=0; i<instance.enemys.Count;i++){
						if ( grid.Object.enemyId[g] == instance.enemys[i].enemyId ){
							LogHelper.Log ("grid[{0}]: assign enemy[{1}], enemyCount={2}...  ", g, grid.Object.enemyId[g], grid.Enemy.Count);
							grid.Enemy.Add( new TEnemyInfo(instance.enemys[i]) );
							break;
						}
					}
				}
//				LogHelper.Log ("===floor[{0}].Add grid{1}", nFloor,floor.Count);
				floor.Add (grid);

			} //end of gridInfo...

			Floors.Add (floor);
		}
	}

	private void convertColors() {
		colors = new List<byte> ();

		const byte HEAD_BIT1 = 0x80 ; // 10000000b
		const byte HEAD_BIT2 = 0xc0 ; // 11000000b


		const byte TAIL_BIT1 = 0x1 ; // 001
		const byte TAIL_BIT2 = 0x3 ; // 011
		const byte BIT3 	 = 0x7 ; // 111
			
		for(int i=0; i <= instance.colors.Length-3; i+=3){
			byte b1 = instance.colors [i];
			byte b2 = instance.colors [i+1];
			byte b3 = instance.colors [i+2];
			LogHelper.Log("bbb:{0},{1},{2}",b1,b2,b3);
			byte b = (byte)((b1 >> 5) & BIT3);
			colors.Add (b);

			b = (byte) ((b1 >> 2) & BIT3);
			colors.Add ( b );

			b = (byte)( ((b1 & TAIL_BIT2) << 1) + ((b2 & HEAD_BIT1)>>7) );
			colors.Add ( b );

			b = (byte) ( (b2 >> 4) & BIT3);
			colors.Add ( b );

			b = (byte) ( (b2 >> 1) & BIT3);
			colors.Add ( b );

			b = (byte) ( ((b2 & TAIL_BIT1) << 2) + ((b3 & HEAD_BIT2)>>6) );
			colors.Add ( b );

			b = (byte) ((b3 >> 3) & BIT3);
			colors.Add ( b );

			b = (byte) (b3 & BIT3);
			colors.Add ( b );
		}


		for( int i=0; i<colors.Count;i++){
			LogHelper.Log ("b[{0}]: {1}", i, colors[i]);
		}

	}
	//////////////////////////////////////////////////////////////
	/// 
	/// 

	public uint				QuestId	{ get { return instance.questId; } }
	public List<byte>		Colors	{ get { return this.colors; } }
	public List<TDropUnit>	DropUnit { get { return this.dropUnit;} }

	public List<TEnemyInfo>	Boss {get { return this.boss;} }
//	EnemyInfo		enemys	

	public List< List<TQuestGrid> >	Floors;
	public int currentFloor = 0;

	//======================by leiliang start=================================

	public TQuestGrid GetSingleFloor(Coordinate coor) {
		if (coor.y == 0 && coor.x == 2) {
			return null;	
		}

		int index = coor.y * 5 + coor.x - 1;
		if (coor.y == 0 && coor.x < 2) {
			index++;
		} 
		return Floors [currentFloor] [index];
	}
	//======================end
	
}


public class TQuestGrid : ProtobufDataBase {
	private QuestGrid	instance;

	public TQuestGrid(QuestGrid inst) : base (inst) { 
		instance = inst;
//		LogHelper.LogError ("TQuestGrid :: instance.trap id : " + instance.trapId);
		Enemy = new List<TEnemyInfo> ();
	}

	public QuestGrid Object { get {return instance;} }

	private TDropUnit	drop;

	public TDropUnit	Drop { 
		get { return this.drop; } 
		set { this.drop = value; } 
	}

	public List<TEnemyInfo> Enemy;

	public int	Position { get { return instance.position; } }
	public int	Color { get { return instance.color; } }
	public int	Coins { get { return instance.coins; } }
	public EGridStar Star { get { return instance.star; } }
	public EQuestGridType Type { get { return instance.type; } }
	public uint	TrapId { get { return instance.trapId; } }
	public TrapBase	TrapInfo { get { 
			if (!GlobalData.trapInfo.ContainsKey(instance.trapId) ) {
				LogHelper.LogError("TQuestGrid.TrapInfo :: cannot find trapId({0}) in Global.trapInfo.", instance.trapId);
				return null;
			}
				
			return GlobalData.trapInfo[instance.trapId]; 
		} 
	}
}


public class TDropUnit : ProtobufDataBase {
	public TDropUnit(DropUnit inst) : base (inst) { 
		instance = inst;
	}

	private DropUnit	instance;

	public uint	DropId { get { return instance.dropId; } }
	public int	Level { get { return instance.level; } }
	public int	AddHp { get { return instance.addHp; } }
	public int	AddAttack { get { return instance.addAttack; } }
	public int	AddDefence{ get { return instance.addDefence; } }

	public TUnitInfo UnitInfo {
		get { return GlobalData.unitInfo [instance.unitId]; }
	}
}

