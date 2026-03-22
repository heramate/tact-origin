using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class DaemonLoginResultInfo
    {
        public DaemonLoginResultInfo() { }
        public DaemonLoginResultInfo(E_LoginResult aResult) { LoginResult = aResult; }

        [Key(0)] public E_LoginResult LoginResult { get; set; } = E_LoginResult.UnknownError;
        [Key(1)] public string Description { get; set; } = string.Empty;
        [Key(2)] public int ClientID { get; set; } = 0;
        [Key(3)] public DaemonProcessInfo DaemonInfo { get; set; } = null;
    }
}
