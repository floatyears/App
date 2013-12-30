using bbproto;
using System.Collections;

public class User : BaseModel
{
	public User(UserInfo instance): base(instance) {

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

	public override bool NetRequest ()
	{
		return false;
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
        MsgCenter.Instance.AddListener(CommandEnum.Person, InvokeWhenUserRankGreaterThan2);
    }

}

public class ModelManagerTest {
	public static void Test(){
		//
		ModelManager manager = ModelManager.Instance;
		
		UserInfo userInfo = new UserInfo();
		userInfo.userId = 127;
		userInfo.userName = "Rose Mary";
		userInfo.exp = 20;
		userInfo.rank = 20;
		userInfo.staminaMax = 128;
		userInfo.staminaNow = 127;
		userInfo.staminaRecover = 127000000;
		userInfo.loginTime = 127;
		
		//
		
		User user = new User(userInfo);
		//
		
		manager.Add(ModelEnum.User, user);
		
		ErrorMsg errMsg = new ErrorMsg();
		User userLoaded = manager.GetData(ModelEnum.User, errMsg) as User;
		
		userLoaded.ChangeRank(3);
		MsgCenter.Instance.Invoke(CommandEnum.Person, userLoaded);
		
		userLoaded.ChangeRank(1);
		MsgCenter.Instance.Invoke(CommandEnum.Person, userLoaded);
	} 
}