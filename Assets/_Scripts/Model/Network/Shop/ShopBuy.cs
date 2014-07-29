using System;

using UnityEngine;
using System.Collections;
using bbproto;


public class ShopBuy: ProtoManager {
    // req && rsp
    private bbproto.ReqShopBuy reqShopBuy;
    private bbproto.RspShopBuy rspShopBuy;
    // state for req
    // data

	public string productId;

    public ShopBuy() {
    }
    
    ~ShopBuy () {
    }
    
    public static void SendRequest(DataListener callBack, string productId) {
        ShopBuy req = new ShopBuy();
		req.productId = productId;
        req.OnRequest(null, callBack);
    }
    
    //make request packet==>TODO rename to request
    public override bool MakePacket() {
		Proto = Protocol.SHOP_BUY;
        reqType = typeof(ReqShopBuy);
        rspType = typeof(RspShopBuy);
        
        reqShopBuy = new ReqShopBuy();
        reqShopBuy.header = new ProtoHeader();
        reqShopBuy.header.apiVer = Protocol.API_VERSION;
        reqShopBuy.header.userId = DataCenter.Instance.UserInfo.UserId;

        //request params
		reqShopBuy.productId = this.productId;

        ErrorMsg err = SerializeData(reqShopBuy); // save to Data for send out
        
        return (err.Code == (int)ErrorCode.SUCCESS);
    }
    
}

