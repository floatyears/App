using UnityEngine;
using System.Collections;
using bbproto;

public class ExcuteTrap  {
	public void Excute (ITrapExcute trap) {
		trap.Excute ();
	}
}
