using bbproto;
using System.Collections;

public class TAccountInfo : ProtobufDataBase {
    public TAccountInfo(AccountInfo inst) : base (inst) { 
        instance = inst;
    }

    private AccountInfo	instance;
	
    //////////////////////////////////////////////////////////////
    /// 
    public	int		PayTotal { get { return instance.payTotal; } }
    public	int		PayMonth { get { return instance.payMonth; } }
    public	int		Money { get { return instance.money; } set { instance.money = value; } }
	public	int		StonePay { get { return instance.stonePay; } set {instance.stonePay = value;} }
	public	int		StoneFree { get { return instance.stoneFree; } set {instance.stoneFree = value;} }
    public	int		Stone { get { return instance.stone; } set {instance.stone = value;}}
    public	int		FriendPoint { get { return instance.friendPoint; } set { instance.friendPoint = value; } }

    public void RefreshAcountInfo(TRspClearQuest clearQuest) {
        instance.money = clearQuest.money;
        instance.friendPoint = clearQuest.friendPoint;
        instance.stone += clearQuest.gotStone;
    }
}