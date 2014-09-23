using bbproto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bbproto{
public partial class NoticeInfo : ProtoBuf.IExtensible {

	//// property ////
	public	List<NoticeItem>	NoticeList { get { return items; }  }


	public string GachaNotice{
		get{
			return gachaNotice;
		}
	}
}
}

