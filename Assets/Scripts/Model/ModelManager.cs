using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

/// <summary>
/// Data operation. Class inherit this interface can get data they need.
/// </summary>
public interface IDataOperation {

    /// <summary>
    /// Loads the protobuf.
    /// </summary>
    /// <returns>The protobuf.</returns>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    T LoadProtobuf<T>();

    /// <summary>
    /// Save the specified newData and errorMsg.
    /// </summary>
    /// <param name="newData">New data.</param>
    /// <param name="errorMsg">Error message.</param>
    void Save(byte[] newData, ErrorMsg errorMsg);

    /// <summary>
    /// Validate the specified instance.
    /// </summary>
    /// <param name="instance">Instance.</param>
    ErrorMsg Validate(byte[] data);
}

public class BaseModel : IDataOperation {
    protected byte[] byteData;

    public BaseModel(){
        Init();
    }

    /// <summary>
    /// Load data from.
    /// </summary>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public T LoadProtobuf<T>(){
        return ProtobufSerializer.ParseFormBytes<T>(byteData);
    }

    public void Save(byte[] newData, ErrorMsg errorMsg){
        // validate
        errorMsg = Validate(newData);
        if (errorMsg.Code == ErrorCode.Succeed){
            byteData = newData;
        }
    }

    /// <summary>
    /// Validate the specified instance.
    /// </summary>
    /// <param name="instance">Instance.</param>
    public virtual ErrorMsg Validate(byte[] data){
        return new ErrorMsg();
    }


    /// <summary>
    /// Init this instance.
    /// </summary>
    public virtual void Init (){
    }
}

public enum ModelName
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

    private Dictionary<ModelName, BaseModel> modelDic = new Dictionary<ModelName, BaseModel>();

    /// <summary>
    /// Init this instance.
    /// </summary>
    public void Init (){
        // init all instance be used for game.
    }

    /// <summary>
    /// Gets the data.
    /// usage: ModelManager.Instance.GetaData<User>
    /// </summary>
    /// <returns>The data.</returns>
    /// <param name="key">Key.</param>
    /// <param name="errorMsg">Error message.</param>
    public BaseModel GetData (ModelName modelType, ErrorMsg errorMsg) {

        BaseModel model = null;

        if (!modelDic.TryGetValue(modelType, out model)){
            errorMsg.Code = ErrorCode.InvalidModelName;
            errorMsg.Msg = String.Format("required key {0}, but it not exist in ModelManager", modelType);
        }
        return model;
    }
}