using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class LoginResultInfo
    {
        public LoginResultInfo() { }
        public LoginResultInfo(E_LoginResult aResult, string aDescription) 
        {
            LoginResult = aResult;
            Description = aDescription;
        }

        [Key(0)] public E_LoginResult LoginResult { get; set; } = E_LoginResult.UnknownError;
        [Key(1)] public string Description { get; set; } = string.Empty;
        [Key(2)] public int ClientID { get; set; } = 0;
        [Key(3)] public int ServerID { get; set; } = 0;
        [Key(4)] public E_UserType UserType { get; set; } = E_UserType.Operator_Area;
        [Key(5)] public int UserID { get; set; } = 0;
        [Key(6)] public UserInfo UserInfo { get; set; } = null;
    }
}
