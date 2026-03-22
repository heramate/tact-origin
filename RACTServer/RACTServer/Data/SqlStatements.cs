namespace RACTServer.Data
{
    /// <summary>
    /// .NET 10 기반 고성능 SQL 문전 관리 클래스
    /// 모든 쿼리는 파라미터화된 상수로 관리하여 쿼리 플랜 캐싱 효율을 높입니다.
    /// </summary>
    public static class SqlStatements
    {
        public static class Group
        {
            public const string SelectGroupInfo = "EXEC SP_ORG_GROUP_INFO";
            public const string SelectFactGroupInfo = "EXEC SP_RACT_Get_FACTGroup @CenterCode";
            public const string GetGroupInfoByUserId = "EXEC SP_RACT_Get_GROUPINFO @UserID";
            public const string ModifyGroupInfo = "EXEC SP_RACT_Modify_GroupInfo @WorkType, @UserID, @ID, @Name, @Description, @TOP_ID, @UP_ID";
        }

        public static class Device
        {
            public const string GetDeviceInfo = "EXEC SP_RACT_GET_DEVICEINFO @CenterCode";
            public const string GetSearchDeviceInfo = "EXEC SP_RACT_GET_SearchDEVICEINFO @CenterCode,@IP,@Name,@Part,@Model,@ModelName,@Center,@Branch,@ORG1,@TpoName,@IPType,@MangType";
            public const string ModifyDeviceInfo = "EXEC SP_RACT_MODIFY_DEVICEINFO @WorkType,@GroupID,@UserID,@DeviceID,@Protocol,@DeviceType,@TelnetPort,@PortName,@BaudRate,@DataBits,@Parity,@StopBits,@Handshake,@ModelName,@IPAddress,@TelnetID1,@TelnetPwd1,@TelnetID2,@TelnetPwd2,@Name,@TpoName,@WAIT,@USERID,@PWD,@USERID2,@PWD2,@MoreString,@MoreMark";
            public const string CountDevice = "EXEC SP_ORG_DEVICE_Count @CenterCode";
        }

        public static class User
        {
            public const string SelectUserInfo = "SELECT UsrID, Account, UserTypeCode, orgType, org2_id, CenterCode, usr_name, password, LimitedCmdUser FROM [dbo].[USR_USER] WITH(NOLOCK) WHERE Uses = 1 AND Account = @Account";
            public const string UpdateLastLogin = "UPDATE usr_user SET Ractlastlogintime = @LastLoginTime WHERE account = @UserAccount";
            public const string GetUserList = "EXEC SP_RACT_GET_USER_LIST @SearchType,@SearchValue,@DeleteUserID";
        }

        public static class Log
        {
            public const string InsertDeviceConnectLog = "EXEC [dbo].[SP_RACT_DeviceConnectLog] @UserID, @DeviceID, @ConnectType, @Description, @ConnectionKind";
            public const string UpdateDeviceDisconnectLog = "UPDATE RACT_LOG_DeviceConnection SET ConnectLogType = 1, DisconnectTime = GETDATE() WHERE id = @ID";
            public const string GetCommandHistory = "SELECT * FROM dbo.RACT_Log_ExcuteCommand WITH(NOLOCK) WHERE ConnectionLogID = @ID";
        }

        public static class System
        {
            public const string SelectUnUsedLimit = "SELECT UnUsedLimit FROM RACT_Manage WITH(NOLOCK)";
            public const string SelectModelInfo = @"SELECT m.modeltypecode, mt.modeltypename, m.modelid, m.modelname, ms.morestring, m.SlotCount, m.Divergence, ms.moremark, 
                                                  m.portcnt, mt.factmaxaccesscnt, m.uses, ec.EmbargoCmd, m.IpTypeCd 
                                                  FROM ne_model m WITH(NOLOCK)
                                                  LEFT OUTER JOIN ne_morestring ms WITH(NOLOCK) ON m.modelID=ms.modelID 
                                                  LEFT OUTER JOIN NE_EMBAGO_CMD ec WITH(NOLOCK) ON m.modelid=ec.modelid 
                                                  INNER JOIN ne_modeltype mt WITH(NOLOCK) ON m.modeltypecode=mt.modeltypecode 
                                                  WHERE mt.Uses=1 AND (m.Uses = 1 OR m.Uses = 3) 
                                                  ORDER BY mt.modeltypecode, m.modelid";
        }
    }
}
