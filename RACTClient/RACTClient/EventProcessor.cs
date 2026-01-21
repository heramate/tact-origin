using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;

namespace RACTClient
{
   /// <summary>
    /// 이벤트 처리 클래스 입니다.
    /// </summary>
    public static class EventProcessor
    {
        /// <summary>
        /// 그룹 변경 이벤트 입니다.
        /// </summary>
        public static event HandlerArgument2<GroupInfo, E_WorkType> OnGroupInfoChange;
        /// <summary>
        /// 장비 변경 이벤트 입니다.
        /// </summary>
        public static event HandlerArgument2<DeviceInfo, E_WorkType> OnDeviceInfoChange;
        /// <summary>
        /// 장비목록 변경 이벤트 입니다.
        /// </summary>
        public static event HandlerArgument2<DeviceInfoCollection, E_WorkType> OnDeviceInfoListChange;
        /// <summary>
        /// 단축 명령 변경 이벤트 입니다.
        /// </summary>
        public static event HandlerArgument2<ShortenCommandInfo, E_WorkType> OnShortenCommandInfoChange;
        /// <summary>
        /// 단축 명령 그룹 변경 이벤트 입니다.
        /// </summary>
        public static event HandlerArgument2<ShortenCommandGroupInfo, E_WorkType> OnShortenCommandGroupInfoChange;
        /// <summary>
        /// 스크립트 그룹 변경 이벤트 입니다.
        /// </summary>
        public static event HandlerArgument2<ScriptGroupInfo, E_WorkType> OnScriptGroupInfoChange;
        /// <summary>
        /// 스크립트 변경 이벤트 입니다.
        /// </summary>
        public static event HandlerArgument2<Script, E_WorkType> OnScriptChange;
        /// <summary>
        /// 로그인 시작 이벤트 입니다.
        /// </summary>
        public static event HandlerArgument1<bool> OnLoginStart;
        /// <summary>
        /// 로그인 시작  변경 이벤트 실행 함수 입니다.
        /// </summary>
        /// <param name="aRefObject">로그인 시작  object</param>
        /// <param name="aProcessingType"></param>
        public static void LoginStarting(bool isCancel)
        {
            if (OnLoginStart != null)
                OnLoginStart(isCancel);
        }

        /// <summary>
        /// 그룹 변경 이벤트 실행 함수 입니다.
        /// </summary>
        /// <param name="aAreaInfo"></param>
        /// <param name="aProcessingType"></param>
        public static void Run(GroupInfo aGroupInfo, E_WorkType aWorkType)
        {
            if (OnGroupInfoChange != null)
                OnGroupInfoChange(aGroupInfo, aWorkType);
        }
        /// <summary>
        /// 장비 변경 이벤트 실행 함수 입니다.
        /// </summary>
        /// <param name="aDeviceInfo"></param>
        /// <param name="aWorkType"></param>
        public static void Run(DeviceInfo aDeviceInfo, E_WorkType aWorkType)
        {
            if (OnDeviceInfoChange != null)
                OnDeviceInfoChange(aDeviceInfo, aWorkType);
        }
        /// <summary>
        /// 장비목록 변경 이벤트 실행 함수 입니다.
        /// </summary>
        /// <param name="aDeviceInfo"></param>
        /// <param name="aWorkType"></param>
        public static void Run(DeviceInfoCollection aDeviceInfoList, E_WorkType aWorkType)
        {
            if (OnDeviceInfoChange != null)
                OnDeviceInfoListChange(aDeviceInfoList, aWorkType);
        }
        /// <summary>
        /// 단축 명령 변경 이벤트 실행 함수 입니다.
        /// </summary>
        /// <param name="aShortenCommandInfo"></param>
        /// <param name="aWorkType"></param>
        public static void Run(ShortenCommandInfo aShortenCommandInfo, E_WorkType aWorkType)
        {
            if (OnShortenCommandInfoChange != null)
                OnShortenCommandInfoChange(aShortenCommandInfo, aWorkType);
        }

        /// <summary>
        /// 단축 명령 그룹 변경 이벤트 실행 함수 입니다.
        /// </summary>
        /// <param name="aShortenCommandInfo"></param>
        /// <param name="aWorkType"></param>
        public static void Run(ShortenCommandGroupInfo aShortenCommandGroupInfo, E_WorkType aWorkType)
        {
            if (OnShortenCommandGroupInfoChange != null)
                OnShortenCommandGroupInfoChange(aShortenCommandGroupInfo, aWorkType);
        }
        /// <summary>
        /// 스크립트 그룹 변경 이벤트 실행 함수 입니다.
        /// </summary>
        /// <param name="aShortenCommandInfo"></param>
        /// <param name="aWorkType"></param>
        public static void Run(ScriptGroupInfo aScriptGroupInfo, E_WorkType aWorkType)
        {
            if (OnScriptGroupInfoChange != null)
                OnScriptGroupInfoChange(aScriptGroupInfo, aWorkType);
        }
        /// <summary>
        /// 스크립트 변경 이벤트 실행 함수 입니다.
        /// </summary>
        /// <param name="aShortenCommandInfo"></param>
        /// <param name="aWorkType"></param>
        public static void Run(Script aScript, E_WorkType aWorkType)
        {
            if (OnScriptChange != null)
                OnScriptChange(aScript, aWorkType);
        }
    }
}
