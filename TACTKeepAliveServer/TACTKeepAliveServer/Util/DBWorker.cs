using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using MKLibrary.MKData;
using System.Data.OleDb;

namespace TACT.KeepAliveServer
{
    public static class DBWorker
    {
        /// <summary>
        /// 데이터베이스 풀 개체입니다.
        /// </summary>
        private static MKOleDBPool m_DBPool = null;
        public static MKOleDBPool DBPool
        {
            get { return m_DBPool; }
            private set { m_DBPool = value; }
        }
        /// <summary>
        /// 실행할 쿼리문
        /// </summary>
        private static string m_QueryString = string.Empty;
        /// <summary>
        /// 쿼리실행 결과 에러값
        /// </summary>
        private static E_DBProcessError m_DBProcessError = E_DBProcessError.UnknownError;

        public static bool OpenDBPool(int aMaxConnectionCount, string aServerName, string aDatabaseName, string aUserName, string aUserPassword)
        {
            E_DBProcessError tError = E_DBProcessError.UnknownError;
            try
            {
                m_DBPool = new MKOleDBPool(E_DatabaseServerType.MSSqlServer, 10);
                m_DBPool.StartDBPool();

                tError = m_DBPool.OpenDatabase(aMaxConnectionCount, aServerName, aDatabaseName, aUserName, aUserPassword);
                if (tError != E_DBProcessError.Success)
                {
                    GlobalClass.PrintLogError("■ 데이터베이스 열기를 실패하였습니다. " + tError.ToString());
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("[GlobalClass.OpenDBPool] ", ex);
                return false;
            }
        }

        public static void StopDBPool()
        {
            if (m_DBPool != null)
            {
                lock (m_DBPool)
                {
                    try
                    {
                        m_DBPool.StopDBPool();
                    }
                    catch (Exception) { }
                    m_DBPool.Dispose();
                    m_DBPool = null;
                }
            }
        }

        /// <summary>
        /// 기초데이터 로딩: Cat.M1 모듈 정보 및 장비(RPCS)매핑 정보 조회
        /// </summary>
        /// <param name="oList"></param>
        public static bool SelectKeepAliveCollections(out KeepAliveCollection oList)
        {
            // 결과데이터 초기화
            oList = new KeepAliveCollection();
            if (m_DBPool == null) return false;

            MKDBWorkItem tDBWI = m_DBPool.GetDBWorkItem();
            m_QueryString = @"SELECT  USIM, IMEI, SerialNumber, LTEModuleName, 
                                      NeID, DeviceIP, DeviceModelName, KeepAliveLastRecvDate 
                                      /*UpdateDate, InsertDate, KeepAliveFirstRecvDate*/
                                FROM  LTE_NE CM WITH(NOLOCK) 
                               WHERE  1=1 ";

            MKDataSet tDataSet = null;
            m_DBProcessError = tDBWI.ExecuteQuery(m_QueryString, out tDataSet);
            if (m_DBProcessError != E_DBProcessError.Success)
            {
                GlobalClass.PrintLogError("[DBWorker.SelectKeepAliveCollections] ExecuteQuery error: " + m_DBProcessError.ToString() + "\r\nQuery= " + m_QueryString);
            }

            if (tDataSet == null) return true; //no data
            for (int i = 0 ; i < tDataSet.RecordCount ; i++)
            {
                KeepAliveMsg tItem = new KeepAliveMsg();
                tItem.LTEModuleName = tDataSet.GetString("LTEModuleName");
                tItem.DeviceIP = tDataSet.GetString("DeviceIP", string.Empty);
                tItem.IMEI = tDataSet.GetString("IMEI", string.Empty);
                tItem.RecvDateTime = tDataSet.GetDateTime("KeepAliveLastRecvDate", DateTime.MinValue);
                tItem.ModelName = tDataSet.GetString("DeviceModelName", string.Empty);
                tItem.SerialNumber = tDataSet.GetString("SerialNumber", string.Empty);
                tItem.USIM = tDataSet.GetString("USIM", string.Empty);
                tItem.NeId = tDataSet.GetInt32("NeID", 0);

                oList.Add(tItem);
                tDataSet.MoveNext();
            }
            return true;
        }



        /// <summary>
        /// Keep-Alive수신정보로 Cat.M1장비정보 및 매핑정보를 UPDATE or INSERT한다. 
        /// 1) LTE_NE : INSERT or UPDATE
        /// 2) LTE_NE_UPDATE_HISTORY : (정보변경시)INSERT
        /// </summary>
        /// <param name="aMsg"></param>
        /// <param name="aIsFullMsg"></param>
        /// <returns>성공여부</returns>
        public static void Update_LTE_NE(ref KeepAliveMsg aMsg)
        {
            if (m_DBPool == null) return;
            Util.Assert(!string.IsNullOrEmpty(aMsg.USIM), "[DBWorker.Update_LTE_NE] USIM은 필수키 값입니다.");
            if (string.IsNullOrEmpty(aMsg.USIM)) return;

            //0     1     2             3              4         5                6                  7
            //@USIM @IMEI @SerialNumber @LTEModuleName @DeviceIP @DeviceModelName @KeepAliveRecvDate @UsrId
            //DB프로시저에서 파라미터가 NULL인 항목은 업데이트(overwrite) 되지 않음.
            //EXEC P_LTE_KeepAlive_UPDATE_LTE_NE '0123456789f', '0123456789', '0123456789', 'LTEModuleName', '127.0.0.2', 'V100', '2018-11-29';
            m_QueryString = string.Format(
                @"EXEC P_LTE_KeepAlive_UPDATE_LTE_NE {0}, {1}, {2}, {3}, {4}, {5}, {6}",
                Util.QuotedStr(aMsg.USIM),  //0
                aMsg.IsFullMessage() ? Util.QuotedStr(aMsg.IMEI) : "null",  //1
                aMsg.IsFullMessage() ? Util.QuotedStr(aMsg.SerialNumber) : "null", //2
                aMsg.IsFullMessage() ? Util.QuotedStr(aMsg.LTEModuleName) : "null", //3
                Util.QuotedStr(aMsg.DeviceIP), //4
                aMsg.IsFullMessage() ? Util.QuotedStr(aMsg.ModelName) : "null", //5
                Util.DateTimeToDBValue(aMsg.RecvDateTime) //6
            );
            
            MKDBWorkItem tDBWI = m_DBPool.GetDBWorkItem();
            MKDataSet tDataSet = null;
            m_DBProcessError = tDBWI.ExecuteQuery(m_QueryString, out tDataSet);
            if (m_DBProcessError != E_DBProcessError.Success)
            {
                GlobalClass.PrintLogError("[DBWorker.Update_LTE_NE] ExecuteQuery error: " + m_DBProcessError.ToString() + "\r\nQuery= " + m_QueryString);
            }

            // 정보 재조회(DB : 메모리 정보 동기화)
            if (tDataSet == null) return;

            string resultCode = tDataSet.GetString("ResultCode");
            if (resultCode.Equals("OK"))
            {
                //aMsg.USIM = tDataSet.GetString("USIM", string.Empty);
                aMsg.LTEModuleName = tDataSet.GetString("LTEModuleName");
                aMsg.DeviceIP = tDataSet.GetString("DeviceIP", string.Empty);
                aMsg.IMEI = tDataSet.GetString("IMEI", string.Empty);
                //aMsg.ServerDateTime = tDataSet.GetDateTime("KeepAliveLastRecvDate", DateTime.MinValue);
                aMsg.RecvDateTime = (DateTime)tDataSet["KeepAliveLastRecvDate"];
                aMsg.ModelName = tDataSet.GetString("DeviceModelName", string.Empty);
                aMsg.SerialNumber = tDataSet.GetString("SerialNumber", string.Empty);
                aMsg.NeId = tDataSet.GetInt32("NeID", 0);
            }
            else //resultCode.Equals("NOK")
            {
                string tErrorMsg = string.Format(
                    @" ErrorNumber={0}, ErrorSeverity={1}, ErrorProcedure={2}, ErrorLine={3}, ErrorMessage={4}"
                    , tDataSet.GetInt32("ErrorNumber")
                    , tDataSet.GetInt32("ErrorSeverity")
                    , tDataSet.GetString("ErrorProcedure")
                    , tDataSet.GetInt32("ErrorLine")
                    , tDataSet.GetString("ErrorMessage")
                );
                GlobalClass.PrintLogError("[ExecuteQuery ExceptionInfo] " + tErrorMsg);
            }
        }


    } // End of class DBWorker
}
