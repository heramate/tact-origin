using RACTCommonClass;

namespace RACTClient
{
    public class DeviceConnectionLogClient : SenderObject
    {
        public DeviceConnectionLogOpenResultInfo OpenLog(DeviceConnectionLogOpenRequestInfo aRequest)
        {
            RequestCommunicationData tRequest = AppGlobal.MakeDefaultRequestData();
            tRequest.CommType = E_CommunicationType.RequestOpenDeviceConnectionLog;
            tRequest.RequestData = aRequest;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequest);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);

            if (m_Result == null)
            {
                return new DeviceConnectionLogOpenResultInfo
                {
                    Success = false,
                    ErrorMessage = "서버로부터 응답이 없습니다."
                };
            }

            if (m_Result.Error != null && m_Result.Error.Error != E_ErrorType.NoError)
            {
                return new DeviceConnectionLogOpenResultInfo
                {
                    Success = false,
                    ErrorMessage = m_Result.Error.ErrorString
                };
            }

            return m_Result.ResultData as DeviceConnectionLogOpenResultInfo;
        }

        public DeviceConnectionLogCloseResultInfo CloseLog(DeviceConnectionLogCloseRequestInfo aRequest)
        {
            RequestCommunicationData tRequest = AppGlobal.MakeDefaultRequestData();
            tRequest.CommType = E_CommunicationType.RequestCloseDeviceConnectionLog;
            tRequest.RequestData = aRequest;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequest);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);

            if (m_Result == null)
            {
                return new DeviceConnectionLogCloseResultInfo
                {
                    Success = false,
                    ErrorMessage = "서버로부터 응답이 없습니다."
                };
            }

            if (m_Result.Error != null && m_Result.Error.Error != E_ErrorType.NoError)
            {
                return new DeviceConnectionLogCloseResultInfo
                {
                    Success = false,
                    ErrorMessage = m_Result.Error.ErrorString
                };
            }

            return m_Result.ResultData as DeviceConnectionLogCloseResultInfo;
        }
    }
}
