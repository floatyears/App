using UnityEngine;
using System.Collections;
using bbproto;

public class ExcuteTrap : ProtobufDataBase {
	public ExcuteTrap (object instance) : base (instance) {

	}	

	public void Excute (ITrapExcute trap) {
		trap.Excute ();
	}
}
