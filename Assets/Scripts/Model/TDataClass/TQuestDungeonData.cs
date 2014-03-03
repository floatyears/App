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

	private List<byte> colors;

	private void assignData() {
		Floors = new  List< List<TQuestGrid> > ();

		foreach(DropUnit drop in instance.drop) {
			TDropUnit du = new TDropUnit (drop);
			dropUnit.Add (du);
		}

		for(int nFloor=0; nFloor < instance.floors.Count; nFloor++){
			List<TQuestGrid> floor = new List<TQuestGrid> ();
						
			LogHelper.Log ("===fill floor[{0}]", nFloor);
			for(int f=0; f< instance.floors[nFloor].gridInfo.Count; f++){
				TQuestGrid grid = new TQuestGrid (instance.floors[nFloor].gridInfo[f]);

				//assign DropUnit
				for(int i=0; i<instance.drop.Count;i++){
					if ( instance.drop[i].dropId == grid.Object.dropId[0] ){
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

			byte b = (byte)((b1 >> 5) & BIT3);
			colors.Add (b);

			b = (byte) ((b1 >> 2) & BIT3);
			colors.Add ( b );

			b = (byte)( (b1 & TAIL_BIT2) << 1 + (b2 & HEAD_BIT1) );
			colors.Add ( b );

			b = (byte) ( (b2 >> 4) & BIT3);
			colors.Add ( b );

			b = (byte) ( (b2 >> 1) & BIT3);
			colors.Add ( b );

			b = (byte) ((b2 & TAIL_BIT1) << 2 + (b3 & HEAD_BIT2));
			colors.Add ( b );

			b = (byte) ((b3 >> 3) & BIT3);
			colors.Add ( b );

			b = (byte) (b3 & BIT3);
			colors.Add ( b );
		}
	}
	//////////////////////////////////////////////////////////////
	/// 
	/// 

	public uint				QuestId	{ get { return instance.questId; } }
	public List<byte>		Colors	{ get { return this.colors; } }
	public List<TDropUnit>	DropUnit { get { return this.dropUnit;} }

	public List<EnemyInfo>	Boss {get { return instance.boss;} }
//	EnemyInfo		enemys	

	public List< List<TQuestGrid> >	Floors;
	
}


public class TQuestGrid : ProtobufDataBase {
	private QuestGrid	instance;

	public TQuestGrid(QuestGrid inst) : base (inst) { 
		instance = inst;
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
	public TrapBase	TrapInfo { get { return GlobalData.trapInfo[instance.trapId]; } }
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

