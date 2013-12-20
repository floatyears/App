using bbproto;
using System.Collections;

public class User : BaseModel
{
    public User(object instance): base(instance){

    }

    protected override ErrorMsg Validate (byte[] byteData)
    {
        ErrorMsg errMsg = new ErrorMsg();
        //do validate
        UserInfo userInfo = ProtobufSerializer.ParseFormBytes<UserInfo>(byteData);
        if (userInfo == null){
            errMsg.Code = ErrorCode.IllegalData;
        }
        else {
            // TODO: other validation
            LogHelper.Log("userInfo not null " + errMsg.Code);
        }
        return errMsg;
    }

    /// <summary>
    /// Init this instance. Each subclass should do own Init
    /// </summary>
    /// <param name="instance">Instance.</param>
    protected override void Init (object instance)
    {

        if (!Utility.IsInstance<UserInfo>(instance)){
            LogHelper.Log("illegal instance");
            return;
        }

        LogHelper.Log("init user with instance");

        UserInfo userInfo = instance as UserInfo;
        // save
        SaveWithProtobuf<UserInfo>(userInfo);

        // TODO remove after
        DataListenerTest test = new DataListenerTest();
        test.register();
    }

    /// <summary>
    /// Load this instance, Each subclass should do this to make other instance call
    /// </summary>
    public UserInfo Load(){
        return LoadProtobuf<UserInfo>();
    }

    public ErrorMsg ChangeRank(int newRank){
        LogHelper.Log("change rank " + newRank);
        UserInfo userInfo = Load();
        userInfo.rank = newRank;
        return SaveWithProtobuf<UserInfo>(userInfo);
    }
}

/// <summary>
/// DataListener test.
/// </summary>
public class DataListenerTest
{
    public void InvokeWhenUserRankGreaterThan2(object user){
        if (!Utility.IsInstance<User>(user)){
            LogHelper.Log("error illegal ");
            return;
        }
        User userData = user as User;
        if(userData.Load().rank > 2){
            LogHelper.Log("user rank greater than 2");
        }
        else {
            LogHelper.Log("user rank smaller than 2");
        }
    }

    public void register(){
        MsgCenter.Instance.AddListener(DataEnum.Person, InvokeWhenUserRankGreaterThan2);
    }

}