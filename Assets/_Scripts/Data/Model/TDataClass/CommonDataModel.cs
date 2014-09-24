using UnityEngine;
using System.Collections;
using bbproto;

public class CommonDataModel : ProtobufDataBase {

	private NoticeInfo noticeInfo;
	public NoticeInfo NoticeInfo { 
		get { return noticeInfo; }
		set { noticeInfo = value; }
	}

}
