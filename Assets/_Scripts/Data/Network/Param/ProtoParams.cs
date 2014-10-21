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
	public ECopyType questType; //普通副本 or 精英副本
}
