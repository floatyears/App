using UnityEngine;
using System.Collections;

namespace bbproto{
	public partial class SkillBase {
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
		public bool IsActiveSkill() {
			return initSkillCooling > 0;
		}

		protected bool coolingDone = false;
		
		public void RefreashCooling () {
			DisposeCooling ();
		}

		public void ResetCooling(){
			coolingDone = true;
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
				StoreSkillCooling();
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
		
		void StoreSkillCooling() {
			if( skillStoreID != null ) {
				GameDataPersistence.Instance.StoreIntDatNoEncypt(skillStoreID, skillCooling);
			}
		}
		
		void ReadSkillCooling () {
			int CD = GameDataPersistence.Instance.GetIntDataNoEncypt (skillStoreID);
			skillCooling = CD;
		}
		
		protected void DisposeCooling () {
			bool temp = coolingDone;
			coolingDone = CheckCooling ();
			if (!temp && coolingDone) {
				//			Excute();
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_activate);
			}
			StoreSkillCooling ();
		}
		
		private bool CheckCooling() {
			if (skillCooling == 0) {
				return true;
			}
			skillCooling --;
			if (skillCooling == 0) {
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
			
			StoreSkillCooling ();
		}
		
		public virtual AttackInfoProto ExcuteByDisk (AttackInfoProto ai) {
			return null;
		}
		
		public virtual object Excute (string userUnitID, int atk = -1) {
			return null;
		}

		public override string ToString ()
		{
			return id.ToString ();
		}

		
		public virtual SkillBase GetBaseInfo(){
			//			throw new excepti
			return null;
		}
	}
}