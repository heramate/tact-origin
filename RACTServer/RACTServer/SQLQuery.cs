using System;
using System.Collections.Generic;
using System.Text;

namespace RACTServer
{
    public class SQLQuery
    {
        #region 기초 데이터 로드 관련 쿼리문을 반환합니다.

        /// <summary>
        /// 그룹정보를 조회합니다.
        /// </summary>
        /// <returns></returns>
        public static string SelectGroupInfo()
        {
            return "Exec SP_ORG_GROUP_INFO";
        }

        /// <summary>
        /// 사용자별 권한 그룹을 가져오기 합니다.
        /// </summary>
        /// <returns></returns>
        public static string SelectFACTGroupInfo()
        {
            return "exec SP_RACT_Get_FACTGroup '{0}'";
        }

        /// <summary>
        /// 메뉴 및 장비 분류 별 일괄 작업 장비 대수 정보를 조회합니다.
        /// </summary>
        /// <returns></returns>
        public static string SelectMaxDeviceCountByModelType()
        {
            return "select * from ne_modeltype_cmd";
        }

        /// <summary>
        /// 명령분류 정보를 조회합니다.
        /// </summary>
        /// <returns></returns>
        public static string SelectCommandPart()
        {
            return "Select * from CMD_CMDPART(nolock)";
        }

        /// <summary>
        /// 사용자 정보 리스트를 조회합니다.
        /// </summary>
        /// <returns>쿼리문자열입니다.</returns>
        public static string SelectUserInfo()
        {
            return "SELECT UsrID, Account, UsrType, orgType, org2_id FROM [dbo].[USR_USER] WITH(NOLOCK) where Uses = 1;";
        }

        /// <summary>
        /// ftp 서버 정보를 조회합니다.
        /// </summary>
        /// <returns></returns>
        public static string SelectFtpServerInfo()
        {
            //2010-08-24 hanjiyeon 수정 - 센터명 표시 변경
            //return "SELECT CenterCode, FTPServerIP, FTPID, FTPPW FROM [dbo].[ORG_BIZPLS]";
            return "SELECT CenterCode, FTPServerIP, FTPID, FTPPW FROM vw_ORG_BIZPLS";
        }

        #endregion //기초 데이터 로드 관련 쿼리문을 반환합니다.
    }
}
