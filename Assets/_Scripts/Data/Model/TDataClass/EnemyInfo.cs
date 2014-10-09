using UnityEngine;
using System.Collections;

namespace bbproto{
public partial class EnemyInfo : global::ProtoBuf.IExtensible{

		public void AddListener () {
			MsgCenter.Instance.AddListener (CommandEnum.SkillPosion, SkillPosion);
			MsgCenter.Instance.AddListener (CommandEnum.DeferAttackRound, DeferAttackRound);
		}
			
		public void RemoveListener () {
			MsgCenter.Instance.RemoveListener (CommandEnum.SkillPosion, SkillPosion);
			MsgCenter.Instance.RemoveListener (CommandEnum.DeferAttackRound, DeferAttackRound);
		}


		public bool isDeferAttackRound = false;
		private AttackInfoProto posionAttack;

		public bool IsInjured () {
			if (currentHp > 0 && currentHp < hp) { return true; }
			else { return false; }
		}

		public int CalculateInjured (AttackInfoProto attackInfo, bool restraint) {
			float injured = 0;
			bool ignoreDefense = attackInfo.ignoreDefense;
			int unitType = attackInfo.attackType;
			float attackvalue = attackInfo.attackValue;
			if (restraint) {
				injured = attackvalue * 2;
			} 
			else {
				int beRestraint = DGTools.BeRestraintType(unitType);
				if(beRestraint == (int)type) {
					injured = attackvalue * 0.5f;
				}
				else{
					injured = attackvalue;
				}
			}
			if (!ignoreDefense) {
				injured -= GetDefense();
			}
			if (injured < 1) {
				injured = 1;
			}
			int value = System.Convert.ToInt32 (injured);
			KillHP (value);
			return value;
		}

		float reduceProportion = 0f;
		public void ReduceDefense(float value, AttackInfoProto ai) {
			reduceProportion = value;
		}

		void SkillPosion(object data) {
			if (currentHp <= 0) {
				return;	
			}
			posionAttack = data as AttackInfoProto;
			if (posionAttack == null) {
				return;	
			}

			int value = System.Convert.ToInt32 (posionAttack.attackValue);
			KillHP (value);
		}

		public void KillHP(int hurtValue) {
			currentHp -= hurtValue;

			bool one = BattleConfigData.Instance.NotDeadEnemy;

			if (NoviceGuideStepManager.isInNoviceGuide()) {
				if(one || !CheckNoviceDeadable()) {
					if (currentHp <= 0) {
						currentHp = 10;
						IsDead = false;
					}
				}
	//			MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
				ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"refresh_enemy",this);
				return;
			}

			if (currentHp <= 0) {
				currentHp = 0;	
				IsDead = true;
			}

	//		MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
			ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"refresh_enemy",this);
		}

		
		public bool CheckNoviceDeadable(){
			int stage = (int)NoviceGuideStepManager.CurrentNoviceGuideStage;
			if (stage >= (int)NoviceGuideStage.ANIMATION && stage <= (int)NoviceGuideStage.FIRST_ATTACK_TWO) {
				if (stage == (int)NoviceGuideStage.FIRST_ATTACK_TWO) {
					return true;
				}
				return false;
			} else if (stage >= (int)NoviceGuideStage.BOSS_ATTACK_ONE && stage <= (int)NoviceGuideStage.BOSS_ATTACK_BOOST) {
				if(UnitID != 86)
					return true;
				if (stage == (int)NoviceGuideStage.BOSS_ATTACK_BOOST) {
					return true;
				}
				return false;
			}
			return true;
		}

		public void Reset() {
			currentHp = GetInitBlood ();
			currentNext = nextAttack;
		}

		public void ResetAttakAround () {
			isDeferAttackRound = false;
			currentNext = nextAttack;
		}

		public void Next () {
			currentNext --;
	//		MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
	//		ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"refresh_enemy",this);
			ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"refresh_enemy",this);
		}

		public void FirstAttack () {
			currentNext++;
	//		MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
	//		ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"refresh_enemy",this);
			ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"refresh_enemy",this);
		}

		void DeferAttackRound(object data) {
			int value = (int)data;
			if (currentHp > 0) {
				currentNext += value;
				isDeferAttackRound = true;
				MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, this);
			}
		}

		public int AttackValue{
			get{
				return attack;
			}
		}

		private uint enemySymbol = 0;
		public uint EnemySymbol {
			get{ return enemySymbol; }
			set{ enemySymbol = value; }
		}

		public uint EnemyID {
			get{
				return enemyId;
			}
		}

		public uint UnitID{
			get{
				return unitId;
			}
		}

		public int GetDefense () {
			int defense = _defense;
			defense = defense - System.Convert.ToInt32 (defense * reduceProportion);
			return defense;
		}

		public int GetInitRound() {
			return nextAttack;
		}

		public int GetInitBlood () {
			return hp;
		}

		public int GetRound () {
			return currentNext;
		}

		public int GetUnitType () {
			return (int)type;
		}

		private bool isDead = false;
		public bool IsDead {
			get { return isDead; }
			set {
				isDead = value; 
				if(isDead) {
	//				RemoveListener();
				}
			}
		}

		private bbproto.EEnemyType _enemyType = bbproto.EEnemyType.NORMAL;
		[global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"enemytype", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
		[global::System.ComponentModel.DefaultValue(bbproto.EEnemyType.NORMAL)]
		public bbproto.EEnemyType enemeyType
		{
			get { return _enemyType; }
			set { _enemyType = value; }
		}

		private bbproto.DropUnit _dropUnit = null;
		[global::ProtoBuf.ProtoMember(11,IsRequired = false, Name=@"dropunit", DataFormat = global::ProtoBuf.DataFormat.Default)]
		[global::System.ComponentModel.DefaultValue(null)]
		public bbproto.DropUnit dropUnit
		{
			get { return _dropUnit; }
			set { _dropUnit = value; }
		}
	}
	public class EnemySortByHP : IComparer {
		public int Compare (object x, object y)
		{
			EnemyInfo tex = (EnemyInfo)x;
			EnemyInfo tey = (EnemyInfo)y;
			return tex.currentHp.CompareTo (tey.currentHp);
		}
	}

	[global::ProtoBuf.ProtoContract(Name=@"EEnemyType")]
	public enum EEnemyType
	{
		
		[global::ProtoBuf.ProtoEnum(Name=@"BOSS", Value=0)]
		BOSS = 0,
		
		[global::ProtoBuf.ProtoEnum(Name=@"NORMAL", Value=1)]
		NORMAL = 1,
	}
}


