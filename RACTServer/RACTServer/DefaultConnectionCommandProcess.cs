using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;
using MKLibrary.MKData;

namespace RACTServer
{
    public class DefaultConnectionCommandProcess
    {
        internal static void GetCommand(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = null;

            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;
            string tQueryString = "";
            FACT_DefaultConnectionCommandSet tCommandSet = new FACT_DefaultConnectionCommandSet();
            FACT_DefaultConnectionCommand tCommand;
            // 2013-05-02 - shinyn - 수동장비인 경우 DeviceInfo에 기본접속 정보가 있다.
            DeviceInfo tDeviceInfo = null;
            try
            {
                tResultData = new ResultCommunicationData(aClientRequest);

                // 2013-05-02 - shinyn - 수동장비인 경우 DeviceInfo에 기본접속 정보가 있다.
                tDeviceInfo = (DeviceInfo)aClientRequest.RequestData;
                // 일반 FOMS연동 장비인경우
                if (tDeviceInfo != null && tDeviceInfo.DeviceType == E_DeviceType.NeGroup)
                {
                    //2013-05-02 - shinyn - 기본정보로드된 Device정보가 있으면 최초 정보 로드된 정보로 보내준다.
                    // 장비 로딩시간이 너무 오래 걸려서 제외
                    /*
                    if (GlobalClass.m_DeviceInfos.Contains(tDeviceInfo.DeviceID) && GlobalClass.m_ModelInfoCollection.Contains(tDeviceInfo.ModelID))
                    {
                        ModelInfo tModelInfo = GlobalClass.m_ModelInfoCollection[tDeviceInfo.ModelID];
                        tResultData.ResultData = tModelInfo.DefaultConnectionCommadSet;
                    }
                    */
                    if(tDeviceInfo != null)
                    {
                        // 2013-08-16 - shinyn - 자동 로그인 명령어 반환 nolock설정 처리
                        tQueryString = string.Format(@"  select cmd_cmd.* 
                          from cmd_cmd with(nolock)
                          inner join Ne_NE NE with(nolock) on NE.neid = {0}
                          inner join cmd_cmdset cmdset with(nolock) on  cmdset.modelid = ne.modelid
                          inner join CMD_CMDMASTER cmdmaster with(nolock) on cmdmaster.cmdsetid = cmdset.cmdsetid
                          where cmdset.cmdpartid = 65
                          and cmdmaster.cmdgrpid = cmd_cmd.cmdgrpid order by CmdSeq", tDeviceInfo.DeviceID);

                        // 2013-05-02 - shinyn - 조회속도 확인합니다.
                        System.Diagnostics.Debug.WriteLine(" 기본접속 명령 조회시작 : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                        tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                        if (tDBWI.ExecuteQuery(tQueryString, out tDataSet) != E_DBProcessError.Success)
                        {
                            System.Diagnostics.Debug.WriteLine(tDBWI.ErrorString);
                            tResultData.Error.Error = E_ErrorType.UnKnownError;
                            tResultData.Error.ErrorString = tDBWI.ErrorString;
                        }
                        else
                        {
                            if (tDataSet != null)
                            {
                                if (aClientRequest.UserData == null)
                                {
                                    for (int i = 0; i < tDataSet.RecordCount; i++)
                                    {
                                        if (tDataSet["Cmd"] != DBNull.Value) //TL1 접속때문에 기본 접속 명령세트의 명령어 레코드가 증가됨. cmd가 null 인경우 명령어를 추가하지 않음.
                                        {
                                            tCommand = new FACT_DefaultConnectionCommand();
                                            tCommand.CMDSeq = tDataSet.GetInt32("cmdseq");
                                            tCommand.CMD = tDataSet.GetString("cmd");
                                            tCommand.Prompt = tDataSet.GetString("t_prompt");
                                            tCommand.ErrorString = tDataSet.GetString("t_ErrStr");

                                            tCommandSet.CommandList.Add(tCommand);
                                        }

                                        tDataSet.MoveNext();
                                    }
                                }
                                else if(aClientRequest.UserData.ToString().Equals("TL1"))
                                {                                    
                                    for (int i = 0; i < tDataSet.RecordCount; i++)
                                    {
                                        tCommand = new FACT_DefaultConnectionCommand();
                                        tCommand.CMDSeq = tDataSet.GetInt32("cmdseq");                                        
                                        tCommand.ErrorString = tDataSet.GetString("t_ErrStr", "");

                                        //2015-09-18 hanjiyeon 추가 TL1 접속 정보                                    
                                        tCommand.TL1_CMD = tDataSet.GetString("T_ImmediateCommand");
                                        tCommand.TL1_Prompt = tDataSet.GetString("T_ImmediatePrompt");

                                        tCommandSet.CommandList.Add(tCommand);

                                        tDataSet.MoveNext();
                                    }
                                }
                            }
                            tResultData.ResultData = tCommandSet;
                        }
                    }
                }
                else if (tDeviceInfo.DeviceType == E_DeviceType.UserNeGroup)
                {
                    // 2013-05-02 - shinyn 수동장비인경우 기본접속정보를 DeviceInfo객체에서 만들어서 보낸다.
                    //수동장비인경우 기본접속 정보 없는경우 아래와 같이 기본 정보로 접속되도록 한다.
                    // 0 : ${WAIT}       default : n:|:
                    // 1 : ${USERID1}    default : d: 
                    // 2 : ${PASSWORD1}  default : #|>
                    // 3 : ${USERID2}    default : d:|#
                    // 4 : ${PASSWORD2}  default : #
                    tCommand = new FACT_DefaultConnectionCommand();
                    tCommand.CMDSeq = 0;
                    tCommand.CMD = "${WAIT}";
                    tCommand.Prompt = tDeviceInfo.WAIT == "" ? "n:|:" : tDeviceInfo.WAIT;
                    tCommand.ErrorString = "";
                    //2015-09-18 hanjiyeon 추가 - TL1 접속 정보
                    if (aClientRequest.UserData != null &&
                        aClientRequest.UserData.ToString() == "TL1")                                
                    {
                        tCommand.TL1_CMD = "${WAIT}";
                        tCommand.TL1_Prompt = "<";
                    }

                    tCommandSet.CommandList.Add(tCommand);

                    tCommand = new FACT_DefaultConnectionCommand();
                    tCommand.CMDSeq = 1;
                    tCommand.CMD = "${USERID1}";
                    tCommand.Prompt = tDeviceInfo.USERID == "" ? "d:" : tDeviceInfo.USERID;
                    tCommand.ErrorString = "";
                    //2015-09-18 hanjiyeon 추가 - TL1 접속 정보
                    if (aClientRequest.UserData != null &&
                        aClientRequest.UserData.ToString() == "TL1")  
                    {
                        tCommand.TL1_CMD = "T";
                        tCommand.TL1_Prompt = ":";
                    }

                    tCommandSet.CommandList.Add(tCommand);

                    tCommand = new FACT_DefaultConnectionCommand();
                    tCommand.CMDSeq = 2;
                    tCommand.CMD = "${PASSWORD1}";
                    tCommand.Prompt = tDeviceInfo.PWD == "" ? "#|>" : tDeviceInfo.PWD;
                    tCommand.ErrorString = "";
                    //2015-09-18 hanjiyeon 추가 - TL1 접속 정보
                    if (aClientRequest.UserData != null &&
                        aClientRequest.UserData.ToString() == "TL1")  
                    {
                        tCommand.TL1_CMD = "SUPERUSER";
                        tCommand.TL1_Prompt = ":";
                    }

                    tCommandSet.CommandList.Add(tCommand);

                    tCommand = new FACT_DefaultConnectionCommand();
                    tCommand.CMDSeq = 3;
                    tCommand.CMD = "${USERID2}";
                    tCommand.Prompt = tDeviceInfo.USERID2 == "" ? "d:|#" : tDeviceInfo.USERID2;
                    tCommand.ErrorString = "";
                    //2015-09-18 hanjiyeon 추가 - TL1 접속 정보
                    if (aClientRequest.UserData != null &&
                        aClientRequest.UserData.ToString() == "TL1")  
                    {
                        tCommand.TL1_CMD = "ANS#150";
                        tCommand.TL1_Prompt = ":";
                    }

                    tCommandSet.CommandList.Add(tCommand);

                    //2015-09-18 hanjiyeon 추가 - TL1 접속이 아닐때만 아래 명령어 추가.
                    if (aClientRequest.UserData == null)
                    {
                        tCommand = new FACT_DefaultConnectionCommand();
                        tCommand.CMDSeq = 4;
                        tCommand.CMD = "${PASSWORD2}";
                        tCommand.Prompt = tDeviceInfo.PWD2 == "" ? "#" : tDeviceInfo.PWD2;
                        tCommand.ErrorString = "";

                        tCommandSet.CommandList.Add(tCommand);
                    }

                    tResultData.ResultData = tCommandSet;

                }

                GlobalClass.SendResultClient(tResultData);
                // 2013-05-02 - shinyn - 조회속도 확인합니다.
                System.Diagnostics.Debug.WriteLine(" 기본접속 명령 조회종료 및 전송 : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());

            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                GlobalClass.SendResultClient(tResultData);
            }
        }

        /// <summary>
        /// TL1 프로토콜을 사용하는 모델인지의 여부를 체크 합니다.
        /// </summary>
        /// <param name="aModelID"></param>
        /// <returns></returns>
        private static bool CheckTL1_Model(int aModelID)
        {
            if (aModelID == 4035 || aModelID == 4036)
            {
                return true;
            }

            return false;
        }
    }
}
