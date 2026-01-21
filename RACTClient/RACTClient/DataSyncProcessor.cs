using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;

namespace RACTClient
{
    /// <summary>
    /// 기초정보 변경 핸들러 입니다.
    /// </summary>
    public delegate void ChangeBaseDataHandler();

    public class DataSyncProcessor
    {
        /// <summary>
        /// 그룹정보 변경 완료 이벤트 입니다.
        /// </summary>
        public event HandlerArgument2<GroupInfo,E_WorkType> OnGroupInfoChangeEvent;
        /// <summary>
        /// 장비 정보 변경 완료 이벤트 입니다.
        /// </summary>
        public event HandlerArgument2<DeviceInfo, E_WorkType> OnDeviceInfoChangeEvent;
        /// <summary>
        /// 장비목록 정보 변경 완료 이벤트 입니다.
        /// </summary>
        public event HandlerArgument2<DeviceInfoCollection, E_WorkType> OnDeviceInfoListChangeEvent;
        /// <summary>
        /// 단축 명령 변경 완료 이벤트 입니다.
        /// </summary>
        public event HandlerArgument2<ShortenCommandInfo, E_WorkType> OnShortenCommandInfoChangeEvent;
        /// <summary>
        /// 단축 명령 변경 완료 이벤트 입니다.
        /// </summary>
        public event HandlerArgument2<ShortenCommandGroupInfo, E_WorkType> OnShortenCommandGroupInfoChangeEvent;
        /// <summary>
        /// 스크립트 그룹 변경 완료 이벤트 입니다.
        /// </summary>
        public event HandlerArgument2<ScriptGroupInfo, E_WorkType> OnScriptGroupInfoChangeEvent;
        /// <summary>
        /// 스크립트 변경 완료 이벤트입니다.
        /// </summary>
        public event HandlerArgument2<Script, E_WorkType> OnScriptChangeEvent;


        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public DataSyncProcessor()
        {
            EventProcessor.OnGroupInfoChange += new HandlerArgument2<GroupInfo, E_WorkType>(Action_OnGroupChanged);
            EventProcessor.OnDeviceInfoChange += new HandlerArgument2<DeviceInfo, E_WorkType>(EventProcessor_OnDeviceInfoChange);
            EventProcessor.OnDeviceInfoListChange += new HandlerArgument2<DeviceInfoCollection, E_WorkType>(EventProcessor_OnDeviceInfoListChange);
            EventProcessor.OnShortenCommandInfoChange += new HandlerArgument2<ShortenCommandInfo, E_WorkType>(EventProcessor_OnShortenCommandInfoChange);
            EventProcessor.OnShortenCommandGroupInfoChange += new HandlerArgument2<ShortenCommandGroupInfo, E_WorkType>(EventProcessor_OnShortenCommandGroupInfoChange);
            EventProcessor.OnScriptGroupInfoChange += new HandlerArgument2<ScriptGroupInfo, E_WorkType>(EventProcessor_OnScriptGroupInfoChange);
            EventProcessor.OnScriptChange += new HandlerArgument2<Script, E_WorkType>(EventProcessor_OnScriptChange);
        }

