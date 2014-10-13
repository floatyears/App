using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public class ActiveSkill : SkillBase {

		private DataListener dataListener;

		public ActiveSkill(){
			
		}
		
		public void AddListener(DataListener listener) {
			dataListener = listener;
			Excute ();
		}
		
		
		void Excute() {
			if(dataListener != null) {
				dataListener(this);
			}
		}

	}
}