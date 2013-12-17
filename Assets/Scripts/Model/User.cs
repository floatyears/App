using bbproto;
using System.Collections;

public class User : BaseModel
{
    public override ErrorMsg Validate (ProtoBuf.IExtensible instance)
    {
        ErrorMsg errMsg = new ErrorMsg();
        //do validate
        UserInfo userInfo = instance as UserInfo;
        if (!ValidateType<UserInfo>(instance)){
            errMsg.Code = ErrorCode.IllegalData;
        }
        else {
            // TODO: other validation
        }
        return errMsg;
    }

}