using UnityEngine;
using System.Collections;
using bbproto;

public class UserUnitInfo : BaseModel {

	public UserUnitInfo(UserUnit instance) : base(instance){
		net = new NetBase (ReceiveNetData);
	}

	protected override void ReceiveNetData (WWW www)
	{
		if (www == null) 
			return;

		//	TODO dispose net data
	}

	public UserUnit Load() {
		return LoadProtobuf<UserUnit>();
	}
}
