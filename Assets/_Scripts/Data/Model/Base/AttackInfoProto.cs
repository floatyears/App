using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class AttackInfoProto{
		private static int sequenceID = -1;
		public static void ClearData () {
			sequenceID = -1;
		}


		public AttackInfoProto (int dummy=0){
			Init ();
		}

		public void Init(){
//			if(continueAttackMultip == 0)
//				continueAttackMultip = 1;
//			if(Mathf.Approximately(attackRate,0f))
//				attackRate = 1f;
			sequenceID++;
			attackID = sequenceID;
		}

		private UISprite attackSprite;
		public UISprite AttackSprite {
			get { return attackSprite; }
			set { attackSprite = value; }
		}

	//	public void PlayAttack () {
	//		if (attackSprite == null) {
	//			return;	
	//		}
	//		attackSprite.spriteName = "";
	//	}
	

		//------------test need data, delete it behind test done------------//
		//------------------------------------------------------------------//
		//public int originIndex = -1;
//
//		public static AttackInfoProto GetInstance() {
//			AttackInfoProto tmp = new AttackInfoProto ();
//		}
	}

	public class AISortByCardNumber : IComparer{
		public int Compare (object x, object y)
		{
			AttackInfoProto ai1 = x as AttackInfoProto;
			AttackInfoProto ai2 = y as AttackInfoProto;
			return ai1.needCardNumber.CompareTo(ai2.needCardNumber);
		}
	}

	public class AISortByUserpos : IComparer{
		public int Compare (object x, object y)
		{
			AttackInfoProto ai1 = x as AttackInfoProto;
			AttackInfoProto ai2 = x as AttackInfoProto;
			return ai1.userPos.CompareTo(ai2.userPos);
		}
	}
}