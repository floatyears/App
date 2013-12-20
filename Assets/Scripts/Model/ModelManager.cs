using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using bbproto;//TODO move after test;

public abstract class BaseModel {
    protected byte[] byteData;

    public BaseModel(object instance){
        Init(instance);
    }

    /// <summary>
    /// Load data from.
    /// </summary>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    protected T LoadProtobuf<T>(){
        return ProtobufSerializer.ParseFormBytes<T>(byteData);
    }

    /// <summary>
    /// Save this instance.
    /// </summary>
    protected ErrorMsg SaveWithProtobuf<T>(T protobufData){
        return Save(ProtobufSerializer.SerializeToBytes<T>(protobufData));
    }

    /// <summary>
    /// Save the specified newData and errorMsg.
    /// </summary>
    /// <param name="newData">New data.</param>
    protected ErrorMsg Save(byte[] newData){
        // validate
        ErrorMsg errorMsg = Validate(newData);
        if (errorMsg.Code == ErrorCode.Succeed){
            byteData = newData;
        }
        return errorMsg;
    }

    /// <summary>
    /// Validate the specified instance.
    /// </summary>
    /// <param name="instance">Instance.</param>
    protected virtual ErrorMsg Validate(byte[] data){
        return new ErrorMsg();
    }

    /// <summary>
    /// Init this instance. Each subclass should do own Init
    /// </summary>
    protected virtual void Init(object instance){
    }

}

public enum ModelEnum
{
    User = 1000,
}

public class ModelManager
{
    private static ModelManager instance;
    
    /// <summary>
    /// singleton 
    /// </summary>
    /// <value>The instance.</value>
    public static ModelManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ModelManager();
            }
            return instance;
        }
    }

    private Dictionary<ModelEnum, BaseModel> modelDic = new Dictionary<ModelEnum, BaseModel>();

    // TODO
    /// <summary>
    /// Init this instance.
    /// </summary>
    public void Init (){
        // init all instance be used for game.
    }

    public void Add (ModelEnum modelType, BaseModel model){
        modelDic.Add(modelType, model);
    }

    /// <summary>
    /// Gets the data.
    /// usage: ModelManager.Instance.GetaData<User>
    /// </summary>
    /// <returns>The data.</returns>
    /// <param name="key">Key.</param>
    /// <param name="errorMsg">Error message.</param>
    public BaseModel GetData (ModelEnum modelType, ErrorMsg errorMsg) {

        BaseModel model = null;

        if (!modelDic.TryGetValue(modelType, out model)){
            errorMsg.Code = ErrorCode.InvalidModelName;
            errorMsg.Msg = String.Format("required key {0}, but it not exist in ModelManager", modelType);
        }
        return model;
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
        user.ChangeRank(3);
        user.ChangeRank(1);
    } 
}