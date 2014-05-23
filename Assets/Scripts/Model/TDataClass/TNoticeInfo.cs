using bbproto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNoticeInfo : ProtobufDataBase {
	private NoticeInfo	instance;
	public TNoticeInfo(NoticeInfo inst):base(inst) { 
		instance = inst;
	}

	//// property ////
	public	List<NoticeItem>	NoticeList { get { return instance.items; }  }

}


