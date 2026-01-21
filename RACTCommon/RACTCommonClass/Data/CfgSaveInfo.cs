using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    [Serializable]
    public class CfgSaveInfo : ICloneableEx<CfgSaveInfo>
    {
        // 2013-01-11 - shinyn - Cfg 바이너리파일 정보입니다.

        public CfgSaveInfo()
        {
        }

        public CfgSaveInfo(CfgSaveInfo aCfgSaveInfo)
        {
            CopyTo(aCfgSaveInfo, this, false);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }


        public CfgSaveInfo CompactClone()
        {
            CfgSaveInfo tCfgSaveInfo = new CfgSaveInfo();
            CopyTo(this, tCfgSaveInfo, true);
            return tCfgSaveInfo;
        }

        public CfgSaveInfo DeepClone()
        {
            CfgSaveInfo tCfgSaveInfo = new CfgSaveInfo();
            CopyTo(this, tCfgSaveInfo, false);
            return tCfgSaveInfo;
        }

        private void CopyTo(CfgSaveInfo aSource, CfgSaveInfo aDest, bool aIsCompactClone)
        {
            aDest.CenterFTPID = aSource.CenterFTPID;
            aDest.CenterFTPPW = aSource.CenterFTPPW;
            aDest.FileExtend = aSource.FileExtend;
            aDest.FileName = aSource.FileName;
            aDest.FTPServerIP = aSource.FTPServerIP;
            aDest.StTime = aSource.StTime;
            aDest.CfgRestoreCommands = aSource.CfgRestoreCommands.DeepClone();
            aDest.CfgRestoreScript = aSource.CfgRestoreScript;
            aDest.FullFileName = aSource.FullFileName;
        }

        private long m_Iden = 0;

        public long Iden 
        {
            get { return m_Iden; }
            set { m_Iden = value; }
        }


        private DateTime m_StTime;

        public DateTime StTime
        {
            get { return m_StTime; }
            set { m_StTime = value; }
        }

        private string m_FileName = string.Empty;

        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }

        private string m_FileExtend = string.Empty;

        public string FileExtend
        {
            get { return m_FileExtend; }
            set { m_FileExtend = value; }
        }

        private string m_FTPServerIP = string.Empty;

        public string FTPServerIP
        {
            get { return m_FTPServerIP; }
            set { m_FTPServerIP = value; }
        }

        private string m_CenterFTPID = string.Empty;

        public string CenterFTPID
        {
            get { return m_CenterFTPID; }
            set { m_CenterFTPID = value; }
        }

        private string m_CenterFTPPW = string.Empty;

        public string CenterFTPPW
        {
            get { return m_CenterFTPPW; }
            set { m_CenterFTPPW = value; }
        }

        private CfgRestoreCommandCollection m_CfgRestoreCommands = new CfgRestoreCommandCollection();

        public CfgRestoreCommandCollection CfgRestoreCommands
        {
            get { return m_CfgRestoreCommands; }
            set { m_CfgRestoreCommands = value; }
        }

        private string m_CfgRestoreScript = string.Empty;

        public string CfgRestoreScript
        {
            get { return m_CfgRestoreScript; }
            set { m_CfgRestoreScript = value; }
        }

        private string m_FullFileName = string.Empty;

        public string FullFileName
        {
            get { return m_FullFileName; }
            set { m_FullFileName = value; }
        }
    }

    [Serializable]
    public class CfgSaveInfoCollection : GenericListMarshalByRef<CfgSaveInfo>, ICloneableEx<CfgSaveInfoCollection>
    {
        public CfgSaveInfoCollection()
        {
        }

        public CfgSaveInfoCollection(CfgSaveInfoCollection aCfgSaveInfoCollection)
        {
            CopyTo(aCfgSaveInfoCollection, this, false);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public CfgSaveInfoCollection CompactClone()
        {
            CfgSaveInfoCollection tCollection = new CfgSaveInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }

        public CfgSaveInfoCollection DeepClone()
        {
            CfgSaveInfoCollection tCollection = new CfgSaveInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }

        private void CopyTo(CfgSaveInfoCollection aSource, CfgSaveInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (CfgSaveInfo tCfgSaveInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((CfgSaveInfo)tCfgSaveInfo.CompactClone());
                else
                    aDest.Add((CfgSaveInfo)tCfgSaveInfo.DeepClone());
            }
        }
    }
}
