using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;

namespace RACTClient
{
    /// <summary>
    /// 기본 핸들러 입니다.
    /// </summary>
    public delegate void DefaultHandler();
    public delegate bool DefaultHandlerReturnBool();
    public delegate void HandlerArgument1(object aValue1);
    public delegate void HandlerArgument2(object aValue1, object aValue2);
    public delegate void HandlerArgument3(object aValue1, object aValue2, object aValue3);
    public delegate void HandlerArgument(object[] aValue);

    // [2008.10.01] mjjoe Handler overload Type Cast 하지 않고 처리하기 위해
    public delegate void HandlerArgument1<UnknownType>(UnknownType aValue1);
    public delegate void HandlerArgument2<UnknownType1, UnknownType2>(UnknownType1 aValue1, UnknownType2 aValue2);
    public delegate void HandlerArgument3<UnknownType1, UnknownType2, UnknownType3>(UnknownType1 aValue1, UnknownType2 aValue2, UnknownType3 aValue3);
    public delegate DialogResult HandlerArgument4<UnknownType1, UnknownType2, UnknownType3, UnknownType4>(UnknownType1 aValue1, UnknownType2 aValue2, UnknownType3 aValue3, UnknownType4 aValue4);
    public delegate DialogResult HandlerArgument5<UnknownType1, UnknownType2, UnknownType3, UnknownType4, UnknownType5>(UnknownType1 aValue1, UnknownType2 aValue2, UnknownType3 aValue3, UnknownType4 aValue4, UnknownType5 aValue5);

    public delegate UnknownReturnType ReturnDefaultHandler<UnknownReturnType>();
    public delegate UnknownReturnType ReturnHandlerArgument1<UnknownReturnType>(object aValue1);
    public delegate UnknownReturnType ReturnHandlerArgument2<UnknownReturnType>(object aValue1, object aValue2);
    public delegate UnknownReturnType ReturnHandlerArgument3<UnknownReturnType>(object aValue1, object aValue2, object aValue3);
    public delegate int ReturnHandlerInt(int aValue);
    /// <summary>
    /// 장비 연결에 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="aDeviceInfo">접속할 장비 주소 입니다.</param>
    /// 2013-05-02 - shinyn - 장비접속 오류를 해결하기 위해 데몬정보와 장비정보를 함께 보내서 접속하도록 수정한다.
    //public delegate void ConnectDeviceHandler (DeviceInfo aDeviceInfo);
    public delegate void ConnectDeviceHandler(DeviceInfo aDeviceInfo, DaemonProcessInfo aDaemonProcessInfo);
    /// <summary>
    /// 그룹 수정에 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="aWorkType">작업 타입 입니다</param>
    public delegate void ModifyGroupHandler(E_WorkType aWorkType, GroupInfo aGroupInfo);

    /// <summary>
    /// 장비 수정에 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="aWorkType"></param>
    /// <param name="aDeviceInfo"></param>
    public delegate void ModifyDeviceHandler(E_WorkType aWorkType, Object aNodeInfo);

    /// <summary>
    /// 2013-01-18 - shinyn - 수동장비 수정에 사용할 핸들러입니다.
    /// </summary>
    /// <param name="aWorkType"></param>
    /// <param name="aNodeInfo"></param>
    public delegate void ModifyUsrDeviceHandler(E_WorkType aWorkType, Object aNodeInfo);

    /// <summary>
    /// 그룹에 속한 장비에 접속 합니다.
    /// </summary>
    /// <param name="aGroupInfo"></param>
    public delegate void ConnectGroupDevice(GroupInfo aGroupInfo);
    /// <summary>
    /// 장비 등록용 핸들러 입니다.
    /// </summary>
    /// <param name="aCollection"></param>
    public delegate void DeviceRegisterHandler(DeviceInfoCollection aCollection);

    /// <summary>
    /// 2013-08-14- shinyn - 사용자 장비 공유 핸들러 입니다.
    /// </summary>
    /// <param name="aCollection"></param>
    public delegate void ShareDeviceHandler(GroupInfo aGroupInfo);

    /// <summary>
    /// 2013-08-14 - shinyn - 선택한 그룹의 사용자 장비목록을 조회목록에 추가합니다.
    /// </summary>
    /// <param name="aDeviceInfos"></param>
    public delegate void AddShareDeviceHandler(DeviceInfoCollection aDeviceInfos);

    /// <summary>
    /// 2013-09-09 - shinyn - 선택한 유저 그룹 사용자를 표시하기 위해 유저정보를 보내줍니다.
    /// </summary>
    /// <param name="aUserInfo"></param>
    public delegate void SelectUserInfoHandler(UserInfo aUserInfo);

}