        void EventProcessor_OnScriptChange(Script aValue1, E_WorkType aValue2)
        {
            try
            {
                switch (aValue2)
                {
                    case E_WorkType.Add:
                        AppGlobal.s_ScriptList[aValue1.GroupID].ScriptList.Add(aValue1);
                        break;
                    case E_WorkType.Modify:
                        AppGlobal.s_ScriptList[aValue1.GroupID].ScriptList[aValue1.ID] = aValue1;
                        break;
                    case E_WorkType.Delete:
                        AppGlobal.s_ScriptList[aValue1.GroupID].ScriptList.Remove(aValue1.ID);
                        break;
                }

                if (OnScriptChangeEvent != null) OnScriptChangeEvent(aValue1, aValue2);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        void EventProcessor_OnScriptGroupInfoChange(ScriptGroupInfo aValue1, E_WorkType aWorkType)
        {
            try
            {
                switch (aWorkType)
                {
                    case E_WorkType.Add:
                        AppGlobal.s_ScriptList.Add(aValue1);
                        break;
                    case E_WorkType.Modify:
                        AppGlobal.s_ScriptList[aValue1.ID] = aValue1;
                        break;
                    case E_WorkType.Delete:
                        AppGlobal.s_ScriptList.Remove(aValue1.ID);
                        break;
                }

                if (OnScriptGroupInfoChangeEvent != null) OnScriptGroupInfoChangeEvent(aValue1, aWorkType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        void EventProcessor_OnDeviceInfoChange(DeviceInfo aDeviceInfo, E_WorkType aWorkType)
        {
            try
            {
                switch (aWorkType)
                {
                    case E_WorkType.Add:
                    case E_WorkType.Modify:
                        AppGlobal.s_GroupInfoList[aDeviceInfo.GroupID].DeviceList.Add(aDeviceInfo);
                        break;
                    case E_WorkType.Delete:
                        AppGlobal.s_GroupInfoList[aDeviceInfo.GroupID].DeviceList.Remove(aDeviceInfo.DeviceID);
                        break;
                }

                if (OnDeviceInfoChangeEvent != null) OnDeviceInfoChangeEvent(aDeviceInfo, aWorkType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        void EventProcessor_OnDeviceInfoListChange(DeviceInfoCollection aDeviceInfoList, E_WorkType aWorkType)
        {
            try
            {
                switch (aWorkType)
                {
                    case E_WorkType.Add:
                    case E_WorkType.Modify:
                        foreach (DeviceInfo tDeviceInfo in aDeviceInfoList)
                        {
                            AppGlobal.s_GroupInfoList[tDeviceInfo.GroupID].DeviceList.Add(tDeviceInfo);
                        }
                        break;
                    case E_WorkType.Delete:
                        foreach (DeviceInfo tDeviceInfo in aDeviceInfoList)
                        {
                            // 2013-01-18 - shinyn - 장비삭제시 객체를 넘긴다.
                            AppGlobal.s_GroupInfoList[tDeviceInfo.GroupID].DeviceList.Remove(tDeviceInfo);
                        }
                        break;
                }

                if (OnDeviceInfoListChangeEvent != null) OnDeviceInfoListChangeEvent(aDeviceInfoList, aWorkType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        void EventProcessor_OnShortenCommandGroupInfoChange(ShortenCommandGroupInfo aValue1, E_WorkType aWorkType)
        {
            try
            {
                switch (aWorkType)
                {
                    case E_WorkType.Add:
                        AppGlobal.s_ShortenCommandList.Add(aValue1);
                        break;
                    case E_WorkType.Modify:
                        AppGlobal.s_ShortenCommandList[aValue1.ID] = aValue1;
                        break;
                    case E_WorkType.Delete:
                        AppGlobal.s_ShortenCommandList.Remove(aValue1.ID);
                        break;
                }

                if (OnShortenCommandGroupInfoChangeEvent != null) OnShortenCommandGroupInfoChangeEvent(aValue1, aWorkType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 단축 명령 메모리 동기화 처리 입니다.
        /// </summary>
        /// <param name="aShortenCommandInfo"></param>
        /// <param name="aProcessingType"></param>
        void EventProcessor_OnShortenCommandInfoChange(ShortenCommandInfo aShortenCommandInfo, E_WorkType aProcessingType)
        {
            try
            {
                switch (aProcessingType)
                {
                    case E_WorkType.Add:
                        AppGlobal.s_ShortenCommandList[aShortenCommandInfo.GroupID].ShortenCommandList.Add(aShortenCommandInfo);
                        break;
                    case E_WorkType.Modify:
                        AppGlobal.s_ShortenCommandList[aShortenCommandInfo.GroupID].ShortenCommandList[aShortenCommandInfo.ID] = aShortenCommandInfo;
                        break;
                    case E_WorkType.Delete:
                        AppGlobal.s_ShortenCommandList[aShortenCommandInfo.GroupID].ShortenCommandList.Remove(aShortenCommandInfo.ID);
                        break;
                }

                if (OnShortenCommandInfoChangeEvent != null) OnShortenCommandInfoChangeEvent(aShortenCommandInfo, aProcessingType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 그룹 정보 메모리 동기화 처리 입니다.
        /// </summary>
        /// <param name="aGroupInfo"></param>
        /// <param name="aProcessingType"></param>
        void Action_OnGroupChanged(GroupInfo aGroupInfo, E_WorkType aProcessingType)
        {
            try
            {
                switch (aProcessingType)
                {
                    case E_WorkType.Add:
                        AppGlobal.s_GroupInfoList.Add(aGroupInfo.ID, aGroupInfo);
                        break;
                    case E_WorkType.Modify:
                        AppGlobal.s_GroupInfoList[aGroupInfo.ID] = aGroupInfo;
                        break;
                    case E_WorkType.Delete:
                        AppGlobal.s_GroupInfoList.Remove(aGroupInfo.ID);
                        break;
                }

                if (OnGroupInfoChangeEvent != null) OnGroupInfoChangeEvent(aGroupInfo, aProcessingType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
