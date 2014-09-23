using bbproto;
using System.Collections;
namespace bbproto{
	public partial class AccountInfo : ProtoBuf.IExtensible {
		
	    //////////////////////////////////////////////////////////////

	    public void RefreshAcountInfo(TRspClearQuest clearQuest) {
	        money = clearQuest.money;
	        friendPoint = clearQuest.friendPoint;
	        stone += clearQuest.gotStone;
	    }
	}
}