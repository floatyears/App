using UnityEngine;
using System.Collections;

public class PassiveCounterattack : ProtobufDataBase {
	public PassiveCounterattack(object instance) : base (instance) {

	}

	public float CounterAttack() {
		return 0f;
	}
}
