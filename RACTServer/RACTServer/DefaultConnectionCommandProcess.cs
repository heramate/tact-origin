using System;
using System.Collections.Generic;
using RACTCommonClass;
using Dapper;
using System.Linq;

namespace RACTServer
{
    public class DefaultConnectionCommandProcess
    {
        internal static void GetCommand(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = null;
            FACT_DefaultConnectionCommandSet tCommandSet = new FACT_DefaultConnectionCommandSet();
            DeviceInfo tDeviceInfo = null;

            try
            {
                tResultData = new ResultCommunicationData(aClientRequest);
                tDeviceInfo = (DeviceInfo)aClientRequest.RequestData;

                if (tDeviceInfo != null && tDeviceInfo.DeviceType == E_DeviceType.NeGroup)
                {
                    using (var conn = GlobalClass.GetSqlConnection())
                    {
                        if (conn == null) throw new Exception("DBConnection Failed");

                        string sql = @"
                            select cmd_cmd.cmdseq, cmd_cmd.cmd, cmd_cmd.t_prompt, cmd_cmd.t_ErrStr, 
                                   cmd_cmd.T_ImmediateCommand, cmd_cmd.T_ImmediatePrompt
                            from cmd_cmd with(nolock)
                            inner join Ne_NE NE with(nolock) on NE.neid = @DeviceID
                            inner join cmd_cmdset cmdset with(nolock) on cmdset.modelid = ne.modelid
                            inner join CMD_CMDMASTER cmdmaster with(nolock) on cmdmaster.cmdsetid = cmdset.cmdsetid
                            where cmdset.cmdpartid = 65
                            and cmdmaster.cmdgrpid = cmd_cmd.cmdgrpid 
                            order by CmdSeq";

                        var results = conn.Query(sql, new { DeviceID = tDeviceInfo.DeviceID });

                        foreach (var row in results)
                        {
                            var tCommand = new FACT_DefaultConnectionCommand();
                            tCommand.CMDSeq = row.cmdseq != null ? (int)row.cmdseq : 0;
                            tCommand.ErrorString = row.t_ErrStr != null ? (string)row.t_ErrStr : "";

                            if (aClientRequest.UserData == null)
                            {
                                if (row.cmd != null)
                                {
                                    tCommand.CMD = (string)row.cmd;
                                    tCommand.Prompt = row.t_prompt != null ? (string)row.t_prompt : "";
                                    tCommandSet.CommandList.Add(tCommand);
                                }
                            }
                            else if (aClientRequest.UserData.ToString().Equals("TL1"))
                            {
                                tCommand.TL1_CMD = row.T_ImmediateCommand != null ? (string)row.T_ImmediateCommand : "";
                                tCommand.TL1_Prompt = row.T_ImmediatePrompt != null ? (string)row.T_ImmediatePrompt : "";
                                tCommandSet.CommandList.Add(tCommand);
                            }
                        }
                    }
                    tResultData.ResultData = tCommandSet;
                }
                else if (tDeviceInfo != null && tDeviceInfo.DeviceType == E_DeviceType.UserNeGroup)
                {
                    // 수동장비 (UserNeGroup) 처리 로직
                    var tCommand = new FACT_DefaultConnectionCommand();
                    tCommand.CMDSeq = 0;
                    tCommand.CMD = "${WAIT}";
                    tCommand.Prompt = string.IsNullOrEmpty(tDeviceInfo.WAIT) ? "n:|:" : tDeviceInfo.WAIT;
                    
                    if (aClientRequest.UserData != null && aClientRequest.UserData.ToString() == "TL1")
                    {
                        tCommand.TL1_CMD = "${WAIT}";
                        tCommand.TL1_Prompt = "<";
                    }
                    tCommandSet.CommandList.Add(tCommand);

                    tCommand = new FACT_DefaultConnectionCommand();
                    tCommand.CMDSeq = 1;
                    tCommand.CMD = "${USERID1}";
                    tCommand.Prompt = string.IsNullOrEmpty(tDeviceInfo.USERID) ? "d:" : tDeviceInfo.USERID;
                    if (aClientRequest.UserData != null && aClientRequest.UserData.ToString() == "TL1")
                    {
                        tCommand.TL1_CMD = "T";
                        tCommand.TL1_Prompt = ":";
                    }
                    tCommandSet.CommandList.Add(tCommand);

                    tCommand = new FACT_DefaultConnectionCommand();
                    tCommand.CMDSeq = 2;
                    tCommand.CMD = "${PASSWORD1}";
                    tCommand.Prompt = string.IsNullOrEmpty(tDeviceInfo.PWD) ? "#|>" : tDeviceInfo.PWD;
                    if (aClientRequest.UserData != null && aClientRequest.UserData.ToString() == "TL1")
                    {
                        tCommand.TL1_CMD = "SUPERUSER";
                        tCommand.TL1_Prompt = ":";
                    }
                    tCommandSet.CommandList.Add(tCommand);

                    tCommand = new FACT_DefaultConnectionCommand();
                    tCommand.CMDSeq = 3;
                    tCommand.CMD = "${USERID2}";
                    tCommand.Prompt = string.IsNullOrEmpty(tDeviceInfo.USERID2) ? "d:|#" : tDeviceInfo.USERID2;
                    if (aClientRequest.UserData != null && aClientRequest.UserData.ToString() == "TL1")
                    {
                        tCommand.TL1_CMD = "ANS#150";
                        tCommand.TL1_Prompt = ":";
                    }
                    tCommandSet.CommandList.Add(tCommand);

                    if (aClientRequest.UserData == null)
                    {
                        tCommand = new FACT_DefaultConnectionCommand();
                        tCommand.CMDSeq = 4;
                        tCommand.CMD = "${PASSWORD2}";
                        tCommand.Prompt = string.IsNullOrEmpty(tDeviceInfo.PWD2) ? "#" : tDeviceInfo.PWD2;
                        tCommandSet.CommandList.Add(tCommand);
                    }

                    tResultData.ResultData = tCommandSet;
                }

                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                if (tResultData != null)
                {
                    tResultData.Error.Error = E_ErrorType.UnKnownError;
                    GlobalClass.SendResultClient(tResultData);
                }
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
            }
        }

        private static bool CheckTL1_Model(int aModelID)
        {
            return aModelID == 4035 || aModelID == 4036;
        }
    }
}
