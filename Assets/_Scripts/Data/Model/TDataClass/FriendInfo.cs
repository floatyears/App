using bbproto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bbproto{
public partial class FriendInfo : ProtoBuf.IExtensible {

	public UserUnit UserUnit { 
		get { 
			return _unit; 
		}
		set{
			_unit = UserUnit.GetUserUnit(userId, unit);
		}
	}
}
}
