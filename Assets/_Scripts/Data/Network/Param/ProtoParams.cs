using UnityEngine;
using System.Collections;
using bbproto;

public class StartQuestParam {
	public uint stageId;
	public uint questId;
	public FriendInfo helperUserUnit;
	public int currPartyId;
	public int startNew;
	public int isUserGuide;
	public ECopyType copyType; //普通副本 or 精英副本
}

namespace bbproto{

public partial class ClearQuestParam  : ProtoBuf.IExtensible {
	public int totalHp;
	public int leftHp;
}

}