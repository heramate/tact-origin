using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using MKLibrary.MKData;


using ACPS.CommonConfigCompareClass;
using RACTCommonClass;

namespace RACTServer
{
    public class BaseDataLoadProcess
    {
        /// <summary>
        /// 서버의 초기정보를 Load하는 클래스 입니다.
        /// </summary>
        /// <returns>데이터 로드의 성공 요부 입니다.</returns>
        public static bool LoadBaseData()
        {
            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "기초 데이터를 로드 합니다.");
            if (!LoadFACTGroupInfo()) return false;
            if (!LoadModelInfo()) return false;
            if (!LoadUnUsedLimit()) return false;
            /* 2019-01-16 제한명령어 명령어별 권한 변경건 수정 */
            //if (!LoadLimitCmdInfo()) return false;
            if (!LoadDefaultCmdInfo()) return false;
            // 2013-05-02- shinyn - 장비정보를 로드합니다.
            //if (!LoadDeviceInfo()) return false;


            return true;

        }
        /*
        /// <summary>
        /// 2013-05-02- shinyn - 장비정보를 로드합니다.
        /// </summary>
        /// <returns></returns>
        private static bool LoadDeviceInfo()
        {
            MKDataSet tDataSet = null;
            string tQueryString = "";
            MKDBWorkItem tDBWI = null;


            try
            {

                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "FACT 장비정보를 로드 합니다.");

                GlobalClass.m_DeviceInfos = new DeviceInfoCollection();
                tQueryString = "EXEC SP_RACT_GET_SearchDEVICEINFO ";

                tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                if (tDBWI.ExecuteQuery(tQueryString, out tDataSet) != E_DBProcessError.Success)
                {
                    System.Diagnostics.Debug.WriteLine(tDBWI.ErrorString);
                }
                else
                {


                    GlobalClass.m_DeviceInfos = new DeviceInfoCollection();

                    DeviceInfo tDeviceInfo;
                    for (int i = 0; i < tDataSet.RecordCount; i++)
                    {

                        tDeviceInfo = new DeviceInfo();
                        tDeviceInfo.DeviceID = int.Parse(tDataSet["NeID"].ToString());
                        tDeviceInfo.ModelID = tDataSet.GetInt32("ModelID");
                        tDeviceInfo.Name = tDataSet["NeName"].ToString();
                        tDeviceInfo.ORG1Code = tDataSet["org1_id"].ToString();
                        tDeviceInfo.ORG2Code = tDataSet["org2_id"].ToString();
                        tDeviceInfo.BranchCode = tDataSet["org2_id"].ToString();
                        tDeviceInfo.CenterCode = tDataSet["CenterCode"].ToString();
                        tDeviceInfo.IPAddress = tDataSet["MasterIP"].ToString();
                        tDeviceInfo.InputFlag = (E_FlagType)(tDataSet.GetBool("InputFlag").GetHashCode());
                        tDeviceInfo.DeviceNumber = tDataSet.GetString("devicenum");
                        tDeviceInfo.DevicePartCode = tDataSet.GetInt32("ModelTypeCode");
                        tDeviceInfo.Version = tDataSet.GetString("OsVersion");
                        tDeviceInfo.TelnetID1 = tDataSet.GetString("TelnetID_1").Trim();
                        tDeviceInfo.TelnetID2 = tDataSet.GetString("TelnetID_2").Trim();
                        tDeviceInfo.TelnetPwd1 = tDataSet.GetString("Passwd_1").Trim();
                        tDeviceInfo.TelnetPwd2 = tDataSet.GetString("Passwd_2").Trim();
                        tDeviceInfo.ORG1Name = tDataSet.GetString("ORG1_Name");
                        tDeviceInfo.ORG2Name = tDataSet.GetString("ORG2_Name");
                        tDeviceInfo.TpoName = tDataSet.GetString("TpoName");
                        tDeviceInfo.CenterName = tDataSet.GetString("BizPlsName");
                        tDeviceInfo.DeviceGroupName = tDataSet.GetString("GroupName");
                        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
                        tDeviceInfo.GroupID = tDataSet.GetString("GroupID").Length > 0 ? tDataSet.GetString("GroupID") : "-1";

                        // 2013-01-11 - shinyn - 모델명 가져오기
                        tDeviceInfo.ModelName = tDataSet.GetString("ModelName");


                        tDataSet.MoveNext();

                        GlobalClass.m_DeviceInfos.Add(tDeviceInfo);
                    }

                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "FACT 장비정보 " + tDataSet.RecordCount.ToString() + "개를 로드했습니다.");
                }
            }
            catch (Exception ex)
            {
                MKOleDBClass.CloseDataSet(tDataSet);
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "장비정보 로드에 실패 했습니다.");
                return false;
            }

            return true;
        }
        */
        /// <summary>
        /// 제한 날짜를 로드 합니다.
        /// </summary>
        /// <returns></returns>
        private static bool LoadUnUsedLimit()
        {
            MKDataSet tDataSet = null;

            try
            {
                MKDBWorkItem tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                string tQuery = "select * from RACT_Manage";
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "제한 날짜를 로드 합니다.");
                if (tDBWI.ExecuteQuery(tQuery, out tDataSet) != E_DBProcessError.Success)
                {
                    return false;
                }

                if (tDataSet.RecordCount > 0)
                {
                    for (int i = 0; i < tDataSet.RecordCount; i++)
                    {
                        GlobalClass.s_UnUsedLimit = tDataSet.GetInt32("UnUsedLimit");
                    }
                }

                MKOleDBClass.CloseDataSet(tDataSet);
                return true;
            }
            catch (Exception ex)
            {
                MKOleDBClass.CloseDataSet(tDataSet);
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "제한 날짜를 로드에 실패 했습니다.");
                return false;
            }
        }

        /// <summary>
        /// 그룹 정보를 로드 합니다.
        /// </summary>
        /// <returns>그룹 정보 로드의 성공 여부 입니다.</returns>
        private static bool LoadFACTGroupInfo()
        {
            FACTGroupInfo tCenterGroupInfo = null;
            FACTGroupInfo tBranchGroupInfo = null;
            FACTGroupInfo tORG1GroupInfo = null;
            FACTGroupInfo tGroupInfo = null;

            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;

            string tOldCenterCode = "";
            string tOldBranchCode = "";
            string tOldORG1Code = "";
            try
            {
                //그룹 정보를 로드 합니다 ---------------------------------------------------------
                GlobalClass.m_FACTGroupInfo = new FACTGroupInfo();

                tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                tDBWI.ExecuteQuery(SQLQuery.SelectGroupInfo(), out tDataSet);

                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "FACT 그룹정보를 로드 합니다.");

                if (tDataSet != null)
                {
                    if (tDataSet.RecordCount > 0)
                    {
                        GlobalClass.m_FACTGroupInfo.ORG1Code = "0";
                        GlobalClass.m_FACTGroupInfo.ORG1Name = "전국";

                        for (int i = 0; i < tDataSet.RecordCount; i++)
                        {
                            tGroupInfo = new FACTGroupInfo();
                            tGroupInfo.BranchCode = tDataSet.GetString("org2_id").Trim();
                            tGroupInfo.BranchName = tDataSet.GetString("org2_name");
                            tGroupInfo.CenterCode = tDataSet.GetString("CenterCode").Trim();
                            tGroupInfo.CenterName = tDataSet.GetString("CenterName");
                            tGroupInfo.ORG1Code = tDataSet.GetString("org1_id");
                            tGroupInfo.ORG1Name = tDataSet.GetString("org1_name");

                            if (tOldORG1Code != tGroupInfo.ORG1Code)
                            {
                                tOldORG1Code = tGroupInfo.ORG1Code;
                                tORG1GroupInfo = new FACTGroupInfo();
                                tORG1GroupInfo.ORG1Code = tDataSet.GetString("org1_id");
                                tORG1GroupInfo.ORG1Name = tDataSet.GetString("org1_name");

                                if (GlobalClass.m_FACTGroupInfo.SubGroups == null)
                                {
                                    GlobalClass.m_FACTGroupInfo.SubGroups = new FACTGroupInfoCollection();
                                }
                                GlobalClass.m_FACTGroupInfo.SubGroups.Add(tORG1GroupInfo);

                            }


                            if (tOldBranchCode != tGroupInfo.BranchCode)
                            {
                                tOldBranchCode = tGroupInfo.BranchCode;
                                tBranchGroupInfo = new FACTGroupInfo();
                                tBranchGroupInfo.BranchCode = tDataSet.GetString("org2_id").Trim();
                                tBranchGroupInfo.BranchName = tDataSet.GetString("org2_name");
                                tBranchGroupInfo.ORG1Code = tDataSet.GetString("org1_id");
                                tBranchGroupInfo.ORG1Name = tDataSet.GetString("org1_name");

                                if (tORG1GroupInfo.SubGroups == null)
                                {
                                    tORG1GroupInfo.SubGroups = new FACTGroupInfoCollection();
                                }
                                tORG1GroupInfo.SubGroups.Add(tBranchGroupInfo);
                            }

                            if (tOldCenterCode != tGroupInfo.CenterCode)
                            {
                                tOldCenterCode = tGroupInfo.CenterCode;

                                tCenterGroupInfo = new FACTGroupInfo();
                                tCenterGroupInfo.ORG1Code = tDataSet.GetString("org1_id");
                                tCenterGroupInfo.ORG1Name = tDataSet.GetString("org1_name");
                                tCenterGroupInfo.BranchCode = tDataSet.GetString("org2_id").Trim();
                                tCenterGroupInfo.BranchName = tDataSet.GetString("org2_name");
                                tCenterGroupInfo.CenterCode = tDataSet.GetString("CenterCode").Trim();
                                tCenterGroupInfo.CenterName = tDataSet.GetString("CenterName");
                               

                                if (tBranchGroupInfo.SubGroups == null)
                                {
                                    tBranchGroupInfo.SubGroups = new FACTGroupInfoCollection();
                                }
                                tBranchGroupInfo.SubGroups.Add(tCenterGroupInfo);
                            }

                            tDataSet.MoveNext();
                        }
                    }
                    else
                    {
                       GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "FACT 그룹 정보가 존재하지 않습니다.");
                    }

                    //GlobalClass.m_FileLog.PrintLogEnter(tDataSet.RecordCount.ToString() + "개의 그룹 정보를 등록하였습니다." + DateTime.Now.ToString());
                    MKOleDBClass.CloseDataSet(tDataSet);
                    FactCountDevice(GlobalClass.m_FACTGroupInfo);
                    System.Diagnostics.Debug.WriteLine("로그 장비 개수 : " +s_TotalDeviceCount);
                    return true;
                }
                else
                {
                   GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "FACT 그룹 정보 로드에 실패 하였습니다.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MKOleDBClass.CloseDataSet(tDataSet);
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, ex.ToString());
                return false;
            }
        }

        private static int s_TotalDeviceCount = 0;
        private static void FactCountDevice(FACTGroupInfo aGroupInfo)
        {

            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;
            try
            {
                if (aGroupInfo.SubGroups != null && aGroupInfo.SubGroups.Count > 0)
                {
                    foreach (FACTGroupInfo tGroupInfo in aGroupInfo.SubGroups)
                    {
                        FactCountDevice(tGroupInfo);
                    }
                }

                if (aGroupInfo.CenterCode == string.Empty) return;


                tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();

                if (tDBWI.ExecuteQuery(string.Format("exec SP_ORG_DEVICE_Count '{0}'", aGroupInfo.CenterCode), out tDataSet) == E_DBProcessError.Success)
                {
                    if (tDataSet.RecordCount > 0)
                    {
                        aGroupInfo.DeviceCount = tDataSet.GetInt32("devicecount");
                        s_TotalDeviceCount += aGroupInfo.DeviceCount;
                    }
                }
                MKOleDBClass.CloseDataSet(tDataSet);
            }
            catch (Exception ex)
            {
                MKOleDBClass.CloseDataSet(tDataSet);
            }
        }

        /// <summary>
		/// 장비 모델 정보를 로드 합니다.
		/// </summary>
		/// <returns>장비 모델 정보 로드의 성공 여부 입니다.</returns>
		private static bool LoadModelInfo()
		{
			MKDataSet tDataSet = null;

			try
			{
				//장비 모델 정보를 로드 합니다 ----------------------------------------------------			
				ModelInfo tModelInfo = null;
				GlobalClass.m_ModelInfoCollection = new ModelInfoCollection();
				

				MKDBWorkItem tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                string tQuery = "select m.modeltypecode, mt.modeltypename, m.modelid, m.modelname, ms.morestring, m.SlotCount, m.Divergence, ms.moremark, " +
                        "m.portcnt, mt.factmaxaccesscnt, m.uses, ec.EmbargoCmd, m.IpTypeCd " +
                    "from ne_model m " +
                        "left outer join ne_morestring ms on m.modelID=ms.modelID " +
                        "left outer join NE_EMBAGO_CMD ec on m.modelid=ec.modelid " +
                        "inner join ne_modeltype mt on m.modeltypecode=mt.modeltypecode " +
                    "where mt.Uses=1 and (m.Uses = 1 OR m.Uses = 3) " +
                        "order by mt.modeltypecode, m.modelid";


                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "모델 정보를 로드 합니다.");
                tDBWI.ExecuteQuery(tQuery, out tDataSet);

				if (tDataSet.RecordCount > 0)
				{
					for (int i = 0; i < tDataSet.RecordCount; i++)
					{
						tModelInfo = new ModelInfo();
						tModelInfo.ModelID = tDataSet.GetInt32("ModelID");
						tModelInfo.ModelName = tDataSet.GetString("ModelName");
						tModelInfo.PortCount = tDataSet.GetInt32("PortCnt");
						tModelInfo.ModelTypeCode = tDataSet.GetInt32("ModelTypeCode");
						tModelInfo.ModelTypeName = tDataSet.GetString("ModelTypeName");

						tModelInfo.MoreMark = tDataSet.GetString("MoreMark");
						tModelInfo.MoreString = tDataSet.GetString("MoreString");
						tModelInfo.SlotCount = tDataSet.GetInt32("SlotCount");
						tModelInfo.Divergence = tDataSet.GetInt32("Divergence");

                        tModelInfo.IpTypeCd = tDataSet.GetInt32("IpTypeCd");

                        //2009-12-03 hanjiyeon 조건 추가 - tasknet 137번:명령 작성시 프롬프트 입력할 때 오류 발생
                        //금칙 문자가 등록되지 않은 장비모델에 대하여 비교 시 db에 금칙문자가 없는 장비모델은
                        //embargocmd를 저장하지 않아야 함.
                        if (tDataSet["EmbargoCmd"] != DBNull.Value)
                        {
                            lock (GlobalClass.m_ModelInfoCollection.SyncRoot)
                            {
                                if (GlobalClass.m_ModelInfoCollection.Contains(tModelInfo.ModelID))
                                {
                                    ModelInfo tmpModelInfo = GlobalClass.m_ModelInfoCollection[tModelInfo.ModelID];

                                    tmpModelInfo.EmbagoCmd.Add(tDataSet.GetString("EmbargoCmd"));
                                }
                                else
                                {
                                    tModelInfo.EmbagoCmd.Add(tDataSet.GetString("EmbargoCmd"));
                                    GlobalClass.m_ModelInfoCollection.Add(tModelInfo);
                                }
                            }
                        }
                        else
                        {
                            lock (GlobalClass.m_ModelInfoCollection.SyncRoot)
                            {
                                if (!GlobalClass.m_ModelInfoCollection.Contains(tModelInfo.ModelID))
                                {
                                    GlobalClass.m_ModelInfoCollection.Add(tModelInfo);
                                }
                            }
                        }

                        MKDataSet tDsCommand = null;

                        tQuery = string.Format("EXEC SP_RACT_GET_COMMANDS {0}, {1};",
                                               tModelInfo.ModelID,
                                               (int)E_CommandPart.ConfigBRRestore);

                        if (tDBWI.ExecuteQuery(tQuery, out tDsCommand) != E_DBProcessError.Success)
                        {
                        }

                        for (int j = 0; j < tDsCommand.RecordCount; j++)
                        {

                            CfgRestoreCommand tCfgRestoreCommand = new CfgRestoreCommand();

                            tCfgRestoreCommand.CmdSeq = tDsCommand.GetInt32("CmdSeq");
                            tCfgRestoreCommand.Cmd = tDsCommand.GetString("Cmd");
                            tCfgRestoreCommand.T_Prompt = tDsCommand.GetString("T_Prompt");

                            tModelInfo.CfgRestoreCommands.Add(tCfgRestoreCommand);

                            tDsCommand.MoveNext();
                        }

                        // 2013-05-02 - shinyn - 기본접속 명령을 로드합니다.
                        MKDataSet tCmdDataSet = null;

                        tQuery = string.Format(@"  select cmd_cmd.* 
                                              from cmd_cmd WITH(NOLOCK),
                                              cmd_cmdset cmdset WITH(NOLOCK)
                                              inner join CMD_CMDMASTER cmdmaster WITH(NOLOCK) on cmdmaster.cmdsetid = cmdset.cmdsetid
                                              where cmdset.cmdpartid = 65
                                              and cmdset.modelid = {0} 
                                              and cmdmaster.cmdgrpid = cmd_cmd.cmdgrpid", tModelInfo.ModelID);

                        if (tDBWI.ExecuteQuery(tQuery, out tCmdDataSet) != E_DBProcessError.Success)
                        {
                        }

                        for (int j = 0; j < tCmdDataSet.RecordCount; j++)
                        {
                            FACT_DefaultConnectionCommand tCommand = new FACT_DefaultConnectionCommand();
                            tCommand.CMDSeq = tCmdDataSet.GetInt32("cmdseq");
                            tCommand.CMD = tCmdDataSet.GetString("cmd");
                            tCommand.Prompt = tCmdDataSet.GetString("t_prompt");
                            tCommand.ErrorString = tCmdDataSet.GetString("t_ErrStr");

                            tModelInfo.DefaultConnectionCommadSet.CommandList.Add(tCommand);

                            tCmdDataSet.MoveNext();
                        }

						tDataSet.MoveNext();
					}
				}
				
				MKOleDBClass.CloseDataSet(tDataSet);
				return true;
			}
			catch (Exception ex)
			{
				MKOleDBClass.CloseDataSet(tDataSet);
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "모델 정보 로드에 실패 했습니다.");
				return false;
			}
		}

        // 15-09-10
        // 제한 명령어 정보를 수집합니다.
        // Gunny
        public static bool LoadLimitCmdInfo(E_UserType tUserType)
        {
            MKDataSet tDataSet = null;

            try
            {
                //장비 모델 정보를 로드 합니다 ----------------------------------------------------			
                EmbagoInfo tEmbagoInfo = null;
                LimitCmdInfo tLimitCmdInfo = null;
                GlobalClass.m_LimitCmdInfoCollection = new LimitCmdInfoCollection();


                MKDBWorkItem tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                //string tQuery = "SELECT EmbargoID , ModelID , EmbargoCmd from fact_main.dbo.RACT_NE_EMBAGO_CMD order by ModelID , EmbargoID";
                string tQuery = "SP_RACT_Get_EmbargoCmdInfo {0}";
                tQuery = string.Format(tQuery, (int)tUserType);

                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "제한 명령어를 로드 합니다.");
                tDBWI.ExecuteQuery(tQuery, out tDataSet);

                if (tDataSet.RecordCount > 0)
                {
                    for (int i = 0; i < tDataSet.RecordCount; i++)
                    {
                        tLimitCmdInfo = new LimitCmdInfo();
                        tLimitCmdInfo.ModelID = tDataSet.GetInt32("ModelID");
                        tLimitCmdInfo.EmbargoID = tDataSet.GetInt32("EmbargoID");

                        if (tDataSet["EmbargoCmd"] != DBNull.Value)
                        {
                            tEmbagoInfo = new EmbagoInfo();
                            tEmbagoInfo.Embargo = tDataSet.GetString("EmbargoCmd");
                            tEmbagoInfo.EmbargoEnble = tDataSet.GetBool("mAdmin");

                            lock (GlobalClass.m_LimitCmdInfoCollection.SyncRoot)
                            {
                                if (GlobalClass.m_LimitCmdInfoCollection.Contains(tLimitCmdInfo.ModelID))
                                {
                                    tLimitCmdInfo = GlobalClass.m_LimitCmdInfoCollection[tLimitCmdInfo.ModelID];

                                    //tLimitCmdInfo.EmbagoCmd.Add(tDataSet.GetString("EmbargoCmd"));
                                    tLimitCmdInfo.EmbagoCmd.Add(tEmbagoInfo);
                                }
                                else
                                {
                                    //tLimitCmdInfo.EmbagoCmd.Add(tDataSet.GetString("EmbargoCmd"));
                                    tLimitCmdInfo.EmbagoCmd.Add(tEmbagoInfo);
                                    GlobalClass.m_LimitCmdInfoCollection.Add(tLimitCmdInfo);
                                }
                            }
                        }

                        tDataSet.MoveNext();
                    }
                }

                MKOleDBClass.CloseDataSet(tDataSet);
                return true;
            }
            catch (Exception ex)
            {
                MKOleDBClass.CloseDataSet(tDataSet);
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "제한 명령어 로드에 실패 했습니다.");
                return false;
            }
        }

        // 15-09-30
        // 기본 명령어 정보를 수집합니다.
        // Gunny
        public static bool LoadDefaultCmdInfo()
        {
            MKDataSet tDataSet = null;

            try
            {
                //장비 모델 정보를 로드 합니다 ----------------------------------------------------			
                DefaultCmdInfo tDefaultCmdInfo = null;
                GlobalClass.m_DefaultCmdInfoCollection = new DefaultCmdInfoCollection();


                MKDBWorkItem tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                
                string tQuery = "SELECT ID , ModelID , Command , Description , UserID from fact_main.dbo.RACT_AutoCommandGuide order by ModelID , ID";
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "장비별 기본 명령어를 로드 합니다.");

                tDBWI.ExecuteQuery(tQuery, out tDataSet);

                if (tDataSet.RecordCount > 0)
                {
                    for (int i = 0; i < tDataSet.RecordCount; i++)
                    {
                        tDefaultCmdInfo = new DefaultCmdInfo();
                        tDefaultCmdInfo.ModelID = tDataSet.GetInt32("ModelID");
                        tDefaultCmdInfo.EmbargoID = tDataSet.GetInt32("ID");

                        if (tDataSet["Command"] != DBNull.Value)
                        {
                            lock (GlobalClass.m_DefaultCmdInfoCollection.SyncRoot)
                            {
                                if (GlobalClass.m_DefaultCmdInfoCollection.Contains(tDefaultCmdInfo.ModelID))
                                {
                                    tDefaultCmdInfo = GlobalClass.m_DefaultCmdInfoCollection[tDefaultCmdInfo.ModelID];

                                    tDefaultCmdInfo.Command.Add(tDataSet.GetString("Command"));
                                    tDefaultCmdInfo.Description.Add(tDataSet.GetString("Description"));
                                }
                                else
                                {
                                    tDefaultCmdInfo.Command.Add(tDataSet.GetString("Command"));
                                    tDefaultCmdInfo.Description.Add(tDataSet.GetString("Description"));
                                    GlobalClass.m_DefaultCmdInfoCollection.Add(tDefaultCmdInfo);
                                }
                            }
                        }

                        tDataSet.MoveNext();
                    }
                }

                MKOleDBClass.CloseDataSet(tDataSet);
                return true;
            }
            catch (Exception ex)
            {
                MKOleDBClass.CloseDataSet(tDataSet);
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "기본 명령어 로드에 실패 했습니다.");
                return false;
            }
        }

        // 15-09-30
        // 기본 명령어 정보를 수집합니다.
        // Gunny
        public static bool LoadAutoCompleteInfo(int userID)
        {
            MKDataSet tDataSet = null;

            try
            {
                //장비 모델 정보를 로드 합니다 ----------------------------------------------------			
                AutoCompleteCmdInfo tAutoCompleteCmdInfo = null;
                GlobalClass.m_AutoCompleteCmdInfoCollection = new AutoCompleteCmdInfoCollection();


                MKDBWorkItem tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();

                string tQuery = 
                "SELECT distinct "+ 
                "LTRIM(RTRIM(A.[Command])) as Command"+
                ",B.UserID"+
                ",C.ModelID "+
                "FROM [FACT_MAIN].[dbo].[RACT_Log_ExcuteCommand] A "+
                "Inner Join [FACT_MAIN].dbo.RACT_LOG_DeviceConnection B On  A.ConnectionLogId= B.ID "+
                "Inner Join [FACT_MAIN].dbo.Ne_NE C On  B.NEID = C.NEID " +
                "where B.UserID='{0}' " +
                "order by Command desc";

                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "자동완성을 로드 합니다.");

                if (tDBWI.ExecuteQuery(string.Format(tQuery, userID), out tDataSet) == E_DBProcessError.Success)
                {
                    if (tDataSet.RecordCount > 0)
                    {
                        for (int i = 0; i < tDataSet.RecordCount; i++)
                        {
                            tAutoCompleteCmdInfo = new AutoCompleteCmdInfo();
                            tAutoCompleteCmdInfo.ModelID = tDataSet.GetInt32("ModelID");

                            if (tDataSet["Command"] != DBNull.Value)
                            {
                                lock (GlobalClass.m_AutoCompleteCmdInfoCollection.SyncRoot)
                                {
                                    if (GlobalClass.m_AutoCompleteCmdInfoCollection.Contains(tAutoCompleteCmdInfo.ModelID))
                                    {
                                        tAutoCompleteCmdInfo = GlobalClass.m_AutoCompleteCmdInfoCollection[tAutoCompleteCmdInfo.ModelID];

                                        tAutoCompleteCmdInfo.Command.Add(tDataSet.GetString("Command"));
                                    }
                                    else
                                    {
                                        tAutoCompleteCmdInfo.Command.Add(tDataSet.GetString("Command"));
                                        GlobalClass.m_AutoCompleteCmdInfoCollection.Add(tAutoCompleteCmdInfo);
                                    }
                                }
                            }

                            tDataSet.MoveNext();
                        }
                    }
                }
                MKOleDBClass.CloseDataSet(tDataSet);
                return true;
            }
            catch (Exception ex)
            {
                MKOleDBClass.CloseDataSet(tDataSet);
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "자동완성을 로드하는데 실패 했습니다.");
                return false;
            }
        }
    }
}
