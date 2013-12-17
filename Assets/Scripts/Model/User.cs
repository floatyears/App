using bbproto;
using System.Collections;

public class User: BaseModel {
    private UserInfo userInfo;

    public UserInfo Load(){
        return userInfo;
    }

    public void Save(UserInfo info, ErrorMsg errorMsg){
        // validate
    }
}
