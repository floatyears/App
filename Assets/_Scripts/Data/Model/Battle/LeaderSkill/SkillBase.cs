using UnityEngine;
using System.Collections;

namespace bbproto{
public partial class SkillBase : ProtoBuf.IExtensible {
	public const string SkillNamePrefix = "SkillName_";
	public const string SkillDescribeFix = "SkillDesc_";

	private int _initSkillCooling = 0;

	public int initSkillCooling {
		set { _initSkillCooling = value; } //Debug.LogError("initSkillCooling : " + value + " class : " + this); }
		get { return _initSkillCooling; }
	}

//	public SkillBase BaseInfo {
//		get {
//			return skillBase;
//		}
//	}

		protected bool coolingDone = false;
		
		public void RefreashCooling () {
			DisposeCooling ();
		}
		
		public bool CoolingDone {
			get {
				return coolingDone;
			}
		}
		
		//	private DataListener dataListener;
		
		private string skillStoreID;
		public void StoreSkillCooling (string id) {
			skillStoreID = id;
			
			if (BattleConfigData.Instance.hasBattleData() > 0) {
				ReadSkillCooling ();
			} else {
				Store();
			}
		}
		
		//	public void AddListener(DataListener listener) {
		//		dataListener = listener;
		//		Excute ();
		//	}
		//	
		
		//	void Excute() {
		//		if(dataListener != null) {
		//			dataListener(this);
		//		}
		//	}
		
		void Store() {
			GameDataPersistence.Instance.StoreIntDatNoEncypt(skillStoreID, skillCooling);
		}
		
		void ReadSkillCooling () {
			int skillCooling = GameDataPersistence.Instance.GetIntDataNoEncypt (skillStoreID);
			skillCooling = skillCooling;
		}
		
		protected void DisposeCooling () {
			bool temp = coolingDone;
			coolingDone = CheckCooling (this);
			if (!temp && coolingDone) {
				//			Excute();
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_activate);
			}
			Store ();
		}
		
		private bool CheckCooling(SkillBase sb) {
			if (sb.skillCooling == 0) {
				return true;
			}
			sb.skillCooling --;
			if (sb.skillCooling == 0) {
				return true;
			} 
			else {
				return false;
			}
		}
		
		public void InitCooling() {
			skillCooling = initSkillCooling;
			if (skillCooling > 0) {
				coolingDone = false;
			}
			
			Store ();
		}
		
		public virtual AttackInfo ExcuteByDisk (AttackInfo ai) {
			return null;
		}
		
		public virtual object Excute (string userUnitID, int atk = -1) {
			return null;
		}
}
}