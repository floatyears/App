using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class TRspClearQuest {
    public int			rank;
    public int			exp;
    public int			money;			
    public int			friendPoint;
    public int			staminaNow;	
    public int			staminaMax;		
    public uint			staminaRecover;	
    public int			gotMoney;
    public int			gotExp;	
    public int			gotStone;
    public int			gotFriendPoint;
    public List<UserUnit>		gotUnit = new List<UserUnit>();
	public UserUnit evolveUser;
}
