using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

/// <summary>
/// Data operation. Class inherit this interface can get data they need.
/// </summary>
public interface IDataOperation {

    /// <summary>
    /// Load this instance.
    /// </summary>
    IExtensible Load();

    /// <summary>
    /// Save the specified instance and errorMsg.
    /// </summary>
    /// <param name="instance">Instance.</param>
    /// <param name="errorMsg">Error message.</param>
    void Save(IExtensible instance, ErrorMsg errorMsg);

    /// <summary>
    /// Validate the specified instance.
    /// </summary>
    /// <param name="instance">Instance.</param>
    ErrorMsg Validate(IExtensible instance);
}

public class BaseModel : IDataOperation {
    protected IExtensible data;
    
    public IExtensible Load(){
        return data as IExtensible;
    }
    
    public void Save(IExtensible newData, ErrorMsg errorMsg){
        // validate
        errorMsg = Validate(newData);
        if (errorMsg.Code == ErrorCode.Succeed){
            data = newData;
        }
    }

    /// <summary>
    /// Validate the specified instance.
    /// </summary>
    /// <param name="instance">Instance.</param>
    public virtual ErrorMsg Validate(IExtensible instance){
        return new ErrorMsg();
    }

    /// <summary>
    /// Validates the type.
    /// </summary>
    /// <returns><c>true</c>, if type was validated, <c>false</c> otherwise.</returns>
    /// <param name="instance">Instance.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public bool ValidateType<T>(Extensible instance){
        return instance is T;
    }

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
                instance.Init();
            }
            return instance;
        }
    }

    private Dictionary<string, BaseModel> modelDic = new Dictionary<string, BaseModel>();

    /// <summary>
    /// Init this instance.
    /// </summary>
    public void Init (){
        // init all instance be used for game.
    }

    public BaseModel GetData(ErrorMsg errorMsg) {
        BaseModel 
    }
}